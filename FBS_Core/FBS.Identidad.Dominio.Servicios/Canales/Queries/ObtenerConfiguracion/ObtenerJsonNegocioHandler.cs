using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Canales.Queries
{
    public class ObtenerJsonNegocioHandler : IRequestHandler<ObtenerJsonNegocioME, JsonNegocioMS>
    {
        private readonly IRepositorioCanal _repositorio;
        private readonly IMapper _mapper;

        public ObtenerJsonNegocioHandler(IRepositorioCanal repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<JsonNegocioMS> Handle(ObtenerJsonNegocioME request, CancellationToken cancellationToken)
        {
            var canal = await _repositorio.GetCanalUsuario(request.IdUsuario);
            if (canal != null)
            {
                return JsonConvert.DeserializeObject<JsonNegocioMS>(canal.JsonNegocio);
            }
            throw new ExcepcionApp("El usuario no se encuentra registrado a ningún canal");
        }

    }
}
