using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerComisionPorTipoTransaccionHandler : IRequestHandler<ObtenerComisionPorTipoTransaccionME, ObtenerComisionPorTipoTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ObtenerComisionPorTipoTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ObtenerComisionPorTipoTransaccionMS> Handle(ObtenerComisionPorTipoTransaccionME request, CancellationToken cancellationToken)
        {
            var _retorno = new ObtenerComisionPorTipoTransaccionMS();
            var _model = await _repositorio.GetForTipo(request.IdTipoTransaccion, request.IdAgente);
            _retorno.Comisiones = new List<ModeloObtenerComisionPorTipoTransaccion>();
            foreach (var item in _model)
            {
                var comisiones = JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(item.Comisiones);
                _retorno.Comisiones.Add(new ModeloObtenerComisionPorTipoTransaccion() { Comisiones = comisiones, Fecha = item.FechaSistema });
            }

            return _retorno;
        }
    }
}
