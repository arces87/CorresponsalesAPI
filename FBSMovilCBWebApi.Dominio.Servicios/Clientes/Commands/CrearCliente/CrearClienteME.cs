using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands
{
    public class CrearClienteME : IRequest<CreaClienteMS>
    {
        public int? SecuencialOficinaGraba { get; set; }
        public bool? SeHaraCliente { get; set; }
        public string Identificacion { get; set; }
        public string DireccionDomicilio { get; set; }
        public int? SecuencialTipoIdentificacion { get; set; }
        public int? SecuencialDivisionActividadEconomica { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public System.DateTime? FechaNacimiento { get; set; }
        public bool? EsMasculino { get; set; }
        public string NumeroPapeletaVotacion { get; set; }

        public int? CargasFamiliares { get; set; }

        public string CodigoEstadoCivil { get; set; }

        public string CodigoTipoVivienda { get; set; }

        public double? ActivosTotales { get; set; }

        public double? PasivosTotales { get; set; }

        public double? Patrimonio { get; set; }

        public string TelefonoDomicilio { get; set; }

        public bool? VisualizaMensajeDeExistencia { get; set; }

        public int? SecuencialDivisionPoliticaResidencia { get; set; }

        public string CodigoTelefono { get; set; }

        public int? NumeroMesesArriendo { get; set; }

        public int? SecuencialDivisionPoliticaNacimiento { get; set; }

        public bool? EsObligadoLLevarContabilidad { get; set; }

        public double? VentasAnuales { get; set; }
    }
}
