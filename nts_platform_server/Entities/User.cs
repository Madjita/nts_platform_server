using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nts_platform_server.Entities;

namespace nts_platform_server.Entities
{
    public class User : BaseEntity
    {
        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Place { get; set; }
        public string Number { get; set; }
        public string Info { get; set; }
        //public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        //public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<UserProject> UserProjects { get; set; }
        public List<Week> Weeks { get; set; }

        public User ()
        {
            UserProjects = new List<UserProject>();
            Weeks = new List<Week>();
        }


    }
}
