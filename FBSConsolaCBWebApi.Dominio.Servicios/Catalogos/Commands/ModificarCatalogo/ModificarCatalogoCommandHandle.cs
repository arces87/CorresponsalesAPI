using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class ModificarCatalogoCommandHandle : IRequestHandler<ModificarCatalogoCommand, int>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ModificarCatalogoCommandHandle(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarCatalogoCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetWithAssociations(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
