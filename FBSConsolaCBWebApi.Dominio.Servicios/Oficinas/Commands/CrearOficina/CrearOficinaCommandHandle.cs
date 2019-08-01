using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands
{
    public class CrearOficinaCommandHandle : IRequestHandler<CrearOficinaCommand, int>
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public CrearOficinaCommandHandle(IRepositorioOficina repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearOficinaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Oficina>(request);
            var identificador = await _repositorio.Add(_model);
            return int.Parse(identificador);
        }
    }
}
