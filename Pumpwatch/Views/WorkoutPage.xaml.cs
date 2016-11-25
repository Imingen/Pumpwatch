using Pumpwatch.Models;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pumpwatch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkoutPage : Page
    {
        //Global variable workout that is set to the selected workout in listbox
        //so that it easily can be used by other methods
        Workout workout;

        public WorkoutPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

        }

        /// <summary>
        /// Runs the method in ViewModel that fills the observablelist with 
        /// workouts from the database whenever this page is navigated too
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadWorkouts();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ListBox control.
        /// Sets the workout variable to the selected item in the listbox
        /// Also sets the variable Workout in ViewModel to be the selected item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                       
            workout = listBox.SelectedItem as Workout;
            ViewModel.Workout = workout;
        }

        /// <summary>
        /// Handles the SelectedWorkout event of the Delete control.
        /// Runs the DeleteSelectedWorkout in ViewModel
        /// Catches exception if no workout is selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the SelectedWorkout event of the SeeMore control.
        /// Navigates to the WorkoutDetailPage and sends the selcected item to the next page
        /// If no item is selected a MessageDialog will show
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
