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
using Windows.UI.Xaml.Controls;

namespace Pumpwatch.ViewModels
{
    public class ExercisePageViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public new event PropertyChangedEventHandler PropertyChanged;

        public ExercisePageViewModel() { }


        public async void LoadExercises()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                var json = await client.GetStringAsync("Exercises");

                Exercise[] exercises = JsonConvert.DeserializeObject<Exercise[]>(json);

                Exercises.Clear();
                foreach (var excercise in exercises)
                {
                    Exercises.Add(excercise);
                }
            }
        }

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
                if (value != this.description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string category;
        public string Category
        {
            get { return category; }
            set
            {
                if (value != category)
                {
                    category = value;
                    NotifyPropertyChanged("Category");
                }
            }
        }


        public void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }


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
