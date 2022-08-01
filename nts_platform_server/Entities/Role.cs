using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class Role : BaseEntity
    {

        //public int Id { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }

        public Role(string title)
        {
            this.Title = title;
            Users = new List<User>();
        }
    }
}
