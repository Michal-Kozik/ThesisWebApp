using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThesisWebApp.Models
{
    public class Mark
    {
        [Key]
        [Display(Name = "Mark ID")]
        public int MarkID { get; set; }

        [Required]
        [Display(Name = "Wystawiono")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        [Display(Name = "Maksymalna ilość punktów")]
        public int MaxPoints { get; set; }

        [Required]
        [Display(Name = "Wynik")]
        public int UserScore { get; set; }


        // Kazda ocena jest jakiegos ucznia.
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // Kazda ocena jest z jakiegos testu.
        [ForeignKey("Exam")]
        [Display(Name = "Exam ID")]
        public int? ExamID { get; set; }
        public Exam Exam { get; set; }
    }
}
