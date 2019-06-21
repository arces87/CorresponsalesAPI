using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador
{
    public class CatalogoRepository : Repository<Catalogo>, ICatalogoRepository
    {
        public CatalogoRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Catalogo> GetAllActive()
        {
            return _context.Set<Catalogo>().Where(a => a.EstaActivo == true);
        }
        public IEnumerable<Catalogo> GetAllWithAssociations()
        {
            return _context.Set<Catalogo>().Where(a => a.EstaActivo == true)
                .Include(c => c.TipoCatalogo);
        }
        public Catalogo GetWithAssociations(int Id)
        {
            return _context.Set<Catalogo>().Where(a => a.EstaActivo == true)
                .Include(c => c.TipoCatalogo).FirstOrDefault(c => c.Id == Id);
        }

        public override void Remove(Catalogo entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }

        public override void Add(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            Context.Catalogos.Add(entity);
            Context.SaveChanges();
        }
        public override void Update(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Context { get { return _context as FBSConsolaCBContext; } }
    }
}
