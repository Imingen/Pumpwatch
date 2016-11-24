using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
    public sealed partial class ExclusivePage : Page
    {
        public ExclusivePage()
        {
            this.InitializeComponent();
        }

        private CancellationTokenSource cts;
       
        private async void DownloadOffer(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            LoadingBar.IsActive = true;
            using (var client = new HttpClient())
            {
                await client.GetAsync(new Uri("http://toppen-il.no/"), cts.Token);
                await Task.Delay(6000);

                if (!cts.IsCancellationRequested)
                {
                    jo.Text = "jjjjij";

                }
                else
                {
                    LoadingBar.IsActive = false;
                    jo.Text = "cancelled";
                }
            }
        }


        private void CancelDownload(object sender, RoutedEventArgs e)
        {
            this.cts.Cancel();
            LoadingBar.IsActive = false;
        }
    }
}
