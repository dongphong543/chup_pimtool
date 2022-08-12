using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIMBackend.DTOs
{
    public class MemStringDTO
    {
        public string VisaStr { get; set; }

        public MemStringDTO()
        {
            this.VisaStr = "";
        }

        public MemStringDTO(string s)
        {
            this.VisaStr = s;
        }
    }
}
