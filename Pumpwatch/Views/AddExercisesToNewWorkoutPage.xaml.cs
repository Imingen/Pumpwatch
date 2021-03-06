﻿using Pumpwatch.Models;
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
    public sealed partial class AddExercisesToNewWorkoutPage : Page
    {
        public AddExercisesToNewWorkoutPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Workout w = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Workout>(e.Parameter?.ToString());
            ViewModel.WorkoutName = w.WorkoutName;
            ViewModel.w1 = w;
            ViewModel.LoadExercises();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exercise exercise = listBox.SelectedItem as Exercise;
            foreach (Exercise ex in e.AddedItems)
            {
                ViewModel.SelectedExercises.Add(ex);
            }
        }
    }
}
