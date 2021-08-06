using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public class Exercise
    {
        [Key]
        [Display(Name = "Exercise ID")]
        public int ExerciseID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Path to file")]
        public string PathToFile { get; set; }

        [Required]
        [Display(Name = "Type of exercise")]
        public string TypeOfExercise { get; set; }



        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "Author ID")]
        public string ApplicationUserID { get; set; }

        // Jedno zadanie bedzie w wielu egzaminach.
        public virtual ICollection<ExerciseExams> ExerciseExams { get; set; }
    }
}
