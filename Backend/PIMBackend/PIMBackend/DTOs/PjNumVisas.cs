using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIMBackend.DTOs
{
    public partial class PjNumVisas
    {
        public PjNumVisas() { }
        public PjNumVisas(decimal ProjectPjNum, string[] EmployeeVisa)
        {
            this.ProjectPjNum = ProjectPjNum;
            this.EmployeeVisas = EmployeeVisa;
        }

        public decimal ProjectPjNum { get; set; }
        public string[] EmployeeVisas { get; set; }

    }
}
