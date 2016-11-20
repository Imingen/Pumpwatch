using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Pumpwatch.DataAccess;
using Pumpwatch.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Pumpwatch.DataService.Controllers
{
    public class WorkoutsController : ApiController
    {
        private PumpwatchContext db = new PumpwatchContext();

        // GET: api/Workouts
        public IQueryable<Workout> GetWorkouts()
        {
            return db.Workouts;
        }

        // GET: api/Workouts/5
        [ResponseType(typeof(Workout))]
        public IHttpActionResult GetWorkout(int id)
        {
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        // PUT: api/Workouts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWorkout(int id, Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workout.WorkoutId)
            {
                return BadRequest();
            }

            db.Entry(workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Workouts
        [ResponseType(typeof(Workout))]
        public IHttpActionResult PostWorkout(Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(workout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = workout.WorkoutId }, workout);
        }

        // DELETE: api/Workouts/5
        [ResponseType(typeof(Workout))]
        public IHttpActionResult DeleteWorkout(int id)
        {
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return NotFound();
            }

            db.Workouts.Remove(workout);
            db.SaveChanges();

            return Ok(workout);
        }

        [HttpGet]
        [Route("api/Workouts/{id}/Exercises")]
        [ResponseType(typeof(Exercise[]))]
        public IHttpActionResult GetExercisesForWorkout(int id)
        {
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
                return NotFound();

            // Force EF to load related objects Authors->Books
            db.Entry(workout).Collection(e => e.Exercises).Load();  // This will create a loop, fix in Global.asax

            return Ok(workout.Exercises);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/Workouts/{WorkoutId}/Exercises/{ExerciseId}")]
        public IHttpActionResult PostWorkoutExercise(int WorkoutId, int ExerciseId)
        {
            if (!WorkoutExists(WorkoutId) || !ExerciseExist(ExerciseId))
            {
                return NotFound();
            }
            else
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PumpwatchTest"].ConnectionString))
                {
                    var cmd = new SqlCommand("INSERT INTO WorkoutWithExercises VALUES(@WorkoutId, @ExerciseId); ", conn);
                    cmd.Parameters.Add(new SqlParameter("@WorkoutId", WorkoutId));
                    cmd.Parameters.Add(new SqlParameter("@ExerciseId", ExerciseId));

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected != 1)
                        return Conflict();
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        private bool ExerciseExist(int exerciseId)
        {
            return db.Exercises.Count(e => e.ExerciseId == exerciseId) > 0;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkoutExists(int id)
        {
            return db.Workouts.Count(e => e.WorkoutId == id) > 0;
        }
    }
}