using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ModeloObtenerDetalleListaEmpresa
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Logo { get; set; }

        public string Ruc { get; set; }

        public string ActividadEconomica { get; set; }

        public string Provincia { get; set; }

        public string Departamento { get; set; }

        public string Distrito { get; set; }

        public DateTime FechaInicioActividad { get; set; }

        public bool EstaActivo { get; set; }
    }
}
