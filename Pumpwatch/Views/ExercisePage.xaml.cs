using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pumpwatch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : Page
    {
        public ExercisePage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

        }

        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// Will run the LoadExercises method in ViewModel so the collection will be filled and the bound listview will show data
        /// Also will run the SetcomboboxValuesToCategoriesAsString in ViewModel to fill the ComboBox with selections
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            await ViewModel.LoadExercises();
            ViewModel.SetcomboboxValuesToCategoriesAsString();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the CategoryCB control.
        /// Runs the SortExerciseList in ViewModel that will sort a collection based on the
        /// selected value on the combobox. Selected value of CB is bound to property in VM and
        /// listbox is bound to the collection in DB that is being sorted
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private async void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          await ViewModel.SortExerciseList();
        }
    }
}
