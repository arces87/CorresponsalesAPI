using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ObtenerListaEmpresaQueryHandler : IRequestHandler<ObtenerListaEmpresaQuery, ModeloObtenerListaEmpresa>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaEmpresaQueryHandler(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaEmpresa> Handle(ObtenerListaEmpresaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerListaEmpresa() { Empresas = _mapper.Map<List<ModeloObtenerDetalleListaEmpresa>>(_model) };
        }
    }
}
