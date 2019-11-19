using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Commands
{
    public class EliminarCanalHandler : IRequestHandler<EliminarCanalME, bool>
    {
        private readonly IRepositorioCanal _repositorio;
        private readonly IMapper _mapper;

        public EliminarCanalHandler(IRepositorioCanal repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarCanalME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Canal>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
