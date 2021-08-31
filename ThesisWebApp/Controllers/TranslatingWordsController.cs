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
            return "Content/Resources/TranslatingWords/" + nameOfFile;
        }

        private void SaveExerciseToTxt(TranslatingWordsSettingsViewModel model, string path)
        {
            // Zapisywanie slow w pliku w odpowiednim formacie - slowo;slowo.
            var logFile = System.IO.File.Create(path);
            var logWriter = new StreamWriter(logFile);
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

        private async Task SaveExerciseInDatabase(TranslatingWordsSettingsViewModel model, string path)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await GetCurrentUserAsync();
                Exercise exercise = new Exercise { ApplicationUserID = user.Id,
                                                   Name = model.ExerciseName, 
                                                   TypeOfExercise = ExerciseType.TRANSLATING_WORDS,
                                                   PathToFile = path, 
                                                   Visible = true,
                                                   Created = DateTime.Today,
                                                   LevelOfExercise = (ExerciseLevel)model.Level };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
            }
        }

        public static TranslatingWordsSettingsViewModel ReadExerciseFromTxt(string path)
        {
            string line;
            int counter = 0;
            var logReader = new StreamReader(path);

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

        [HttpPost]
        public IActionResult Settings(TranslatingWordsSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["NumberOfWords"] = model.NumberOfWords;
                TempData["ExerciseName"] = model.ExerciseName;
                TempData["Level"] = model.Level;
                return RedirectToAction("Add");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (TempData["ExerciseName"] == null || TempData["NumberOfWords"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            TranslatingWordsSettingsViewModel model = new TranslatingWordsSettingsViewModel();
            model.NumberOfWords = (int)TempData["NumberOfWords"];
            model.ExerciseName = TempData["ExerciseName"].ToString();
            model.Level = (int)TempData["Level"];
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TranslatingWordsSettingsViewModel model)
        {
            if (ValidateWords(model))
            {
                string path = CreateFilePath();
                SaveExerciseToTxt(model, path);
                await SaveExerciseInDatabase(model, path);
                TempData["Path"] = path;
                return RedirectToAction("Save");
            }
            else
            {
                ModelState.AddModelError("", "Wypełnij wszystkie pola!");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Save()
        {
            TranslatingWordsSettingsViewModel model;
            model = ReadExerciseFromTxt(TempData["Path"].ToString());
            TempData.Clear();
            return View(model);
        }
    }
}
