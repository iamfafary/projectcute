using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gradpath
{
    public static class JobMatchService
    {
        private static List<Job> _jobList = new List<Job>
        {
            new Job { Title = "Software Developer", Description = "Develop and maintain software applications.", Skills = new List<string> { "Programming", "IT", "Python", "Java", "C#", "Debugging" } },
            new Job { Title = "Data Analyst", Description = "Analyze data to help businesses make decisions.", Skills = new List<string> { "Data Analysis", "SQL", "Python", "Data Visualization", "Statistical Knowledge" } },
            new Job { Title = "Web Developer", Description = "Design and develop websites.", Skills = new List<string> { "HTML", "CSS", "JavaScript", "Front-End Frameworks", "Web Design" } },
            new Job { Title = "Cybersecurity Analyst", Description = "Protect systems and networks from cyber threats.", Skills = new List<string> { "Cybersecurity", "Risk Assessment", "Penetration Testing", "Security Tools", "Ethical Hacking" } },
            new Job { Title = "Network Administrator", Description = "Maintain network infrastructure.", Skills = new List<string> { "Networking", "TCP/IP", "LAN/WAN Management", "Cisco Tools", "Problem-Solving" } },
            new Job { Title = "Mobile App Developer", Description = "Develop mobile applications.", Skills = new List<string> { "Mobile Programming", "Cross-Platform Frameworks", "UI/UX for Mobile", "Debugging" } },
            new Job { Title = "IT Support Specialist", Description = "Provide IT support and troubleshooting.", Skills = new List<string> { "Hardware Troubleshooting", "Software Troubleshooting", "Customer Support", "OS Knowledge", "Problem Resolution" } },
            new Job { Title = "System Administrator", Description = "Manage and maintain servers.", Skills = new List<string> { "Server Management", "Virtualization", "Backup Solutions", "Linux", "Windows Server" } },
            new Job { Title = "Cloud Engineer", Description = "Manage cloud infrastructure.", Skills = new List<string> { "Cloud Platforms", "Docker", "Kubernetes", "Infrastructure as Code", "Resource Optimization" } },
            new Job { Title = "Database Administrator", Description = "Manage databases and ensure data security.", Skills = new List<string> { "Database Management", "SQL", "Query Optimization", "Data Security", "Backup Strategies" } },
            new Job { Title = "UI/UX Designer", Description = "Design user interfaces and improve user experience.", Skills = new List<string> { "Adobe XD", "Figma", "User Research", "Prototyping", "Usability Testing" } },
            new Job { Title = "DevOps Engineer", Description = "Implement CI/CD and manage infrastructure.", Skills = new List<string> { "CI/CD", "Scripting", "Monitoring Tools", "Kubernetes", "Jenkins" } },
            new Job { Title = "QA Tester (Quality Assurance)", Description = "Ensure the quality of software products.", Skills = new List<string> { "Manual Testing", "Automated Testing", "Test Cases", "Debugging", "Defect Reporting" } },
            new Job { Title = "Game Developer", Description = "Design and develop video games.", Skills = new List<string> { "Game Engines", "C#", "3D Modeling", "Animation Basics", "Game Physics" } },
            new Job { Title = "AI/ML Engineer", Description = "Develop AI and machine learning models.", Skills = new List<string> { "TensorFlow", "PyTorch", "Python", "Neural Networks", "Deep Learning" } },
            new Job { Title = "IT Project Manager", Description = "Manage IT projects from start to finish.", Skills = new List<string> { "Project Management", "Agile", "Jira", "Leadership", "Risk Management" } }
        };

        public static async Task<List<Job>> GetJobMatches(string skills, string careerInterests)
        {
            var skillList = skills.Split(',').Select(s => s.Trim()).ToList();
            var interestList = careerInterests.Split(',').Select(s => s.Trim()).ToList();

            var jobMatches = _jobList.Where(job =>
                job.Skills.Any(skill => skillList.Contains(skill)) ||
                interestList.Any(interest => job.Title.Contains(interest, System.StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return await Task.FromResult(jobMatches);
        }
    }
}
