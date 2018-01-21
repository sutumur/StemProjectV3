using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StemProjectV3.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int ProjectID { get; set; }
        public int StudentID { get; set; }
        
        public Student Student { get; set; }
        public Project Project { get; set; }
    }
}
