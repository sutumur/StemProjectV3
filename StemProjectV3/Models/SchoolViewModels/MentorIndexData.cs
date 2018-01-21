using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StemProjectV3.Models.SchoolViewModels
{
    public class MentorIndexData
    {
        public IEnumerable<Mentor> Mentors { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
