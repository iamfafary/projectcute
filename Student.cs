using System;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Models;

namespace Gradpath
{
    public class Student : BaseModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("career_interests")]
        public string career_interests { get; set; }

        [JsonPropertyName("skills")]
        public string skills { get; set; }

        [JsonPropertyName("goals")]
        public string goals { get; set; }

        [JsonPropertyName("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }
    }

}