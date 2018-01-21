using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StemProjectV3.Models
{
    public class ProjectAssignment
    {
        public int MentorID { get; set; }
        public int ProjectID { get; set; }
        public Mentor Mentor { get; set; }
        public Project Project { get; set; }
    }
}
