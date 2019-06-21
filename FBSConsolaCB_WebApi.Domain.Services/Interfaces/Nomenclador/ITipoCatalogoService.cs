using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador
{
    public interface ITipoCatalogoService
    {
        IEnumerable<TipoCatalogoModel> List();
        FuenteDatosModel<TipoCatalogoModel> List(PaginacionModel filtro);
        TipoCatalogoModel Get(int Id);
        Task<TipoCatalogoModel> Update(TipoCatalogoModel role);


    }
}
