using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Supabase.Interfaces;

namespace Gradpath
{
    [QueryProperty(nameof(StudentId), "studentId")]
    [QueryProperty(nameof(CounselorId), "counselorId")]
    public partial class StudentDetailPage : ContentPage
    {
        public string StudentId { get; set; }
        public string CounselorId { get; set; }

        private SupabaseClient _supabaseClient;

        public StudentDetailPage()
        {
            InitializeComponent();
            _supabaseClient = ServiceLocator.SupabaseClient;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (!string.IsNullOrEmpty(StudentId) && !string.IsNullOrEmpty(CounselorId))
            {
                var student = await _supabaseClient.GetStudentDetails(Guid.Parse(StudentId));
                BindingContext = new StudentDetailViewModel(student, CounselorId);
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e) 
        {
            await Shell.Current.GoToAsync("//CounselorDashboardPage");
        }
    }
}