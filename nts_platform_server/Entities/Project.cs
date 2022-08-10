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
        public int indexAdd { get; set; }          // Позиция номера проекта по очереди добавления
        public string Code { get; set; }         // Код проекта может быть с символами
        public string Title { get; set; }        // Название проекта
        public int Progress { get; set; }        // Прогресс в процентах
        public bool Done { get; set; }           // Выполенн или нет
        public int MaxHour { get; set; }         // Максимальное количество часов
        public int ActualHour {

           /* get {

                int acuum = 0;
                if(UserProjects!= null)
                {
                    foreach (var user in UserProjects)
                    {
                        if(user.Weeks != null)
                        {
                            foreach (var week in user.Weeks)
                            {
                                acuum += (int)week.SumHour;
                            }
                        }
                    }
                }

               
                return acuum;
            }*/
            get;set;
            //set;
        }      // Актуальное количество часов
        public Status Status { get; set; }       // Статус проекта ( план, в работе, в архиве)
        public DateTime Start { get; set; }      // Дата начала
        public DateTime End { get; set; }        // Дата  фактического завершения
        public string Description { get; set; } // Дополнительная информация о проекте
        public User EnginerCreater { get; set; }           // Тот кто создал проект


        public List<UserProject> UserProjects { get; set; }       // Список рабочих относящихся к проекту
        public List<ContactProject> ContactProjects { get; set; } // Список контактов относящихся к проекту

        public Project()
        {
            UserProjects = new List<UserProject>();
            ContactProjects = new List<ContactProject>();
        }
    }
}
