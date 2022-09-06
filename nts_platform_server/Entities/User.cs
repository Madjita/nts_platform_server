using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nts_platform_server.Entities;

namespace nts_platform_server.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        //public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public List<UserProject> UserProjects { get; set; }
        public List<Week> Weeks { get; set; }
        
        public User ()
        {
            UserProjects = new List<UserProject>();
            Weeks = new List<Week>();
            Profile = new Profile ();
        }


    }
}
