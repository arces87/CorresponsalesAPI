using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaHandler : IRequestHandler<CrearAlertaME, bool>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public CrearAlertaHandler(IRepositorioAlerta repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion,
            IRepositorioAgente repositorioAgente, IHttpContextAccessor httpContext)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
        }

        public async Task<bool> Handle(CrearAlertaME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Alerta>(request);
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            _model.Agente = agente;
            _model.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAlertaNueva").Valor) };
            var identificador = await _repositorio.Add(_model);
            return true;
        }
    }
}
