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
    [Authorize]
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
                Exam exam = new Exam { ApplicationUserID = user.Id,
                                       Name = model.Name,
                                       ExercisesPattern = model.ExercisePattern,
                                       Visible = model.Visible,
                                       ManyAttempts = model.ManyAttempts,
                                       Password = model.Password,
                                       Archived = false };
                context.Exams.Add(exam);
                await context.SaveChangesAsync();
            }
        }

        private bool CanAddToExam()
        {
            string cookie = Request.Cookies["ChoosenExercises"];
            string[] idArray = cookie.Split('-');
            if (idArray.Length >= 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsExerciseInExam(string exerciseID)
        {
            string cookie = Request.Cookies["ChoosenExercises"];
            string[] idArray = cookie.Split('-');
            foreach (string id in idArray)
            {
                if (id == exerciseID)
                {
                    return true;
                }
            }
            return false;
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PublicExams(int? pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybranie danych.
            var exams = context.Exams.Include(e => e.ApplicationUser).Where(e => e.Visible && !e.Archived).AsQueryable();
            PaginatedList<Exam> model = await PaginatedList<Exam>.CreateAsync(exams.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> MyExams(int? pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybranie danych.
            var user = await GetCurrentUserAsync();
            var exams = context.Exams.Where(e => e.ApplicationUserID == user.Id).AsQueryable();
            PaginatedList<Exam> model = await PaginatedList<Exam>.CreateAsync(exams.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult EditExam(int examID)
        {
            var exam = context.Exams.Where(e => e.ExamID == examID).FirstOrDefault();
            if (exam == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            if (exam.Archived)
            {
                return RedirectToAction("ArchivedExam", new { examID = examID });
            }
            ExamViewModel model = new ExamViewModel();
            model.ExamID = exam.ExamID;
            model.Name = exam.Name;
            model.Password = exam.Password;
            model.Visible = exam.Visible;
            model.ManyAttempts = exam.ManyAttempts;
            model.ExercisePattern = exam.ExercisesPattern;
            model.Archived = exam.Archived;

            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ArchiveExam(int examID)
        {
            using (var context = new ApplicationDbContext())
            {
                var exam = context.Exams.Where(e => e.ExamID == examID).FirstOrDefault();
                exam.Archived = true;
                await context.SaveChangesAsync();
            }
            return RedirectToAction("MyExams", "Exam");
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> EditExam(ExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    var exam = context.Exams.Where(e => e.ExamID == model.ExamID).FirstOrDefault();
                    bool shouldSave = false;
                    if (exam.Visible != model.Visible)
                    {
                        exam.Visible = model.Visible;
                        shouldSave = true;
                    }
                    if (exam.Name != model.Name)
                    {
                        exam.Name = model.Name;
                        shouldSave = true;
                    }
                    if (exam.Password != model.Password)
                    {
                        exam.Password = model.Password;
                        shouldSave = true;
                    }

                    if (shouldSave)
                    {
                        await context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("MyExams", "Exam");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> DoneExams(int? pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 10;

            // Wybranie danych.
            var user = await GetCurrentUserAsync();
            var marks = context.Marks.Include(m => m.Exam.ApplicationUser).Where(m => m.ApplicationUserID == user.Id).AsQueryable();
            PaginatedList<Mark> model = await PaginatedList<Mark>.CreateAsync(marks.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ArchivedExam(int? pageNumber, int examID)
        {
            ViewData["ExamID"] = examID;
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 10;

            // Wybranie danych.
            var user = await GetCurrentUserAsync();
            var marks = context.Marks.Include(m => m.ApplicationUser).Where(m => m.ExamID == examID).AsQueryable();
            PaginatedList<Mark> model = await PaginatedList<Mark>.CreateAsync(marks.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> ExamsResults(int? pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybranie danych.
            var user = await GetCurrentUserAsync();
            var exams = context.Exams.Include(e => e.ApplicationUser).Where(e => e.ApplicationUserID == user.Id).AsQueryable();
            PaginatedList<Exam> model = await PaginatedList<Exam>.CreateAsync(exams.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> ParticularExamResults(int? pageNumber, int examID)
        {
            ViewData["ExamID"] = examID;
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 10;

            // Wybranie danych.
            var exam = context.Exams.Where(e => e.ExamID == examID).FirstOrDefault();
            ViewData["ExamName"] = exam.Name.ToUpper();
            var marks = context.Marks.Include(m => m.ApplicationUser).Where(m => m.ExamID == examID).AsQueryable();
            PaginatedList<Mark> model = await PaginatedList<Mark>.CreateAsync(marks.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> CreateExam(int? pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            int pageSize = 3;

            // Wybranie danych.
            var user = await GetCurrentUserAsync();
            var exercises = context.Exercises.Include(ex => ex.ApplicationUser).Where(ex => ex.ApplicationUserID == user.Id).AsQueryable();
            PaginatedList<Exercise> model = await PaginatedList<Exercise>.CreateAsync(exercises.AsNoTracking(), pageNumber ?? 1, pageSize); 
            ViewBag.choosenExercises = Request.Cookies["ChoosenExercises"];
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult ShowExercise(int exerciseID)
        {
            var exercise = context.Exercises.Where(ex => ex.ExerciseID == exerciseID).FirstOrDefault();
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                ViewBag.canAdd = true;
                ViewBag.isExerciseInExam = false;
            }   
            else
            {
                ViewBag.canAdd = CanAddToExam();
                ViewBag.isExerciseInExam = IsExerciseInExam(exerciseID.ToString());
            }
            return View(exercise);
        }

        [Authorize(Roles = "Teacher")]
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
                return View(model);
            }
            else
            {
                return RedirectToAction("DeadEnd", "Home");
            }
        }

        [Authorize(Roles = "Teacher")]
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

        [HttpGet]
        public async Task<IActionResult> ProceedExam(int examID)
        {
            // Czy znaleziono test.
            var exam = context.Exams.Where(e => e.ExamID == examID).FirstOrDefault();
            if (exam == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            // Czy user moze jeszcze podejsc do testu.
            var user = await GetCurrentUserAsync();
            var mark = context.Marks.Where(m => m.ExamID == examID && m.ApplicationUserID == user.Id).FirstOrDefault();
            if (mark != null && !exam.ManyAttempts)
            {
                return RedirectToAction("MaxAttemptsReached");
            }
            // Czy test ma haslo.
            if (String.IsNullOrEmpty(exam.Password))
            {
                return RedirectToAction("StartExam", new { ExamID = exam.ExamID });
            }
            StartExamViewModel model = new StartExamViewModel();
            model.ExamID = exam.ExamID;
            model.ExamPassword = exam.Password;
            return View(model);
        }

        [HttpPost]
        public IActionResult ProceedExam(StartExamViewModel model)
        {
            // Jesli haslo pasuje.
            if (model.InputPassword == model.ExamPassword)
            {
                return RedirectToAction("StartExam", new { examID = model.ExamID });
            }
            else
            {
                ModelState.AddModelError("", "Niepoprawne hasło.");
                return View(model);
            }
        }

        public IActionResult MaxAttemptsReached()
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult AddToExam(int exerciseID)
        {
            if (String.IsNullOrEmpty(Request.Cookies["ChoosenExercises"]))
            {
                Response.Cookies.Append("ChoosenExercises", exerciseID.ToString());
            }
            else
            {
                if (!IsExerciseInExam(exerciseID.ToString()))
                {
                    string oldCookie = Request.Cookies["ChoosenExercises"];
                    Response.Cookies.Append("ChoosenExercises", $"{oldCookie}-{exerciseID}");
                }
            }
            return RedirectToAction("CreateExam");
        }

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "Teacher")]
        public IActionResult ClearCookie()
        {
            Response.Cookies.Delete("ChoosenExercises");
            return RedirectToAction("CreateExam");
        }

        public async Task<IActionResult> StartExam(int examID)
        {
            var exam = context.Exams.Where(e => e.ExamID == examID).FirstOrDefault();
            if (exam == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            // Test ma jedno podejscie.
            if (!exam.ManyAttempts)
            {
                TempData["ExamWithOneAttemptID"] = exam.ExamID;
                // Zapisanie zerowego wyniku na wypadek opuszczenia testu.
                using (var context = new ApplicationDbContext())
                {
                    var user = await GetCurrentUserAsync();
                    Mark mark = new Mark
                    {
                        ApplicationUserID = user.Id,
                        ExamID = exam.ExamID,
                        Created = DateTime.Now,
                        UserScore = 0,
                        MaxPoints = 1
                    };
                    context.Marks.Add(mark);
                    await context.SaveChangesAsync();
                }
            }
            string cookie = exam.ExercisesPattern;
            Response.Cookies.Append("ChoosenExercises", cookie);
            string[] exercisesIDsArray = cookie.Split('-');
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

        public async Task<IActionResult> EndExam()
        {
            if (TempData["Points"] == null)
            {
                return RedirectToAction("DeadEnd", "Home");
            }
            ExamResult model = new ExamResult();
            model.Points = (int)TempData["Points"];
            model.MaxPoints = (int)TempData["MaxPoints"];
            // Zapisanie wyniku jesli test mial jedno podejscie.
            if (TempData["ExamWithOneAttemptID"] != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await GetCurrentUserAsync();
                    var mark = context.Marks.Where(m => m.ApplicationUserID == user.Id && m.ExamID == (int)TempData["ExamWithOneAttemptID"]).FirstOrDefault();
                    if (mark == null)
                    {
                        return RedirectToAction("DeadEnd", "Home");
                    }
                    mark.Created = DateTime.Now;
                    mark.UserScore = model.Points;
                    mark.MaxPoints = model.MaxPoints;
                    await context.SaveChangesAsync();
                }
            }
            Response.Cookies.Delete("ChoosenExercises");
            TempData.Clear();
            return View(model);
        }
    }
}
