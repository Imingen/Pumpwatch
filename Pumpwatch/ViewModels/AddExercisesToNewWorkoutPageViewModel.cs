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
   public class AddExercisesToNewWorkoutPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; set; } = new ObservableCollection<Exercise>();

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


        public async void LoadExercises()
        {
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
