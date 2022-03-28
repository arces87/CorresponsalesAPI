using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    class ObtenerEstadoReposicionHandler : IRequestHandler<ObtenerEstadoReposicionAgenteME, ObtenerEstadoReposicionAgenteMS>
    {
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        public ObtenerEstadoReposicionHandler(IRepositorioTransaccion repositorioTransaccion)
        {
            _repositorioTransaccion = repositorioTransaccion;
        }

        public async Task<ObtenerEstadoReposicionAgenteMS> Handle(ObtenerEstadoReposicionAgenteME request, CancellationToken cancellationToken)
        {

            var transaccionesRepuestas = await _repositorioTransaccion.TransaccionesRepuestas(request.AgenteId);
            var transaccionesProcesadas = await _repositorioTransaccion.TransaccionesProcesadas(request.AgenteId);

            return new ObtenerEstadoReposicionAgenteMS
            {
                EstadoReposicion = transaccionesRepuestas == transaccionesProcesadas
            };
        }
    }
}
