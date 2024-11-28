using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Gradpath
{
    public partial class JobMarketPage : ContentPage
    {
        public ObservableCollection<Job> Jobs { get; set; }

        public JobMarketPage()
        {
            InitializeComponent();
            Jobs = new ObservableCollection<Job>
            {
                new Job { Title = "Software Engineer", Description = "High demand for software development skills." },
                new Job { Title = "Data Scientist", Description = "Growing need for data analysis and machine learning." }
            };
            lstJobs.ItemsSource = Jobs;
        }

        private async void OnJobSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedJob = e.SelectedItem as Job;
            if (selectedJob != null)
            {
                // Implement logic for job selection
                await DisplayAlert("Job Selected", $"You selected: {selectedJob.Title}", "OK");
            }
        }
    }
}