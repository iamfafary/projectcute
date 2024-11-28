using Supabase.Interfaces;

namespace Gradpath
{
    public partial class MainPage : ContentPage
    {
        private SupabaseClient supabaseClient;
        private int _count = 0;
        public MainPage()
        {
            InitializeComponent();

            string url = "https://boewazjeesdjnirgzbbu.supabase.co"; 
            string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJvZXdhemplZXNkam5pcmd6YmJ1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE1NzU2MzMsImV4cCI6MjA0NzE1MTYzM30.pagd_l6tgBgdaObC1ZKHwi7Jw4MErksW--d6ySAdnYk"; 
            supabaseClient = new SupabaseClient(url, key);
            // Check if the user is authenticated
            if (IsUserAuthenticated())
            {
                // Redirect to Dashboard
                Shell.Current.GoToAsync("//DashboardPage");
            }
        }

        private bool IsUserAuthenticated()
        {
            // Implement your authentication check logic here
            // For example, check if a valid token is stored in secure storage
            return SecureStorage.GetAsync("auth_token") != null;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            _count++;
            ((Button)sender).Text = $"Clicked {_count} times";

            // You can add any additional logic here, such as:
            // Displaying a message
            DisplayAlert("Button Clicked", $"You have clicked {_count} times", "OK");

            // Or navigating to another page
            // await Shell.Current.GoToAsync("//DashboardPage");
        }
        private async void OnSignUpPageButtonClicked(object sender, EventArgs e)
        { // Navigate to SignUpPage and pass the SupabaseClient instance
          await Navigation.PushAsync(new SignUpPage()); }
        }

}
