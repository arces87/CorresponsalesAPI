using AutoMapper;
using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class CrearCatalogoHandler : IRequestHandler<CrearCatalogoME, string>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public CrearCatalogoHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearCatalogoME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Catalogo>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
