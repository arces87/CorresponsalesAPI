using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands
{
    public class EliminarEmpresaCommandHandle : IRequestHandler<EliminarEmpresaCommand, bool>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public EliminarEmpresaCommandHandle(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarEmpresaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Empresa>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
