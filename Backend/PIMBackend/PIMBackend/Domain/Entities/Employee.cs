using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.Domain.Entities
{
    public partial class Employee: BaseEntity
    {
        public Employee()
        {
            Group = new Group();
            Projects = new HashSet<Project>();
        }

        public override decimal Id { get; set; }
        public string Visa { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Version { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
