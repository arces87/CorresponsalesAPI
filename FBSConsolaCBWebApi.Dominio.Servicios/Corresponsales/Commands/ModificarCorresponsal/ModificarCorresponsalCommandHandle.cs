using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class ModificarCorresponsalCommandHandle : IRequestHandler<ModificarCorresponsalCommand, int>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ModificarCorresponsalCommandHandle(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarCorresponsalCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetWithAssociations(request.Id) as Corresponsal;
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
