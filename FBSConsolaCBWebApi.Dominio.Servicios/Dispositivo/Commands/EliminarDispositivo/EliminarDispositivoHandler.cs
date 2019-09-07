using AutoMapper;
using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class EliminarDispositivoHandler : IRequestHandler<EliminarDispositivoME, bool>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public EliminarDispositivoHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarDispositivoME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Dispositivo>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
