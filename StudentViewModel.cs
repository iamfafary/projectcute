using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gradpath
{
    public class StudentDetailViewModel : INotifyPropertyChanged
    {
        private readonly SupabaseClient _supabaseClient;
        public string ProfilePictureUrl { get; set; }
        public string FullName { get; set; }
        public string CareerInterests { get; set; }
        public string Goals { get; set; }
        public string Skills { get; set; }
        public ObservableCollection<Feedback> Feedback { get; set; }
        public string NewComment { get; set; }
        public ICommand SubmitCommentCommand { get; set; }
        public string CounselorId { get; set; }
        public ObservableCollection<Job> JobMatches { get; set; }

        public StudentDetailViewModel(Student student, string userId)
        {
            _supabaseClient = ServiceLocator.SupabaseClient;
            ProfilePictureUrl = student.ProfilePictureUrl;
            FullName = student.FullName;
            CareerInterests = student.career_interests;
            Goals = student.goals;
            Skills = student.skills;
            Feedback = new ObservableCollection<Feedback>();
            JobMatches = new ObservableCollection<Job>();
            CounselorId = userId;

            SubmitCommentCommand = new Command(async () => await SubmitComment(student.Id.ToString()));
            LoadFeedback(student.Id.ToString());
            LoadJobMatches(student.skills, student.career_interests);
        }

        private async Task LoadFeedback(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                Console.WriteLine("Student ID is null or empty.");
                return;
            }

            var feedbackList = await _supabaseClient.GetFeedbackForStudent(studentId);
            if (feedbackList == null)
            {
                Console.WriteLine("Feedback list is null.");
                return;
            }

            Console.WriteLine($"Fetched {feedbackList.Count} feedback entries."); // Debug statement
            Feedback.Clear();
            foreach (var feedback in feedbackList)
            {
                if (feedback == null)
                {
                    Console.WriteLine("Feedback entry is null.");
                    continue;
                }

                if (feedback.Counselor == null)
                {
                    Console.WriteLine("Counselor is null for feedback entry.");
                }
                else
                {
                    Console.WriteLine($"Feedback: {feedback.Comment} by {feedback.Counselor.FullName}"); // Debug statement
                }

                Feedback.Add(feedback);
            }
            OnPropertyChanged(nameof(Feedback));
        }


        private async Task SubmitComment(string studentId)
        {
            if (!string.IsNullOrEmpty(NewComment))
            {
                var currentCounselor = await _supabaseClient.GetCounselorDetails(Guid.Parse(CounselorId));
                Console.WriteLine($"Fetched counselor details: {currentCounselor?.FullName}");
                if (currentCounselor != null)
                {
                    bool isAdded = await _supabaseClient.AddFeedback(studentId, CounselorId, NewComment);
                    if (isAdded)
                    {
                        Feedback.Add(new Feedback { Comment = NewComment, Counselor = currentCounselor });
                        NewComment = string.Empty;
                        OnPropertyChanged(nameof(NewComment));
                    }
                }
            }
        }

        private async Task LoadJobMatches(string skills, string careerInterests)
        {
            var jobMatches = await JobMatchService.GetJobMatches(skills, careerInterests);
            JobMatches.Clear();
            foreach (var job in jobMatches)
            {
                JobMatches.Add(job);
            }
            OnPropertyChanged(nameof(JobMatches));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
