using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Gradpath
{
    public partial class CareerDevelopmentPage : ContentPage
    {
        public ObservableCollection<CareerDevelopmentTips> CareerDevelopmentTips { get; set; }
        public ObservableCollection<RecommendedCourse> RecommendedCourses { get; set; }

        public CareerDevelopmentPage()
        {
            InitializeComponent();
            CareerDevelopmentTips = new ObservableCollection<CareerDevelopmentTips>
            {
                new CareerDevelopmentTips { Title = "Improve Communication Skills", Description = "Take a course on effective communication to enhance your interpersonal skills." },
                new CareerDevelopmentTips { Title = "Learn a New Programming Language", Description = "Expand your programming skills by learning a new language such as Python or Java." }
            };

            RecommendedCourses = new ObservableCollection<RecommendedCourse>
            {
                new RecommendedCourse { Title = "Effective Communication", Description = "A comprehensive course on improving communication skills.", Link = "https://example.com/communication-course" },
                new RecommendedCourse { Title = "Python for Beginners", Description = "An introductory course on Python programming.", Link = "https://example.com/python-course" }
            };

            BindingContext = this;
        }
    }
}
