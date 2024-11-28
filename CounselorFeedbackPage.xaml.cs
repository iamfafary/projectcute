using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Gradpath
{
    public partial class CounselorFeedbackPage : ContentPage
    {
        public CounselorFeedbackPage()
        {
            InitializeComponent();
        }

        private async void OnSubmitFeedbackClicked(object sender, EventArgs e)
        {
            string studentId = txtStudentId.Text;
            string title = txtTitle.Text;
            string comments = txtComments.Text;

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(comments))
            {
                await DisplayAlert("Error", "Please enter all the required fields.", "OK");
                return;
            }

            bool success = await FeedbackService.SaveFeedback(new CounselorFeedback { StudentId = studentId, Title = title, Comments = comments });

            if (success)
            {
                await DisplayAlert("Success", "Your feedback has been submitted. Thank you!", "OK");
                await Shell.Current.GoToAsync("//CounselorDashboardPage"); // Navigate back to the counselor dashboard
            }
            else
            {
                await DisplayAlert("Error", "Failed to submit your feedback. Please try again.", "OK");
            }
        }
    }
}