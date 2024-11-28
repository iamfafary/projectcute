using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Gradpath
{
    public partial class ComparisonDataPage : ContentPage
    {
        public ObservableCollection<ComparisonData> ComparisonData { get; set; }

        public ComparisonDataPage()
        {
            InitializeComponent();
            ComparisonData = new ObservableCollection<ComparisonData>
            {
                new ComparisonData { Title = "Industry Standards", Description = "Compare your skills with industry standards.", Details = "Your programming skills are above average compared to industry standards." },
                new ComparisonData { Title = "Peer Comparison", Description = "Compare your progress with your peers.", Details = "You are in the top 10% of your peer group in terms of programming skills." },
                new ComparisonData { Title = "Job Market Trends", Description = "Insights into current job market trends.", Details = "Data analysis is currently one of the most in-demand skills in the job market." }
            };

            BindingContext = this;
        }
    }
}