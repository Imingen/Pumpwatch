using Newtonsoft.Json;
using Pumpwatch.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Template10.Mvvm;
using Windows.UI.Popups;

namespace Pumpwatch.ViewModels
{
    public class AddExercisesToWorkoutPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Exercise> Exercises { get; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get;} = new ObservableCollection<Exercise>();

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

        public Workout Workout { get; set; }

        /// <summary>
        /// Posts the workout with selected exercises
        /// </summary>
        public async void PostWorkoutWithExercises()
        {
            try {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/Workouts/");
                    var json = JsonConvert.SerializeObject(Workout);

                    var httpContent = new StringContent(json);
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    foreach (Exercise ex in SelectedExercises)
                    {
                        await client.PostAsync($"{Workout.WorkoutId}/Exercises/{ex.ExerciseId}", httpContent);
                    }
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Could not establish connection with the database");
                await msg.ShowAsync();
            }
            finally { 
                GotoWorkoutPage();
            }
        }

        /// <summary>
        /// GETs all the exercises from the database
        /// </summary>
        public async void LoadExercises()
        {
            try {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                    var json = await client.GetStringAsync("Exercises");

                    Exercise[] exercises = JsonConvert.DeserializeObject<Exercise[]>(json);

                    Exercises.Clear();
                    foreach (var w in exercises)
                    {
                        Exercises.Add(w);
                    }
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Could not establish connection with the database");
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

        public void GotoWorkoutPage() =>
                NavigationService.Navigate(typeof(Views.WorkoutPage));

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
