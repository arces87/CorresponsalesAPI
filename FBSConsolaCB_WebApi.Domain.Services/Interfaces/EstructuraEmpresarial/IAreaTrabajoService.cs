using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface IAreaTrabajoService
    {
        IEnumerable<AreaTrabajoModel> List();
        FuenteDatosModel<AreaTrabajoModel> List(PaginacionModel filtro);
        AreaTrabajoModel Get(int Id);
        AreaTrabajoModel Create(AreaTrabajoModel role);
        AreaTrabajoModel Update(AreaTrabajoModel role);
        AreaTrabajoModel Delete(int Id);



    }
}
