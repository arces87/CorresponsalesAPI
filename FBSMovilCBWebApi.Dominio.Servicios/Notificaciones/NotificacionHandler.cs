using FBS.Dominio.Servicios.CorreoElectronico;
using FBSServiciosSMSTulcan.EnviarSMS;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Notificaciones
{
    public class NotificacionHandler : INotificationHandler<NotificacionME>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMediator _mediador;
        public NotificacionHandler(IFBSCorresponsalesApi financialApi, IMediator mediador)
        {
            _financialApi = financialApi;
            _mediador = mediador;
        }

        public async Task Handle(NotificacionME notification, CancellationToken cancellationToken)
        {
            if (notification.PlantillaCorreoElectronico != null)
            {
                foreach (var key in notification.ValoresEmail.Keys)
                {
                    if (notification.PlantillaCorreoElectronico != null)
                        notification.PlantillaCorreoElectronico.Replace(key, notification.ValoresEmail[key]);
                }

                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = notification.AsuntoCorreoElectronico,
                    Mensaje = notification.PlantillaCorreoElectronico,
                    DireccionesDestino = new List<ModeloCuentaCorreo> { new ModeloCuentaCorreo {
                        Direccion = notification.CorreoElectronicoDestinatario,
                        Nombre = notification.NombreDestinatario} }
                });

            }

            if (notification.PlantillaSMS != null)
            {
                foreach (var key in notification.ValoresSms.Keys)
                {
                    if (notification.PlantillaSMS != null)
                        notification.PlantillaSMS.Replace(key, notification.ValoresSms[key]);
                }

                var request = new EnviarSmsME
                {
                    Mensaje =  new SmsModelo()
                    {
                        Destinatario = notification.NumeroCliente.ToString(),
                        Mensaje = notification.PlantillaSMS
                    }
                };

                await _mediador.Publish(request);
            }
        }
    }
}
