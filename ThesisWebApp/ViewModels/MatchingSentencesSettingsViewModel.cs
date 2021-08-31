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
        [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Nazwa ćwiczenia może zawierać jedynie litery alfabetu angielskiego.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa ćwiczenia musi mieć długość od 5 do 50 znaków.")]
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana.")]
        public string ExerciseName { get; set; }

        [Display(Name = "Ilość zdań")]
        public int NumberOfSentences { get; set; }

        [Display(Name = "Poziom")]
        public int Level { get; set; }

        public string[] SentencesFirstPart { get; set; }
        public string[] SentencesSecondPart { get; set; }
        public string[] UserAnswers { get; set; }


        public string[] ShuffleAnswers()
        {
            // Dodac warunek sprawdzajacy czy tablice posiadaja dane.
            Random rnd = new Random();
            string[] result = new string[NumberOfSentences];
            bool[] leftSentences = new bool[NumberOfSentences];
            int counter = 0;
            for (int i = 0; i < NumberOfSentences; i++)
            {
                if (rnd.Next(2) == 0)
                {
                    result[counter] = SentencesSecondPart[i];
                    counter++;
                }
                else
                {
                    leftSentences[i] = true;
                }
            }
            for (int i = 0; i < NumberOfSentences; i++)
            {
                if (leftSentences[i] == true)
                {
                    result[counter] = SentencesSecondPart[i];
                    counter++;
                }
            }
            return result;
        }
    }
}
