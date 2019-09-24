using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarActivacionHandler : IRequestHandler<SolicitarActivacionME, bool>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public SolicitarActivacionHandler(IMediator mediador, IRepositorioAgente repositorioAgente, IMapper mapper, IJsonConfiguracion jsonConfiguracion, IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
        }

        public async Task<bool> Handle(SolicitarActivacionME request, CancellationToken cancellationToken)
        {
            try
            {
                var agente = await _repositorioAgente.GetForUserName(request.Usuario);
                var idEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoUbicado").Valor;
                if (agente != null) //Comprobacion de existencia del Agente
                {
                    if (agente.Dispositivo != null && agente.Dispositivo.Imei == request.Imei && agente.Dispositivo.MacAddress == request.Mac) //Comprobación de existencia de dispositivo y sus datos
                    {
                        var _usuario = _mapper.Map<LoginUsuarioME>(request);
                        var usuarioAutenticado = await _mediador.Send(_usuario);
                        if (usuarioAutenticado.Errores == null)
                        {
                            var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
                            if (geolocalizacion != null) //Guardando Geolocalización
                            {
                                await _repositorioGeolocalizacion.Remove(geolocalizacion);
                            }
                            await _repositorioGeolocalizacion.AdicionarGeolocalizacionAgente(request.Latitud, request.Longitud, agente.Id.ToString());
                        }
                    }

                }

                throw new Exception("Error en la validación de los datos de autenticación");
            }
            catch (Exception)
            {
                throw new Exception("Error en la validación de los datos de autenticación");
            }
        }
    }
}
