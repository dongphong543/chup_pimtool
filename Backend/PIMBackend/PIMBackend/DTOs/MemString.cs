using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIMBackend.DTOs
{
    public class MemString
    {
        public string VisaStr;

        public MemString()
        {
            this.VisaStr = "";
        }

        public MemString(string s)
        {
            this.VisaStr = s;
        }
    }
}
