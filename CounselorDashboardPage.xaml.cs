using Microsoft.Maui.Controls;
using System;

namespace Gradpath
{
    public partial class CounselorDashboardPage : ContentPage
    {
        public CounselorDashboardPage()
        {
            InitializeComponent();
            BindingContext = new CounselorDashboardViewModel();

            // Subscribe to alert messages
            MessagingCenter.Subscribe<CounselorDashboardViewModel, AlertMessage>(this, "DisplayAlert", async (sender, args) =>
            {
                await DisplayAlert(args.Title, args.Message, args.Cancel);
            });
        }

        private async void OnStudentTapped(object sender, EventArgs e)
        {
            var tappedEventArgs = e as TappedEventArgs;
            var student = tappedEventArgs?.Parameter as Student;

            if (student != null)
            {
                var counselorId = Preferences.Get("UserId", string.Empty); // Replace with the actual ID of the logged-in counselor
                await Shell.Current.GoToAsync($"{nameof(StudentDetailPage)}?studentId={student.Id}&counselorId={counselorId}");
            }
            else
            {
                Console.WriteLine("Tapped student is null.");
            }
        }
    }
}
