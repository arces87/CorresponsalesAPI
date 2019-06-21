using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface IEmpresaService
    {
        IEnumerable<EmpresaModel> List();
        FuenteDatosModel<EmpresaModel> List(PaginacionModel filtro);
        EmpresaModel Get(int Id);
        EmpresaModel Create(EmpresaModel role);
        EmpresaModel Update(EmpresaModel role);
        EmpresaModel Delete(int Id);


    }
}
