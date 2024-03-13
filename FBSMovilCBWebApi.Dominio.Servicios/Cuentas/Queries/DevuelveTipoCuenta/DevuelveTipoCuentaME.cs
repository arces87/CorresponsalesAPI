using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Corresponsales.Query.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaME : IRequest<DevuelveTiposDeCuentasDeUnClienteResponse>
    {
        public int SecuencialCliente { get; set; }
        public int SecuencialEmpresa { get; set; }
        public string CodigoProductoVista { get; set; }

        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
