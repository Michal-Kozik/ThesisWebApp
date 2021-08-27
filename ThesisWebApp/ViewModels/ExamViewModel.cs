using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;

namespace ThesisWebApp.ViewModels
{
    public class ExamViewModel
    {
        public ExamViewModel()
        {
            Exercises = new List<Exercise>();
        }

        [Display(Name = "Nazwa testu")]
        [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Nazwa testu może zawierać jedynie litery alfabetu angielskiego.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa testu musi mieć długość od 5 do 50 znaków.")]
        [Required(ErrorMessage = "Nazwa testu jest wymagana.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Widoczność testu")]
        public bool Visible { get; set; }

        public string ExercisePattern { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
