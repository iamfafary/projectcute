using Microsoft.Maui.Controls;

namespace Gradpath
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            Task.Delay(3000).ContinueWith(_ => MainThread.BeginInvokeOnMainThread(NavigateToIntro));
        }

        private async void NavigateToIntro()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
