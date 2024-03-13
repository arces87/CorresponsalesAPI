using Microsoft.AspNetCore.Http;
using Polly.Retry;
using Polly;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.Threading.Tasks;
using System.Threading;

namespace Corresponsales.AccesoFinancial.Api
{
    public class FinancialRequestInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthInfo _authInfo;

        public FinancialRequestInfo(IHttpContextAccessor httpContextAccessor, AuthInfo authInfo)
        {
            _httpContextAccessor = httpContextAccessor;
            _authInfo = authInfo;
        }

        private string GetUserClient()
            => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Client"].ToString();

        private string GetCodigoUsuario()
             => _httpContextAccessor.HttpContext?.Request.Headers["X-CodigoUsuario"].ToString();

        private string GetClave()
             => _httpContextAccessor.HttpContext?.Request.Headers["X-Clave"].ToString();

        private User GetTokenData()
        {
            var userAgent = GetUserClient();

            if (userAgent == ClienteConstants.Web)
                return _authInfo.Users[_authInfo.UsuarioAdmin];

            var codigoUsuario = GetCodigoUsuario();

            var result = _authInfo.Users.TryGetValue(codigoUsuario, out User user);

            if(!result)
            {
                user = new User { Usuario = codigoUsuario, Password = GetClave() };
                _authInfo.Users.Add(codigoUsuario, user);
            }
            return user;
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
                            maquina = "BANCA-VIRTUAL",
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
                                        maquina = "BANCA-VIRTUAL",
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

