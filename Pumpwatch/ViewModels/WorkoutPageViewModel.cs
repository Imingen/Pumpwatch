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
using Windows.UI.Xaml;

namespace Pumpwatch.ViewModels
{
    public class WorkoutPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        public WorkoutPageViewModel(){}

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != this.name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string resultText;
        public string ResultText
        {
            get { return resultText; }
            set
            {
                if (value != this.resultText)
                {
                    resultText = value;
                    NotifyPropertyChanged("ResultText");
                }
            }
        }

        //id for workouts, bound to codebehind so that i can get the id for selectedelement in listbox and pass that as parameter in deletemethod
        public int id { get; set; }

        public int exerciseId { get; set; }

        public async void PostWorkout_Click(object sender, RoutedEventArgs e)
        {
            var WorkoutName = name;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("WorkoutName", WorkoutName)
                });
                var result = await client.PostAsync("Workouts", content);
                Workout workout = JsonConvert.DeserializeObject<Workout>(await result.Content.ReadAsStringAsync());
                GoToAddExercisesToWorkout(workout);
            }
            
        }

        public async void DeleteSelectedWorkout(object sender, RoutedEventArgs e)
        {
            var workoutId = id;

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:50562/api/Workouts/");
                await client.DeleteAsync($"{id}");
                
            }
            LoadWorkouts();
        }

        public async void LoadWorkouts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                var json = await client.GetStringAsync("Workouts");

                Workout[] workouts = JsonConvert.DeserializeObject<Workout[]>(json);

                Workouts.Clear();
                foreach (var w in workouts)
                {
                    Workouts.Add(w);
                }
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

        public void GoToAddExercisesToWorkout(Workout w) =>
            NavigationService.Navigate(typeof(Views.AddExercisesToNewWorkoutPage), w);

        public void GotoAddNewWorkout() =>
            NavigationService.Navigate(typeof(Views.AddWorkoutPage));

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
