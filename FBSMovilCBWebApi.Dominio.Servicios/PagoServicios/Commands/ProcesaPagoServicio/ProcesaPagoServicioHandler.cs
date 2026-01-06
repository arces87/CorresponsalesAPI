using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Excepciones;
using Microsoft.Extensions.Configuration;
using Corresponsales.Query.Api;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesaPagoServicioHandler : IRequestHandler<ProcesaPagoServicioME, ProcesaPagoServicioResponse>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly Corresponsales.Command.Api.IPagoApi _pagoApi;
        private readonly Corresponsales.Query.Api.ICaptacionesVistaApi _cuentaApi;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;
        private readonly byte[] _llave;
        private readonly IConfiguration _configuracion;

        public ProcesaPagoServicioHandler(
            IMediator mediador,
            IJsonConfiguracion jsonConfiguracion,
            Corresponsales.Command.Api.IPagoApi pagoApi,
            Corresponsales.Query.Api.ICaptacionesVistaApi cuentaApi,
            IRepositorioTransaccion repositorioTransaccion,
            IRepositorioAgente repositorioAgente,
            IRepositorioCuenta repositorioCuenta,
            IHttpContextAccessor httpContext,
            IConfiguration configuracion)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _pagoApi = pagoApi;
            _cuentaApi = cuentaApi;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _configuracion = configuracion;
        }

        public async Task<ProcesaPagoServicioResponse> Handle(ProcesaPagoServicioME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdPagoServicio").Valor;
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request.Request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());

            ValidarCuentaAsocida(cuenta);

            var respuestaCuentaAsociada = await _cuentaApi.DevuelveCuentaAsync(new Corresponsales.Query.Model.DevuelveCuentaRequest() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) });
            var saldoCuenta = respuestaCuentaAsociada.DisponibleParaTransaccion;

            var transacciones = await _repositorioTransaccion.GetForAgente(agente.Id.ToString());
            var transaccionesDiarias = transacciones.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);
            var transaccionesTipo = transacciones.Where(t => t.Tipo == IdTipoAccion);
            var transaccionesDiariasTipo = transaccionesTipo.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);

            var cantidadTransacciones = transacciones.Count();
            var cantidadTransaccionesTipo = transaccionesTipo.Count();
            var cantidadTransaccionesDiarias = transaccionesDiarias.Count();
            var cantidadTransaccionesDiariasTipo = transaccionesDiariasTipo.Count();

            var montoTransacciones = transacciones.Aggregate(0.0, (result, t) => result + Math.Abs(t.Valor));
            var montoTransaccionesTipo = transaccionesTipo.Aggregate(0.0, (result, t) => result + Math.Abs(t.Valor));
            var montoTransaccionesDiarias = transaccionesDiarias.Aggregate(0.0, (result, t) => result + Math.Abs(t.Valor));
            var montoTransaccionesTipoDiarias = transaccionesDiariasTipo.Aggregate(0.0, (result, t) => result + Math.Abs(t.Valor));

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            ValidarTransaccion(
                request.Request.ValorAfectado,
                cuenta != null,
                saldoActual,
                saldoCuenta,
                cantidadTransacciones,
                cantidadTransaccionesTipo,
                cantidadTransaccionesDiarias,
                cantidadTransaccionesDiariasTipo,
                montoTransacciones,
                montoTransaccionesTipo,
                montoTransaccionesTipoDiarias,
                montoTransaccionesDiarias,
                jsonNegocio);

            // Preparar transacción
            var transaccion = new Transaccion()
            {
                CanalId = _jsonConfiguracion.IdCanal,
                Estado = new FBS.DAL.Nomenclador.Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                Comisiones = JsonConvert.SerializeObject(new { }), // Ajustar si hay comisiones
                Agente = agente,
                Descripcion = "Pago de Servicio",
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.Request.IdentificacionTitular,
                NombreCliente = request.Request.TitularCuenta,
                SecuencialCuenta = cuenta.SecuencialCuenta.ToString(),
                Valor = request.Request.ValorAfectado,
                JsonDatos = JsonConvert.SerializeObject(request.Request),
                SaldoDisponible = saldoActual + request.Request.ValorAfectado,
                Tipo = IdTipoAccion,
                EstaActivo = true,
                Criptografia = Encoding.UTF8.GetString(FBS.Identidad.Dominio.Servicios.Utilidad.Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request.Request), _llave, _llave))
            };

            // Llamar al API
            var response = await _pagoApi.ProcesaPagoServicioAsync(request.Request);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(response),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            transaccion.Estado = new FBS.DAL.Nomenclador.Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = response.SaldoCuentaCorresponsal;
            await _repositorioTransaccion.Add(transaccion);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(response),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });

            return response;
        }

        private static void ValidarCuentaAsocida(FBSConsolaCBWebApi.DAL.Corresponsales.Cuenta cuenta)
        {
            if (cuenta == null)
            {
                throw new ExcepcionApp("El corresponsal no tiene cuenta asociada.");
            }
        }

        private static void ValidarTransaccion(
           double Valor,
           bool cuentaAsociada,
           double saldoActual,
           double saldoCuenta,
           int cantidadTransacciones,
           int cantidadTransaccionesTipo,
           int cantidadTransaccionesDiarias,
           int cantidadTransaccionesDiariasTipo,
           double montoTransacciones,
           double montoTransaccionesTipo,
           double montoTransaccionesTipoDiarias,
           double montoTransaccionesDiarias,
           JsonNegocioMS jsonNegocio)
        {
            // Ajustar validaciones según límites de pago de servicios
            if (cantidadTransaccionesDiariasTipo + 1 > 100) // Ejemplo, ajustar
            {
                throw new ExcepcionApp("Límite de transacciones diarias alcanzado.");
            }

            if (Valor > 1000) // Ejemplo
            {
                throw new ExcepcionApp("Monto máximo excedido.");
            }

            if (cuentaAsociada && saldoCuenta - Valor <= 0)
            {
                throw new ExcepcionApp("Saldo insuficiente en la cuenta.");
            }
        }
    }
}