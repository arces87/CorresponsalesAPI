using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente
{
    public class VerificarAgenteHandler : IRequestHandler<VerificarAgenteME, bool>
    {
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IHttpContextAccessor _httpContext;

        public VerificarAgenteHandler(IRepositorioAgente repositorioAgente, IRepositorioDispositivoAgente repositorioDispositivoAgente, IRepositorioGeolocalizacion repositorioGeolocalizacion, IRepositorioDispositivo repositorioDispositivo, IHttpContextAccessor httpContext, UserManager<Usuario> manejadorUsuario)
        {
            _repositorioAgente = repositorioAgente;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _repositorioDispositivo = repositorioDispositivo;
            _httpContext = httpContext;
            _manejadorUsuario = manejadorUsuario;
        }
        public async Task<bool> Handle(VerificarAgenteME request, CancellationToken cancellationToken)
        {
            var idUsuario = _httpContext.HttpContext.User.Identity.Name;

            var usuario = await _manejadorUsuario.FindByIdAsync(idUsuario);

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);

            var dispositivoAgente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());

            var dispositivo = await _repositorioDispositivo.Get(dispositivoAgente.DispositivoId.ToString());

            var error = "Error en la validación de los datos de autenticación ";

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            if (agente == null || usuario == null)
            {
                throw new ExcepcionApp($"{error} | A001");
            }

            if (agente.Usuario.Id.ToString().ToLower() != idUsuario.ToLower())
            {
                throw new ExcepcionApp($"{error} | A001");
            }

            if (!ValidarDispositivo(request.Imei, request.Mac, dispositivo))
            {
                throw new ExcepcionApp($"{error} | A002");
            }
            else if (request.VerificarGeolocalizacion && jsonNegocio.VerificarGeolocalizacion)
            {
                var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
                if (!ValidarGeolocalizacion(geolocalizacion, request.Latitud, request.Longitud))
                {
                    throw new ExcepcionApp($"{error} | A006");
                }
            }

            return true;
        }

        public bool ValidarDispositivo(string imei, string mac, Dispositivo dispositivo)
        {            
            return dispositivo != null && dispositivo.EstaActivo && dispositivo.Imei.ToUpper() == imei.ToUpper() && dispositivo.MacAddress.ToUpper() == mac.ToUpper();
        }

        public bool ValidarGeolocalizacion(Geolocalizacion geolocalizacion, double latitud, double longitud)
        {

            if (geolocalizacion == null) return false;

            var latitud_inicio = geolocalizacion.Latitud - 1;
            var latitud_fin = geolocalizacion.Latitud + 1;
            var longitud_inicio = geolocalizacion.Longitud - 1;
            var longitud_fin = geolocalizacion.Longitud + 1;

            return latitud >= latitud_inicio && latitud <= latitud_fin && longitud >= longitud_inicio && longitud <= longitud_fin;
        }
    }
}
