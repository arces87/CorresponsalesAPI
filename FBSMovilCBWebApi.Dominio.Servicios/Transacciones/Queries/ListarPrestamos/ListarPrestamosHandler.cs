using MediatR;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos
{
    public class ListarPrestamosHandler: IRequestHandler<ListarPrestamosME, InformacionPrestamosMSL>
    {
        private readonly IPrestamosApi _prestamo;
        public ListarPrestamosHandler(IPrestamosApi prestamo)
        {

            _prestamo = prestamo;

        }

        public async Task<InformacionPrestamosMSL> Handle(ListarPrestamosME request, CancellationToken cancellationToken)
        {

            var modelo = (PorIdentificacionClienteActivaME) request;

            var prestamos = await _prestamo.PrestamosDevuelveInformacionDePrestamosAsync(modelo);

            return prestamos;
        }
    }
}
