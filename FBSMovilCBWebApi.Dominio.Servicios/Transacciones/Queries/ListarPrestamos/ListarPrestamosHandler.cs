using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos
{
    public class ListarPrestamosHandler: IRequestHandler<ListarPrestamosME, DevuelveInformacionDePrestamosResponse>
    {
        private readonly ICarteraApi _prestamo;
        public ListarPrestamosHandler(ICarteraApi prestamo)
        {

            _prestamo = prestamo;

        }

        public async Task<DevuelveInformacionDePrestamosResponse> Handle(ListarPrestamosME request, CancellationToken cancellationToken)
        {
            var prestamos = await _prestamo.DevuelveInformacionDePrestamosAsync(request);
            return prestamos;
        }
    }
}
