using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gradpath
{
    public class Feedback
    {
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("counselor")]
        public Counselor Counselor { get; set; }
    }
}