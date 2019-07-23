using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands
{
    public class ModificarTipoCatalogoCommandHandle : IRequestHandler<ModificarTipoCatalogoCommand, int>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ModificarTipoCatalogoCommandHandle(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarTipoCatalogoCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
