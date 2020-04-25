using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Notificaciones
{
    public class NotificacionHandler : INotificationHandler<NotificacionME>
    {
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMediator _mediador;
        public NotificacionHandler(IJsonConfiguracion jsonConfiguracion, IFBSCorresponsalesApi financialApi, IMediator mediador)
        {
            _jsonConfiguracion = jsonConfiguracion;
            _financialApi = financialApi;
            _mediador = mediador;
        }

        public async Task Handle(NotificacionME notification, CancellationToken cancellationToken)
        {
            var notificarCorreoElectronico = bool.Parse(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == $"{notification.Operacion}NotificarCorreoElectronico").Valor);

            var notificarSMS = bool.Parse(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == $"{notification.Operacion}NotificarSMS").Valor);
            var plantillaCorreoElectronico = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == $"{notification.Operacion}PlantillaCorreoElectronico").Valor;
            var plantillaSMS = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == $"{notification.Operacion}PlantillaSMS").Valor;
            foreach (var key in notification.Valores.Keys)
            {
                plantillaCorreoElectronico.Replace(key, notification.Valores[key]);
                plantillaSMS.Replace(key, notification.Valores[key]);
            }
            if (notificarCorreoElectronico)
            {
                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = notification.AsuntoCorreoElectronico,
                    DireccionesDestino = new List<ModeloCuentaCorreo> { new ModeloCuentaCorreo { 
                        Direccion = notification.CorreoElectronicoDestinatario,
                        Nombre = notification.NombreDestinatario} }
                });
            }

            if (notificarSMS)
            {
                var request = new PorNumeroClienteDeUnaEmpresaMensajeME
                {
                    NumeroCliente = notification.NumeroCliente,
                    SecuencialEmpresa = notification.SecuencialEmpresa,
                    Mensaje = plantillaSMS
                };
                var response = await _financialApi.MensajeriaSMS.EnvioSMSAsync(request);
            }
        }
    }
}
