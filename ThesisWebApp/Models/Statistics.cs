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

        [Display(Name = "Level UNKNOWN")]
        [Required]
        public int ExercisesUknown { get; set; }

        [Display(Name = "Level A1")]
        [Required]
        public int ExercisesA1 { get; set; }

        [Display(Name = "Level A2")]
        [Required]
        public int ExercisesA2 { get; set; }

        [Display(Name = "Level B1")]
        [Required]
        public int ExercisesB1 { get; set; }

        [Display(Name = "Level B2")]
        [Required]
        public int ExercisesB2 { get; set; }

        [Display(Name = "Level C1")]
        [Required]
        public int ExercisesC1 { get; set; }

        [Display(Name = "Level C2")]
        [Required]
        public int ExercisesC2 { get; set; }


        // Jedne statystyki naleza do jednego uzytkownika.
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
