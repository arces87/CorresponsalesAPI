using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Nomenclador
{
    public interface IServicioCatalogo
    {
        Task<IEnumerable<ModeloCatalogo>> List(int Tipo);
        Task<ModeloFuenteDatos<ModeloCatalogo>> List(ModeloPaginacion filtro);
        Task<ModeloCatalogo> Get(int Id);
        Task<ModeloCatalogo> Create(ModeloCatalogo role);
        Task<ModeloCatalogo> Update(ModeloCatalogo role);
        Task<ModeloCatalogo> Delete(int Id);


    }
}
