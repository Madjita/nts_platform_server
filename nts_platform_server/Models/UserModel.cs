using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using nts_platform_server.Entities;

namespace nts_platform_server.Models
{
    public class UserModelRegister
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Role Role { get; set; }
        public string Company { get; set; }
    }

    public class UserModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        //public virtual List<UserProjectModel> UserProjects { get; set; }

    }
}
