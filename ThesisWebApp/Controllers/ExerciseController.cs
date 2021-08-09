using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Data;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;

namespace ThesisWebApp.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();



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
            ViewBag.exercises = context.Exercises.ToList();
            return View();
        }

        [HttpGet]
        public IActionResult ChoosenExercise(int ExerciseID)
        {
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == ExerciseID).FirstOrDefault();
            switch (exercise.TypeOfExercise)
            {
                case "Translating Words":
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
    }
}
