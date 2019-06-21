using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Consola;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.Consola
{
    public interface IDispositivoService
    {
        IEnumerable<DispositivoModel> List(int Tipo);
        FuenteDatosModel<DispositivoModel> List(PaginacionModel filtro);
        DispositivoModel Get(int Id);
        DispositivoModel Create(DispositivoModel role);
        DispositivoModel Update(DispositivoModel role);
        DispositivoModel Delete(int Id);


    }
}
