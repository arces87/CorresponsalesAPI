using Corresponsales.Command.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class ProcesaPagoCuentaME : IRequest<ProcesaPagoCuentaMS>
    {
        public string IdentificacionCliente { get; set; }            
        public IList<RubroPorCobrarRequest> CuentasPorCobrar { get; set; }       
        public double ValorAfectado {  get; set; }
        public int SecuencialCuenta { get; set; }
        public string NombreCliente { get; set; }
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
