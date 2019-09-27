using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class CrearTransaccionHandler : IRequestHandler<CrearTransaccionME, string>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IMapper _mapper;

        public CrearTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearTransaccionME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Transaccion>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
