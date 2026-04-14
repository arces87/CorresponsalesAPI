using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Excepciones;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Notificaciones
{
    public class NotificacionHandler : INotificationHandler<NotificacionME>
    {
        private readonly IGeneralesApi _envioSMSApi;
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        public NotificacionHandler(IGeneralesApi envioSMSApi, IMediator mediador, IJsonConfiguracion jsonConfiguracion)
        {
            _envioSMSApi = envioSMSApi;
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task Handle(NotificacionME notification, CancellationToken cancellationToken)
        {
            if (notification.PlantillaCorreoElectronico != null)
            {
                
                notification.PlantillaCorreoElectronico = Regex.Replace(notification.PlantillaCorreoElectronico, @"\t|\n|\r", "");
                notification.PlantillaCorreoElectronico = notification.PlantillaCorreoElectronico.Replace("\"", "'");
                foreach (var key in notification.ValoresEmail.Keys)
                {
                    if (notification.PlantillaCorreoElectronico != null)
                        notification.PlantillaCorreoElectronico = notification.PlantillaCorreoElectronico.Replace(key, notification.ValoresEmail[key]);
                }

                try
                {
                    var email = new EnviarCorreoElectronicoME
                    {
                        Asunto = notification.AsuntoCorreoElectronico,
                        Mensaje = notification.PlantillaCorreoElectronico,
                        DireccionesDestino = new List<ModeloCuentaCorreo> { new ModeloCuentaCorreo {
                        Direccion = notification.CorreoElectronicoDestinatario,
                        Nombre = notification.NombreDestinatario} }
                    };

                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(email),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    await _mediador.Publish(email);
                }
                catch (Exception e)
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(e.Message),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                    throw new ExcepcionApp($"Error en el envio del correo electrónico al notificar la operación");
                }              
            }

            if (notification.PlantillaSMS != null)
            {
                foreach (var key in notification.ValoresSms.Keys)
                {
                    if (notification.PlantillaSMS != null)
                        notification.PlantillaSMS = notification.PlantillaSMS.Replace(key, notification.ValoresSms[key]);
                }

                try
                {
                    var mensajeSMS = new EnvioSmsRequest()
                    {
                        CodigoUsuarioCorresponsal = notification.NombreUsuarioCorresponsal,
                        MensajeTexto = notification.PlantillaSMS,
                        NumeroIdentificacion = notification.Identificacion,
                        SecuencialTipoIdentificacion = notification.TipoIdentificacion,
                        NumeroCelular = notification.NumeroMovilCliente,
                    };

                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(mensajeSMS),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(respuesta),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    if (!(bool)respuesta.EsExitoso)
                    {
                        throw new ExcepcionApp(respuesta.MensajeRespuesta);
                    }

                } catch(Exception e) {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(e.Message),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                    throw new ExcepcionApp($"Error en el envio de sms al notificar la operación");
                }                
            }
        }
    }
}
