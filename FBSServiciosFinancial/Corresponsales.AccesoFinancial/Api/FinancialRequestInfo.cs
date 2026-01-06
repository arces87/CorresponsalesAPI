using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Retry;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Corresponsales.AccesoFinancial.Api
{
    public class FinancialRequestInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthInfo _authInfo;
        private readonly IConfiguration _configuracion;

        public FinancialRequestInfo(IHttpContextAccessor httpContextAccessor, AuthInfo authInfo, IConfiguration configuration = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _authInfo = authInfo;
            _configuracion = configuration;
        }

        private string GetUserAgent()
            => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Agent"].ToString();

        private string GetAuthenticatedUsername()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // Intentar obtener desde el claim específico
                var username = user.FindFirst("financial_username")?.Value;
                if (!string.IsNullOrEmpty(username))
                    return username;

                // Fallback al username del claim Name
                return user.Identity.Name ?? user.FindFirst(ClaimTypes.Name)?.Value;
            }
            return null;
        }

        private string GetAuthenticatedPassword()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var encryptedPassword = user.FindFirst("financial_password")?.Value;
                if (!string.IsNullOrEmpty(encryptedPassword) && _configuracion != null)
                {
                    try
                    {
                        var encryptionKey = Encoding.UTF8.GetBytes(_configuracion["JwtKey"]);
                        var cipherText = Convert.FromBase64String(encryptedPassword);
                        var decryptedPassword = DecryptStringFromBytes_Aes(cipherText, encryptionKey, encryptionKey);
                        return decryptedPassword;
                    }
                    catch
                    {
                        // Si falla la desencriptación, retornar null para usar fallback
                        return null;
                    }
                }
            }
            return null;
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold the decrypted text.
            string plaintext = null;

            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private User GetTokenData()
        {
            // Asegurar que Users esté inicializado
            if (_authInfo.Users == null)
                _authInfo.Users = new System.Collections.Generic.Dictionary<string, User>();

            // Prioridad 1: Usar credenciales del usuario autenticado (solo para API Móvil)
            var authenticatedUsername = GetAuthenticatedUsername();
            var authenticatedPassword = GetAuthenticatedPassword();

            if (!string.IsNullOrEmpty(authenticatedUsername) && !string.IsNullOrEmpty(authenticatedPassword))
            {
                var userKey = $"authenticated_{authenticatedUsername}";

                if (!_authInfo.Users.TryGetValue(userKey, out User user))
                {
                    user = new User
                    {
                        Usuario = authenticatedUsername,
                        Password = authenticatedPassword
                    };
                    _authInfo.Users[userKey] = user;
                }
                else
                {
                    // Actualizar credenciales si han cambiado
                    user.Usuario = authenticatedUsername;
                    user.Password = authenticatedPassword;
                }

                return user;
            }

            // Prioridad 2: Usar credenciales preconfiguradas (fallback para Consola o si no hay credenciales en JWT)
            // Intentar usar UsuarioAdmin si está configurado
            if (!string.IsNullOrEmpty(_authInfo.UsuarioAdmin) && _authInfo.Users.TryGetValue(_authInfo.UsuarioAdmin, out User adminUser))
                return adminUser;

            // Si hay usuarios pero no hay UsuarioAdmin configurado, usar el primero disponible
            if (_authInfo.Users.Count > 0)
                return _authInfo.Users.First().Value;

            var userAgent = GetUserAgent();

            if (string.IsNullOrEmpty(userAgent) || !_authInfo.Users.TryGetValue(userAgent, out User configuredUser))
                throw new Exception($"No se ha configurado el usuario para el User-Agent {userAgent}");

            return configuredUser;
        }

        public async Task<RestResponse<T>> ExecAsync<T>(RestRequest req, CancellationToken cancellationToken = default)
        {
            var usuario = GetTokenData();

            var timeoutPolicy = Policy
                    .HandleResult<RestResponse>(ex => ex.StatusCode == HttpStatusCode.RequestTimeout)
                    .RetryAsync(3, async (exception, retryCount) =>
                        await Task.Delay(500).ConfigureAwait(false));

            AsyncRetryPolicy<RestResponse> unauthorizedPolicy = null;

            if (usuario.Token is null)
            {
                var newToken = await Policy
                    .HandleResult<RestResponse<TokenData>>(ex => ex.StatusCode == HttpStatusCode.RequestTimeout)
                    .RetryAsync(3, async (tokenRequest, retry) => await Task.Delay(500))
                    .ExecuteAsync(async () =>
                    {
                        var request = new RestRequest(_authInfo.LoginEndpoint, Method.Post);

                        request.AddJsonBody(new
                        {
                            usuario = usuario.Usuario,
                            password = usuario.Password,
                            numeroDeIntento = 1,
                            usaHuellaDigital = false,
                            maquina = "AMB-CS",
                            ipMaquinaIngreso = "fe80::7cea:e2a4:d0f7:6adb%5"
                        });
                        return await CreateCliente(usuario.Token).ExecuteAsync<TokenData>(request);
                    });

                if (newToken.StatusCode == HttpStatusCode.OK && newToken.Data != null)
                    usuario.Token = newToken.Data;
            }

            if (usuario.Token != null)
            {
                unauthorizedPolicy = Policy
                    .HandleResult<RestResponse>(CaducoAutorizacion)
                    .RetryAsync(async (exception, retryCount, context) =>
                    {
                        if (usuario.Token.RefreshToken != null)
                        {
                            var newAccessToken = await Policy
                            .HandleResult<RestResponse<TokenData>>(ex =>
                            ex.StatusCode == HttpStatusCode.RequestTimeout)
                            .RetryAsync(3, async (tokenRequest, retry) => await Task.Delay(500))
                            .ExecuteAsync(async () =>
                            {
                                var request = new RestRequest(_authInfo.RefreshEndpoint, Method.Post);
                                request.AddJsonBody(usuario.Token);
                                return await CreateCliente(usuario.Token).ExecuteAsync<TokenData>(request);
                            });
                            if (newAccessToken.StatusCode == HttpStatusCode.OK && newAccessToken.Data != null)
                            {
                                usuario.Token = newAccessToken.Data;
                                context["access_token"] = usuario.Token.AccessToken;
                                context["refresh_token"] = usuario.Token.RefreshToken;
                            }
                            else if (newAccessToken.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                newAccessToken = await Policy
                                .HandleResult<RestResponse<TokenData>>(ex =>
                                ex.StatusCode == HttpStatusCode.RequestTimeout)
                                .RetryAsync(3, async (tokenRequest, retry) => await Task.Delay(500))
                                .ExecuteAsync(async () =>
                                {
                                    var request = new RestRequest(_authInfo.LoginEndpoint, Method.Post);
                                    request.AddJsonBody(new
                                    {
                                        usuario = usuario.Usuario,
                                        password = usuario.Password,
                                        numeroDeIntento = 1,
                                        usaHuellaDigital = false,
                                        maquina = "AMB-CS",
                                        ipMaquinaIngreso = "fe80::7cea:e2a4:d0f7:6adb%5"
                                    });
                                    return await CreateCliente(usuario.Token).ExecuteAsync<TokenData>(request);
                                });

                                if (newAccessToken.StatusCode == HttpStatusCode.OK && newAccessToken.Data != null)
                                {
                                    usuario.Token = newAccessToken.Data;
                                    context["access_token"] = usuario.Token.AccessToken;
                                    context["refresh_token"] = usuario.Token.RefreshToken;
                                }
                            }
                        }
                    });
            }

            RestResponse<T> response;

            var client = CreateCliente(usuario.Token);

            if (unauthorizedPolicy != null)
            {
                var policyResult = await unauthorizedPolicy
                    .WrapAsync(timeoutPolicy)
                    .ExecuteAndCaptureAsync(() => client.ExecuteAsync(req, cancellationToken))
                    .ConfigureAwait(false);
                response = policyResult.Outcome == OutcomeType.Successful
                    ? client.Deserialize<T>(policyResult.Result)
                    : new RestResponse<T>(req) { ErrorException = policyResult.FinalException };
            }
            else
            {
                var policyResult = await timeoutPolicy
                    .ExecuteAndCaptureAsync(() => client.ExecuteAsync(req, cancellationToken))
                    .ConfigureAwait(false);
                response = policyResult.Outcome == OutcomeType.Successful
                    ? client.Deserialize<T>(policyResult.Result)
                    : new RestResponse<T>(req) { ErrorException = policyResult.FinalException };
            }

            return response;
        }

        private RestClient CreateCliente(TokenData tokenData)
        {
            var clientOptions = new RestClientOptions(_authInfo.BaseUrl)
            {
                //ClientCertificates = configuration.ClientCertificates,
                MaxTimeout = 100000,
                //Proxy = configuration.Proxy,
                //UserAgent = configuration.UserAgent,
                //UseDefaultCredentials = configuration.UseDefaultCredentials,
                RemoteCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true
            };

            if (tokenData != null)
                clientOptions.Authenticator = new JwtAuthenticator(tokenData.AccessToken);

            RestClient client = new(clientOptions);

            return client;
        }

        private JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            }
        };

        private bool CaducoAutorizacion(RestResponse ex) => ex.StatusCode == HttpStatusCode.Unauthorized;
    }

}

