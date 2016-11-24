﻿using Pumpwatch.Models;
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
    public sealed partial class WorkoutDetailPage : Page
    {
        public WorkoutDetailPage()
        {
            this.InitializeComponent();
        }

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

        private void Edit_Name(object sender, RoutedEventArgs e)
        {
            WorkoutName.IsReadOnly = false;
            WorkoutDesc.IsReadOnly = false;
        }

        private async void Save_Name(object sender, RoutedEventArgs e)
        {
            await ViewModel.PutWorkout();
            WorkoutName.IsReadOnly = true;
            WorkoutDesc.IsReadOnly = true;
        }

        private void SortAlphabetically(object sender, RoutedEventArgs e)
        {
            ViewModel.SortAlfabetically();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ex = listBox.SelectedItem as Exercise;
            ViewModel.Exercise = ex;
        }

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
    }
}
