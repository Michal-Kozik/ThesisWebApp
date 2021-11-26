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
using Microsoft.AspNetCore.Identity;

namespace ThesisWebApp.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> userManager;

        public ExerciseController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private async Task CreateStatisticsForUser(string userID)
        {
            using (var context = new ApplicationDbContext())
            {
                Statistics statistics = new Statistics
                {
                    ApplicationUserID = userID,
                    ExercisesA1 = 0,
                    ExercisesA2 = 0,
                    ExercisesB1 = 0,
                    ExercisesB2 = 0,
                    ExercisesC1 = 0,
                    ExercisesC2 = 0,
                    ExercisesUknown = 0
                };
                context.Statistics.Add(statistics);
                await context.SaveChangesAsync();
            }
        }

        private async Task UpdateStatisticsForUser(string userID, int level)
        {
            var statistics = context.Statistics.Where(s => s.ApplicationUserID == userID).FirstOrDefault();
            switch (level)
            {
                case 1:
                    statistics.ExercisesA1++;
                    break;
                case 2:
                    statistics.ExercisesA2++;
                    break;
                case 3:
                    statistics.ExercisesB1++;
                    break;
                case 4:
                    statistics.ExercisesB2++;
                    break;
                case 5:
                    statistics.ExercisesC1++;
                    break;
                case 6:
                    statistics.ExercisesC2++;
                    break;
                default:
                    statistics.ExercisesUknown++;
                    break;
            }
            await context.SaveChangesAsync();
        }

        private async Task<bool> CanRemoveExercise(int exerciseID)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await GetCurrentUserAsync();
                List<Exam> userExams = context.Exams.Where(e => e.ApplicationUserID == user.Id).ToList();
                foreach (Exam exam in userExams)
                {
                    string[] ids = exam.ExercisesPattern.Split('-');
                    if (ids.Contains(exerciseID.ToString()))
                    {
                        return false;
                    }
                }
                return true;
            }
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateExercise()
        {
            return View();
        }

        public async Task<IActionResult> MyExercises(int? pageNumber, string typeParam)
        {
            ViewData["ExerciseTypeParam"] = String.IsNullOrEmpty(typeParam) ? "" : typeParam;
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybieranie danych.
            IQueryable<Exercise> exercises;
            //IQueryable<Exam> exams;
            var user = await GetCurrentUserAsync();
            exercises = context.Exercises.Where(ex => ex.ApplicationUserID == user.Id).AsQueryable();
            //exams = context.Exams.Where(e => e.ApplicationUserID == user.Id).AsQueryable();
            switch (typeParam)
            {
                case "translatingWords":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.TRANSLATING_WORDS).AsQueryable();
                    break;
                case "readingTitles":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.READING_TITLES).AsQueryable();
                    break;
                case "matchingSentences":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.MATCHING_SENTENCES).AsQueryable();
                    break;
                default:
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).AsQueryable();
                    break;
            }

            PaginatedList<Exercise> model = await PaginatedList<Exercise>.CreateAsync(exercises.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        public async Task<IActionResult> ListExercises(int? pageNumber, string sortOrder, string typeParam)
        {
            ViewData["ExerciseTypeParam"] = String.IsNullOrEmpty(typeParam) ? "" : typeParam;
            ViewData["SortParam"] = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybieranie danych.
            IQueryable<Exercise> exercises;
            switch (typeParam)
            {
                case "translatingWords":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.TRANSLATING_WORDS && ex.Visible).AsQueryable();
                    break;
                case "readingTitles":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.READING_TITLES && ex.Visible).AsQueryable();
                    break;
                case "matchingSentences":
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.TypeOfExercise == ExerciseType.MATCHING_SENTENCES && ex.Visible).AsQueryable();
                    break;
                default:
                    exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.Visible).AsQueryable();
                    break;
            }
            
            // Sortowanie.
            switch (sortOrder)
            {
                case "level_asc":
                    exercises = exercises.OrderBy(ex => ex.LevelOfExercise);
                    break;
                case "level_desc":
                    exercises = exercises.OrderByDescending(ex => ex.LevelOfExercise);
                    break;
                case "date_asc":
                    exercises = exercises.OrderBy(ex => ex.Created);
                    break;
                default:
                    exercises = exercises.OrderByDescending(ex => ex.Created);
                    break;
            }
            PaginatedList<Exercise> model = await PaginatedList<Exercise>.CreateAsync(exercises.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [HttpGet]
        public IActionResult ShowExercise(int exerciseID)
        {
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            return View(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibility(Exercise model)
        {
            using (var context = new ApplicationDbContext())
            {
                var exercise = context.Exercises.Where(ex => ex.ExerciseID == model.ExerciseID).FirstOrDefault();
                exercise.Visible = model.Visible;
                await context.SaveChangesAsync();
            }
                
            return RedirectToAction("MyExercises", "Exercise");
        }

        public async Task<IActionResult> DeleteExercise(int exerciseID)
        {
            if (await CanRemoveExercise(exerciseID))
            {
                string path;
                using (var context = new ApplicationDbContext())
                {
                    var targetExercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
                    path = targetExercise.PathToFile;
                    context.Exercises.Remove(targetExercise);
                    await context.SaveChangesAsync();
                }
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return RedirectToAction("MyExercises", "Exercise");
            }
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            return RedirectToAction("ExerciseWarning", "Exercise", new { exerciseName = exercise.Name });
        }

        [HttpGet]
        public IActionResult ExerciseWarning(string exerciseName)
        {
            ViewBag.exerciseName = exerciseName;
            return View();
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
            model.Level = (int)exercise.LevelOfExercise;
            TempData.Remove("TranslatingWords");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TranslatingWordsScore(TranslatingWordsSettingsViewModel model)
        {
            // Jesli zadanie jest aktualnie czescia testu...
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
            // Jesli zadanie jest samodzielnym skladnikiem...
            ViewBag.points = TranslatingWordsCheck(model);
            var user = await GetCurrentUserAsync();
            var currentStats = context.Statistics.Where(s => s.ApplicationUserID == user.Id).FirstOrDefault();
            if (currentStats == null)
            {
                await CreateStatisticsForUser(user.Id);
            }
            await UpdateStatisticsForUser(user.Id, model.Level);
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
            model.Level = (int)exercise.LevelOfExercise;
            TempData.Remove("ReadingTitles");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReadingTitlesScore(ReadingTitlesSettingsViewModel model)
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
            var user = await GetCurrentUserAsync();
            var currentStats = context.Statistics.Where(s => s.ApplicationUserID == user.Id).FirstOrDefault();
            if (currentStats == null)
            {
                await CreateStatisticsForUser(user.Id);
            }
            await UpdateStatisticsForUser(user.Id, model.Level);
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
            model.Level = (int)exercise.LevelOfExercise;
            TempData.Remove("MatchingSentences");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MatchingSentencesScore(MatchingSentencesSettingsViewModel model)
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
            var user = await GetCurrentUserAsync();
            var currentStats = context.Statistics.Where(s => s.ApplicationUserID == user.Id).FirstOrDefault();
            if (currentStats == null)
            {
                await CreateStatisticsForUser(user.Id);
            }
            await UpdateStatisticsForUser(user.Id, model.Level);
            return View(model);
        }
    }
}
