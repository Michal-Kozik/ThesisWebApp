using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ThesisWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Is Teacher")]
        public bool IsTeacher { get; set; }

        // Jeden uzytkownik moze stworzyc wiele testow.
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
