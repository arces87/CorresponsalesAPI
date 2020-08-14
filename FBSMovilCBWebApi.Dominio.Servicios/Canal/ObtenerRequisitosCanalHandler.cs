using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Canal
{
    public class ObtenerRequisitosCanalHandler : IRequestHandler<ObtenerRequisitoCanalME, RequisitosCanalMS>
    {
        private IMediator _mediador;
        private readonly IRepositorioCanal _canalRepositorio;
        public ObtenerRequisitosCanalHandler(IRepositorioCanal canalRepositorio)
        {
            _canalRepositorio = canalRepositorio;
        }
        public async Task<RequisitosCanalMS> Handle(ObtenerRequisitoCanalME request, CancellationToken cancellationToken)
        {

            var canal = await _canalRepositorio.GetWithAssociations(request.CanalId);

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(canal.JsonNegocio);

            return new RequisitosCanalMS
            {
                VerificarGeolocalizacion = jsonNegocio.ValidarGeolocalizacion
            };
        }
    }
}
