using System;
using System.Collections.Generic;

namespace nts_platform_server.Models
{

    public class UserProjectModel
    {
        public string Email { get; set; }
        public string Project { get; set; }
    };

    public class UserProjectModelList
    {
        public virtual List<UserProjectModel> UserProjects { get; set; }
    }
}

