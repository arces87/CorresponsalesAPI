using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using ServiciosFinancial;
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
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ObtenerDistribuidosHandler(IFBSCorresponsalesApi financialApi, IRepositorioCatalogo repositorioCatalogo, IJsonConfiguracion jsonConfiguracion, IMapper mapper)
        {
            _financialApi = financialApi;
            _repositorioCatalogo = repositorioCatalogo;
            _jsonConfiguracion = jsonConfiguracion;
            _mapper = mapper;
        }

        public async Task<ObtenerDistribuidosMS> Handle(ObtenerDistribuidosME request, CancellationToken cancellationToken)
        {
            var catalogos = await _repositorioCatalogo.GetAllWithAssociations(true);
            var respuesta = await _financialApi.Clientes.DevuelveTiposIdentificacionWithHttpMessagesAsync();
            var tiposAlertas = catalogos.Where(c => c.TipoCatalogo.Id == new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTipoAlerta").Valor)).ToList();
            return new ObtenerDistribuidosMS() { TiposIdentificaciones = _mapper.Map<IEnumerable<DistribuidoTipoIdentificacion>>(respuesta.Body.TiposIdentificacion), TiposAlertas = _mapper.Map<IEnumerable<DistribuidoAlerta>>(tiposAlertas) };
        }
    }
}
