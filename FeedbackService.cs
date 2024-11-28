using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gradpath
{
    public static class FeedbackService
    {
        public static async Task<bool> SaveFeedback(CounselorFeedback feedback)
        {
            // Implement logic to save feedback to the database
            // For now, we simulate success
            return await Task.FromResult(true);
        }

        public static async Task<List<CounselorFeedback>> GetFeedbackForStudent(string studentId)
        {
            // Implement logic to retrieve feedback for a specific student from the database
            // For now, we simulate data retrieval
            return await Task.FromResult(new List<CounselorFeedback>
            {
                new CounselorFeedback { Title = "Great Progress", Comments = "You have shown significant improvement in your skills.", StudentId = studentId },
                new CounselorFeedback { Title = "Need to Work on Soft Skills", Comments = "I recommend focusing more on communication and teamwork.", StudentId = studentId }
            });
        }
    }
}
