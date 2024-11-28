using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Gradpath
{
    public partial class DashboardPage : ContentPage
    {
        private readonly SupabaseClient _supabaseClient;

        public ObservableCollection<Notification> Notifications { get; set; }
        public ObservableCollection<JobListing> JobListings { get; set; }
        public ObservableCollection<Job> JobMatches { get; set; }
        public ObservableCollection<CareerDevelopmentTip> CareerDevelopmentTips { get; set; }
        public ObservableCollection<CounselorFeedback> Feedback { get; set; }
        public ObservableCollection<ComparisonData> ComparisonData { get; set; } // Corrected property name

        public string CounselingGuidance { get; set; }
        public string ProfilePicture { get; set; }
        public string FullName { get; set; }
        public string CareerInterests { get; set; }
        public string Goals { get; set; }
        public string Skills { get; set; }
        public string CareerChallenges { get; set; }
        public string JobMarketData { get; set; }

        public DashboardPage()
        {
            InitializeComponent();
            _supabaseClient = ServiceLocator.SupabaseClient;

            // Initialize notifications and job listings with placeholder data
            Notifications = new ObservableCollection<Notification>
            {
                new Notification { Title = "Welcome!", Message = "Thank you for joining GradPath." },
                new Notification { Title = "New Feature", Message = "Check out our new dashboard!" }
            };

            JobListings = new ObservableCollection<JobListing>
            {
                new JobListing { Position = "Software Developer", Company = "ABC Corp" },
                new JobListing { Position = "Data Analyst", Company = "XYZ Inc" }
            };

            JobMatches = new ObservableCollection<Job>();
            CareerDevelopmentTips = new ObservableCollection<CareerDevelopmentTip>
            {
                new CareerDevelopmentTip { Title = "Improve Communication Skills", Description = "Take a course on effective communication to enhance your interpersonal skills." },
                new CareerDevelopmentTip { Title = "Learn a New Programming Language", Description = "Expand your programming skills by learning a new language such as Python or Java." }
            };

            Feedback = new ObservableCollection<CounselorFeedback>();
            ComparisonData = new ObservableCollection<ComparisonData>
            {
                new ComparisonData { Title = "Industry Standards", Description = "Compare your skills with industry standards.", Details = "Your programming skills are above average compared to industry standards." },
                new ComparisonData { Title = "Peer Comparison", Description = "Compare your progress with your peers.", Details = "You are in the top 10% of your peer group in terms of programming skills." },
                new ComparisonData { Title = "Job Market Trends", Description = "Insights into current job market trends.", Details = "Data analysis is currently one of the most in-demand skills in the job market." }
            };

            CounselingGuidance = "Regular sessions with a career counselor.";

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUserDetails();
            await LoadJobMatches();
            await LoadFeedback();
            await LoadComparisonData();
        }

        private async Task LoadUserDetails()
        {
            var userIdString = Preferences.Get("UserId", string.Empty);
            Console.WriteLine($"Retrieved UserID from Preferences: {userIdString}");

            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userGuid))
            {
                // Attempt to get student details first
                var userDetails = await _supabaseClient.GetStudentDetails(userGuid);
                if (userDetails != null)
                {
                    ProfilePicture = userDetails.ProfilePictureUrl;
                    FullName = userDetails.FullName;
                    CareerInterests = userDetails.career_interests;
                    Goals = userDetails.goals;
                    Skills = userDetails.skills;

                    OnPropertyChanged(nameof(ProfilePicture));
                    OnPropertyChanged(nameof(FullName));
                    OnPropertyChanged(nameof(CareerInterests));
                    OnPropertyChanged(nameof(Goals));
                    OnPropertyChanged(nameof(Skills));
                    return;
                
                }

                await DisplayAlert("Error", "User details not found", "OK");
            }
            else
            {
                await DisplayAlert("Error", "User ID not found", "OK");
            }
        }

        private async Task LoadJobMatches()
        {
            var jobMatches = await JobMatchService.GetJobMatches(Skills, CareerInterests);
            JobMatches.Clear();
            foreach (var job in jobMatches)
            {
                JobMatches.Add(job);
            }
        }

        private async Task LoadFeedback()
        {
            var userIdString = Preferences.Get("UserId", string.Empty);
            var feedbackList = await FeedbackService.GetFeedbackForStudent(userIdString);
            Feedback.Clear();
            foreach (var feedback in feedbackList)
            {
                Feedback.Add(feedback);
            }
        }

        private async Task LoadComparisonData()
        {
            // Simulated data retrieval
            var comparisonDataList = new List<ComparisonData>
            {
                new ComparisonData { Title = "Industry Standards", Description = "Compare your skills with industry standards.", Details = "Your programming skills are above average compared to industry standards." },
                new ComparisonData { Title = "Peer Comparison", Description = "Compare your progress with your peers.", Details = "You are in the top 10% of your peer group in terms of programming skills." },
                new ComparisonData { Title = "Job Market Trends", Description = "Insights into current job market trends.", Details = "Data analysis is currently one of the most in-demand skills in the job market." }
            };

            ComparisonData.Clear();
            foreach (var comparisonData in comparisonDataList)
            {
                ComparisonData.Add(comparisonData);
            }
        }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class JobListing
    {
        public string Position { get; set; }
        public string Company { get; set; }
    }

    public class CareerDevelopmentTip
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
