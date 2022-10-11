using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace nts_platform_server.Entities
{
    public class BusinessTrip : BaseEntity  //Связующая таблица командировок
    {
        public int UserProjectId {get;set;}
        public UserProject UserProject { get; set; }

        public string Name { get; set; }            // навзвание командировки
        public string Descriptions { get; set; }    // дополнительная информация


        [Column(TypeName = "date")]
        public DateTime DateStart { get; set; }    // дата начала командировки

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }      // дата окончания командировки


        private int _Spent { get; set; }
        public int Spent
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
                    result = _Spent;
                }

                return result;
            }
            set => _Spent = value;
        }

        public virtual List<ReportCheck> ReportChecks { get; set; } // для отчетов по чекам на проект по конкретному пользователю

        public BusinessTrip()
        {
            ReportChecks = new List<ReportCheck>();
        }
    }
}

