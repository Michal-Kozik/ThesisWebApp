using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public enum ExerciseType
    {
        TRANSLATING_WORDS = 0,
        READING_TITLES = 1,
        MATCHING_SENTENCES = 2
    }

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

        [Required]
        [Display(Name = "Type of exercise")]
        [EnumDataType(typeof(ExerciseType))]
        public ExerciseType TypeOfExercise { get; set; }

        [Required]
        [Display(Name = "Visible")]
        public bool Visible { get; set; }


        // Kazde zadanie ma swojego autora.
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "Author ID")]
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
