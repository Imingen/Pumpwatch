using Pumpwatch.Models;
using Windows.UI.Xaml.Controls;
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
            if (e != null)
            {
                Workout w = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Workout>(e.Parameter?.ToString());
                if (w != null)
                {
                    ViewModel.WorkoutName = w.WorkoutName;
                    ViewModel.Workout = w;
                    ViewModel.LoadExercises();
                }
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Exercise ex in e.AddedItems)
            {
                ViewModel.SelectedExercises.Add(ex);
            }
        }
    }
}
