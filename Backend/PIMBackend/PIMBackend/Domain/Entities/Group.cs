using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.Domain.Entities
{
    public partial class Group: BaseEntity
    {
        public Group()
        {
            Projects = new HashSet<Project>();
        }

        public override decimal Id { get; set; }
        public decimal GroupLeaderId { get; set; }
        public decimal Version { get; set; }

        public virtual Employee GroupLeader { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
