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
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using Microsoft.Extensions.Configuration;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesarPagoHandler : IRequestHandler<ProcesarPagoME, AfectacionMS>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IFBSCorresponsalesApi _financialApi;
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
            IFBSCorresponsalesApi financialApi,
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
            _financialApi = financialApi;
            _mapper = mapper;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _apiKeyGenerator = apiKeyGenerator;
            _configuracion = configurarion;
        }

        public async Task<AfectacionMS> Handle(ProcesarPagoME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            request.Valor = DeterminarValorAPagar(request);

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

            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            DevuelveCuentaME cuentaAsociada = new DevuelveCuentaME() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _financialApi.Cuentas.DevuelveCuentaWithHttpMessagesAsync(cuentaAsociada, customHeaders);
            var saldoCuenta = respuestaCuentaAsociada.Body.Saldo.Value;           

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

            var transaccionesRepuestas = await _repositorioTransaccion.TransaccionesRepuestas(agente.Id.ToString());
            var transaccionesProcesadas = await _repositorioTransaccion.TransaccionesProcesadas(agente.Id.ToString());

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            var enReposicion = transaccionesRepuestas == transaccionesProcesadas;

            saldoCuenta = (saldoCuenta == 0 && transacciones.Count() == 0) || enReposicion ? jsonNegocio.Limites.SaldoMaximoCuentaAsociada.Value : saldoCuenta;

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
                SecuencialCuenta = request.SecuencialCuentaCliente.ToString(),
                Valor = request.Valor,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + request.Valor + comision.Agente.Value + comision.AdministracionCanal.Value + comision.Cooperativa.Value + request.Comision.Value,
                Tipo = IdTipoAccion,
                EstaActivo = true,
                Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
            };
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", Valor = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", Valor = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", Valor = comision.Cooperativa });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Pago Agil", Valor = request.Comision });
            var modelo = new AfectacionME()
            {
                CodigoUsuario = agente.Usuario.UserName,
                JsonComision = JsonConvert.SerializeObject(arregloComisiones),
                SecuencialCuentaCorresponsal = cuenta != null ? int.Parse(cuenta.SecuencialCuenta) : 0,
                SecuencialServicio = request.SecuencialServicio,
                SecuencialRequerimientoConsulta = request.SecuencialRequerimientoConsulta,
                Campos = request.Campos,
                CorreoCliente = request.CorreoCliente
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(modelo),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });            

            var respuestaHttp = await _financialApi.PagoServiciosPagoAgil.AfectacionMethodWithHttpMessagesAsync(modelo, customHeaders);
            var respuesta = respuestaHttp.Body;

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = respuesta.SaldoCuentaCorresponsal.Value;
            await _repositorioTransaccion.Update(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return respuesta;
        }

        private double DeterminarValorAPagar(ProcesarPagoME request)
        {

            if (request.Campos.Count == 0)
            {
                throw new ExcepcionApp("No se ha especificado el campo pago.");
            }

            double ValorAPagar = double.Parse(request.Campos[0].Valor);

            if (request.Campos.Count > 1)
            {
                throw new ExcepcionApp("Ambiguedad en los campos pago para este producto.");
            }
        
            return ValorAPagar;
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
