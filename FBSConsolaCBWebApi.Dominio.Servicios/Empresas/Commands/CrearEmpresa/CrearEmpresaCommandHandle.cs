using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands
{
    public class CrearEmpresaCommandHandle : IRequestHandler<CrearEmpresaCommand, int>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public CrearEmpresaCommandHandle(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearEmpresaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Empresa>(request);
            var identificador = await _repositorio.Add(_model);
            return int.Parse(identificador);
        }
    }
}
