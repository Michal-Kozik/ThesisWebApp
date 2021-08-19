using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Data;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class MatchingSentencesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public MatchingSentencesController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private bool ValidateInputs(MatchingSentencesSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfSentences; i++)
            {
                if (String.IsNullOrEmpty(model.SentencesFirstPart[i]) || String.IsNullOrEmpty(model.SentencesSecondPart[i]))
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
            return "Content/Resources/MatchingSentences/" + nameOfFile;
        }

        private void SaveExerciseToTxt(MatchingSentencesSettingsViewModel model, string path)
        {
            // Format: 1 linijka - <N_zdan>
            //         N linii - <1_czesc_zdania>;<2_czesc_zdania>
            var logFile = System.IO.File.Create(path);
            var logWriter = new StreamWriter(logFile);
            logWriter.WriteLine(model.NumberOfSentences.ToString());
            for (int i = 0; i < model.NumberOfSentences; i++)
            {
                logWriter.WriteLine(model.SentencesFirstPart[i] + ';' + model.SentencesSecondPart[i]);
            }
            logWriter.Dispose();
        }

        public static MatchingSentencesSettingsViewModel ReadExerciseFromTxt(string path)
        {
            string line;
            var logReader = new StreamReader(path);

            // Wpisywanie danych do modelu
            MatchingSentencesSettingsViewModel model = new MatchingSentencesSettingsViewModel();
            line = logReader.ReadLine();
            model.NumberOfSentences = Int32.Parse(line);
            for (int i = 0; i < model.NumberOfSentences; i++)
            {
                line = logReader.ReadLine();
                string[] pair = line.Split(';');
                model.SentencesFirstPart[i] = pair[0];
                model.SentencesSecondPart[i] = pair[1];
            }
            logReader.Close();
            return model;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private async Task SaveExerciseInDatabase(string exerciseName, string path)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await GetCurrentUserAsync();
                Exercise exercise = new Exercise { ApplicationUserID = user.Id, Name = exerciseName, TypeOfExercise = ExerciseType.MATCHING_SENTENCES, PathToFile = path };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
            }
        }



        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Settings([Bind("ExerciseName, NumberOfSentences")] MatchingSentencesSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Add", new { numberOfSentences = model.NumberOfSentences, exerciseName = model.ExerciseName });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Add(int numberOfSentences, string exerciseName)
        {
            MatchingSentencesSettingsViewModel model = new MatchingSentencesSettingsViewModel();
            model.NumberOfSentences = numberOfSentences;
            model.ExerciseName = exerciseName;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(MatchingSentencesSettingsViewModel model)
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
        public IActionResult Result(MatchingSentencesSettingsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(MatchingSentencesSettingsViewModel model)
        {
            string path = CreateFilePath();
            SaveExerciseToTxt(model, path);
            await SaveExerciseInDatabase(model.ExerciseName, path);

            model = ReadExerciseFromTxt(path);
            return View(model);
        }
    }
}
