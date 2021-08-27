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
using ThesisWebApp.ViewModels;

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

        private async Task SaveExamInDatabase(ExamViewModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await GetCurrentUserAsync();
                Exam exam = new Exam { ApplicationUserID = user.Id, Name = model.Name, ExercisesPattern = model.ExercisePattern, Visible = model.Visible };
                context.Exams.Add(exam);
                await context.SaveChangesAsync();
            }
        }

        private bool CanAddToExam(string exerciseID)
        {
            string cookie = Request.Cookies["ChoosenExercises"];
            string[] idArray = cookie.Split('-');
            if (idArray.Length >= 10)
            {
                return false;
            }
            foreach (string id in idArray)
            {
                if (id == exerciseID)
                    return false;
            }
            return true;
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
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
                ViewBag.canAdd = true;
            else
                ViewBag.canAdd = CanAddToExam(exerciseID.ToString());
            return View(exercise);
        }

        [HttpGet]
        public IActionResult ExamSettings()
        {
            if (!String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                string cookie = Request.Cookies["ChoosenExercises"];
                string[] idArray = cookie.Split('-');
                List<string> idList = idArray.ToList();
                ExamViewModel model = new ExamViewModel();
                model.Exercises = context.Exercises.Where(ex => idList.Contains(ex.ExerciseID.ToString())).ToList();
                //ViewBag.choosenExercises = Request.Cookies["ChoosenExercises"];
                return View(model);
            }
            else
            {
                return RedirectToAction("DeadEnd", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ExamSettings(ExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ExercisePattern = Request.Cookies["ChoosenExercises"];
                await SaveExamInDatabase(model);
                Response.Cookies.Delete("ChoosenExercises");
                return RedirectToAction("Index", "Exam");
            }
            else
            {
                string cookie = Request.Cookies["ChoosenExercises"];
                string[] idArray = cookie.Split('-');
                List<string> idList = idArray.ToList();
                model.Exercises = context.Exercises.Where(ex => idList.Contains(ex.ExerciseID.ToString())).ToList();
                return View(model);
            }
        }

        public IActionResult AddToExam(int exerciseID)
        {
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                Response.Cookies.Append("ChoosenExercises", exerciseID.ToString());
            }
            else
            {
                if (CanAddToExam(exerciseID.ToString()))
                {
                    string oldCookie = Request.Cookies["ChoosenExercises"];
                    Response.Cookies.Append("ChoosenExercises", $"{oldCookie}-{exerciseID}");
                }
            }
            return RedirectToAction("CreateExam");
        }

        public IActionResult RemoveFromExam(int exerciseID)
        {
            if (!String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                string oldCookie = Request.Cookies["ChoosenExercises"];
                string[] idArray = oldCookie.Split('-');
                string cookie = "";
                for (int i = 0; i < idArray.Length; i++)
                {
                    if (idArray[i] == exerciseID.ToString())
                        idArray[i] = "";
                    cookie += $"-{idArray[i]}";
                }
                // Przy usuwaniu pierwszego i ostatniego zadania.
                cookie = cookie.Trim('-');
                // Przy usuwaniu srodkowego zadania.
                cookie = cookie.Replace("--", "-");
                Response.Cookies.Append("ChoosenExercises", cookie);
            }
            return RedirectToAction("CreateExam");
        }

        public IActionResult ClearCookie()
        {
            Response.Cookies.Delete("ChoosenExercises");
            return RedirectToAction("CreateExam");
        }

        public IActionResult StartExam()
        {
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            string exercisesIDs = Request.Cookies["ChoosenExercises"];
            string[] exercisesIDsArray = exercisesIDs.Split('-');
            TempData["CurrentExercise"] = 0;
            int firstExerciseID = Int32.Parse(exercisesIDsArray[0]);
            return RedirectToAction("ChoosenExercise", "Exercise", new { ExerciseID = firstExerciseID });
        }

        public IActionResult ContinueExam()
        {
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
