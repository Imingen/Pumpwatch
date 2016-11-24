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
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Pumpwatch.ViewModels
{
    public class WorkoutPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Workout> Workouts { get;} = new ObservableCollection<Workout>();

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

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if(value != this.description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public Workout Workout { get; set; }

       // public int exerciseId { get; set; }

        /// <summary>
        /// Posts the workout.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Workout> PostWorkout()
        {
            var WorkoutName = name;
            var WorkoutDesc = description;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("WorkoutName", WorkoutName),
                        new KeyValuePair<string, string>("WorkoutDescription", WorkoutDesc)
                    });
                    var result = await client.PostAsync("Workouts", content);
                    if (!result.IsSuccessStatusCode)
                        throw new HttpRequestException();
                    return JsonConvert.DeserializeObject<Workout>(await result.Content.ReadAsStringAsync());
                }
            }
            catch(HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Bad response from database");
                await msg.ShowAsync();
                return null;
            }
        }

        /// <summary>
        /// Deletes the selected workout from database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Error trying to delete the workout</exception>
        public async Task DeleteSelectedWorkout()
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/Workouts/");
                    var result = await client.DeleteAsync($"{Workout.WorkoutId}");
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException("Error trying to delete the workout");
                    }
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Error trying to delete the workout \n Troubleshooting: \n Check internet connection \n remember to select a workout");
                await msg.ShowAsync();
            }
            finally { LoadWorkouts(); }
        }

        /// <summary>
        /// GETs all the workout from the database.
        /// </summary>
        public async void LoadWorkouts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                    var result = await client.GetStringAsync("Workouts");

                    Workout[] workouts = JsonConvert.DeserializeObject<Workout[]>(result);

                    Workouts.Clear();
                    foreach (var w in workouts)
                    {
                        Workouts.Add(w);
                    }
                }
            }
            catch(HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Couldnt load workouts. Check connection");
                await msg.ShowAsync();
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
