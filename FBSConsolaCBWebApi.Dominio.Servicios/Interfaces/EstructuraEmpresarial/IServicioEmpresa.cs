using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial
{
    public interface IServicioEmpresa
    {
        Task<IEnumerable<ModeloEmpresa>> List();
        Task<ModeloFuenteDatos<ModeloEmpresa>> List(ModeloPaginacion filtro);
        Task<ModeloEmpresa> Get(int Id);
        Task<ModeloEmpresa> Create(ModeloEmpresa role);
        Task<ModeloEmpresa> Update(ModeloEmpresa role);
        Task<ModeloEmpresa> Delete(int Id);


    }
}
