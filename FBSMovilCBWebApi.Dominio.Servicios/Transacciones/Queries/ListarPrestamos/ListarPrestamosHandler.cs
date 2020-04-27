using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos
{
    public class ListarPrestamosHandler: IRequestHandler<ListarPrestamosME, InformacionPrestamosMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        public ListarPrestamosHandler(IFBSCorresponsalesApi financialApi)
        {

            _financialApi = financialApi;

        }

        public async Task<InformacionPrestamosMSL> Handle(ListarPrestamosME request, CancellationToken cancellationToken)
        {

            var modelo = (PorIdentificacionClienteActivaME) request;

            var prestamos = await _financialApi.Prestamos.DevuelveInformacionDePrestamosAsync(modelo);

            return prestamos;
        }
    }
}
