using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class VerificarDispositivoHandler : IRequestHandler<VerificarDispositivoME, bool>
    {
        private readonly IRepositorioDispositivo _repositorio;

        public VerificarDispositivoHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Handle(VerificarDispositivoME request, CancellationToken cancellationToken)
        {
            return await _repositorio.VerificarDispositivo(request.Marca,request.Modelo,request.NoSerie);
        }
    }
}
