using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial
{
    public interface ICargoService
    {
        IEnumerable<CargoModel> List();
        FuenteDatosModel<CargoModel> List(PaginacionModel filtro);
        CargoModel Get(int Id);
        CargoModel Create(CargoModel role);
        CargoModel Update(CargoModel role);
        CargoModel Delete(int Id);


    }
}
