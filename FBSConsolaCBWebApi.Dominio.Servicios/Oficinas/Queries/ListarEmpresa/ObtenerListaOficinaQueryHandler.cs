using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries
{
    public class ObtenerListaOficinaQueryHandler : IRequestHandler<ObtenerListaOficinaQuery, ModeloObtenerListaOficina>
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaOficinaQueryHandler(IRepositorioOficina repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaOficina> Handle(ObtenerListaOficinaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return new ModeloObtenerListaOficina() { Empresas = _mapper.Map<List<ModeloObtenerDetalleListaOficina>>(_model) };
        }
    }
}
