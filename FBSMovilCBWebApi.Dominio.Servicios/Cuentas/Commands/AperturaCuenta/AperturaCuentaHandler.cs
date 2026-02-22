using AutoMapper;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands.AperturaCuenta
{
    public class AperturaCuentaHandler : IRequestHandler<AperturaCuentaME, AperturaCuentaResponse>
    {
        private readonly ICaptacionesVistaApi _captacionesVistaApi;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;
        private readonly Corresponsales.Query.Api.ICaptacionesVistaApi _cuentaApi;
        private readonly byte[] _llave;

        public AperturaCuentaHandler(
            ICaptacionesVistaApi captacionesVistaApi,
            IMapper mapper,
            IMediator mediador,
            IJsonConfiguracion jsonConfiguracion,
            IHttpContextAccessor httpContextAccesor,
            IApiKeyGenerator apiKeyGenerator,
            IHttpContextAccessor httpContext,
            IRepositorioTransaccion repositorioTransaccion,
            IRepositorioAgente repositorioAgente,
            Corresponsales.Query.Api.ICaptacionesVistaApi cuentaApi,
            IRepositorioCuenta repositorioCuenta)
        {
            _captacionesVistaApi = captacionesVistaApi;
            _mapper = mapper;
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _httpContextAccesor = httpContextAccesor;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _cuentaApi = cuentaApi;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<AperturaCuentaResponse> Handle(AperturaCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAperturaCuenta")?.Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAperturaCuenta")?.Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAperturaCuenta").Valor;

            DevuelveCuentaRequest cuentaAsociada = new DevuelveCuentaRequest() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _cuentaApi.DevuelveCuentaAsync(cuentaAsociada);
            var saldoCuenta = respuestaCuentaAsociada.DisponibleParaTransaccion;

            var transaccion = new Transaccion()
            {
                CanalId = _jsonConfiguracion.IdCanal,
                Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Deposito.Comisiones),
                Agente = agente,
                Descripcion = "Apertura cuenta",
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.IdentificacionCliente,
                NombreCliente = request.NombreCliente,
                SecuencialCuenta = request.SecuencialCuentaSocio.ToString(),
                Valor = (double)request.ValorApertura,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + (double)request.ValorApertura,
                Tipo = IdTipoAccion,
                EstaActivo = true,
                Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
            };
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);

            var mapResult = _mapper.Map<AperturaCuentaRequest>(request);
            mapResult.SecuencialCuentaCorresponsal = int.Parse(cuenta.SecuencialCuenta);
            var respuesta = await _captacionesVistaApi.AperturaCuentaAsync(mapResult, cancellationToken: cancellationToken);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAperturaCuenta")?.Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = saldoCuenta - (double)request.ValorApertura;
            await _repositorioTransaccion.Update(transaccion);

            return respuesta;
        }
    }
}
