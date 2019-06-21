using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface ICorresponsalService
    {
        FuenteDatosModel<CorresponsalModel> List(PaginacionModel filtro);
        object Get(int Id);
        CorresponsalModel Create(CorresponsalModel model);
        CorresponsalModel Update(CorresponsalModel model);
        CorresponsalModel Delete(int Id);


    }
}
