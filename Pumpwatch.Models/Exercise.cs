using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpwatch.Models
{
    public enum Category
    {
        All,
        Chest,
        Back,
        Shoulder,
        Legs,
        Arms
    }

    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }
        [Required]
        public string ExerciseName { get; set; }
        public string ExerciseDescription { get; set; }
        public Category Category { get; set; }
        //I have supress warning on this, it asked me to remove the setter but when i tried i got propblems running the program
        public virtual ICollection<Workout> Workouts { get; set; }


        public override string ToString()
        {
            return $"{ExerciseName} - {Category}\n{ExerciseDescription}";
        }

    }
}
