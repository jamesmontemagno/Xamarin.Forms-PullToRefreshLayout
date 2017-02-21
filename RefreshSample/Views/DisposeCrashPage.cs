using Refractored.XamForms.PullToRefresh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RefreshSample.Views
{
    class DisposeCrashPage : ContentPage
    {
        private readonly View mainView;

        public DisposeCrashPage()
        {
            Title = "Test Page";
            mainView = new PullToRefreshLayout
            {
                Content = new ScrollView()
                {
                    Content = new Label() { Text = "This is a test." }
                }
            };
            Content = mainView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Content = new ActivityIndicator { HorizontalOptions = LayoutOptions.Center, IsEnabled = true, IsVisible = true, IsRunning = true };
            await Task.Delay(3000);
            try
            {
                Content = mainView;
            }
            catch (ObjectDisposedException ex)
            {
                // Just a place to put a debug breakpoint
                throw;
            }
        }
    }
}
