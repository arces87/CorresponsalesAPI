using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola
{
    public interface IServicioLog
    {
        Task<IEnumerable<ModeloLog>> List();
        Task<ModeloFuenteDatos<ModeloLog>> List(ModeloPaginacion filtro);
        Task<ModeloLog> Get(int Id);
        Task<ModeloLog> Create(ModeloLog role);
    }
}
