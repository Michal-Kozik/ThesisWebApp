using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Data;
using ThesisWebApp.Models;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ExamController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> userManager;

        public ExamController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PublicExams()
        {
            return View();
        }

        public IActionResult DoneExams()
        {
            return View();
        }

        public IActionResult ExamsResults()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateExam()
        {
            var user = await GetCurrentUserAsync();
            ViewBag.userExercises = context.Exercises.Include(e => e.ApplicationUser).Where(e => e.ApplicationUserID == user.Id).ToList();
            ViewBag.choosenExercises = Request.Cookies["ChoosenExercises"];
            return View();
        }

        [HttpGet]
        public IActionResult ShowExercise(int exerciseID)
        {
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            return View(exercise);
        }

        public IActionResult AddToExam(int exerciseID)
        {
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                Response.Cookies.Append("ChoosenExercises", exerciseID.ToString());
            }
            else
            {
                string oldCookie = Request.Cookies["ChoosenExercises"];
                Response.Cookies.Append("ChoosenExercises", $"{oldCookie}-{exerciseID}");
            }
            return RedirectToAction("CreateExam");
        }

        public IActionResult StartExam()
        {
            // przetestowac try catch.
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            string exercisesIDs = Request.Cookies["ChoosenExercises"];
            string[] exercisesIDsArray = exercisesIDs.Split('-');
            TempData["CurrentExercise"] = 0;
            //TempData["NumberOfExercises"] = exercisesIDsArray.Length;
            int firstExerciseID = Int32.Parse(exercisesIDsArray[0]);
            return RedirectToAction("ChoosenExercise", "Exercise", new { ExerciseID = firstExerciseID });
        }

        public IActionResult ContinueExam()
        {
            // przetestowac try catch
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            string exercisesIDs = Request.Cookies["ChoosenExercises"];
            string[] exercisesIDsArray = exercisesIDs.Split('-');
            int currentExercise = (int)TempData["CurrentExercise"] + 1;
            if (currentExercise >= exercisesIDsArray.Length)
            {
                return RedirectToAction("EndExam");
            }
            int currentExerciseID = Int32.Parse(exercisesIDsArray[currentExercise]);
            TempData["CurrentExercise"] = currentExercise;
            return RedirectToAction("ChoosenExercise", "Exercise", new { ExerciseID = currentExerciseID });
        }

        public IActionResult EndExam()
        {
            // przetestowac try catch
            if (TempData["Points"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            ExamResult model = new ExamResult();
            model.Points = (int)TempData["Points"];
            model.MaxPoints = (int)TempData["MaxPoints"];
            Response.Cookies.Delete("ChoosenExercises");
            TempData.Clear();
            return View(model);
        }
    }
}
