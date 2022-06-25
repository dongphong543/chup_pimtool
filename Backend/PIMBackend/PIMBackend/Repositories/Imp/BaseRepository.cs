using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories.Imp
{
    /// <summary>
    ///     Base of all repositories
    /// </summary>
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PIMContext _pimContext;
        protected readonly DbSet<T> Set;

        protected BaseRepository(PIMContext pimContext)
        {
            _pimContext = pimContext;
            Set = _pimContext.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return Set;
        }

        public T Get(decimal id)
        {
            return Set.SingleOrDefault(x => x.Id == id);
        }

        //it used to be IEnumerable<T> type:
        public void Add(params T[] entities)
        {
            Set.AddRange(entities);
        }

        public void Delete(params decimal[] ids)
        {
            Set.RemoveRange(Set.Where(x => ids.Contains(x.Id)));
        }

        public void Delete(params T[] entities)
        {
            Set.RemoveRange(entities);
        }

        public void SaveChange()
        {
            _pimContext.SaveChanges();
        }
    }
}