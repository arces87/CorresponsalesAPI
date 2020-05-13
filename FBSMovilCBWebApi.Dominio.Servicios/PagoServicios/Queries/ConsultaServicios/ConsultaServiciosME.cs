using MediatR;
using ServiciosFinancial.Models;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ConsultaServiciosME : ConsultaME, IRequest<ConsultaMS>
    {
        
    }
}
