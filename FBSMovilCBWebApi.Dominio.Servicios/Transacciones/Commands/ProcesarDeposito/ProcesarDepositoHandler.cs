using FBS.Identidad.DAL.Modelado;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarDepositoHandler : IRequestHandler<ProcesarDepositoME, ProcesarDepositoMS>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        public ProcesarDepositoHandler(IMediator mediador, IJsonConfiguracion jsonConfiguracion)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ProcesarDepositoMS> Handle(ProcesarDepositoME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });
            var respuesta = new ProcesarDepositoMS()
            {
                FechaTransaccion = DateTime.Now,
                NoCuenta = "435345245",
                NoTransaccion = "123132",
                Valor = 200
            };
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(respuesta),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return respuesta;
        }
    }
}
