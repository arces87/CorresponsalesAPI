using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Commands
{
    public class ModificarCanalHandler : IRequestHandler<ModificarCanalME, string>
    {
        private readonly IRepositorioCanal _repositorio;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;

        public ModificarCanalHandler(IRepositorioCanal repositorio, IRepositorioAgente repositorioAgente, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarCanalME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            await ActualizarAgentes(new Guid(request.Id), request.JsonNegocio);
            return _model.Id.ToString();
        }

        private async Task ActualizarAgentes(Guid idCanal, string jsonCooperativa)
        {

            var agentes = await _repositorioAgente.GetAll();
            var jsonNegocioCooperativa = JsonConvert.DeserializeObject<JsonNegocioMS>(jsonCooperativa);
            foreach (var agente in agentes)
            {
                var jsonNegocioAgente = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

                if (jsonNegocioCooperativa.Limites.MontoMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.MontoMaximoDiarioDeTransacciones)
                    jsonNegocioAgente.Limites.MontoMaximoDiarioDeTransacciones = jsonNegocioCooperativa.Limites.MontoMaximoDiarioDeTransacciones;

                if (jsonNegocioCooperativa.Limites.NumeroMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                    jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones = jsonNegocioCooperativa.Limites.NumeroMaximoDiarioDeTransacciones;

                if (jsonNegocioCooperativa.Limites.SaldoMaximoAgente < jsonNegocioAgente.Limites.SaldoMaximoAgente)
                    jsonNegocioAgente.Limites.SaldoMaximoCuentaAsociada = jsonNegocioCooperativa.Limites.SaldoMaximoCuentaAsociada;

                if (jsonNegocioCooperativa.Limites.NumeroMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                    jsonNegocioAgente.Limites.SaldoMaximoAgente = jsonNegocioCooperativa.Limites.SaldoMaximoAgente;

                if (jsonNegocioCooperativa.Limites.ExistenciaCaja < jsonNegocioCooperativa.Limites.ExistenciaCaja)
                    jsonNegocioCooperativa.Limites.ExistenciaCaja = jsonNegocioCooperativa.Limites.ExistenciaCaja;

                CompararCambios(jsonNegocioCooperativa.Retiro, jsonNegocioAgente.Retiro);
                CompararCambios(jsonNegocioCooperativa.CobroServicios, jsonNegocioAgente.CobroServicios);
                CompararCambios(jsonNegocioCooperativa.AbonoPrestamos, jsonNegocioAgente.AbonoPrestamos);
                CompararCambios(jsonNegocioCooperativa.Deposito, jsonNegocioAgente.Deposito);

                agente.JsonAgente = JsonConvert.SerializeObject(jsonNegocioAgente);
                await _repositorioAgente.Update(agente);
            }
        }

        private void CompararCambios(Operacion operacionCooperativa, Operacion operacionAgente)
        {
            operacionAgente.Activo = operacionCooperativa.Activo;
            operacionAgente.NotificarCorreoElectronico = operacionCooperativa.NotificarCorreoElectronico;
            operacionAgente.NotificarSMS = operacionCooperativa.NotificarSMS;
            operacionAgente.PlantillaCorreoElectronico = operacionCooperativa.PlantillaCorreoElectronico;
            operacionAgente.PlantillaSMS = operacionCooperativa.PlantillaSMS;
            operacionAgente.ValidarOtpAgente = operacionCooperativa.ValidarOtpAgente;
            operacionAgente.ValidarOtpCliente = operacionCooperativa.ValidarOtpCliente;

            if (operacionCooperativa.Limites.MontoMaximoDiarioDeTransacciones < operacionAgente.Limites.MontoMaximoDiarioDeTransacciones)
                operacionAgente.Limites.MontoMaximoDiarioDeTransacciones = operacionCooperativa.Limites.MontoMaximoDiarioDeTransacciones;
            if (operacionCooperativa.Limites.MontoMaximoPorTransaccion < operacionAgente.Limites.MontoMaximoPorTransaccion)
                operacionAgente.Limites.MontoMaximoPorTransaccion = operacionCooperativa.Limites.MontoMaximoPorTransaccion;
            if (operacionCooperativa.Limites.MontoMinimoPorTransaccion > operacionAgente.Limites.MontoMinimoPorTransaccion)
                operacionAgente.Limites.MontoMinimoPorTransaccion = operacionCooperativa.Limites.MontoMinimoPorTransaccion;
            if (operacionCooperativa.Limites.NumeroMaximoDiarioDeTransacciones < operacionAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                operacionAgente.Limites.NumeroMaximoDiarioDeTransacciones = operacionCooperativa.Limites.NumeroMaximoDiarioDeTransacciones;
            if (operacionCooperativa.Comisiones.AdministracionCanal < operacionAgente.Comisiones.AdministracionCanal)
                operacionAgente.Comisiones.AdministracionCanal = operacionCooperativa.Comisiones.AdministracionCanal;
            if (operacionCooperativa.Comisiones.Agente < operacionAgente.Comisiones.Agente)
                operacionAgente.Comisiones.Agente = operacionCooperativa.Comisiones.Agente;
            if (operacionCooperativa.Comisiones.Cooperativa < operacionAgente.Comisiones.Cooperativa)
                operacionAgente.Comisiones.Cooperativa = operacionCooperativa.Comisiones.Cooperativa;
        }
    }
}

    
