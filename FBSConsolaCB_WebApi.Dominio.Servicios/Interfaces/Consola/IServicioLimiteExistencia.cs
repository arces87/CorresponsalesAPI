using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola
{
    public interface IServicioLimiteExistencia
    {
        Task<IEnumerable<ModeloLimiteExistencia>> List();
        Task<ModeloFuenteDatos<ModeloLimiteExistencia>> List(ModeloPaginacion filtro);
        Task<ModeloLimiteExistencia> Get(int Id);
        Task<ModeloLimiteExistencia> Create(ModeloLimiteExistencia role);
        Task<ModeloLimiteExistencia> Update(ModeloLimiteExistencia role);
        Task<ModeloLimiteExistencia> Delete(int Id);
    }
}
