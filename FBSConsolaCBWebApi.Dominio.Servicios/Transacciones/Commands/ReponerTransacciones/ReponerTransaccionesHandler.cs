using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ReponerTransaccionesHandler : IRequestHandler<ReponerTransaccionesME, bool>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IMapper _mapper;

        public ReponerTransaccionesHandler(IRepositorioTransaccion repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ReponerTransaccionesME request, CancellationToken cancellationToken)
        {
            await _repositorio.ReponerTransaccion(request.IdAgente);
            return true;
        }
    }
}
