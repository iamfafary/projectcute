using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace Gradpath
{
    public class CounselorDashboardViewModel : INotifyPropertyChanged
    {
        private readonly SupabaseClient _supabaseClient;
        public ObservableCollection<Student> Students { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CounselorFullName { get; set; }
        public string CounselorSchool { get; set; }
        public int CounselorYearsOfExperience { get; set; }

        public CounselorDashboardViewModel()
        {
            _supabaseClient = ServiceLocator.SupabaseClient;
            Students = new ObservableCollection<Student>();
            LoadCounselorDetails();
            LoadStudentList();
        }

        private async void LoadCounselorDetails()
        {
            var userIdString = Preferences.Get("UserId", string.Empty);
            Console.WriteLine($"Retrieved UserID from Preferences: {userIdString}");

            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userGuid))
            {
                var userDetails = await _supabaseClient.GetCounselorDetails(userGuid);
                if (userDetails != null)
                {
                    ProfilePictureUrl = userDetails.ProfilePictureUrl;
                    CounselorFullName = userDetails.FullName;
                    CounselorSchool = userDetails.SchoolName;
                    CounselorYearsOfExperience = userDetails.YearsOfExperience;

                    OnPropertyChanged(nameof(ProfilePictureUrl));
                    OnPropertyChanged(nameof(CounselorFullName));
                    OnPropertyChanged(nameof(CounselorSchool));
                    OnPropertyChanged(nameof(CounselorYearsOfExperience));
                    return;
                }

                MessagingCenter.Send(this, "DisplayAlert", new AlertMessage("Error", "User details not found", "OK"));
            }
            else
            {
                MessagingCenter.Send(this, "DisplayAlert", new AlertMessage("Error", "User ID not found", "OK"));
            }
        }

        private async void LoadStudentList()
        {
            var students = await _supabaseClient.GetStudents();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
