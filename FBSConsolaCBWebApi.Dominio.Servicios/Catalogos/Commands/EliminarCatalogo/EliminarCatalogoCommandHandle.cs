using AutoMapper;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class EliminarCatalogoCommandHandle : IRequestHandler<EliminarCatalogoCommand, bool>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public EliminarCatalogoCommandHandle(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarCatalogoCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Catalogo>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
