using AutoMapper;
using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class EliminarCatalogoHandler : IRequestHandler<EliminarCatalogoME, bool>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public EliminarCatalogoHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarCatalogoME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Catalogo>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
