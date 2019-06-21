using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador
{
    public interface ICatalogoService
    {
        IEnumerable<CatalogoModel> List(int Tipo);
        FuenteDatosModel<CatalogoModel> List(PaginacionModel filtro);
        CatalogoModel Get(int Id);
        CatalogoModel Create(CatalogoModel role);
        CatalogoModel Update(CatalogoModel role);
        CatalogoModel Delete(int Id);


    }
}
