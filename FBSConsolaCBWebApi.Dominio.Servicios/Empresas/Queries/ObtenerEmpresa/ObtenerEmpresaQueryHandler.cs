using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ObtenerEmpresaQueryHandler : IRequestHandler<ObtenerEmpresaQuery, ObtenerModeloEmpresa>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public ObtenerEmpresaQueryHandler(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloEmpresa> Handle(ObtenerEmpresaQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloEmpresa>(await _repositorio.Get(request.Id));
        }
    }
}
