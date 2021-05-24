using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThesisWebApp.Models
{
    public class TypeOfExercise
    {
        [Key]
        [Display(Name = "Type of exercise ID")]
        public int TypeOfExerciseID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        // Jeden typ zadania bedzie w wielu zadaniach.
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
