using Supabase.Interfaces;

namespace Gradpath
{
    public partial class LoginPage : ContentPage
    {
        private SupabaseClient _supabaseClient;

        public LoginPage()
        {
            InitializeComponent();
            _supabaseClient = ServiceLocator.SupabaseClient;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Login Failed", "Please enter both email and password.", "OK");
                return;
            }

            try
            {
                var userId = await _supabaseClient.LoginUser(email, password);
                Console.WriteLine($"Login User ID: {userId}");

                if (userId != Guid.Empty)
                {
                    // Store User ID in Preferences
                    Preferences.Set("UserId", userId.ToString());
                    // Get the user role
                    var userRole = await _supabaseClient.GetUserRole(userId);
                    Console.WriteLine($"User Role: {userRole}");

                    if (string.IsNullOrEmpty(userRole))
                    {
                        Console.WriteLine("User role is not set, redirecting to RoleSelectionPage.");
                        await Shell.Current.GoToAsync($"{nameof(RoleSelectionPage)}?userId={userId}");
                    }
                    else
                    {
                        // Check if user details are set
                        var userDetailsSet = await _supabaseClient.CheckUserDetails(userId);
                        Console.WriteLine($"User Details Set: {userDetailsSet}");

                        if (!userDetailsSet)
                        {
                            Console.WriteLine($"User details are not set, redirecting to AddDetailsPage for role: {userRole}");

                            if (userRole == "Counselor")
                            {
                                await Shell.Current.GoToAsync($"{nameof(CounselorAddDetailsPage)}?userId={userId}");
                            }
                            else if (userRole == "Student")
                            {
                                await Shell.Current.GoToAsync($"{nameof(StudentAddDetailsPage)}?userId={userId}");
                            }
                            else
                            {
                                await DisplayAlert("Login Failed", "Unknown user role.", "OK");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"User details are set, redirecting to Dashboard for role: {userRole}");

                            if (userRole == "Counselor")
                            {
                                await Shell.Current.GoToAsync(nameof(CounselorDashboardPage));
                            }
                            else if (userRole == "Student")
                            {
                                await Shell.Current.GoToAsync(nameof(DashboardPage));
                            }
                            else
                            {
                                await DisplayAlert("Login Failed", "Unknown user role.", "OK");
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during login: {ex.Message}");
                await DisplayAlert("Login Failed", $"Exception during login: {ex.Message}", "OK");
            }
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SignUpPage)); // Navigate to SignUpPage
        }

        private async void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            // Handle Google Login
        }

        private async void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            // Handle Facebook Login
        }
    }
}
