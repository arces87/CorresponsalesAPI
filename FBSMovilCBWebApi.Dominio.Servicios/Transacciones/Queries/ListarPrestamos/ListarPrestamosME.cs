using MediatR;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos
{
    public class ListarPrestamosME: PorIdentificacionClienteActivaME, IRequest<InformacionPrestamosMSL>
    {

    }
}
