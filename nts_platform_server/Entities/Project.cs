using System;
using System.Collections.Generic;

namespace nts_platform_server.Entities
{

    public enum Status
    {
        plan = 1,
        work,
        archive
    }

    /*public class Project : BaseEntity
    {
        public string Name { get; set; }
        //public string ProjectManager { get; set;}
        //public string ManEnginer { get; set; }

        public int ActualHours { get; set; }    // Актуальное количество затраченных часов
        public int MaxHours { get; set; }       // Количество часов выделенное на проект
        public DateTime DateStart { get; set; } // Дата старта
        public DateTime DateStop { get; set; }  // Дата завершения

        public Status Status { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<DocHour> DocHours { get; set; }

        public Project()
        {
            Users = new List<User>();
            DocHours = new List<DocHour>();
        }
    }*/

    public class Project : BaseEntity
    {
        //public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int Progress { get; set; }
        public bool Done { get; set; }
        public int MaxHour { get; set; }
        public int ActualHour { get; set; }
        public Status Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Descriptions { get; set; }
        public List<UserProject> UserProjects { get; set; }
        public List<ContactProject> ContactProjects { get; set; }

        public Project()
        {
            UserProjects = new List<UserProject>();
            ContactProjects = new List<ContactProject>();
        }
    }
}
