using Pumpwatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pumpwatch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkoutPage : Page
    {
        Workout workout;

        public WorkoutPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadWorkouts();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                       
            workout = listBox.SelectedItem as Workout;
            ViewModel.workout = workout;
        }

        private async void Delete_SelectedWorkout(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.DeleteSelectedWorkout();
            }
            catch (NullReferenceException)
            {
                MessageDialog msg = new MessageDialog("Select a workout");
                await msg.ShowAsync();
            }
        }

        private async void SeeMore_SelectedWorkout(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Select a workout");

            if (workout != null)
                Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(WorkoutDetailPage), workout);
            else
                await msg.ShowAsync();

        }
    }
}
