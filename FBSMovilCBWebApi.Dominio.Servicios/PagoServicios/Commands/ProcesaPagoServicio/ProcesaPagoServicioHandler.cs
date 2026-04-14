using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using Corresponsales.Query.Api;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio").Valor;
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
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).CobroServicios.Comisiones),                                                         
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
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });
            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).CobroServicios.Comisiones;
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", ValorComision = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", ValorComision = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", ValorComision = comision.Cooperativa });

            var modelo = new ProcesaPagoServicioRequest()
            {
                Servicio = request.Request.Servicio,
                Recibos = request.Request.Recibos,
                CamposAdicionales = request.Request.CamposAdicionales,
                IdUnidad = request.Request.IdUnidad,
                ProveedorServicio = request.Request.ProveedorServicio,
                TitularCuenta = request.Request.TitularCuenta,
                IdentificacionTitular = request.Request.IdentificacionTitular,                
                SecuencialCuentaDebito = cuenta != null ? int.Parse(cuenta.SecuencialCuenta) : 0,
                JsonComision = JsonConvert.SerializeObject(arregloComisiones),
                CodigoUsuario = agente.Usuario.UserName,                
                Concepto = request.Request.Concepto,                                
                ValorAfectado = request.Request.ValorAfectado,                
                Referencia = request.Request.Referencia,
                IdTransaccion = await generarNumeroDocumentoAsync(),
            };
            var JsonModelo = JsonConvert.SerializeObject(modelo);
           // Llamar al API
           var response = await _pagoApi.ProcesaPagoServicioAsync(modelo);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(modelo),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            transaccion.Estado = new FBS.DAL.Nomenclador.Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = response.SaldoCuentaCorresponsal;
            await _repositorioTransaccion.Update(transaccion);            

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
            if (!jsonNegocio.CobroServicios.Activo.Value)
            {
                throw new ExcepcionApp("Usted no posee acceso para ejecutar esta operación. En caso de requerir acceso a esta funcionalidad comunicarse con su supervisor.");
            }

            if (cantidadTransaccionesDiarias + 1 > jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para su corresponsal solidario que es: {jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (cantidadTransaccionesDiariasTipo + 1 > jsonNegocio.CobroServicios.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para este tipo de transacción que es: {jsonNegocio.CobroServicios.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (Valor > jsonNegocio.CobroServicios.Limites.MontoMaximoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor excede el monto máximo permitido para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es: {jsonNegocio.CobroServicios.Limites.MontoMaximoPorTransaccion} PEN.");
            }

            if (Valor < jsonNegocio.CobroServicios.Limites.MontoMinimoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor no alcanza el monto el mínimo definido para este tipo de transacción. Su monto mínimo permitido para este tipo de transacción es de: {jsonNegocio.CobroServicios.Limites.MontoMinimoPorTransaccion} PEN.");
            }

            if (montoTransaccionesDiarias + Valor > jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la transacción porque excedería el monto máximo diario permitido para todas las operaciones en {(jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesDiarias + Valor)) * -1} soles para su corresponsal solidario. Su monto máximo diario para todas las transacciones es de {jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }

            if (montoTransaccionesTipoDiarias + Valor > jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el monto máximo diario en {(jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesTipoDiarias + Valor)) * -1} para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es de {jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }            

            //if (cuentaAsociada && saldoCuenta - Valor <= 0)
            //{
            //    throw new ExcepcionApp("No puede realizar la operación porque no posee saldo disponible en la cuenta");
            //}
        }

        private async Task<string> generarNumeroDocumentoAsync()
        {
            //string documento = "0000000000";
            //var cantidadTransaccionesPago = await _repositorioTransaccion.GetCountTipoPago();
            //var secuencial = cantidadTransaccionesPago + 1;
            //var longSecuencial = secuencial >= 1000000 ? 7 : 6;
            //var secuencialFormateado = secuencial.ToString().PadLeft(longSecuencial, '0');
            //documento = DateTime.UtcNow.ToString("yyyy") + secuencialFormateado;
            var documento = Guid.NewGuid().ToString();
            return documento;
        }

    }
}