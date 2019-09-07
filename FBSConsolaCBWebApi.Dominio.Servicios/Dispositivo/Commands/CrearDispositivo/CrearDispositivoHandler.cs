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
    public class CrearDispositivoHandler : IRequestHandler<CrearDispositivoME, string>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public CrearDispositivoHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearDispositivoME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Dispositivo>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
