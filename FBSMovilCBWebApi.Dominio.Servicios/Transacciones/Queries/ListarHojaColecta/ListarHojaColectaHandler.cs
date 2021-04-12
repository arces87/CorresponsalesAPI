using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarHojaColectaHandler : IRequestHandler<ListarHojaColectaME, ListarHojaColectaMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;

        public ListarHojaColectaHandler(IRepositorioTransaccion repositorio, IRepositorioAgente repositorioAgente, IMapper mapper, IMediator mediador)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<ListarHojaColectaMS> Handle(ListarHojaColectaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var transacciones = await _repositorio.GetForAgente(agente.Id.ToString());
            transacciones = transacciones.Where(t => !t.ReposicionRealizada).ToList();
            var _retorno = new ListarHojaColectaMS();
            _retorno.Transacciones = _mapper.Map<IEnumerable<ModeloListarHojaColecta>>(transacciones);
            return _retorno;
        }
    }
}
