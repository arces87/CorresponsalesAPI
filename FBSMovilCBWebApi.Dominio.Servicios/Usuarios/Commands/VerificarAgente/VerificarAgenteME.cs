using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente
{
    public class VerificarAgenteME: IRequest<bool>
    {    
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
