﻿using Pumpwatch.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Pumpwatch.ViewModels
{
    public class ExercisePageViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public ObservableCollection<Exercise> Exercises { get; } = new ObservableCollection<Exercise>();
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();

        DatabaseOperator DatabaseOperator;
     

        public new event PropertyChangedEventHandler PropertyChanged;

        public ExercisePageViewModel() {}

        /// <summary>
        /// GETS the exercises from the database and adds each to the Exercises collection
        /// </summary>
        /// <returns></returns>
        public async Task LoadExercises()
        {
            DatabaseOperator = new DatabaseOperator();
            Exercise[] exercises = await DatabaseOperator.LoadData<Exercise>("Exercises");
            Exercises.Clear();
            foreach(Exercise ex in exercises)
            {
                Exercises.Add(ex);
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

        /// <summary>
        /// Converts the Enums of Category to a list of string and add the strings to observablecollection for easier use
        /// </summary>
        public void SetcomboboxValuesToCategoriesAsString()
        {
            string[] x =  Enum.GetNames(typeof(Category)).ToArray();
            Categories.Clear();
            foreach(var d in x)
            {
                Categories.Add(d);
            }
        }

        /// <summary>
        /// Sorts the exercise list based on the selected string in combobox
        /// </summary>
        /// <returns></returns>
        public async Task SortExerciseList()
        {
            if(category != "All")
            {
            await LoadExercises();
               var catQuery =
               from ex in Exercises
               where ex.Category.ToString() == category 
               select ex;

                var observable = new ObservableCollection<Exercise>(catQuery);
                Exercises.Clear();
                    
                foreach(var ex1 in observable)
                {
                    Exercises.Add(ex1);
                }
            }
            else
            {
                await LoadExercises();
            }
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
