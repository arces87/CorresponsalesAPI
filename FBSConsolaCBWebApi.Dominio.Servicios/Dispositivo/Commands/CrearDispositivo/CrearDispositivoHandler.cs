using AutoMapper;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class CrearDispositivoHandler : IRequestHandler<CrearDispositivoME, string>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IRepositorioImagen _repositorioImagen;
        private readonly IMapper _mapper;

        public CrearDispositivoHandler(IRepositorioDispositivo repositorio, IRepositorioImagen repositorioImagen, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioImagen = repositorioImagen;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearDispositivoME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Dispositivo>(request);
            var identificador = await _repositorio.Add(_model);
            foreach (var item in request.Imagenes)
            {
                var idImagen = await _repositorioImagen.Add(new Imagen() { DireccionImagen = item.Imagen, Dispositivo = _model });
            }
            return identificador;
        }
    }
}
