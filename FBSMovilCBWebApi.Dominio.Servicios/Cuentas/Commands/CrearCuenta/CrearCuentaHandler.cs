using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class CrearCuentaHandler : IRequestHandler<CrearCuentaME, CreaCuentaMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public CrearCuentaHandler(IFBSCorresponsalesApi financialApi, IMapper mapper,
            IMediator mediador, IJsonConfiguracion jsonConfiguracion, IHttpContextAccessor httpContextAccesor)
        {
            _financialApi = financialApi;
            _mapper = mapper;
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _httpContextAccesor = httpContextAccesor;
        }

        public async Task<CreaCuentaMSL> Handle(CrearCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });
            var respuesta = await _financialApi.Cuentas.CreaCuentaWithHttpMessagesAsync(_mapper.Map<CreaCuentaME>(request));
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return respuesta.Body;
        }
    }
}
