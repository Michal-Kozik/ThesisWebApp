﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;

namespace ThesisWebApp.Controllers
{
    public class ExerciseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateExercise()
        {
            return View();
        }
    }
}