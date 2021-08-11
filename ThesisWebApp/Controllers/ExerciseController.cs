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



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateExercise()
        {
            return View();
        }

        public IActionResult ListExercises()
        {
            ViewBag.exercises = context.Exercises.Include(e => e.ApplicationUser).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult ChoosenExercise(int ExerciseID)
        {
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == ExerciseID).FirstOrDefault();
            switch (exercise.TypeOfExercise)
            {
                case ExerciseType.TRANSLATING_WORDS:
                    return RedirectToAction("TranslatingWordsAttempt", exercise);
                default:
                    return View(exercise);
            }
        }

        [HttpGet]
        public IActionResult TranslatingWordsAttempt(Exercise exercise)
        {
            TranslatingWordsSettingsViewModel model = TranslatingWordsController.ReadExerciseFromTxt(exercise.PathToFile);
            return View(model);
        }

        [HttpPost]
        public IActionResult TranslatingWordsScore(TranslatingWordsSettingsViewModel model)
        {
            ViewBag.points = TranslatingWordsCheck(model);
            return View(model);
        }
    }
}
