using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente
{
    public class VerificarAgenteHandler : IRequestHandler<VerificarAgenteME, bool>
    {
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;

        public VerificarAgenteHandler(IRepositorioAgente repositorioAgente, IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _repositorioAgente = repositorioAgente;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
        }
        public async Task<bool> Handle(VerificarAgenteME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForUserName(request.Usuario);

            Geolocalizacion geolocalizacion = null;

            if  (request.VerificarGeolocalizacion)
            {
                geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
            }
            
            var error = "";
            if(agente == null)
            {
                error = " | A001";
            }
            else if (! agente.ValdiarDispotivo(request.Imei, request.Mac))
            {
                error = " | A002";
            } else if (request.VerificarGeolocalizacion && !(geolocalizacion != null && agente.ValdiarGeolocalizacion(geolocalizacion, request.Latitud, request.Longitud)))
            {
                error = " | A006";
            }

            if (error.Length > 0)
                throw new Exception($"Error en la validación de los datos de autenticación {error}");

            return true;
        }
    }
}
