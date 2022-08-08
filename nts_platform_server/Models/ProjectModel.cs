using System;
using nts_platform_server.Entities;

namespace nts_platform_server.Models
{
    public class ProjectModel
    {
        public string Code { get; set; } //Код проекта может быть с символами
        public string NameProject { get; set; }
        public string Description { get; set; }
        public int MaxHours { get; set; }       // Количество часов выделенное на проект
        public string DateStart { get; set; } // Дата старта
        public string DateStop { get; set; }  // Дата завершения
        public Status Status { get; set; } // Статус проекта ( план, в работе, в архиве)

        public string EngineerCreaterEmail { get; set; } // Тот кто создал проект
    }

    public class ProjectEditModel
    {

        public ProjectModel OldProjectInfromation { get; set; }
        public ProjectModel NewProjectInfromation { get; set; }
    }
}
