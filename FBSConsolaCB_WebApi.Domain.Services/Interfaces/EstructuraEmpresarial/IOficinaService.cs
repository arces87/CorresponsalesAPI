using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface IOficinaService
    {
        IEnumerable<OficinaModel> List();
        FuenteDatosModel<OficinaModel> List(PaginacionModel filtro);
        OficinaModel Get(int Id);
        OficinaModel Create(OficinaModel role);
        OficinaModel Update(OficinaModel role);
        OficinaModel Delete(int Id);
    }
}
