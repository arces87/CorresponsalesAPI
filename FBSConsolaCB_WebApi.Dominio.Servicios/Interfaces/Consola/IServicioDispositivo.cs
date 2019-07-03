using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola
{
    public interface IServicioDispositivo
    {
        Task<IEnumerable<ModeloDispositivo>> List();
        Task<ModeloFuenteDatos<ModeloDispositivo>> List(ModeloPaginacion filtro);
        Task<ModeloDispositivo> Get(int Id);
        Task<ModeloDispositivo> Create(ModeloDispositivo role);
        Task<ModeloDispositivo> Update(ModeloDispositivo role);
        Task<ModeloDispositivo> Delete(int Id);
    }
}
