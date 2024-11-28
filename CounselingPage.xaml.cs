using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Gradpath
{
    public partial class CounselingPage : ContentPage
    {
        public CounselingPage()
        {
            InitializeComponent();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string description = txtDescription.Text;

            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "Please enter a description of your current career situation and goals.", "OK");
                return;
            }

            bool success = await CounselingService.SaveCounselingRequest(new CounselingRequest { Description = description });

            if (success)
            {
                await DisplayAlert("Success", "Your request has been submitted. A counselor will reach out to you soon.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to submit your request. Please try again.", "OK");
            }
        }
    }

    public static class CounselingService
    {
        public static async Task<bool> SaveCounselingRequest(CounselingRequest request)
        {
            // Implement logic to save the counseling request to the database
            // For now, we simulate success
            return await Task.FromResult(true);
        }
    }
}
