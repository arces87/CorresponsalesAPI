using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFacilito;
using ServiciosFacilito.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands
{
    public class ProcesarPagoHandler : IRequestHandler<ProcesarPagoME, PagoResponse>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IFBSFacilitoAPI _facilitoApi;
        private readonly IMapper _mapper;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;

        public ProcesarPagoHandler(IMediator mediador, IJsonConfiguracion jsonConfiguracion, IFBSFacilitoAPI facilitoApi,
            IMapper mapper, IRepositorioTransaccion repositorioTransaccion, IRepositorioAgente repositorioAgente, IRepositorioCuenta repositorioCuenta,
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _facilitoApi = facilitoApi;
            _mapper = mapper;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
        }

        public async Task<PagoResponse> Handle(ProcesarPagoME request, CancellationToken cancellationToken)
        {
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio").Valor;
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());
            var saldoCuenta = await _repositorioTransaccion.GetSaldoCuenta(agente.Id.ToString());
            var transacciones = await _repositorioTransaccion.GetForTipo(agente.Id.ToString(), IdTipoAccion);
            transacciones = transacciones.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            saldoCuenta = saldoCuenta == 0 && transacciones.Count() == 0 ? jsonNegocio.Limites.SaldoMaximoCuentaAsociada.Value : saldoCuenta;
            if (!jsonNegocio.Deposito.Activo.Value)
            {
                throw new Exception("Usted no tiene acceso para realizar este tipo de operación");
            }
            if (request.Valor > jsonNegocio.Deposito.Limites.MontoMaximoPorTransaccion)
            {
                throw new Exception("No puede realizar esta operación porque excede el monto máximo definido para este tipo de operación");
            }
            if (request.Valor < jsonNegocio.Deposito.Limites.MontoMinimoPorTransaccion)
            {
                throw new Exception("No puede realizar esta operación porque no alcanza el monto mínimo definido para este tipo de operación");
            }
            if (saldoActual > jsonNegocio.Deposito.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new Exception("No puede realizar esta operación porque excede el monto máximo diario definido para este tipo de operación");
            }
            if (transacciones.Count() > jsonNegocio.Deposito.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new Exception("No puede realizar esta operación porque excede el número máximo diario definido para este tipo de operación");
            }
            if (saldoActual > jsonNegocio.Limites.SaldoMaximoAgente.Value)
            {
                throw new Exception("No puede realizar esta operación porque excede el Saldo Máximo establecido para mantener en caja");
            }
            if (cuenta != null && saldoCuenta - request.Valor < 0)
            {
                throw new Exception("No puede realizar esta operación porque no posee saldo en la cuenta");
            }
            var transaccion = new Transaccion()
            {
                CanalId = _jsonConfiguracion.IdCanal,
                Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).CobroServicios.Comisiones),
                Agente = agente,
                Descripcion = request.Descripcion,
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.IdentificacionCliente,
                NombreCliente = request.NombreCliente,
                NumeroCuenta = "",
                Valor = request.Valor,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + request.Valor,
                SaldoCuenta = cuenta != null ? saldoCuenta - request.Valor : 0,
                Tipo = IdTipoAccion,
                EstaActivo = true
            };
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });
            var modelo = _mapper.Map<PagoRequest>(request);
            var respuesta = await _facilitoApi.ServicioPagoWithHttpMessagesAsync(modelo);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            await _repositorioTransaccion.Update(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return respuesta.Body;
        }
    }
}
