using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMBackend.DTOs
{
    public partial class GroupDTO
    {
        public decimal Id { get; set; }
        public decimal GroupLeaderId { get; set; }
        public byte[] Version { get; set; }

        public virtual EmployeeDTO GroupLeader { get; set; }

    }
}
