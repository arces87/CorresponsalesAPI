using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Newtonsoft.Json;
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
                        
            var error = "Error en la validación de los datos de autenticación ";

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            if (agente == null)
            {
                throw new ExcepcionApp($"{error} | A001");
            }
            else if (! agente.ValdiarDispotivo(request.Imei, request.Mac))
            {
                throw new ExcepcionApp($"{error} | A002");
            }
            else if (request.VerificarGeolocalizacion && jsonNegocio.VerificarGeolocalizacion)
            {
                var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
                if (!agente.ValdiarGeolocalizacion(geolocalizacion, request.Latitud, request.Longitud))
                {
                    throw new ExcepcionApp($"{error} | A006");
                }
            }

            return true;
        }
    }
}
