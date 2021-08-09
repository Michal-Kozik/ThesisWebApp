using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Data;
using ThesisWebApp.Models;

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
    }
}
