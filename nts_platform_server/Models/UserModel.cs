using System;
using System.Text.Json.Serialization;
using nts_platform_server.Entities;

namespace nts_platform_server.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Role Role { get; set; }
        public string Company { get; set; }
    }
}
