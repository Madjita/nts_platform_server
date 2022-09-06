using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class UserProject : BaseEntity  //Связующая таблица почасовки, работника и проекта
    {
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Week> Weeks { get; set; }
        public virtual List<ReportCheck> ReportChecks { get; set; } // новое добавил "Sergei" для отчетов по чекам на проект по конкретному пользователю


        public UserProject()
        {
            Weeks = new List<Week>();
            ReportChecks = new List<ReportCheck>(); // новое добавил "Sergei"
        }

    }
}
