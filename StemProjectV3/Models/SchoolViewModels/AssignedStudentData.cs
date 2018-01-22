using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StemProjectV3.Models.SchoolViewModels
{
    public class AssignedStudentData
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Assigned { get; set; }
    }
}
