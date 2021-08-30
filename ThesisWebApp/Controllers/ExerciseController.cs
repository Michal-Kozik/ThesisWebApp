using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Data;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ThesisWebApp.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();



        private int TranslatingWordsCheck(TranslatingWordsSettingsViewModel model)
        {
            int points = 0;
            for (int i = 0; i < model.NumberOfWords; i++)
            {
                if (model.UserAnswers[i] == model.TranslateToArray[i])
                    points++;
            }
            return points;
        }

        private int ReadingTitlesCheck(ReadingTitlesSettingsViewModel model)
        {
            int points = 0;
            for (int i = 0; i < model.NumberOfParagraphs; i++)
            {
                if (model.UserAnswers[i] == model.CorrectTitles[i])
                    points++;
            }
            return points;
        }

        private int MatchingSentencesCheck(MatchingSentencesSettingsViewModel model)
        {
            int points = 0;
            for (int i = 0; i < model.NumberOfSentences; i++)
            {
                if (model.UserAnswers[i] == model.SentencesSecondPart[i])
                    points++;
            }
            return points;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateExercise()
        {
            return View();
        }

        public async Task<IActionResult> ListExercises(int? pageNumber, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name" : sortOrder;
            if (pageNumber < 1)
                pageNumber = 1;
            var exercises = context.Exercises.Include(e => e.ApplicationUser).AsQueryable();
            int pageSize = 2;

            // Sortowanie.
            switch (sortOrder)
            {
                case "name_desc":
                    exercises = exercises.OrderByDescending(e => e.Name);
                    break;
                default:
                    exercises = exercises.OrderBy(e => e.Name);
                    break;
            }
            PaginatedList<Exercise> model = await PaginatedList<Exercise>.CreateAsync(exercises.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [HttpGet]
        public IActionResult ChoosenExercise(int exerciseID, ExerciseType? typeOfExercise)
        {
            // w przypadku testow, kazde zadanie trzeba pobrac z bazy.
            if (typeOfExercise == null)
            {
                var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
                typeOfExercise = exercise.TypeOfExercise;
            }
            //var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            switch (typeOfExercise)
            {
                case ExerciseType.TRANSLATING_WORDS:
                    TempData["TranslatingWords"] = exerciseID;
                    return RedirectToAction("TranslatingWordsAttempt");
                case ExerciseType.READING_TITLES:
                    TempData["ReadingTitles"] = exerciseID;
                    return RedirectToAction("ReadingTitlesAttempt");
                case ExerciseType.MATCHING_SENTENCES:
                    TempData["MatchingSentences"] = exerciseID;
                    return RedirectToAction("MatchingSentencesAttempt");
                default:
                    // w przypadku nie znalezionego typu zadania.
                    return View(exerciseID);
            }
        }

        [HttpGet]
        public IActionResult TranslatingWordsAttempt()
        {
            if (TempData["TranslatingWords"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            int exerciseID = (int)TempData["TranslatingWords"];
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            TranslatingWordsSettingsViewModel model = TranslatingWordsController.ReadExerciseFromTxt(exercise.PathToFile);
            TempData.Remove("TranslatingWords");
            return View(model);
        }

        [HttpPost]
        public IActionResult TranslatingWordsScore(TranslatingWordsSettingsViewModel model)
        {
            // Jesli zadanie jest aktualnie czescia testu
            if (TempData["CurrentExercise"] != null)
            {
                if (TempData["Points"] == null)
                    TempData["Points"] = TranslatingWordsCheck(model);
                else
                    TempData["Points"] = (int)TempData["Points"] + TranslatingWordsCheck(model);

                if (TempData["MaxPoints"] == null)
                    TempData["MaxPoints"] = model.NumberOfWords;
                else
                    TempData["MaxPoints"] = (int)TempData["MaxPoints"] + model.NumberOfWords;
                return RedirectToAction("ContinueExam", "Exam");
            }
            // Jesli zadanie jest samodzielnym skladnikiem
            ViewBag.points = TranslatingWordsCheck(model);
            return View(model);
        }

        [HttpGet]
        public IActionResult ReadingTitlesAttempt()
        {
            if (TempData["ReadingTitles"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            int exerciseID = (int)TempData["ReadingTitles"];
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            ReadingTitlesSettingsViewModel model = ReadingTitlesController.ReadExerciseFromTxt(exercise.PathToFile);
            TempData.Remove("ReadingTitles");
            return View(model);
        }

        [HttpPost]
        public IActionResult ReadingTitlesScore(ReadingTitlesSettingsViewModel model)
        {
            if (TempData["CurrentExercise"] != null)
            {
                if (TempData["Points"] == null)
                    TempData["Points"] = ReadingTitlesCheck(model);
                else
                    TempData["Points"] = (int)TempData["Points"] + ReadingTitlesCheck(model);

                if (TempData["MaxPoints"] == null)
                    TempData["MaxPoints"] = model.NumberOfParagraphs;
                else
                    TempData["MaxPoints"] = (int)TempData["MaxPoints"] + model.NumberOfParagraphs;
                return RedirectToAction("ContinueExam", "Exam");
            }
            ViewBag.points = ReadingTitlesCheck(model);
            return View(model);
        }

        [HttpGet]
        public IActionResult MatchingSentencesAttempt()
        {
            if (TempData["MatchingSentences"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            int exerciseID = (int)TempData["MatchingSentences"];
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            MatchingSentencesSettingsViewModel model = MatchingSentencesController.ReadExerciseFromTxt(exercise.PathToFile);
            TempData.Remove("MatchingSentences");
            return View(model);
        }

        [HttpPost]
        public IActionResult MatchingSentencesScore(MatchingSentencesSettingsViewModel model)
        {
            if (TempData["CurrentExercise"] != null)
            {
                if (TempData["Points"] == null)
                    TempData["Points"] = MatchingSentencesCheck(model);
                else
                    TempData["Points"] = (int)TempData["Points"] + MatchingSentencesCheck(model);

                if (TempData["MaxPoints"] == null)
                    TempData["MaxPoints"] = model.NumberOfSentences;
                else
                    TempData["MaxPoints"] = (int)TempData["MaxPoints"] + model.NumberOfSentences;
                return RedirectToAction("ContinueExam", "Exam");
            }
            ViewBag.points = MatchingSentencesCheck(model);
            return View(model);
        }
    }
}
