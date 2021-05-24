using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public class Exam
    {
        [Key]
        [Display(Name = "Exam ID")]
        public int ExamID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string ApplicationUserID { get; set; }

        // W jednym tescie bedzie wiele cwiczen.
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
