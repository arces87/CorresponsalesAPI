using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioPersona : IRepositorio<Persona>
    {
        Task<IEnumerable<Persona>> GetAllActive();
        Task<IEnumerable<Persona>> GetAllWithAssociations();
        Task<IEnumerable<Supervisor>> GetSupervisoresWithAssociations();
        Task<IEnumerable<Corresponsal>> GetCorresponsalesWithAssociations();
        Task<object> GetWithAssociations(int Id);
        Task<Persona> GetForUserName(string userName);
        Task<bool> GetEstadoCorresponsalPorIdUsuario(string idUsuario);
        Task Add(Supervisor entity);
        Task Add(Corresponsal entity);
        Task Update(Supervisor entity);
        Task Update(Corresponsal entity);

        Task<Dispositivo> ObtenerDispositivo(int idCorresponsal);
    }
}
