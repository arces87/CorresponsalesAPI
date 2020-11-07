using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class NotificaSupervisorHandler : INotificationHandler<NotificaSupervisorME>
    {
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IMediator _mediador;

        public NotificaSupervisorHandler(IRepositorioAgente repositorio, IMediator mediador)
        {
            _repositorioAgente = repositorio;
            _mediador = mediador;
        }

        public async Task Handle(NotificaSupervisorME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(request.Id);

            //await _mediador.Publish(new EnviarCorreoElectronicoME
            //{
            //    Asunto = "Notificación de error en operación del agente",
            //    Mensaje = "Se ha producido un problema en la operación realizada por el agente: " + agente.NombreAgente + ", error: " + request.MensajeExcepcion + ", remitase a la consola administrativa para más detalles",
            //    DireccionesDestino = new List<ModeloCuentaCorreo>() {
            //            new ModeloCuentaCorreo() {
            //                Direccion = agente.Supervisor.Email,
            //                Nombre = agente.Supervisor.NombreCompleto
            //            }
            //        }
            //});            
        }
    }
}