using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.Models
{
    public class Statistics
    {
        [Key]
        [Display(Name = "Statistics ID")]
        public int StatisticsID { get; set; }

        [Display(Name = "Number Of Tests")]
        public int NumberOfTests { get; set; }

        [Display(Name = "Number Of Exercises")]
        public int NumberOfExercises { get; set; }

        
        // Jedne statystyki naleza do jednego uzytkownika.
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
