using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwebapi.Models
{
    public class Faculty
    {
        public int FacultyID { get; set; }

        public String FacultyName { get; set; }

        public String DepartmentName { get; set; }

        public String DateOfJoining { get; set; }

        public String PhotoFileName { get; set; }
    }
}