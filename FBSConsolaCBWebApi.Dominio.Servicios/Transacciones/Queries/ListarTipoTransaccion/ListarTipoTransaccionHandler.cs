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

        private double ContabilizarComisiones(ComisionPorTipoTransaccion comisiones)
        {
            var total = comisiones.AdministracionCanal.Value + comisiones.Agente.Value + comisiones.Cooperativa.Value + (comisiones.Facilito != null ? comisiones.Facilito.Value : 0);
            return total;
        }
        public async Task<ListarTipoTransaccionMS> Handle(ListarTipoTransaccionME request, CancellationToken cancellationToken)
        {
            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            var idAbonoPrestamo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAbonoPrestamo")?.Valor;
            var idObligaciones = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdObligacion")?.Valor;
            var idAperturaCuenta = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAperturaCuenta")?.Valor;

            var valorDeposito = 0.0;
            var comisionDeposito = 0.0;
            var valorRetiro = 0.0;
            var comisionRetiro = 0.0;
            var valorCobroServicio = 0.0;
            var comisionCobroServicio = 0.0;
            var valorAbonoPrestamo = 0.0;
            var comisionAbonoPrestamo = 0.0;
            var valorObligaciones = 0.0;
            var comisionObligaciones = 0.0;
            var valorAperturaCuenta = 0.0;
            var comisionAperturaCuenta = 0.0;

            var saldo = await _repositorio.GetSaldoActual(request.IdAgente);
            var _retorno = new ListarTipoTransaccionMS() { SaldoCaja = saldo };

            if (idDeposito != null)
            {
                var _modelDeposito = await _repositorio.GetForTipo(idDeposito, request.IdAgente);
                valorDeposito = _modelDeposito.Sum(m => m.Valor);
                comisionDeposito = _modelDeposito.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idRetiro != null)
            {
                var _modelRetiro = await _repositorio.GetForTipo(idRetiro, request.IdAgente);
                valorRetiro = _modelRetiro.Sum(m => m.Valor);
                comisionRetiro = _modelRetiro.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idCobroServicio != null)
            {
                var _modelCobroServicios = await _repositorio.GetForTipo(idCobroServicio, request.IdAgente);
                valorCobroServicio = _modelCobroServicios.Sum(m => m.Valor);
                comisionCobroServicio = _modelCobroServicios.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idAbonoPrestamo != null)
            {
                var _modelAbonoPrestamo = await _repositorio.GetForTipo(idAbonoPrestamo, request.IdAgente);
                valorAbonoPrestamo = _modelAbonoPrestamo.Sum(m => m.Valor);
                comisionAbonoPrestamo = _modelAbonoPrestamo.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idObligaciones != null)
            {
                var _modelObligaciones= await _repositorio.GetForTipo(idObligaciones, request.IdAgente);
                valorObligaciones = _modelObligaciones.Sum(m => m.Valor);
                comisionObligaciones = _modelObligaciones.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idAperturaCuenta != null)
            {
                var _modelAperturaCuenta = await _repositorio.GetForTipo(idAperturaCuenta, request.IdAgente);
                valorAperturaCuenta = _modelAperturaCuenta.Sum(m => m.Valor);
                comisionAperturaCuenta = _modelAperturaCuenta.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }

            _retorno.TiposTransacciones = new List<ModeloListaTipoTransaccion>() {
                new ModeloListaTipoTransaccion(){
                    Id = idAperturaCuenta,
                    Nombre = "Apertura de Cuentas",
                    Comisiones = comisionAperturaCuenta,
                    Valor = valorAperturaCuenta
                },
                new ModeloListaTipoTransaccion(){
                        Id = idObligaciones,
                        Nombre = "Obligaciones",
                        Comisiones = comisionObligaciones,
                        Valor = valorObligaciones
                    },
                new ModeloListaTipoTransaccion(){
                        Id = idAbonoPrestamo,
                        Nombre = "Abono de Préstamos",
                        Comisiones = comisionAbonoPrestamo,
                        Valor = valorAbonoPrestamo
                    },
                new ModeloListaTipoTransaccion(){
                        Id = idCobroServicio,
                        Nombre = "Cobro de Servicios",
                        Comisiones = comisionCobroServicio,
                        Valor = valorCobroServicio
                    },
                    new ModeloListaTipoTransaccion(){
                        Id = idDeposito,
                        Nombre = "Depósito",
                        Comisiones = comisionDeposito,
                        Valor = valorDeposito
                    },
                    new ModeloListaTipoTransaccion(){
                        Id = idRetiro,
                        Nombre = "Retiro",
                        Comisiones = comisionRetiro,
                        Valor = valorRetiro
                    }
            };
            return _retorno;
        }
    }
}
