using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionHandler : IRequestHandler<ListarTipoTransaccionME, ListarTipoTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ListarTipoTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ListarTipoTransaccionMS> Handle(ListarTipoTransaccionME request, CancellationToken cancellationToken)
        {
            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            var valorDeposito = 0.0;
            var valorRetiro = 0.0;
            var valorCobroServicio = 0.0;
            var _retorno = new ListarTipoTransaccionMS();
            if (idDeposito != null)
            {
                var _model = await _repositorio.GetForTipo(idDeposito, request.IdAgente);
                valorDeposito = _model.Sum(m => m.Valor);
            }
            if (idRetiro != null)
            {
                var _model = await _repositorio.GetForTipo(idRetiro, request.IdAgente);
                valorRetiro = _model.Sum(m => m.Valor);
            }
            if (idCobroServicio != null)
            {
                var _model = await _repositorio.GetForTipo(idCobroServicio, request.IdAgente);
                valorCobroServicio = _model.Sum(m => m.Valor);
            }
            _retorno.TiposTransacciones = new List<ModeloListaTipoTransaccion>() {
                    new ModeloListaTipoTransaccion(){
                        Id = idCobroServicio,
                        Nombre = "Cobro de Servicios",
                        Valor = valorCobroServicio
                    },
                    new ModeloListaTipoTransaccion(){
                        Id = idDeposito,
                        Nombre = "Depósito",
                        Valor = valorDeposito
                    },
                    new ModeloListaTipoTransaccion(){
                        Id = idRetiro,
                        Nombre = "Retiro",
                        Valor = 0-valorRetiro
                    },
            };
            return _retorno;
        }
    }
}
