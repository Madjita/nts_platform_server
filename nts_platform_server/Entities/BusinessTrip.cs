using System;
using System.Collections.Generic;
using System.Linq;

namespace nts_platform_server.Entities
{
    public class BusinessTrip : BaseEntity  //Связующая таблица командировок
    {
        public int UserProjectId {get;set;}
        public UserProject UserProject { get; set; }

        private int _spent { get; set; }
        public int spent
        {
            get
            {
                var result = 0;
                if (ReportChecks.Count > 0)
                {
                    result = ReportChecks
                        .GroupBy(x => x.Id)
                        .Select(n => n.Sum(m => m.Value)).Sum();
                }
                else
                {
                    result = _spent;
                }

                return result;
            }
            set => _spent = value;
        }

        public virtual List<ReportCheck> ReportChecks { get; set; } // для отчетов по чекам на проект по конкретному пользователю

        public BusinessTrip()
        {
            ReportChecks = new List<ReportCheck>();
        }
    }
}

