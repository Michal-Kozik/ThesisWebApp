using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;
using System.IO;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ReadingTitlesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ReadingTitlesController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private bool ValidateInputs(ReadingTitlesSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfParagraphs; i++)
            {
                if (String.IsNullOrEmpty(model.Paragraphs[i]) || String.IsNullOrEmpty(model.CorrectTitles[i]))
                {
                    return false;
                }
            }
            for (int i = 0; i < model.NumberOfAdditionalTitles; i++)
            {
                if (String.IsNullOrEmpty(model.AdditionalTitles[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private string CreateFilePath()
        {
            DateTime now = DateTime.Now;
            string nameOfFile = "exercise-";
            nameOfFile += now.Day.ToString();
            nameOfFile += now.Month.ToString();
            nameOfFile += now.Year.ToString();
            nameOfFile += now.Hour.ToString();
            nameOfFile += now.Minute.ToString();
            nameOfFile += now.Second.ToString();
            nameOfFile += ".txt";
            return "Content/Resources/ReadingTitles/" + nameOfFile;
        }

        private void SaveExerciseToTxt(ReadingTitlesSettingsViewModel model, string path)
        {
            // Format: 1 linijka - <N_akapitow>;<M_nadmiarowych_odpowiedzi>
            //         N linii - <akapit>;<odpowiedz>
            //         M linii - <nadmiarowa_odpowiedz>
            var logFile = System.IO.File.Create(path);
            var logWriter = new StreamWriter(logFile);
            logWriter.WriteLine(model.NumberOfParagraphs.ToString() + ';' + model.NumberOfAdditionalTitles.ToString());
            for (int i = 0; i < model.NumberOfParagraphs; i++)
            {
                logWriter.WriteLine(model.Paragraphs[i] + ';' + model.CorrectTitles[i]);
            }
            for (int i = 0; i < model.NumberOfAdditionalTitles; i++)
            {
                logWriter.WriteLine(model.AdditionalTitles[i]);
            }
            logWriter.Dispose();
        }

        public static ReadingTitlesSettingsViewModel ReadExerciseFromTxt(string path)
        {
            string line;
            var logReader = new StreamReader(path);

            // Wpisywanie danych do modelu
            ReadingTitlesSettingsViewModel model = new ReadingTitlesSettingsViewModel();
            line = logReader.ReadLine();
            string[] pair = line.Split(';');
            model.NumberOfParagraphs = Int32.Parse(pair[0]);
            model.NumberOfAdditionalTitles = Int32.Parse(pair[1]);
            for (int i = 0; i < model.NumberOfParagraphs; i++)
            {
                line = logReader.ReadLine();
                pair = line.Split(';');
                model.Paragraphs[i] = pair[0];
                model.CorrectTitles[i] = pair[1];
            }
            for (int i = 0; i < model.NumberOfAdditionalTitles; i++)
            {
                line = logReader.ReadLine();
                model.AdditionalTitles[i] = line;
            }
            logReader.Close();
            return model;
        }



        [HttpGet]
        public IActionResult Settings()
        {
            return View(new ReadingTitlesSettingsViewModel());
        }

        [HttpGet]
        public IActionResult Add(int numberOfParagraphs, int numberOfAdditionalTitles, string exerciseName)
        {
            ReadingTitlesSettingsViewModel model = new ReadingTitlesSettingsViewModel();
            model.NumberOfParagraphs = numberOfParagraphs;
            model.NumberOfAdditionalTitles = numberOfAdditionalTitles;
            model.ExerciseName = exerciseName;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ReadingTitlesSettingsViewModel model)
        {
            if (ValidateInputs(model))
            {
                return RedirectToAction("Result", model);
            }
            else
            {
                ModelState.AddModelError("", "Wypełnij wszystkie pola!");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Result(ReadingTitlesSettingsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(ReadingTitlesSettingsViewModel model)
        {
            string path = CreateFilePath();
            SaveExerciseToTxt(model, path);

            model = ReadExerciseFromTxt(path);
            return View(model);
        }
    }
}
