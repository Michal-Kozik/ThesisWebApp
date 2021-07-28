using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;

namespace ThesisWebApp.Controllers
{
    public class TranslatingWordsController : Controller
    {
        private bool ValidateWords(TranslatingWordsSettingsViewModel model)
        {
            for (int i = 0; i < model.NumberOfWords; i++)
            {
                if (String.IsNullOrEmpty(model.TranslateFromArray[i]) || String.IsNullOrEmpty(model.TranslateToArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        [HttpGet]
        public IActionResult TranslatingWordsSettings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TranslatingWordsAdd(TranslatingWordsSettingsViewModel model)
        {
            if (ValidateWords(model))
            {
                return RedirectToAction("TranslatingWordsResult", model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TranslatingWordsResult(TranslatingWordsSettingsViewModel model)
        {
            return View(model);
        }
    }
}
