using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Consola
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
