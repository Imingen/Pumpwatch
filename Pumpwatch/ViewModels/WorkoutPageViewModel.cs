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

        public WorkoutPageViewModel()
        {
            LoadWorkouts();
        }

        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();
        //public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

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

        public bool IsSelected { get; set; }

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

        public async void PostWorkout_Click(object sender, RoutedEventArgs e)
        {
            var WorkoutName = name;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:55016/api/");

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("WorkoutName", WorkoutName)
                });
                var result = await client.PostAsync("Workouts", content);

                resultText = "Workout " + WorkoutName + "successfully created";

                LoadWorkouts();
            }
        }

        //public async void DeleteWorkout(object sender, RoutedEventArgs e)
        //{

        //}

        private async void LoadWorkouts()
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
