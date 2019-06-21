using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Consola
{
    public class DispositivoRepository : Repository<Dispositivo>, IDispositivoRepository
    {
        public DispositivoRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Dispositivo> GetAllActive()
        {
            return _context.Set<Dispositivo>().Where(a => a.EstaActivo == true);
        }
        public IEnumerable<Dispositivo> GetAllWithAssociations()
        {
            return _context.Set<Dispositivo>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.TipoDispositivo);
        }
        public override void Add(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            Context.Dispositivos.Add(entity);
            Context.SaveChanges();
        }
        public override void Update(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Dispositivo GetWithAssociations(int Id)
        {
            return _context.Set<Dispositivo>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.TipoDispositivo).FirstOrDefault(c => c.Id == Id);
        }

        public override void Remove(Dispositivo entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Context { get { return _context as FBSConsolaCBContext; } }
    }
}
