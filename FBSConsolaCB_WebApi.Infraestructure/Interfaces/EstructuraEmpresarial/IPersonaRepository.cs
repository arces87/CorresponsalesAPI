using FBS_Core.Base.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IPersonaRepository : IRepository<Persona>
    {
        IEnumerable<Persona> GetAllActive();
        IEnumerable<Persona> GetAllWithAssociations();
        object GetWithAssociations(int Id);
        Persona GetForUserName(string userName);

        void Add(Supervisor entity);
        void Add(Corresponsal entity);
        void Update(Supervisor entity);
        void Update(Corresponsal entity);
    }
}
