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
    public class ReadingTitlesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ReadingTitlesController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private bool ValidateInputs(ReadingTitlesSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfParagraphs; i++)
            {
                if (String.IsNullOrEmpty(model.Paragraphs[i]) || String.IsNullOrEmpty(model.CorrectTitles[i]))
                {
                    return false;
                }
            }
            for (int i = 0; i < model.NumberOfAdditionalTitles; i++)
            {
                if (String.IsNullOrEmpty(model.AdditionalTitles[i]))
                {
                    return false;
                }
            }
            return true;
        }



        [HttpGet]
        public IActionResult Settings()
        {
            return View(new ReadingTitlesSettingsViewModel());
        }

        [HttpGet]
        public IActionResult Add(int numberOfParagraphs, int numberOfAdditionalTitles, string exerciseName)
        {
            ReadingTitlesSettingsViewModel model = new ReadingTitlesSettingsViewModel();
            model.NumberOfParagraphs = numberOfParagraphs;
            model.NumberOfAdditionalTitles = numberOfAdditionalTitles;
            model.ExerciseName = exerciseName;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ReadingTitlesSettingsViewModel model)
        {
            if (ValidateInputs(model))
            {
                //redirect to action Result.
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
