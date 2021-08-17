using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;

namespace ThesisWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class MatchingSentencesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public MatchingSentencesController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private bool ValidateInputs(MatchingSentencesSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfSentences; i++)
            {
                if (String.IsNullOrEmpty(model.SentencesFirstPart[i]) || String.IsNullOrEmpty(model.SentencesSecondPart[i]))
                {
                    return false;
                }
            }
            return true;
        }



        [HttpGet]
        public IActionResult Settings()
        {
            return View(new MatchingSentencesSettingsViewModel());
        }

        [HttpGet]
        public IActionResult Add(int numberOfSentences, string exerciseName)
        {
            MatchingSentencesSettingsViewModel model = new MatchingSentencesSettingsViewModel();
            model.NumberOfSentences = numberOfSentences;
            model.ExerciseName = exerciseName;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(MatchingSentencesSettingsViewModel model)
        {
            if (ValidateInputs(model))
            {
                // do zmiany.
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Wypełnij wszystkie pola!");
                return View(model);
            }
        }
    }
}
