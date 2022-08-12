using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIMBackend.DTOs
{
    public class ProjectAddDTO
    {
        public ProjectDTO project { get; set; }
        public string memString { get; set; }

    public ProjectAddDTO()
        {
            this.project = null;
            this.memString = "";
        }

        public ProjectAddDTO(ProjectDTO p, string s)
        {
            this.project = p;
            this.memString = s;
        }
    }
}
