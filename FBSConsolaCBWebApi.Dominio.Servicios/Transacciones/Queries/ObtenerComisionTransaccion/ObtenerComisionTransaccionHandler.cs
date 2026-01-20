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
    public class ObtenerComisionTransaccionHandler : IRequestHandler<ObtenerComisionTransaccionME, ObtenerComisionTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ObtenerComisionTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ObtenerComisionTransaccionMS> Handle(ObtenerComisionTransaccionME request, CancellationToken cancellationToken)
        {
            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            var idAbonoPrestamo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAbonoPrestamo")?.Valor;
            var idObligaciones = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdObligacion")?.Valor;

            var valorDeposito = 0.0;
            var valorRetiro = 0.0;
            var valorCobroServicio = 0.0;
            var valorAbonoPrestamo = 0.0;
            var valorObligaciones = 0.0;

            var _retorno = new ObtenerComisionTransaccionMS();
            var _comisionDeposito = new ComisionTransaccion() { Agente = 0, AdministracionCanal = 0, Cooperativa = 0 };
            var _comisionRetiro = new ComisionTransaccion() { Agente = 0, AdministracionCanal = 0, Cooperativa = 0 };
            var _comisionCobroServicio = new ComisionTransaccion() { Agente = 0, AdministracionCanal = 0, Cooperativa = 0 };
            var _comisionAbonoPrestamo = new ComisionTransaccion() { Agente = 0, AdministracionCanal = 0, Cooperativa = 0 };
            var _comisionObligaciones = new ComisionTransaccion() { Agente = 0, AdministracionCanal = 0, Cooperativa = 0 };

            if (idDeposito != null)
            {
                var _model = await _repositorio.GetForTipo(idDeposito, request.IdAgente);
                foreach (var item in _model)
                {
                    var comisiones = JsonConvert.DeserializeObject<ComisionTransaccion>(item.Comisiones);
                    _comisionDeposito.Agente += comisiones.Agente != null ? comisiones.Agente.Value : 0.0;
                    _comisionDeposito.AdministracionCanal += comisiones.AdministracionCanal != null ? comisiones.AdministracionCanal.Value : 0.0;
                    _comisionDeposito.Cooperativa += comisiones.Cooperativa != null ? comisiones.Cooperativa.Value : 0.0;
                }
                valorDeposito += _comisionDeposito.Agente.Value + _comisionDeposito.AdministracionCanal.Value + _comisionDeposito.Cooperativa.Value;
            }
            if (idRetiro != null)
            {
                var _model = await _repositorio.GetForTipo(idRetiro, request.IdAgente);
                foreach (var item in _model)
                {
                    var comisiones = JsonConvert.DeserializeObject<ComisionTransaccion>(item.Comisiones);
                    _comisionRetiro.Agente += comisiones.Agente != null ? comisiones.Agente.Value : 0.0;
                    _comisionRetiro.AdministracionCanal += comisiones.AdministracionCanal != null ? comisiones.AdministracionCanal.Value : 0.0;
                    _comisionRetiro.Cooperativa += comisiones.Cooperativa != null ? comisiones.Cooperativa.Value : 0.0;
                }
                valorRetiro += _comisionRetiro.Agente.Value + _comisionRetiro.AdministracionCanal.Value + _comisionRetiro.Cooperativa.Value;
            }
            if (idCobroServicio != null)
            {
                var _model = await _repositorio.GetForTipo(idCobroServicio, request.IdAgente);
                foreach (var item in _model)
                {
                    var comisiones = JsonConvert.DeserializeObject<ComisionTransaccion>(item.Comisiones);
                    _comisionCobroServicio.Agente += comisiones.Agente != null ? comisiones.Agente.Value : 0.0;
                    _comisionCobroServicio.AdministracionCanal += comisiones.AdministracionCanal != null ? comisiones.AdministracionCanal.Value : 0.0;
                    _comisionCobroServicio.Cooperativa += comisiones.Cooperativa != null ? comisiones.Cooperativa.Value : 0.0;
                }
                valorCobroServicio += _comisionCobroServicio.Agente.Value + _comisionCobroServicio.AdministracionCanal.Value + _comisionCobroServicio.Cooperativa.Value;
            }
            if (idAbonoPrestamo != null)
            {
                var _model = await _repositorio.GetForTipo(idAbonoPrestamo, request.IdAgente);
                foreach (var item in _model)
                {
                    var comisiones = JsonConvert.DeserializeObject<ComisionTransaccion>(item.Comisiones);
                    _comisionAbonoPrestamo.Agente += comisiones.Agente != null ? comisiones.Agente.Value : 0.0;
                    _comisionAbonoPrestamo.AdministracionCanal += comisiones.AdministracionCanal != null ? comisiones.AdministracionCanal.Value : 0.0;
                    _comisionAbonoPrestamo.Cooperativa += comisiones.Cooperativa != null ? comisiones.Cooperativa.Value : 0.0;
                }
                valorAbonoPrestamo += _comisionAbonoPrestamo.Agente.Value + _comisionAbonoPrestamo.AdministracionCanal.Value + _comisionAbonoPrestamo.Cooperativa.Value;
            }
            if (idObligaciones != null)
            {
                var _model = await _repositorio.GetForTipo(idObligaciones, request.IdAgente);
                foreach (var item in _model)
                {
                    var comisiones = JsonConvert.DeserializeObject<ComisionTransaccion>(item.Comisiones);
                    _comisionObligaciones.Agente += comisiones.Agente != null ? comisiones.Agente.Value : 0.0;
                    _comisionObligaciones.AdministracionCanal += comisiones.AdministracionCanal != null ? comisiones.AdministracionCanal.Value : 0.0;
                    _comisionObligaciones.Cooperativa += comisiones.Cooperativa != null ? comisiones.Cooperativa.Value : 0.0;
                }
                valorObligaciones += _comisionObligaciones.Agente.Value + _comisionObligaciones.AdministracionCanal.Value + _comisionObligaciones.Cooperativa.Value;
            }


            _retorno.TiposTransacciones = new List<ModeloObtenerComisionTransaccion>() {
                    new ModeloObtenerComisionTransaccion(){
                        Id = idCobroServicio,
                        Nombre = "Cobro de Servicios",
                        Valor = valorCobroServicio,
                        Comisiones = _comisionCobroServicio
                    },
                    new ModeloObtenerComisionTransaccion(){
                        Id = idDeposito,
                        Nombre = "Depósito",
                        Valor = valorDeposito,
                        Comisiones = _comisionDeposito
                    },
                    new ModeloObtenerComisionTransaccion(){
                        Id = idRetiro,
                        Nombre = "Retiro",
                        Valor = valorRetiro,
                        Comisiones=_comisionRetiro
                    },
                    new ModeloObtenerComisionTransaccion(){
                        Id = idAbonoPrestamo,
                        Nombre = "Abono de Préstamos",
                        Valor = valorAbonoPrestamo,
                        Comisiones=_comisionAbonoPrestamo
                    },
                    new ModeloObtenerComisionTransaccion(){
                        Id = idObligaciones,
                        Nombre = "Obligaciones",
                        Valor = valorObligaciones,
                        Comisiones=_comisionObligaciones
                    }
            };
            return _retorno;
        }
    }
}
