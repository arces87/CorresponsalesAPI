using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSServiciosSMSTulcan.EnviarSMS
{
    public class EnviarSmsME : IRequest<EnviarSmsMS>
    {
        public SmsModelo Mensaje { get; set; }
    }
}
