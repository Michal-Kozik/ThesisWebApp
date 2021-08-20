using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.ViewModels
{
    public class TranslatingWordsSettingsViewModel
    {
        public TranslatingWordsSettingsViewModel()
        {
            TranslateFromArray = new string[15];
            TranslateToArray = new string[15];
            UserAnswers = new string[15];
        }

        [Display(Name = "Nazwa ćwiczenia")]
        [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Nazwa ćwiczenia może zawierać jedynie litery alfabetu angielskiego.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa ćwiczenia musi mieć długość od 5 do 50 znaków.")]
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana.")]
        public string ExerciseName { get; set; }

        [Display(Name = "Ilość słów")]
        public int NumberOfWords { get; set; }

        public string[] TranslateFromArray { get; set; }
        public string[] TranslateToArray { get; set; }
        public string[] UserAnswers { get; set; }
    }
}
