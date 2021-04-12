using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FBS.Infraestructura.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> Get(string Id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate);

        Task<string> Add(T Entity);
        Task AddRange(IEnumerable<T> Entities);

        Task Update(T Entity);

        Task Remove(T Entity);
        Task RemoveRange(IEnumerable<T> Entities);
    }
}
