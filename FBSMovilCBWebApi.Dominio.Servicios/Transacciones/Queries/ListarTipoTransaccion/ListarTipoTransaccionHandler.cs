using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionHandler : IRequestHandler<ListarTipoTransaccionME, ListarTipoTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;

        public ListarTipoTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion, IMediator mediador, IRepositorioAgente repositorioAgente)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
        }

        private double ContabilizarComisiones(ComisionPorTipoTransaccion comisiones)
        {
            var total = comisiones.AdministracionCanal.Value + comisiones.Agente.Value + comisiones.Cooperativa.Value + (comisiones.Facilito != null ? comisiones.Facilito.Value : 0);
            return total;
        }
        public async Task<ListarTipoTransaccionMS> Handle(ListarTipoTransaccionME request, CancellationToken cancellationToken)
        {

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            var idAbonoPrestamo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAbonoPrestamo")?.Valor;
            var valorDeposito = 0.0;
            var comisionDeposito = 0.0;
            var valorRetiro = 0.0;
            var comisionRetiro = 0.0;
            var valorCobroServicio = 0.0;
            var comisionCobroServicio = 0.0;
            var valorAbonoPrestamos = 0.0;
            var comisionAbonoPrestamos = 0.0;

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);

            var saldo = await _repositorio.GetSaldoActual(agente.Id.ToString());
            var _retorno = new ListarTipoTransaccionMS() { SaldoCaja = saldo };
            if (idDeposito != null)
            {
                var _modelDeposito = await _repositorio.GetForTipo(idDeposito, agente.Id.ToString());
                valorDeposito = _modelDeposito.Sum(m => m.Valor);
                comisionDeposito = _modelDeposito.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idRetiro != null)
            {
                var _modelRetiro = await _repositorio.GetForTipo(idRetiro, agente.Id.ToString());
                valorRetiro = _modelRetiro.Sum(m => m.Valor);
                comisionRetiro = _modelRetiro.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idCobroServicio != null)
            {
                var _modelCobroServicios = await _repositorio.GetForTipo(idCobroServicio, agente.Id.ToString());
                valorCobroServicio = _modelCobroServicios.Sum(m => m.Valor);
                comisionCobroServicio = _modelCobroServicios.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            if (idAbonoPrestamo != null)
            {
                var _modelAbonoPrestamo = await _repositorio.GetForTipo(idAbonoPrestamo, agente.Id.ToString());
                valorAbonoPrestamos = _modelAbonoPrestamo.Sum(m => m.Valor);
                comisionAbonoPrestamos = _modelAbonoPrestamo.Sum(m => ContabilizarComisiones(JsonConvert.DeserializeObject<ComisionPorTipoTransaccion>(m.Comisiones)));
            }
            _retorno.TiposTransacciones = new List<ModeloListaTipoTransaccion>() {
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
                    },
                     new ModeloListaTipoTransaccion(){
                        Id = idAbonoPrestamo,
                        Nombre = "Abono de Préstamos",
                        Comisiones = comisionAbonoPrestamos,
                        Valor = valorAbonoPrestamos
                    },
            };
            return _retorno;
        }
    }
}
