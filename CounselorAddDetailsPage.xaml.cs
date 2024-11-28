namespace Gradpath
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class CounselorAddDetailsPage : ContentPage
    {
        public string UserId { get; set; }
        private SupabaseClient supabaseClient;
        private string profilePictureUrl;

        public CounselorAddDetailsPage()
        {
            InitializeComponent();
            supabaseClient = ServiceLocator.SupabaseClient;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
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

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (Guid.TryParse(UserId, out Guid userGuid))
            {
                string fullName = fullNameEntry.Text;
                string schoolName = schoolNameEntry.Text;
                string yearsOfExperience = yearsOfExperienceEntry.Text;

                try
                {
                    bool isDetailsSaved = await supabaseClient.InsertCounselorDetails(userGuid, fullName, schoolName, int.Parse(yearsOfExperience), profilePictureUrl);
                    if (isDetailsSaved)
                    {
                        bool isRoleSaved = await supabaseClient.SaveUserRole(userGuid, "Counselor");
                        if (isRoleSaved)
                        {
                            await DisplayAlert("Success", "Details saved successfully.", "OK");
                            await Shell.Current.GoToAsync("//CounselorDashboardPage");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to save user role.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to save details.", "OK");
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
                await DisplayAlert("Error", "Invalid user ID.", "OK");
            }
        }

        private async void OnUploadProfilePictureClicked(object sender, EventArgs e)
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a profile picture",
                FileTypes = FilePickerFileType.Images
            });

            if (fileResult != null)
            {
                Console.WriteLine($"File selected: {fileResult.FileName}");

                var stream = await fileResult.OpenReadAsync();
                if (stream == null)
                {
                    Console.WriteLine("Stream is null.");
                    await DisplayAlert("Error", "Failed to open the selected file.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(UserId))
                {
                    Console.WriteLine("User Id is null or empty.");
                    await DisplayAlert("Error", "User Id is not set.", "OK");
                    return;
                }

                // Use the new UploadAndSaveProfilePicture method
                profilePictureUrl = await supabaseClient.UploadAndSaveProfilePicture(UserId, stream, fileResult.FileName, "counselor");

                if (!string.IsNullOrEmpty(profilePictureUrl))
                {
                    Console.WriteLine($"Profile picture uploaded successfully. URL: {profilePictureUrl}");
                    profilePicture.Source = ImageSource.FromUri(new Uri(profilePictureUrl));
                    await DisplayAlert("Success", "Profile picture uploaded successfully.", "OK");
                }
                else
                {
                    Console.WriteLine("Failed to upload profile picture.");
                    await DisplayAlert("Error", "Failed to upload profile picture.", "OK");
                }
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        }
    }
}
