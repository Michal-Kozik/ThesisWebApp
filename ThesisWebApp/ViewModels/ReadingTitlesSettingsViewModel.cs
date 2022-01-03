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
        [RegularExpression(@"[a-zA-Z\s]*$", ErrorMessage = "Nazwa ćwiczenia może zawierać jedynie litery alfabetu angielskiego.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa ćwiczenia musi mieć długość od 5 do 50 znaków.")]
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana.")]
        public string ExerciseName { get; set; }

        [Display(Name = "Ilość akapitów")]
        public int NumberOfParagraphs { get; set; }

        [Display(Name = "Ilość nadmiarowych tytułów")]
        public int NumberOfAdditionalTitles { get; set; }

        [Display(Name = "Poziom")]
        public int Level { get; set; }

        [Display(Name = "Widoczny")]
        public bool Visible { get; set; }

        public string[] Paragraphs { get; set; }
        public string[] CorrectTitles { get; set; }
        public string[] AdditionalTitles { get; set; }
        public string[] UserAnswers { get; set; }


        public string[] ShuffleAnswers()
        {
            // Dodac warunek sprawdzajacy czy tablice posiadaja dane.
            int correctTitleCount = 0;
            int additionalTitleCount = 0;
            string[] result = new string[NumberOfParagraphs + NumberOfAdditionalTitles];
            Random rnd = new Random();
            for (int i = 0; i < result.Length; i++)
            {
                if (rnd.Next(2) == 0)
                {
                    if (correctTitleCount < NumberOfParagraphs)
                    {
                        result[i] = CorrectTitles[correctTitleCount];
                        correctTitleCount++;
                    }
                    else
                    {
                        result[i] = AdditionalTitles[additionalTitleCount];
                        additionalTitleCount++;
                    }
                }
                else
                {
                    if (additionalTitleCount < NumberOfAdditionalTitles)
                    {
                        result[i] = AdditionalTitles[additionalTitleCount];
                        additionalTitleCount++;
                    }
                    else
                    {
                        result[i] = CorrectTitles[correctTitleCount];
                        correctTitleCount++;
                    }
                }
            }
            return result;
        }
    }
}
