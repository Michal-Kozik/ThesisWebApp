using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using ThesisWebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TranslatingWordsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TranslatingWordsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



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
            return "Content/Resources/" + nameOfFile;
        }

        private void SaveExerciseToTxt(TranslatingWordsSettingsViewModel model, string path)
        {
            // Zapisywanie slow w pliku w odpowiednim formacie - slowo;slowo.
            var logFile = System.IO.File.Create(path);
            var logWriter = new System.IO.StreamWriter(logFile);
            for (int i = 0; i < model.NumberOfWords; i++)
            {
                logWriter.WriteLine(model.TranslateFromArray[i] + ';' + model.TranslateToArray[i]);
            }
            logWriter.Dispose();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private async Task SaveExerciseInDatabase(string path)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await GetCurrentUserAsync();
                Exercise exercise = new Exercise { ApplicationUserID = user.Id, Name = "Zadanie testowe", TypeOfExercise = "Translating Words", PathToFile = path };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
            }
        }

        public static TranslatingWordsSettingsViewModel ReadExerciseFromTxt(string path)
        {
            string line;
            int counter = 0;
            var logReader = new System.IO.StreamReader(path);

            // Wpisywanie slow do modelu.
            TranslatingWordsSettingsViewModel model = new TranslatingWordsSettingsViewModel();
            while ((line = logReader.ReadLine()) != null)
            {
                string[] pair = line.Split(';');
                model.TranslateFromArray[counter] = pair[0];
                model.TranslateToArray[counter] = pair[1];
                counter++;
            }
            logReader.Close();
            model.NumberOfWords = counter;
            return model;
        }



        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(int numberOfWords)
        {
            TranslatingWordsSettingsViewModel model = new TranslatingWordsSettingsViewModel();
            model.NumberOfWords = numberOfWords;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(TranslatingWordsSettingsViewModel model)
        {
            if (ValidateWords(model))
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
        public IActionResult Result(TranslatingWordsSettingsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(TranslatingWordsSettingsViewModel model)
        {
            string path = CreateFilePath();
            SaveExerciseToTxt(model, path);
            await SaveExerciseInDatabase(path);

            model = ReadExerciseFromTxt(path);
            return View(model);
        }
    }
}
