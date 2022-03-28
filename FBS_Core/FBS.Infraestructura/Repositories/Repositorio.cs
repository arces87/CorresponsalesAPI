using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using FBS.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;

namespace FBS.Infraestructura.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected readonly DbContext _contexto;

        public Repositorio(DbContext context)
        {
            _contexto = context;
        }

        public virtual async Task<string> Add(T entidad)
        {
            _contexto.Set<T>().Add(entidad);
            await _contexto.SaveChangesAsync();
            return "";
        }

        public virtual async Task AddRange(IEnumerable<T> Entities)
        {
            _contexto.Set<T>().AddRange(Entities);
            await _contexto.SaveChangesAsync();
        }

        public virtual async Task<T> Get(string Id)
        {
            //return await _contexto.Set<T>().Where($"t => t.Id == {Id}").FirstOrDefaultAsync();
            return await _contexto.Set<T>().FirstOrDefaultAsync(t => t.GetType().GetProperty("Id").GetValue(t).ToString() == Id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _contexto.Set<T>().ToListAsync();
        }

        public virtual async Task Remove(T entidad)
        {
            _contexto.Set<T>().Remove(entidad);
            await _contexto.SaveChangesAsync();
        }

        public virtual async Task RemoveRange(IEnumerable<T> Entities)
        {
            _contexto.Set<T>().RemoveRange(Entities);
            await _contexto.SaveChangesAsync();
        }

        public virtual async Task Update(T entidad)
        {
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }
        public virtual async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate)
        {
            return await _contexto.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
