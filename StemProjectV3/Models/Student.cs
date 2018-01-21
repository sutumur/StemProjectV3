using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StemProjectV3.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="First Name cannot be longer than 50 characters")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        [Display(Name = "Grad Date")]
        public DateTime GradDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
