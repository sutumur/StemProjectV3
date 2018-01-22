using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StemProjectV3.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name="Number")]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        public string Abstract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Project Date")]
        public DateTime ProjectDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
