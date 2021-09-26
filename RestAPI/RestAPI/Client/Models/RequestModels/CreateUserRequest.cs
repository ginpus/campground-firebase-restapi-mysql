﻿using System.Text.Json.Serialization;

namespace RestAPI.Client.Models.RequestModels
{
    public class CreateUserRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("returnSecureToken")]
        public bool ReturnSecureToken { get; set; }
    }
}