using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands
{
    public class ModificarEmpresaCommandHandle : IRequestHandler<ModificarEmpresaCommand, int>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public ModificarEmpresaCommandHandle(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarEmpresaCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
