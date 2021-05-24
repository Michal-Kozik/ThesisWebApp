using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public class Exercise
    {
        [Key]
        [Display(Name = "Exercise ID")]
        public int ExerciseID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Path to file")]
        public string PathToFile { get; set; }

        [ForeignKey("ExamID")]
        public Exam Exam { get; set; }

        [Required]
        [Display(Name = "Exam ID")]
        public int ExamID { get; set; }

        [ForeignKey("TypeOfExerciseID")]
        public TypeOfExercise TypeOfExercise { get; set; }

        [Required]
        [Display(Name = "Type of exercise ID")]
        public int TypeOfExerciseID { get; set; }
    }
}
