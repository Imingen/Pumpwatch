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
    public sealed partial class AddWorkoutPage : Page
    {
        public AddWorkoutPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ExList.LoadExercises();
        }

        /// <summary>
        /// Handles the Click event of the CreateWorkout control.
        /// Calls the PostWorkout method in ViewModel and will pass the returned value to the next page 
        /// with the navigationservice from template 10
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void CreateWorkout_Click(object sender, RoutedEventArgs e)
        {
            var w = await ViewModel.PostWorkout();
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(AddExercisesToNewWorkoutPage), w);
        }
    }
}
