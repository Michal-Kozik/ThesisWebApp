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



        // W jednym tescie bedzie wiele zadan.
        public virtual ICollection<ExerciseExams> ExerciseExams { get; set; }
    }
}
