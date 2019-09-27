using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloObtenerComisionPorTipoTransaccion
    {
        public DateTime Fecha { get; set; }
        public ComisionPorTipoTransaccion Comisiones { get; set; }
    }
}