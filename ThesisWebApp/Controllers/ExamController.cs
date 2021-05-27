using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.Controllers
{
    public class ExamController : Controller
    {
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
    }
}
