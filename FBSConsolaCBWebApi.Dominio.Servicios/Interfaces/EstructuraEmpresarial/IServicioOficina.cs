using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial
{
    public interface IServicioOficina
    {
        Task<IEnumerable<ModeloOficina>> List();
        Task<ModeloFuenteDatos<ModeloOficina>> List(ModeloPaginacion filtro);
        Task<ModeloOficina> Get(int Id);
        Task<ModeloOficina> Create(ModeloOficina role);
        Task<ModeloOficina> Update(ModeloOficina role);
        Task<ModeloOficina> Delete(int Id);
    }
}
