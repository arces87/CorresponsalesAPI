using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;


namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarDepositoHandler : IRequestHandler<ProcesarDepositoME, AfectacionAUnCorresponsalDepositoMS>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IAfectacionApi _afectacionApi;
        private readonly ICuentasApi _cuentaApi;
        private readonly IMapper _mapper;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly byte[] _llave;


        public ProcesarDepositoHandler(
            IMediator mediador, 
            IJsonConfiguracion jsonConfiguracion,
            IAfectacionApi afectacionApi,
            ICuentasApi cuentaApi,
            IMapper mapper, 
            IRepositorioTransaccion repositorioTransaccion, 
            IRepositorioAgente repositorioAgente, 
            IRepositorioCuenta repositorioCuenta,
            IHttpContextAccessor httpContext,
            IApiKeyGenerator apiKeyGenerator)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _afectacionApi = afectacionApi;
            _cuentaApi = cuentaApi;
            _mapper = mapper;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<AfectacionAUnCorresponsalDepositoMS> Handle(ProcesarDepositoME request, CancellationToken cancellationToken)
        {
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor;
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());

            ValidarCuentaAsocida(cuenta);

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

            var transaccion = new Transaccion()
            {
                CanalId = _jsonConfiguracion.IdCanal,
                Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Deposito.Comisiones),
                Agente = agente,
                Descripcion = request.Descripcion,
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.IdentificacionCliente,
                NombreCliente = request.NombreCliente,
                SecuencialCuenta = request.SecuencialCuenta.ToString(),
                Valor = request.Valor,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + request.Valor,
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
            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Deposito.Comisiones;
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", ValorComision = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", ValorComision = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", ValorComision = comision.Cooperativa });

            var modelo = new AfectacionAUnCorresponsalME()
            {
                TipoTransaccion = "NCCliente",
                CodigoUsuario = agente.Usuario.UserName,
                JsonComision = JsonConvert.SerializeObject(arregloComisiones),
                SecuencialCuentaCorresponsal = cuenta != null ? int.Parse(cuenta.SecuencialCuenta) : 0,
                SecuencialCuentaSocio = request.SecuencialCuenta,
                ValorAfectado = request.Valor,
                EsUnSoloCobroComision = true,
                SecuencialTipoIdentificacionCliente = request.TipoIdentificacionCliente,
                IdentificacionCliente = request.IdentificacionCliente
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(modelo),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var respuesta = await _afectacionApi.AfectacionAfectacionAUnCorresponsalAsync(modelo);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = respuesta.SaldoCuentaCorresponsal;
            await _repositorioTransaccion.Update(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });

            var afectacionAUnCorresponsalDepositoMS = new AfectacionAUnCorresponsalDepositoMS
            {
                FechaTransaccion = respuesta.FechaTransaccion,
                NumeroCuenta = respuesta.NumeroCuenta,
                NumeroTransaccion = respuesta.NumeroTransaccion,
                SaldoCuentaCorresponsal = respuesta.SaldoCuentaCorresponsal,
                Valor = respuesta.Valor
            };
            return afectacionAUnCorresponsalDepositoMS;
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
            if (!jsonNegocio.Deposito.Activo.Value)
            {
                throw new ExcepcionApp("Usted no posee acceso para ejecutar esta operación. En caso de requerir acceso a esta funcionalidad comunicarse con su supervisor.");
            }
            
            if (cantidadTransaccionesDiarias + 1 > jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para su corresponsal solidario que es: {jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (cantidadTransaccionesDiariasTipo + 1 > jsonNegocio.Deposito.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para este tipo de transacción que es: { jsonNegocio.Deposito.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (Valor > jsonNegocio.Deposito.Limites.MontoMaximoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor excede el monto máximo permitido para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es: {jsonNegocio.Deposito.Limites.MontoMaximoPorTransaccion} USD.");
            }

            if (Valor < jsonNegocio.Deposito.Limites.MontoMinimoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor no alcanza el monto el mínimo definido para este tipo de transacción. Su monto mínimo permitido para este tipo de transacción es de: {jsonNegocio.Deposito.Limites.MontoMinimoPorTransaccion} USD.");
            }

            if (montoTransaccionesDiarias + Valor > jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la transacción porque excedería el monto máximo diario permitido para todas las operaciones en {(jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesDiarias + Valor)) * -1} dolares para su corresponsal solidario. Su monto máximo diario para todas las transacciones es de {jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones} USD.");
            }

            if (montoTransaccionesTipoDiarias + Valor > jsonNegocio.Deposito.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el monto máximo diario en {(jsonNegocio.Deposito.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesTipoDiarias + Valor)) * -1} para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es de {jsonNegocio.Deposito.Limites.MontoMaximoDiarioDeTransacciones} USD.");
            }

            if (cuentaAsociada && saldoActual + Valor >  jsonNegocio.Limites.SaldoMaximoAgente.Value)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el saldo máximo de la caja en: { (jsonNegocio.Limites.SaldoMaximoAgente.Value - (saldoActual + Valor)) * -1} . Su saldo máximo en caja permitido es {jsonNegocio.Limites.SaldoMaximoAgente.Value} USD");
            }

            if (cuentaAsociada && (saldoCuenta - Valor <= 0))
            {
                throw new ExcepcionApp("No puede realizar la operación porque no posee saldo disponible en la cuenta. Saldo disponible para transacción: "+saldoCuenta+". Monto: "+Valor);
            }
        }
    }
}
