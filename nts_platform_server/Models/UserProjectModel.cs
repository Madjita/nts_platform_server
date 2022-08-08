using System;
using System.Collections.Generic;
using nts_platform_server.Entities;

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

    public class UserProjectModelHours //Связующая таблица почасовки, работника и проекта
    {
        public UserModel User { get; set; }
        public ProjectModel Project { get; set; }
        public List<Week> Weeks { get; set; }

    }
}

