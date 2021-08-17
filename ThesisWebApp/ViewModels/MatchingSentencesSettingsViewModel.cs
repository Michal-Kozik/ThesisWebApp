using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.ViewModels
{
    public class MatchingSentencesSettingsViewModel
    {
        public MatchingSentencesSettingsViewModel()
        {
            SentencesFirstPart = new string[10];
            SentencesSecondPart = new string[10];
            UserAnswers = new string[10];
        }

        [Display(Name = "Nazwa ćwiczenia")]
        public string ExerciseName { get; set; }

        [Display(Name = "Ilość zdań")]
        public int NumberOfSentences { get; set; }

        public string[] SentencesFirstPart { get; set; }
        public string[] SentencesSecondPart { get; set; }
        public string[] UserAnswers { get; set; }
    }
}
