using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace Gradpath
{
    public class Counselor 
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("school_name")]
        public string SchoolName { get; set; }

        [JsonPropertyName("years_of_experience")]
        public int YearsOfExperience { get; set; }

        [JsonPropertyName("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }
    }
}