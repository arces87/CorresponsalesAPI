using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class ModificarCatalogoHandler : IRequestHandler<ModificarCatalogoME, string>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ModificarCatalogoHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarCatalogoME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id.ToString();
        }
    }
}
