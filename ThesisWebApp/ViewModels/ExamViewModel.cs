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
            Archived = false;
        }

        public int ExamID { get; set; }

        [Display(Name = "Nazwa testu")]
        [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Nazwa testu może zawierać jedynie litery alfabetu angielskiego.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa testu musi mieć długość od 5 do 50 znaków.")]
        [Required(ErrorMessage = "Nazwa testu jest wymagana.")]
        public string Name { get; set; }

        [Display(Name = "Hasło do testu")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{5,}$", ErrorMessage = "Hasło musi zawierać conajmniej 5 znaków, w tym conajmniej 1 literę i 1 cyfrę i nie może mieć spacji.")]
        [StringLength(25, ErrorMessage = "Długość hasła nie może być dłuższa niż 25 znaków.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Widoczność testu")]
        public bool Visible { get; set; }

        public bool Archived { get; set; }

        [Display(Name = "Wiele podejść")]
        public bool ManyAttempts { get; set; }

        public string ExercisePattern { get; set; }

        public List<Exercise> Exercises { get; set; }
    }
}
