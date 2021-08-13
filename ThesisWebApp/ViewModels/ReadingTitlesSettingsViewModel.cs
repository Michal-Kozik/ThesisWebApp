using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.ViewModels
{
    public class ReadingTitlesSettingsViewModel
    {
        public ReadingTitlesSettingsViewModel()
        {
            Paragraphs = new string[6];
            CorrectTitles = new string[6];
            AdditionalTitles = new string[6];
            UserAnswers = new string[6];
        }

        [Display(Name = "Nazwa ćwiczenia")]
        public string ExerciseName { get; set; }

        [Display(Name = "Ilość akapitów")]
        public int NumberOfParagraphs { get; set; }

        [Display(Name = "Ilość nadmiarowych tytułów")]
        public int NumberOfAdditionalTitles { get; set; }

        public string[] Paragraphs { get; set; }
        public string[] CorrectTitles { get; set; }
        public string[] AdditionalTitles { get; set; }
        public string[] UserAnswers { get; set; }
    }
}
