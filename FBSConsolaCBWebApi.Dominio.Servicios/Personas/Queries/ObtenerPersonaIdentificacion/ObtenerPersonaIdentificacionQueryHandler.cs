using AutoMapper;
using Financial_Services_Banca;
using Financial_Services_Banca.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerPersonaIdentificacionQueryHandler : IRequestHandler<ObtenerPersonaIdentificacionQuery, ObtenerModeloPersonaIdentificacion>
    {
        private readonly IMapper _mapper;
        private readonly IFBSBancaApi _bancaVirtual;

        public ObtenerPersonaIdentificacionQueryHandler(IMapper mapper, IFBSBancaApi bancaVirtual)
        {
            _mapper = mapper;
            _bancaVirtual = bancaVirtual;
        }

        public async Task<ObtenerModeloPersonaIdentificacion> Handle(ObtenerPersonaIdentificacionQuery request, CancellationToken cancellationToken)
        {
            var parametro = new PorIdentificacionSocioME()
            {
                Identificacion = request.Identificacion
            };
            var _retorno = await _bancaVirtual.Clientes.DevuelveDatosPersonaIdentificacionWithHttpMessagesAsync(parametro);
            var contenido = _retorno.Body;

            if (contenido != null)
            {
                var retorno = _mapper.Map<ObtenerModeloPersonaIdentificacion>(contenido);
                return retorno;
            }
            return null;
        }
    }
}
