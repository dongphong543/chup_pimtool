using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIMBackend.DTOs
{
    public partial class ProjectEmployee
    {
        public decimal ProjectId { get; set; }
        public decimal EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
