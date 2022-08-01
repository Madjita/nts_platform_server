using System;
namespace nts_platform_server.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public int MaxHours { get; set; }       // Количество часов выделенное на проект
        public string DateStart { get; set; } // Дата старта
        public string DateStop { get; set; }  // Дата завершения
    }
}
