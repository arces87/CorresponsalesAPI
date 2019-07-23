using MediatR;
using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands
{
    public class CrearEmpresaCommand : IRequest<int>
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Logo { get; set; }

        public string Ruc { get; set; }

        public string ActividadEconomica { get; set; }

        public string Provincia { get; set; }

        public string Departamento { get; set; }

        public string Distrito { get; set; }

        public DateTime FechaInicioActividad { get; set; }
    }
}
