using System.Collections.Generic;
using System.Linq;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Get();

        /// <summary>
        ///     Get specific entity by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(decimal id);

        /// <summary>
        ///     Add entity to set
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>

        //it used to be IEnumerable<T> type:
        void Add(params T[] entities);

        /// <summary>
        ///     Delete entities by its ids
        /// </summary>
        /// <param name="ids"></param>
        void Delete(params decimal[] ids);

        /// <summary>
        ///     Delete entities
        /// </summary>
        /// <param name="entities"></param>
        void Delete(params T[] entities);

        /// <summary>
        ///     Save context to database
        /// </summary>
        void SaveChange();
    }
}