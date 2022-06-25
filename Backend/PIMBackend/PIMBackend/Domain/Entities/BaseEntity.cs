
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.Domain.Entities
{
    /// <summary>
    ///     Base entity of all entities
    /// </summary>
    public abstract class BaseEntity
    {
        // it used to be not virtual:
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual decimal Id { get; set; }
    }
}