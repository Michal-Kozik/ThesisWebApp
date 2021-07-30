using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TranslatingWordsController : Controller
    {
        private bool ValidateWords(TranslatingWordsSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfWords; i++)
            {
                if (String.IsNullOrEmpty(model.TranslateFromArray[i]) || String.IsNullOrEmpty(model.TranslateToArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private void SaveExerciseToTxt(TranslatingWordsSettingsViewModel model)
        {
            // Tworzenie nazwy pliku txt.
            DateTime now = DateTime.Now;
            string nameOfFile = "uzytkownik";
            nameOfFile += now.Day.ToString();
            nameOfFile += now.Month.ToString();
            nameOfFile += now.Year.ToString();
            nameOfFile += now.Hour.ToString();
            nameOfFile += now.Minute.ToString();
            nameOfFile += now.Second.ToString();
            nameOfFile += ".txt";

            // Zapisywanie slow w pliku w odpowiednim formacie - slowo;slowo.
            string path = "Content/Resources/" + nameOfFile;
            var logFile = System.IO.File.Create(path);
            var logWriter = new System.IO.StreamWriter(logFile);
            for (int i = 0; i < model.NumberOfWords; i++)
            {
                logWriter.WriteLine(model.TranslateFromArray[i] + ';' + model.TranslateToArray[i]);
            }
            logWriter.Dispose();
        }

        [HttpGet]
        public IActionResult TranslatingWordsSettings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TranslatingWordsAdd(TranslatingWordsSettingsViewModel model)
        {
            if (ValidateWords(model))
            {
                return RedirectToAction("TranslatingWordsResult", model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TranslatingWordsResult(TranslatingWordsSettingsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult TranslatingWordsSave(TranslatingWordsSettingsViewModel model)
        {
            SaveExerciseToTxt(model);
            return View();
        }
    }
}
