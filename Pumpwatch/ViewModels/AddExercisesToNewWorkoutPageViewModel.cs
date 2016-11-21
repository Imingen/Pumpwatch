using Pumpwatch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Pumpwatch.ViewModels
{
   public class AddExercisesToNewWorkoutPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();










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
