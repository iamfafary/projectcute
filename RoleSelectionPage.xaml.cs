namespace Gradpath
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class RoleSelectionPage : ContentPage
    {
        public string UserId { get; set; }
        private SupabaseClient supabaseClient;

        public RoleSelectionPage()
        {
            InitializeComponent();
            supabaseClient = ServiceLocator.SupabaseClient;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            Console.WriteLine($"OnNavigatedTo called. UserId: {UserId}");

            if (Guid.TryParse(UserId, out _))
            {
                Console.WriteLine($"User ID successfully received: {UserId}");
            }
            else
            {
                Console.WriteLine("Invalid User ID received.");
                DisplayAlert("Error", "Invalid user ID.", "OK");
            }
        }

        private async void OnRoleSelected(object sender, EventArgs e)
        {
            var button = sender as Button;
            var role = button.CommandParameter.ToString();

            Console.WriteLine($"Role selected: {role}");

            if (Guid.TryParse(UserId, out Guid userGuid))
            {
                try
                {
                    Console.WriteLine($"Attempting to save user role: {role} for user ID: {userGuid}");
                    bool isRoleSaved = await supabaseClient.SaveUserRole(userGuid, role);
                    if (!isRoleSaved)
                    {
                        await DisplayAlert("Error", "Failed to save user role.", "OK");
                        Console.WriteLine($"Failed to save user role: {role} for user ID: {userGuid}");
                        return;
                    }

                    // Redirect to appropriate details page
                    if (role == "Counselor")
                    {
                        Console.WriteLine($"Redirecting to CounselorAddDetailsPage for user ID: {userGuid}");
                        await Shell.Current.GoToAsync($"{nameof(CounselorAddDetailsPage)}?userId={userGuid}");
                    }
                    else if (role == "Student")
                    {
                        Console.WriteLine($"Redirecting to StudentAddDetailsPage for user ID: {userGuid}");
                        await Shell.Current.GoToAsync($"{nameof(StudentAddDetailsPage)}?userId={userGuid}");
                    }
                    else
                    {
                        Console.WriteLine($"Unknown role selected: {role}");
                        await DisplayAlert("Error", "Unknown role selected.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during saving user role: {ex.Message}");
                    await DisplayAlert("Error", $"Exception during saving user role: {ex.Message}", "OK");
                }
            }
            else
            {
                Console.WriteLine("Invalid user ID");
                await DisplayAlert("Error", "Invalid user ID.", "OK");
            }
        }
    }
}
