using AutoMapper;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Infraestructura.Excepciones;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class ProcesaPagoCuentaHandler : IRequestHandler<ProcesaPagoCuentaME, ProcesaPagoCuentaMS>
    {
        private readonly ICuentasPorCobrarApi _cuentaPorCobrarApi;
        private readonly IHttpContextAccessor _httpContextAccesor;        
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly Corresponsales.Query.Api.ICaptacionesVistaApi _cuentaApi;
        private readonly byte[] _llave;

        public ProcesaPagoCuentaHandler(
            ICuentasPorCobrarApi cuentaPorCobrarApi,             
            IMediator mediador, 
            IJsonConfiguracion jsonConfiguracion, 
            IHttpContextAccessor httpContextAccesor,
            IApiKeyGenerator apiKeyGenerator, 
            IRepositorioAgente repositorioAgente,
            IRepositorioCuenta repositorioCuenta,
            IRepositorioTransaccion repositorioTransaccion,
            Corresponsales.Query.Api.ICaptacionesVistaApi cuentaApi)
        {
            _cuentaPorCobrarApi = cuentaPorCobrarApi;            
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _httpContextAccesor = httpContextAccesor;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _repositorioTransaccion = repositorioTransaccion;
            _cuentaApi = cuentaApi;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<ProcesaPagoCuentaMS> Handle(ProcesaPagoCuentaME request, CancellationToken cancellationToken)
        {
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdObligacion").Valor;

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

            var agente = await _repositorioAgente.GetForId(_httpContextAccesor.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var saldoActual = await _repositorioTransaccion.GetSaldoActual(agente.Id.ToString());

            ValidarCuentaAsocida(cuenta);

            DevuelveCuentaRequest cuentaAsociada = new DevuelveCuentaRequest() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _cuentaApi.DevuelveCuentaAsync(cuentaAsociada);
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
               request.ValorAfectado,
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
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Obligaciones.Comisiones),
                Agente = agente,
                Descripcion = "PagoCuentaPorCobrar",
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.IdentificacionCliente,
                NombreCliente = request.NombreCliente,
                SecuencialCuenta = request.SecuencialCuenta.ToString(),
                Valor = request.ValorAfectado,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + request.ValorAfectado,
                Tipo = IdTipoAccion,
                EstaActivo = true,
                Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
            };
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);

            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Obligaciones.Comisiones;
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", ValorComision = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", ValorComision = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", ValorComision = comision.Cooperativa });

            var procesaPago = new ProcesaPagoCuentasPorCobrarRequest(
                request.IdentificacionCliente,
                "PagoCuentaPorCobrar",
                (List<RubroPorCobrarRequest>)request.CuentasPorCobrar,
                int.Parse(cuenta.SecuencialCuenta),
                request.ValorAfectado,
                JsonConvert.SerializeObject(arregloComisiones),
                request.Usuario
            );

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(procesaPago),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var respuesta = await _cuentaPorCobrarApi.ProcesaPagoCuentasPorCobrarAsync(procesaPago);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = respuesta?.SaldoCuentaCorresponsal ?? (saldoCuenta - request.ValorAfectado);
            await _repositorioTransaccion.Update(transaccion);

            var procesaPagoCuentaMS = new ProcesaPagoCuentaMS
            {
                PagoCuentaResponse = respuesta?.PagoCuentaResponse,
                Fecha = respuesta?.Fecha,
                SaldoCuentaCorresponsal = respuesta?.SaldoCuentaCorresponsal ?? (saldoCuenta - request.ValorAfectado)
            };

            return procesaPagoCuentaMS;
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
            if (!jsonNegocio.Obligaciones.Activo.Value)
            {
                throw new ExcepcionApp("Usted no posee acceso para ejecutar esta operación. En caso de requerir acceso a esta funcionalidad comunicarse con su supervisor.");
            }

            if (cantidadTransaccionesDiarias + 1 > jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para su corresponsal solidario que es: {jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (cantidadTransaccionesDiariasTipo + 1 > jsonNegocio.Obligaciones.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para este tipo de transacción que es: {jsonNegocio.Obligaciones.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (Valor > jsonNegocio.Obligaciones.Limites.MontoMaximoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor excede el monto máximo permitido para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es: {jsonNegocio.Obligaciones.Limites.MontoMaximoPorTransaccion} PEN.");
            }

            if (Valor < jsonNegocio.Obligaciones.Limites.MontoMinimoPorTransaccion)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque el valor no alcanza el monto el mínimo definido para este tipo de transacción. Su monto mínimo permitido para este tipo de transacción es de: {jsonNegocio.Obligaciones.Limites.MontoMinimoPorTransaccion} PEN.");
            }

            if (montoTransaccionesDiarias + Valor > jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la transacción porque excedería el monto máximo diario permitido para todas las operaciones en {(jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesDiarias + Valor)) * -1} soles para su corresponsal solidario. Su monto máximo diario para todas las transacciones es de {jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }

            if (montoTransaccionesTipoDiarias + Valor > jsonNegocio.Obligaciones.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el monto máximo diario en {(jsonNegocio.Obligaciones.Limites.MontoMaximoDiarioDeTransacciones - (montoTransaccionesTipoDiarias + Valor)) * -1} para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es de {jsonNegocio.Obligaciones.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }

            if (cuentaAsociada && saldoActual + Valor > jsonNegocio.Limites.SaldoMaximoAgente.Value)
            {
                throw new ExcepcionApp($"No puede realizar la operación porque excedería el saldo máximo de la caja en: {(jsonNegocio.Limites.SaldoMaximoAgente.Value - (saldoActual + Valor)) * -1} . Su saldo máximo en caja permitido es {jsonNegocio.Limites.SaldoMaximoAgente.Value} PEN");
            }

            if (cuentaAsociada && (saldoCuenta - Valor <= 0))
            {
                throw new ExcepcionApp("No puede realizar la operación porque no posee saldo disponible en la cuenta. Saldo disponible para transacción: " + saldoCuenta + ". Monto: " + Valor);
            }
        }
    }
}
