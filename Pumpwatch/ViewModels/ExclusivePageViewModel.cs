using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Pumpwatch.ViewModels
{
   public class ExclusivePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
 
        private bool check;

        public bool Check
        {
            get { return check; }
            set
            {
                if(value != this.check)
                {
                    check = value;
                    NotifyPropertyChanged("Check");
                }
            }

        }

        CancellationTokenSource cts;

        public async Task DownloadOffer()
        {
            check = true;
            cts = new CancellationTokenSource();
            using (var client = new HttpClient())
            {
                await client.GetAsync(new Uri("http://toppen-il.no/"), cts.Token);

                if (!cts.IsCancellationRequested)
                {
                    await Task.Delay(6000);
                }
                else
                {
                    check = false;
                }
            }
        }

        public void Cancel()
        {
            this.cts.Cancel();
        }





        public new event PropertyChangedEventHandler PropertyChanged;
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
