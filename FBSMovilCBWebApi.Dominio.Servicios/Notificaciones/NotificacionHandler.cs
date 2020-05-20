using FBS.Dominio.Servicios.CorreoElectronico;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
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
                        notification.PlantillaCorreoElectronico = notification.PlantillaCorreoElectronico.Replace(key, notification.ValoresEmail[key]);
                }

                try
                {
                    await _mediador.Publish(new EnviarCorreoElectronicoME
                    {
                        Asunto = notification.AsuntoCorreoElectronico,
                        Mensaje = notification.PlantillaCorreoElectronico,
                        DireccionesDestino = new List<ModeloCuentaCorreo> { new ModeloCuentaCorreo {
                        Direccion = notification.CorreoElectronicoDestinatario,
                        Nombre = notification.NombreDestinatario} }
                    });
                }
                catch (Exception)
                {
                    throw new Exception($"Error en el envio del correo electrónico al notificar la operación");
                }
              

            }

            if (notification.PlantillaSMS != null)
            {
                foreach (var key in notification.ValoresSms.Keys)
                {
                    if (notification.PlantillaSMS != null)
                        notification.PlantillaSMS = notification.PlantillaSMS.Replace(key, notification.ValoresSms[key]);
                }

                var mensajeSMS = new EnvioSMSME()
                {
                    CodigoUsuarioCorresponsal = notification.NombreUsuarioCorresponsal,
                    MensajeTexto = notification.PlantillaSMS,
                    NumeroIdentificacion = notification.IdentificacionCorresponsal,
                    SecuencialTipoIdentificacion = notification.TipoIdentificacionCorresponsal
                };

                try
                {
                    await _financialApi.MensajeriaSMS.EnvioSMSAsync(mensajeSMS);
                } catch(Exception) {
                    throw new Exception($"Error en el envio de sms al notificar la operación");
                }
                
            }
        }
    }
}
