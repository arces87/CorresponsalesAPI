using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands
{
    public class EliminarOficinaCommandHandle : IRequestHandler<EliminarOficinaCommand, bool>
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public EliminarOficinaCommandHandle(IRepositorioOficina repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarOficinaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Oficina>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
