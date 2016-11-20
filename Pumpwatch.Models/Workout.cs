using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpwatch.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }
        [Required]
        public string WorkoutName { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }


        public override string ToString()
        {
            return $"{WorkoutName}\n{Exercises}";
        }
    }
}
