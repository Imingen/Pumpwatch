using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpwatch.Models
{
    public enum Categories
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
        public string ExersiceDescription { get; set; }
        public byte[] Image { get; set; }
        public Categories Category { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }


        public override string ToString()
        {
            return $"{ExerciseName} - {Category}\n{ExersiceDescription}";
        }

    }
}
