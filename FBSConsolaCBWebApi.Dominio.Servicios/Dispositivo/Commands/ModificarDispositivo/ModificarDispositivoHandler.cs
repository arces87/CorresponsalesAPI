using AutoMapper;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class ModificarDispositivoHandler : IRequestHandler<ModificarDispositivoME, string>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IRepositorioImagen _repositorioImagen;
        private readonly IMapper _mapper;

        public ModificarDispositivoHandler(IRepositorioDispositivo repositorio, IRepositorioImagen repositorioImagen, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioImagen = repositorioImagen;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarDispositivoME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            await _repositorioImagen.RemoveAllForDispositivo(_model.Id.ToString());
            foreach (var item in request.Imagenes)
            {
                await _repositorioImagen.Add(new Imagen() { DireccionImagen = item.Imagen, Dispositivo = _model });
            }
            return _model.Id.ToString();
        }
    }
}
