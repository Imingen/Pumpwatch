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
    public sealed partial class WorkoutDetailPage : Page
    {
        public WorkoutDetailPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// Sets a Workout variable equal to the workout object that was passed to this page
        /// with navigatonservice.
        /// Sets the corresponding properties in viewmodel to be different values of the object that was passed along the navigation
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Workout w = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Workout>(e.Parameter?.ToString());
            //Sets the corresponding properties in viewmodel to the name and description
            //of the selected workout so that its easier to bind the textboxes to the correct properties
            ViewModel.WorkoutName = w.WorkoutName;
            ViewModel.Description = w.WorkoutDescription;
            ViewModel.Workout = w;
            await ViewModel.LoadWorkoutExerciseList();
        }

        /// <summary>
        /// Handles the Name event of the Edit control.
        /// Will make the textboxes not readonly so the user can edit the content of the textboses
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Edit_Name(object sender, RoutedEventArgs e)
        {
            WorkoutName.IsReadOnly = false;
            WorkoutDesc.IsReadOnly = false;
        }

        /// <summary>
        /// Handles the Name event of the Save control.
        /// Runs teh putworkout method in ViewModel and then sets the textbox.IsReadOnly to true
        /// so they cant be editable
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Save_Name(object sender, RoutedEventArgs e)
        {
            await ViewModel.PutWorkout();
            WorkoutName.IsReadOnly = true;
            WorkoutDesc.IsReadOnly = true;
        }

        /// <summary>
        /// Sorts the listbox alphabetically.
        /// By running the SortAlfabetically method in ViewModel that sorts the collection the listbox is bound to 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SortAlphabetically(object sender, RoutedEventArgs e)
        {
            ViewModel.SortAlfabetically();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the listBox control.
        /// Will set the property Exercise in ViewModel to the selecteditem in the listbox
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ex = listBox.SelectedItem as Exercise;
            ViewModel.Exercise = ex;
        }

        /// <summary>
        /// Handles the SelectedExercise event of the Delete control.
        /// Deletes the selected workout by runnin the DeleteExerciseFromWorkout method in ViewModel
        /// If no exercise is selected it will catch exception
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Delete_SelectedExercise(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.DeleteExerciseFromWorkout();
            }
            catch (NullReferenceException)
            {
                MessageDialog msg = new MessageDialog("Select an Exercise to delete");
                await msg.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the Workout event of the Add control.
        /// Will add more exercises to the selected workout.
        /// Will pass the selected workout to AddExercisesToWorkout
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Add_Exercise(object sender, RoutedEventArgs e)
        {
            Workout w = ViewModel.Workout;
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(AddExercisesToNewWorkoutPage), w);
        }


    }
}
