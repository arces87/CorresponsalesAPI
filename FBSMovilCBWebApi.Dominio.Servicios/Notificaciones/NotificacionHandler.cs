using FBS.Dominio.Servicios.CorreoElectronico;
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
            //foreach (var key in notification.Valores.Keys)
            //{
            //    if (notification.PlantillaCorreoElectronico != null)
            //        notification.PlantillaCorreoElectronico.Replace(key, notification.Valores[key]);
            //    if (notification.PlantillaSMS != null)
            //        notification.PlantillaSMS.Replace(key, notification.Valores[key]);
            //}
            //if (notification.PlantillaCorreoElectronico != null)
            //{
            //    await _mediador.Publish(new EnviarCorreoElectronicoME
            //    {
            //        Asunto = notification.AsuntoCorreoElectronico,
            //        Mensaje = notification.PlantillaCorreoElectronico,
            //        DireccionesDestino = new List<ModeloCuentaCorreo> { new ModeloCuentaCorreo {
            //            Direccion = notification.CorreoElectronicoDestinatario,
            //            Nombre = notification.NombreDestinatario} }
            //    });
            //}

            //if (notification.PlantillaSMS != null)
            //{
            //    var request = new PorNumeroClienteDeUnaEmpresaMensajeME
            //    {
            //        NumeroCliente = notification.NumeroCliente,
            //        SecuencialEmpresa = notification.SecuencialEmpresa,
            //        Mensaje = notification.PlantillaSMS
            //    };
            //    var response = await _financialApi.MensajeriaSMS.EnvioSMSAsync(request);
            //}
        }
    }
}
