using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class ProcesarLoginMS
    {
        public Guid GuidID { get; set; }
        public bool TieneOtp { get; set; }
        public bool EsPrimeraVez { get; set; }
    }
}
