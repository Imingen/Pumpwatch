using Pumpwatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadWorkouts();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            int id;
            workout = listBox.SelectedItem as Workout;
            foreach(Workout w1 in e.AddedItems)
            {
                id = w1.WorkoutId;
                ViewModel.id = id;
            }
            
        }

        private async void Delete_SelectedWorkout(object sender, RoutedEventArgs e)
        {
           await ViewModel.DeleteSelectedWorkout();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SeeMore_SelectedWorkout(object sender, RoutedEventArgs e)
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(WorkoutDetailPage), workout);
        }
    }
}
