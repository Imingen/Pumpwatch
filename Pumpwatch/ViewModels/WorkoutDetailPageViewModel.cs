using Newtonsoft.Json;
using Pumpwatch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Pumpwatch.ViewModels
{
    public class WorkoutDetailPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Exercise> WorkoutHasExercises { get; set; } = new ObservableCollection<Exercise>();

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

        public Workout w1 { get; set; }

        public async Task LoadWorkoutExerciseList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                var json = await client.GetStringAsync($"Workouts/{w1.WorkoutId}/Exercises");

                Exercise[] exercises = JsonConvert.DeserializeObject<Exercise[]>(json);

                WorkoutHasExercises.Clear();
                foreach (var e in exercises)
                {
                    WorkoutHasExercises.Add(e);
                }
            }
        }

        public async Task PutWorkout()
        {
            using (var client = new HttpClient()) {


                client.BaseAddress = new Uri(@"http://localhost:50562/api/");
                w1.WorkoutName = WorkoutName;
                w1.WorkoutDescription = Description;

                var json = JsonConvert.SerializeObject(w1);
                var content = new StringContent(json);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                await client.PutAsync($"Workouts/{w1.WorkoutId}", content);
              }
        }

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
