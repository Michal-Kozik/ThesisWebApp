using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public class Exam
    {
        [Key]
        [Display(Name = "Exam ID")]
        public int ExamID { get; set; }

        [Required]
        [Display(Name = "Nazwa testu")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Exercises Pattern")]
        public string ExercisesPattern { get; set; }

        [Required]
        [Display(Name = "Widoczność testu")]
        public bool Visible { get; set; }

        [Display(Name = "Hasło do testu")]
        public string Password { get; set; }

        [Display(Name = "Zarchiwizowany")]
        public bool Archived { get; set; }


        // Kazdy test ma swojego autora.
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "Author ID")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // Kazdy test moze miec wiele zdobytych ocen.
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
