using FBS_Core.Base.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.Consola;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola
{
    public interface IDispositivoRepository : IRepository<Dispositivo>
    {
        IEnumerable<Dispositivo> GetAllActive();
        IEnumerable<Dispositivo> GetAllWithAssociations();
        Dispositivo GetWithAssociations(int Id);

    }
}
