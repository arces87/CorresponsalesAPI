using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola
{
    public interface IServicioLimiteTransaccional
    {
        Task<IEnumerable<ModeloLimiteTransaccional>> List();
        Task<ModeloFuenteDatos<ModeloLimiteTransaccional>> List(ModeloPaginacion filtro);
        Task<ModeloLimiteTransaccional> Get(int Id);
        Task<ModeloLimiteTransaccional> Create(ModeloLimiteTransaccional role);
        Task<ModeloLimiteTransaccional> Update(ModeloLimiteTransaccional role);
        Task<ModeloLimiteTransaccional> Delete(int Id);
    }
}
