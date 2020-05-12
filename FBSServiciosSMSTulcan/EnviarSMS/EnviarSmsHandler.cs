using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSServiciosSMSTulcan.EnviarSMS
{
    public class EnviarSmsHandler : IRequestHandler<EnviarSmsME, EnviarSmsMS>, IRequestHandler<EnviarSmsLoteME, EnviarSmsLoteMS>
    {
        public async Task<EnviarSmsMS> Handle(EnviarSmsME request, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"username:password");

            var url = "https://private.savia-digital.com/Messaging.aspx/SendSMS";

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", byteArray.ToString());
            client.DefaultRequestHeaders.Add("Content-type", "application/json");
            client.DefaultRequestHeaders.Add("Username", "cooptulcan");
            client.DefaultRequestHeaders.Add("Password", "S@Yveloz");
            var content = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            var result = await response.Content?.ReadAsStringAsync();
             
            EnviarSmsMS respuesta = null;

            if (result != null)
            {
                dynamic dataJson = JsonConvert.DeserializeObject(result);
                respuesta = dataJson.d.ToObject<EnviarSmsMS>();
            }

            return respuesta;
        }

        public async Task<EnviarSmsLoteMS> Handle(EnviarSmsLoteME request, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"username:password");

            var url = "https://private.savia-digital.com/Messaging.aspx/SendBulkSMS";

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", byteArray.ToString());
            client.DefaultRequestHeaders.Add("Content-type", "application/json");
            client.DefaultRequestHeaders.Add("Username", "cooptulcan");
            client.DefaultRequestHeaders.Add("Password", "S@Yveloz");
            var content = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            var result = await response.Content?.ReadAsStringAsync();

            EnviarSmsLoteMS respuesta = null;

            if (result != null)
            {
                dynamic dataJson = JsonConvert.DeserializeObject(result);
                respuesta = dataJson.d.ToObject<EnviarSmsLoteMS>();
            }

            return respuesta;
        }
    }
}
