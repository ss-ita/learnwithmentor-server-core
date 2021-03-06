﻿using System.Numerics;
using Newtonsoft.Json;

namespace LearnWithMentor.Models
{
    public class GoogleUserData
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("given_name")]
        public string FirstName { get; set; }
        [JsonProperty("family_name")]
        public string LastName { get; set; }
        [JsonProperty("picture")]
        public string Picture { get; set; }
    }
}
