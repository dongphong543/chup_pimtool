using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.DTOs
{
    public partial class Employee
    {
        public Employee()
        {
            Groups = new Group();
            ProjectEmployees = new HashSet<ProjectEmployee>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public string Visa { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Version { get; set; }

        public virtual Group Groups { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
