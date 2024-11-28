namespace Gradpath
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class StudentAddDetailsPage : ContentPage
    {
        public string UserId { get; set; }
        private SupabaseClient supabaseClient;
        private string profilePictureUrl;

        public List<string> AvailableSkills { get; set; }
        public string SelectedSkills { get; set; } = string.Empty;

        public StudentAddDetailsPage()
        {
            InitializeComponent();
            supabaseClient = ServiceLocator.SupabaseClient;

            AvailableSkills = new List<string>
            {
                "Programming", "C#", "IT", "Python", "Java", "Data Analysis", "SQL", "HTML", "CSS", "JavaScript",
                "Cybersecurity", "Networking", "Mobile Programming", "Customer Support", "Server Management",
                "Cloud Platforms", "Database Management", "Adobe XD", "Figma", "CI/CD", "Manual Testing",
                "Game Engines", "TensorFlow", "Project Management"
            };
            BindingContext = this;
        }

        private void OnSkillsPickerChanged(object sender, EventArgs e)
        {
            if (skillsPicker.SelectedItem != null)
            {
                SelectedSkills += skillsPicker.SelectedItem.ToString() + ", ";
                OnPropertyChanged(nameof(SelectedSkills));
            }
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
                string careerInterests = careerInterestsEntry.Text;
                string skills = SelectedSkills.TrimEnd(',', ' '); // Remove trailing comma and space
                string goals = goalsEntry.Text;

                try
                {
                    bool isDetailsSaved = await supabaseClient.InsertStudentDetails(userGuid, fullName, careerInterests, skills, goals, profilePictureUrl);
                    if (isDetailsSaved)
                    {
                        bool isRoleSaved = await supabaseClient.SaveUserRole(userGuid, "Student");
                        if (isRoleSaved)
                        {
                            await DisplayAlert("Success", "Details saved successfully.", "OK");
                            await Shell.Current.GoToAsync("//DashboardPage");
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
