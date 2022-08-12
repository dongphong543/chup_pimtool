
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.Domain.Entities
{
    /// <summary>
    ///     Base entity of all entities
    /// </summary>
    public abstract class BaseEntity
    {
        public virtual decimal Id { get; set; }
    }
}