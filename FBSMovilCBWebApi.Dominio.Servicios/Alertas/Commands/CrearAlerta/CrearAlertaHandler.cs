using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaHandler : IRequestHandler<CrearAlertaME, string>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public CrearAlertaHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearAlertaME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Alerta>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
