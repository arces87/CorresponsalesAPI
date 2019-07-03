using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Nomenclador
{
    public interface IServicioTipoCatalogo
    {
        Task<IEnumerable<ModeloTipoCatalogo>> List();
        Task<ModeloFuenteDatos<ModeloTipoCatalogo>> List(ModeloPaginacion filtro);
        Task<ModeloTipoCatalogo> Get(int Id);
        Task<ModeloTipoCatalogo> Update(ModeloTipoCatalogo role);


    }
}
