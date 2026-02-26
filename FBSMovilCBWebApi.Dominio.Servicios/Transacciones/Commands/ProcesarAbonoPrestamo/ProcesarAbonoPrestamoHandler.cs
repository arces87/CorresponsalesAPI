using AutoMapper;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarAbonoPrestamoHandler : IRequestHandler<ProcesarAbonoPrestamoME, EfectivizacionPrestamoResponse>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly ICarteraApi _prestamo;
        private readonly IMapper _mapper;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IHttpContextAccessor _httpContext;
        private readonly byte[] _llave;

        public ProcesarAbonoPrestamoHandler(
            IMediator mediador, 
            IJsonConfiguracion jsonConfiguracion,
            ICarteraApi prestamo,
            IMapper mapper, 
            IRepositorioTransaccion repositorioTransaccion, 
            IRepositorioAgente repositorioAgente, 
            IRepositorioCuenta repositorioCuenta,
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _prestamo = prestamo;
            _mapper = mapper;
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
            _httpContext = httpContext;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<EfectivizacionPrestamoResponse> Handle(ProcesarAbonoPrestamoME request, CancellationToken cancellationToken)
        {
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAbonoPrestamo").Valor;
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

            var transacciones = await _repositorioTransaccion.GetForAgente(agente.Id.ToString());
            //transacciones = transacciones.Where(t => t.FechaDispositivo.Date == DateTime.Now.Date);
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

            var transaccionesRepuestas = await _repositorioTransaccion.TransaccionesRepuestas(agente.Id.ToString());
            var transaccionesProcesadas = await _repositorioTransaccion.TransaccionesProcesadas(agente.Id.ToString());

            var enReposicion = transaccionesRepuestas == transaccionesProcesadas;

            saldoCuenta = (saldoCuenta == 0 && transacciones.Count() == 0) || enReposicion ? jsonNegocio.Limites.SaldoMaximoCuentaAsociada.Value : saldoCuenta;

            ValidarTransaccion(
               request.Valor,
               cuenta != null,
               saldoActual,
               saldoCuenta,
               cantidadTransaccionesDiarias,
               cantidadTransaccionesDiariasTipo,
               montoTransaccionesDiarias,
               montoTransaccionesTipoDiarias,
               jsonNegocio);            
          
            var transaccion = new Transaccion()
            {
                CanalId = _jsonConfiguracion.IdCanal,
                Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaRecibida").Valor) },
                Comisiones = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).AbonoPrestamos.Comisiones),
                Agente = agente,
                Descripcion = request.Concepto,
                FechaDispositivo = DateTime.Now,
                FechaSistema = DateTime.Now,
                HoraDispositivo = DateTime.Now.TimeOfDay,
                IdentificacionCliente = request.IdentificacionCliente,
                NombreCliente = request.NombreCliente,
                SecuencialCuenta = cuenta.SecuencialCuenta.ToString(),
                Valor = request.Valor,
                JsonDatos = JsonConvert.SerializeObject(request),
                SaldoDisponible = saldoActual + request.Valor,
                Tipo = IdTipoAccion,
                EstaActivo = true,
                Criptografia = Encoding.UTF8.GetString(Criptografia.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(request), _llave, _llave))
            };
            var idTransaccion = await _repositorioTransaccion.Add(transaccion);
            
            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).AbonoPrestamos.Comisiones;
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", ValorComision = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", ValorComision = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", ValorComision = comision.Cooperativa });
            var modelo = new EfectivizacionPrestamoRequest()
            {                
                CodigoUsuario = agente.Usuario.UserName,
                JsonComision = JsonConvert.SerializeObject(arregloComisiones),
                SecuencialCuentaCorresponsal = cuenta != null ? int.Parse(cuenta.SecuencialCuenta) : 0,                
                ValorAfectado = (decimal)request.Valor,
                EsUnSoloCobroComision = true,
                Concepto = request.Concepto,
                NumeroPrestamo = request.NumeroPrestamo
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(modelo),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var respuesta = await _prestamo.EfectivizacionPrestamoAsync(modelo);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            //if (respuesta.OMsnTecni == "ERR")
            //{
            //    throw new Exception($"No se pudo realizar la operación: {respuesta.OMsnPerso}");
            //}

            transaccion.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTransferenciaProcesada").Valor) };
            transaccion.SaldoCuenta = respuesta.SaldoCuentaCorresponsal;
            await _repositorioTransaccion.Update(transaccion);
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return respuesta;
        }

        private static void ValidarTransaccion(
            double Valor,
            bool cuentaAsociada,
            double saldoActual,
            double saldoCuenta,
            int cantidadTransaccionesDiarias,
            int cantidadTransaccionesDiariasTipo,
            double montoTransaccionesDiarias,
            double montoTransaccionesTipoDiarias,
            JsonNegocioMS jsonNegocio)
        {
            if (!jsonNegocio.AbonoPrestamos.Activo.Value)
            {
                throw new Exception("Usted no posee acceso para ejecutar esta operación. En caso de requerir acceso a esta funcionalidad comunicarse con su supervisor.");
            }

            if (cantidadTransaccionesDiarias + 1 > jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new Exception($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para su corresponsal solidario que es: {jsonNegocio.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (cantidadTransaccionesDiariasTipo + 1 > jsonNegocio.AbonoPrestamos.Limites.NumeroMaximoDiarioDeTransacciones)
            {
                throw new Exception($"No puede realizar la operación porque ha alcanzado el número máximo de transacciones diarias para este tipo de transacción que es: { jsonNegocio.AbonoPrestamos.Limites.NumeroMaximoDiarioDeTransacciones}.");
            }

            if (Valor > jsonNegocio.AbonoPrestamos.Limites.MontoMaximoPorTransaccion)
            {
                throw new Exception($"No puede realizar la operación porque el valor excede el monto máximo permitido para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es: {jsonNegocio.AbonoPrestamos.Limites.MontoMaximoPorTransaccion} PEN.");
            }

            if (Valor < jsonNegocio.AbonoPrestamos.Limites.MontoMinimoPorTransaccion) 
            {
                throw new Exception($"No puede realizar la operación porque el valor no alcanza el monto el mínimo definido para este tipo de transacción. Su monto mínimo permitido para este tipo de transacción es de: {jsonNegocio.AbonoPrestamos.Limites.MontoMinimoPorTransaccion} PEN.");
            }

            if (montoTransaccionesDiarias + Valor > jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new Exception($"No puede realizar la transacción porque excedería el monto máximo diario permitido para todas las operaciones en {(jsonNegocio.Limites.SaldoMaximoAgente.Value - (saldoActual + Valor)) * -1} soles para su corresponsal solidario. Su monto máximo diario para todas las transacciones es de {jsonNegocio.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }

            if (montoTransaccionesTipoDiarias + Valor > jsonNegocio.AbonoPrestamos.Limites.MontoMaximoDiarioDeTransacciones)
            {
                throw new Exception($"No puede realizar la operación porque excedería el monto máximo diario en {(jsonNegocio.Retiro.Limites.MontoMaximoDiarioDeTransacciones - (saldoActual + Valor)) * -1} para este tipo de transacción. Su monto máximo permitido para este tipo de transacción es de {jsonNegocio.AbonoPrestamos.Limites.MontoMaximoDiarioDeTransacciones} PEN.");
            }

            if (cuentaAsociada && saldoCuenta - Valor <= 0)
            {
                throw new Exception("No puede realizar la operación porque no posee saldo disponible en la cuenta");
            }

        }
    }
}
