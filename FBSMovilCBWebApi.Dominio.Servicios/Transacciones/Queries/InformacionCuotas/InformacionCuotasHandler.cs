using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.InformacionCuotas
{
    public class InformacionCuotasHandler : IRequestHandler<InformacionCuotasME, NumeroCuotasValorAdelantoListaResponse>
    {
        private readonly ICarteraApi _carteraApi;

        public InformacionCuotasHandler(ICarteraApi carteraApi)
        {
            _carteraApi = carteraApi;
        }

        public async Task<NumeroCuotasValorAdelantoListaResponse> Handle(InformacionCuotasME request, CancellationToken cancellationToken)
        {
            var apiResponse = await _carteraApi.DevuelveInformacionCuotasValorACobrarWithHttpInfoAsync(request, 0, cancellationToken);
            var respuesta = apiResponse.Data;

            // Si la respuesta tiene datos en la lista, devolver tal cual
            if (respuesta?.ListaCuotasValorAdelanto != null)
                return respuesta;

            // Si la lista viene null pero hay contenido, el backend puede estar devolviendo:
            // 1) Un array en la raíz: [ { "numeroCuota": 1, ... }, ... ]
            // 2) Un objeto con otra clave (ej. "listCuotasValorAdelanto")
            var rawContent = apiResponse.RawContent;
            if (string.IsNullOrWhiteSpace(rawContent))
                return respuesta ?? new NumeroCuotasValorAdelantoListaResponse(new List<CuotaValorAdelantoResponse>());

            rawContent = rawContent.Trim();
            try
            {
                // Caso: respuesta es un array en la raíz
                if (rawContent.StartsWith("["))
                {
                    var lista = JsonConvert.DeserializeObject<List<CuotaValorAdelantoResponse>>(rawContent);
                    return new NumeroCuotasValorAdelantoListaResponse(lista ?? new List<CuotaValorAdelantoResponse>());
                }

                // Caso: objeto con clave distinta (ej. listCuotasValorAdelanto)
                var jobj = JObject.Parse(rawContent);
                var token = jobj["listaCuotasValorAdelanto"] ?? jobj["listCuotasValorAdelanto"];
                if (token != null && token.Type == JTokenType.Array)
                {
                    var lista = token.ToObject<List<CuotaValorAdelantoResponse>>();
                    return new NumeroCuotasValorAdelantoListaResponse(lista ?? new List<CuotaValorAdelantoResponse>());
                }
            }
            catch
            {
                // Si falla el fallback, devolver la respuesta original
            }

            return respuesta ?? new NumeroCuotasValorAdelantoListaResponse(new List<CuotaValorAdelantoResponse>());
        }
    }
}
