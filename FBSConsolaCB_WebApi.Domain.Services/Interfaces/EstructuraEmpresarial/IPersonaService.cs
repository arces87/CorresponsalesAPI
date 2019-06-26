using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface IPersonaService
    {
        FuenteDatosModel<PersonaModel> List(PaginacionModel filtro);
        object Get(int Id);
        CorresponsalModel Create(CorresponsalModel model);
        SupervisorModel Create(SupervisorModel model);
        CorresponsalModel Update(CorresponsalModel model);
        SupervisorModel Update(SupervisorModel model);
        PersonaModel Delete(int Id);


    }
}
