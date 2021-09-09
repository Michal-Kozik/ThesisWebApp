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


        // Kazdy uzytkownik ma swoje statystyki (relacja 1 do 1).
        public Statistics Statistics { get; set; }

        // Jeden uzytkownik moze stworzyc wiele cwiczen.
        public virtual ICollection<Exercise> Exercises { get; set; }

        // Jeden uzytkownik moze stworzyc wiele testow.
        public virtual ICollection<Exam> Exams { get; set; }

        // Jeden uzytkownik moze miec wiele ocen
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
