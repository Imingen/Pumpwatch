using Newtonsoft.Json;
using Pumpwatch.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;

namespace Pumpwatch.ViewModels
{
    public class WorkoutDetailPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Exercise> WorkoutHasExercises { get;} = new ObservableCollection<Exercise>();

        private string workoutName;
        public string WorkoutName
        {
            get { return workoutName; }
            set
            {
                if (value != this.workoutName)
                {
                    workoutName = value;
                    NotifyPropertyChanged("WorkoutName");
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != this.description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public Workout Workout { get; set; }

        public Exercise Exercise { get; set; }

        /// <summary>
        /// Deletes the selected exercise from the selected workout object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task DeleteExerciseFromWorkout()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/Workouts/");
                    var result = await client.DeleteAsync($"{Workout.WorkoutId}/Exercises/{Exercise.ExerciseId}");
                    if (!result.IsSuccessStatusCode)
                        throw new HttpRequestException();
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Database error");
            }
            finally
            {
                await LoadWorkoutExerciseList();
            }
        }

        /// <summary>
        /// GETS the exercises that corresponds with the selected workout.
        /// </summary>
        /// <returns></returns>
        public async Task LoadWorkoutExerciseList()
        {
            DatabaseOperator DatabaseOperator = new DatabaseOperator();
            Exercise[] exercises = await DatabaseOperator.LoadData<Exercise>($"Workouts/{Workout.WorkoutId}/Exercises");

                    WorkoutHasExercises.Clear();
                    foreach (var e in exercises)
                    {
                        WorkoutHasExercises.Add(e);
                    }
        }

        /// <summary>
        /// PUT, updates the workout name and/or description.
        /// </summary>
        /// <returns></returns>
        public async Task PutWorkout()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/");
                    Workout.WorkoutName = WorkoutName;
                    Workout.WorkoutDescription = Description;

                    var result = JsonConvert.SerializeObject(Workout);
                    var content = new StringContent(result);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    await client.PutAsync($"Workouts/{Workout.WorkoutId}", content);
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Cannot connect with the database");
            }
        }

        /// <summary>
        /// Sorts the exercise collection alfabetically
        /// </summary>
        public void SortAlfabetically()
        {
            var AlfaQuery = WorkoutHasExercises.OrderBy(Exercise => Exercise.ExerciseName).Select(Exercise => Exercise);

            var observ = new ObservableCollection<Exercise>(AlfaQuery);
            WorkoutHasExercises.Clear();
            foreach (var ex1 in observ)
            {
                WorkoutHasExercises.Add(ex1);
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage));

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
