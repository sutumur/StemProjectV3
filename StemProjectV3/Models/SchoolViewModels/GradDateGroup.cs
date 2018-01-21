using System;
using System.ComponentModel.DataAnnotations;

namespace StemProjectV3.Models.SchoolViewModels
{
    public class GradDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? GradDate { get; set; }
        public int StudentCount { get; set; }
    }
}
