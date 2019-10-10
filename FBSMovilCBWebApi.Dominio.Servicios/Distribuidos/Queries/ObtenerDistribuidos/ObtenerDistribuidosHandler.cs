using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries
{
    public class ObtenerDistribuidosHandler : IRequestHandler<ObtenerDistribuidosME, ObtenerDistribuidosMS>
    {

        private readonly IRepositorioCatalogo _repositorioCatalogo;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ObtenerDistribuidosHandler(IRepositorioCatalogo repositorioCatalogo, IJsonConfiguracion jsonConfiguracion, IMapper mapper)
        {
            _repositorioCatalogo = repositorioCatalogo;
            _jsonConfiguracion = jsonConfiguracion;
            _mapper = mapper;
        }

        public async Task<ObtenerDistribuidosMS> Handle(ObtenerDistribuidosME request, CancellationToken cancellationToken)
        {
            var catalogos = await _repositorioCatalogo.GetAllWithAssociations();
            var tiposIdentificacion = catalogos.Where(c => c.TipoCatalogo.Id == new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTipoIdentificacion").Valor)).ToList();
            return new ObtenerDistribuidosMS() { TiposIdentificaciones = _mapper.Map<IEnumerable<DistribuidoIdentificacion>>(tiposIdentificacion) };
        }
    }
}
