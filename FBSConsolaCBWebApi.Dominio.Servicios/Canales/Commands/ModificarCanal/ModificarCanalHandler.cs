using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
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
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;

        public ModificarCanalHandler(
            IRepositorioCanal repositorio, 
            IRepositorioAgente repositorioAgente, 
            IMapper mapper, 
            IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
        }

        public async Task<string> Handle(ModificarCanalME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            var jsonCanalNegocioActual = JsonConvert.DeserializeObject<JsonNegocioMS>(_model.JsonNegocio);
            var jsonCanalNegocioNuevo = JsonConvert.DeserializeObject<JsonNegocioMS>(request.JsonNegocio);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            await ActualizarAgentes(new Guid(request.Id), jsonCanalNegocioNuevo, jsonCanalNegocioActual);
            return _model.Id.ToString();
        }

        private async Task ActualizarAgentes(Guid idCanal, JsonNegocioMS jsonCanalNegocioNuevo, JsonNegocioMS jsonCanalNegocioActual)
        {

            var agentes = await _repositorioAgente.GetAllWithAssociations();
            try
            {
                foreach (var agente in agentes)
                {
                    var jsonNegocioAgente = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

                    CompararLimites(jsonCanalNegocioNuevo, ref jsonNegocioAgente);

                    jsonNegocioAgente.Retiro = CompararCambios(jsonCanalNegocioNuevo.Retiro, jsonNegocioAgente.Retiro);
                    jsonNegocioAgente.CobroServicios = CompararCambios(jsonCanalNegocioNuevo.CobroServicios, jsonNegocioAgente.CobroServicios);
                    jsonNegocioAgente.Deposito = CompararCambios(jsonCanalNegocioNuevo.Deposito, jsonNegocioAgente.Deposito);

                    if (jsonCanalNegocioActual.VerificarGeolocalizacion != jsonCanalNegocioNuevo.VerificarGeolocalizacion)
                    {
                        var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());

                        if (geolocalizacion != null)
                        {

                            if (jsonCanalNegocioNuevo.VerificarGeolocalizacion == false)
                            {
                                geolocalizacion.Latitud = 0;
                                geolocalizacion.Longitud = 0;
                            }
                            else
                            {
                                geolocalizacion.FechaBaja = DateTime.Now;
                                geolocalizacion.EstaActivo = false;
                            }

                            await _repositorioGeolocalizacion.Update(geolocalizacion);
                        }

                        jsonNegocioAgente.VerificarGeolocalizacion = jsonCanalNegocioNuevo.VerificarGeolocalizacion;

                    }
                    agente.JsonAgente = JsonConvert.SerializeObject(jsonNegocioAgente);
                    await _repositorioAgente.Update(agente);
                }
            }
            catch ( Exception e)
            {
                throw new ExcepcionApp("No fue posible modificar el canal.");
            }
        }

        private static void CompararLimites(JsonNegocioMS jsonCanal, ref JsonNegocioMS jsonNegocioAgente)
        {
            if (jsonCanal.Limites.MontoMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.MontoMaximoDiarioDeTransacciones)
                jsonNegocioAgente.Limites.MontoMaximoDiarioDeTransacciones = jsonCanal.Limites.MontoMaximoDiarioDeTransacciones;

            if (jsonCanal.Limites.NumeroMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones = jsonCanal.Limites.NumeroMaximoDiarioDeTransacciones;

            if (jsonCanal.Limites.SaldoMaximoAgente < jsonNegocioAgente.Limites.SaldoMaximoAgente)
                jsonNegocioAgente.Limites.SaldoMaximoCuentaAsociada = jsonCanal.Limites.SaldoMaximoCuentaAsociada;

            if (jsonCanal.Limites.NumeroMaximoDiarioDeTransacciones < jsonNegocioAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                jsonNegocioAgente.Limites.SaldoMaximoAgente = jsonCanal.Limites.SaldoMaximoAgente;

            if (jsonCanal.Limites.ExistenciaCaja < jsonNegocioAgente.Limites.ExistenciaCaja)
                jsonNegocioAgente.Limites.ExistenciaCaja = jsonCanal.Limites.ExistenciaCaja;
        }

        private Operacion CompararCambios(Operacion operacionCanal, Operacion operacionAgente)
        {
            operacionAgente.Activo = operacionCanal.Activo;
            operacionAgente.NotificarCorreoElectronico = operacionCanal.NotificarCorreoElectronico;
            operacionAgente.NotificarSMS = operacionCanal.NotificarSMS;
            operacionAgente.PlantillaCorreoElectronico = operacionCanal.PlantillaCorreoElectronico;
            operacionAgente.PlantillaSMS = operacionCanal.PlantillaSMS;
            operacionAgente.ValidarOtpAgente = operacionCanal.ValidarOtpAgente;
            operacionAgente.ValidarOtpCliente = operacionCanal.ValidarOtpCliente;

            if (operacionAgente.Limites == null)
            {
                operacionAgente.Limites = operacionCanal.Limites;
            } else
            {
                if (operacionCanal.Limites.MontoMaximoDiarioDeTransacciones < operacionAgente.Limites.MontoMaximoDiarioDeTransacciones)
                    operacionAgente.Limites.MontoMaximoDiarioDeTransacciones = operacionCanal.Limites.MontoMaximoDiarioDeTransacciones;
                if (operacionCanal.Limites.MontoMaximoPorTransaccion < operacionAgente.Limites.MontoMaximoPorTransaccion)
                    operacionAgente.Limites.MontoMaximoPorTransaccion = operacionCanal.Limites.MontoMaximoPorTransaccion;
                if (operacionCanal.Limites.MontoMinimoPorTransaccion > operacionAgente.Limites.MontoMinimoPorTransaccion)
                    operacionAgente.Limites.MontoMinimoPorTransaccion = operacionCanal.Limites.MontoMinimoPorTransaccion;
                if (operacionCanal.Limites.NumeroMaximoDiarioDeTransacciones < operacionAgente.Limites.NumeroMaximoDiarioDeTransacciones)
                    operacionAgente.Limites.NumeroMaximoDiarioDeTransacciones = operacionCanal.Limites.NumeroMaximoDiarioDeTransacciones;
            }


            if (operacionAgente.Comisiones == null)
            {
                operacionAgente.Comisiones = operacionCanal.Comisiones;
            } else
            {
                if (operacionCanal.Comisiones.AdministracionCanal < operacionAgente.Comisiones.AdministracionCanal)
                    operacionAgente.Comisiones.AdministracionCanal = operacionCanal.Comisiones.AdministracionCanal;
                if (operacionCanal.Comisiones.Agente < operacionAgente.Comisiones.Agente)
                    operacionAgente.Comisiones.Agente = operacionCanal.Comisiones.Agente;
                if (operacionCanal.Comisiones.Cooperativa < operacionAgente.Comisiones.Cooperativa)
                    operacionAgente.Comisiones.Cooperativa = operacionCanal.Comisiones.Cooperativa;
            }

            return operacionAgente;
        }
    }
}

    
