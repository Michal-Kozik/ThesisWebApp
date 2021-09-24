using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class ProfileController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }



        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private Statistics GetStatistics(string userID)
        {
            var statistics = context.Statistics.Where(s => s.ApplicationUserID == userID).FirstOrDefault();
            return statistics;
        }



        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            Statistics model = GetStatistics(user.Id);
            return View(model);
        }
    }
}
