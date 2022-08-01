using System;
using System.Text.Json.Serialization;
using nts_platform_server.Entities;

namespace nts_platform_server.Models
{
    public class AuthenticateResponse
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            MiddleName = user.MiddleName;
            Email = user.Email;
            Token = token;

            if(user.Role != null)
            {
                Role = user.Role.Title;
            }
        }
    }
}
