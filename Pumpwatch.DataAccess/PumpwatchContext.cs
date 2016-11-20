using Pumpwatch.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Pumpwatch.DataAccess
{
    public class PumpwatchContext : DbContext
    {
        public PumpwatchContext() :
            base("PumpwatchTest")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>().HasMany<Exercise>(e => e.Exercises).WithMany(w => w.Workouts).Map(cs =>
            {
                cs.MapLeftKey("WorkoutId");
                cs.MapRightKey("ExerciseId");
                cs.ToTable("WorkoutWithExercises");
            });
        }
    }
}
