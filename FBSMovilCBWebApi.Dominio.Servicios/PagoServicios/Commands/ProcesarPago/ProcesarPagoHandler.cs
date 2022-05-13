using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using Microsoft.Extensions.Configuration;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesarPagoHandler : IRequestHandler<ProcesarPagoME, PagoFacilitoMSL>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IPagoServiciosFacilitoApi _pago;
        private readonly ICuentasApi _cuentaApi;
        private readonly IMapper _mapper;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;
        private readonly byte[] _llave;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IConfiguration _configuracion;

        public ProcesarPagoHandler(
            IMediator mediador, 
            IJsonConfiguracion jsonConfiguracion,
            IPagoServiciosFacilitoApi pago,
            ICuentasApi cuentaApi,
            IMapper mapper, 
            IRepositorioTransaccion repositorioTransaccion, 
            IRepositorioAgente repositorioAgente, 
            IRepositorioCuenta repositorioCuenta,
            IHttpContextAccessor httpContext,
            IApiKeyGenerator apiKeyGenerator,
            IConfiguration configurarion)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _pago = pago;
            _cuentaApi = cuentaApi;            
            _mapper = mapper;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _apiKeyGenerator = apiKeyGenerator;
            _configuracion = configurarion;
        }

        public async Task<PagoFacilitoMSL> Handle(ProcesarPagoME request, CancellationToken cancellationToken)
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
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());

            ValidarCuentaAsocida(cuenta);

            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            DevuelveCuentaME cuentaAsociada = new Org.OpenAPITools.Model.DevuelveCuentaME() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _cuentaApi.CuentasDevuelveCuentaAsync(cuentaAsociada);
            var saldoCuenta = respuestaCuentaAsociada.DisponibleParaTransaccion;         

            var transacciones = await _repositorioTransaccion.GetForAgente(agente.Id.ToString());
            var transaccionesDiarias = transacciones.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);
            var transaccionesTipo = transacciones.Where(t => t.Tipo == IdTipoAccion);
            var transaccionesDiariasTipo = transaccionesTipo.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);

            var cantidadTransacciones = transacciones.Count();
            var cantidadTransaccionesTipo = transaccionesTipo.Count();
            var cantidadTransaccionesDiarias = transaccionesDiarias.Count();
            var cantidadTransaccionesDiariasTipo = transaccionesDiariasTipo.Count();

            var montoTransacciones = transacciones.Aggregate(0.0, (result, t) => result + t.Valor);
            var montoTransaccionesTipo = transaccionesTipo.Aggregate(0.0, (result, t) => result + t.Valor);
            var montoTransaccionesDiarias = transaccionesDiarias.Aggregate(0.0, (result, t) => result + t.Valor);
            var montoTransaccionesTipoDiarias = transaccionesDiariasTipo.Aggregate(0.0, (result, t) => result + t.Valor);            

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
                        
            ValidarTransaccion(
                request.Valor,
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

            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).CobroServicios.Comisiones;
            var comisionPago = _mapper.Map<ComisionPago>(comision);
            comisionPago.Facilito = request.Comision;
            var comisiones = JsonConvert.SerializeObject(comisionPago);

            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", Valor = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", Valor = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", Valor = comision.Cooperativa });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Facilito", Valor = request.Comision });
            var transaccionesNuevas = new List<Transaccion>();

            PreprarTransacciones(request, IdTipoAccion, agente, cuenta, saldoActual, comision, comisiones, transaccionesNuevas);

            var respuesta = new PagoFacilitoMSL()
            {
                PagosFacilito = new List<PagoFacilitoMS>()
            };

            int? secuencialCuenta = cuenta == null ? 0 : Convert.ToInt32(cuenta.SecuencialCuenta);

            await EjecutarPagos(request, IdTipoAccion, arregloComisiones, transaccionesNuevas, respuesta, secuencialCuenta);

            return respuesta;
        }

        private async Task EjecutarPagos(
            ProcesarPagoME request, 
            string IdTipoAccion, 
            List<ComisionFinancial> arregloComisiones, 
            List<Transaccion> transaccionesNuevas, 
            PagoFacilitoMSL respuesta, 
            int? secuencialCuenta)
        {
            for (int indice = 0; indice < transaccionesNuevas.Count; indice++)
            {

                var transaccion = transaccionesNuevas[indice];

                var pago = new PagoFacilitoME
                {
                    CodigoPagarPensionesAlimenticiaEmpresa = request.CodigoPagarPensionesAlimenticiaEmpresa,
                    CodigoUsuarioBanca = request.Usuario,
                    ComisionRubro = (bool)request.ComisionRubro,
                    EsUnSoloCobroComision = true,
                    IdProducto = (Guid)request.IdProducto,
                    Identificacion = request.Identificacion,
                    JsonComision = JsonConvert.SerializeObject(arregloComisiones),
                    NumeroCuotasPensionesAlimenticiaPersona = (int)request.NumeroCuotasPensionesAlimenticiaPersona,
                    Referencia = request.Referencia,
                    Rubros = (List<RubroME>)request.Rubros,
                    SecuencialCuentaCorresponsal = (int)secuencialCuenta,
                    SecuencialResultadoTransaccion = (int)request.SecuencialResultadoTransaccion,
                    Valor = transaccion.Valor,
                    ValorTonelaje = request.ValorTonelaje
                };

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(pago),
                    IdTipoAccion = IdTipoAccion,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                });

                var respuestaHttp = await _pago.PagoServiciosFacilitoPagoFacilitoAsync(pago);

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(respuestaHttp),
                    IdTipoAccion = IdTipoAccion,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
                });

                var respuestaFacilito = respuestaHttp;
                var pagoFacilito = respuestaFacilito.PagosFacilito[0];

                transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
                transaccion.SaldoCuenta = respuestaFacilito.SaldoCuentaCorresponsal;
                await _repositorioTransaccion.Add(transaccion);

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(pagoFacilito),
                    IdTipoAccion = IdTipoAccion,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });

                respuesta.PagosFacilito.Add(pagoFacilito);
                respuesta.SaldoCuentaCorresponsal = respuestaFacilito.SaldoCuentaCorresponsal;
            }
        }

        private void PreprarTransacciones(
            ProcesarPagoME request, 
            string IdTipoAccion,
            FBSConsolaCBWebApi.DAL.Corresponsales.Agente agente, 
            Cuenta cuenta, 
            double saldoActual, 
            FBS.Identidad.Dominio.Servicios.Canales.Queries.ComisionOperacion comision, 
            string comisiones, 
            List<Transaccion> transaccionesNuevas)
        {
            if ((bool)request.ComisionRubro)
            {
                foreach (var rubro in request.Rubros)
                {
                    var transaccion = new Transaccion()
                    {
                        CanalId = _jsonConfiguracion.IdCanal,
                        Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                        Comisiones = comisiones,
                        Agente = agente,
                        Descripcion = request.Descripcion,
                        FechaDispositivo = DateTime.Now,
                        FechaSistema = DateTime.Now,
                        HoraDispositivo = DateTime.Now.TimeOfDay,
                        IdentificacionCliente = request.Identificacion,
                        NombreCliente = request.NombreCliente,
                        SecuencialCuenta = cuenta.SecuencialCuenta.ToString(),
                        Valor = (double)rubro.ValorPagado,
                        JsonDatos = JsonConvert.SerializeObject(request),
                        SaldoDisponible = saldoActual + request.Valor + comision.Agente.Value + comision.AdministracionCanal.Value + comision.Cooperativa.Value + request.Comision.Value,
                        Tipo = IdTipoAccion,
                        EstaActivo = true,
                        Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
                    };
                    transaccionesNuevas.Add(transaccion);
                }
            }
            else
            {

                var transaccion = new Transaccion()
                {
                    CanalId = _jsonConfiguracion.IdCanal,
                    Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                    Comisiones = comisiones,
                    Agente = agente,
                    Descripcion = request.Descripcion,
                    FechaDispositivo = DateTime.Now,
                    FechaSistema = DateTime.Now,
                    HoraDispositivo = DateTime.Now.TimeOfDay,
                    IdentificacionCliente = request.Identificacion,
                    NombreCliente = request.NombreCliente,
                    SecuencialCuenta = cuenta.SecuencialCuenta.ToString(),
                    Valor = request.Valor,
                    JsonDatos = JsonConvert.SerializeObject(request),
                    SaldoDisponible = saldoActual + request.Valor + comision.Agente.Value + comision.AdministracionCanal.Value + comision.Cooperativa.Value + request.Comision.Value,
                    Tipo = IdTipoAccion,
                    EstaActivo = true,
                    Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
                };
                transaccionesNuevas.Add(transaccion);
            }
        }

        private static void ValidarCuentaAsocida(Cuenta cuenta)
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
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para este tipo de transacción que es: { jsonNegocio.CobroServicios.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (Valor > jsonNegocio.CobroServicios.Limites.MontoMaximoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor excede el monto máximo permitido para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es: {jsonNegocio.CobroServicios.Limites.MontoMaximoPorTransaccion} USD.");
            }

            if (Valor < jsonNegocio.CobroServicios.Limites.MontoMinimoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor no alcanza el monto el mínimo definido para este tipo de transacción. Su monto mínimo permitido para este tipo de transacción es de: {jsonNegocio.CobroServicios.Limites.MontoMinimoPorTransaccion} USD.");
            }

            if (montoTransaccionesDiarias + Valor > jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la transacción porque excedería el monto máximo diario permitido para todas las operaciones en {(jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesDiarias + Valor)) * -1} dolares para su corresponsal solidario. Su monto máximo diario para todas las transacciones es de {jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones} USD.");
            }

            if (montoTransaccionesTipoDiarias + Valor > jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el monto máximo diario en {(jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesTipoDiarias + Valor)) * -1} para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es de {jsonNegocio.CobroServicios.Limites.MontoMaximoDiarioDeTransacciones} USD.");
            }

            if (cuentaAsociada && saldoActual + Valor > jsonNegocio.Limites.SaldoMaximoAgente.Value)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el saldo máximo de la caja en: { (jsonNegocio.Limites.SaldoMaximoAgente.Value - (saldoActual + Valor)) * -1} . Su saldo máximo en caja permitido es {jsonNegocio.Limites.SaldoMaximoAgente.Value} USD");
            }

            if (cuentaAsociada && saldoCuenta - Valor <= 0)
            {
                throw new ExcepcionApp("No puede realizar la operación porque no posee saldo disponible en la cuenta");
            }
        }

        
    }
}
