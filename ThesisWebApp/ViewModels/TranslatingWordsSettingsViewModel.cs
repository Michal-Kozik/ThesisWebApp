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

        [Display(Name = "Ilość słów")]
        public int NumberOfWords { get; set; }

        public string[] TranslateFromArray { get; set; }
        public string[] TranslateToArray { get; set; }
        public string[] UserAnswers { get; set; }
    }
}
