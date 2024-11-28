using Supabase.Interfaces;
using System.Text.RegularExpressions;

namespace Gradpath
{
    public partial class SignUpPage : ContentPage
    {
        private SupabaseClient supabaseClient;

        public SignUpPage()
        {
            InitializeComponent();
            supabaseClient = ServiceLocator.SupabaseClient;
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Sign Up Failed", "Invalid email format.", "OK");
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Sign Up Failed", "Password should be at least 6 characters long.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Sign Up Failed", "Passwords do not match.", "OK");
                return;
            }

            string userId = null;
            try
            {
                Console.WriteLine("Calling RegisterUser...");
                userId = await supabaseClient.RegisterUser(email, password);
                Console.WriteLine($"Received userId: {userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                await DisplayAlert("Error", $"Exception during registration: {ex.Message}", "OK");
                return;
            }

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid userGuid))
            {
                await DisplayAlert("Success", "Successfully registered! You can now log in to your account.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                Console.WriteLine("Registration failed with userId being null or invalid.");
                await DisplayAlert("Error", "Registration failed. Please try again.", "OK");
            }
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}
