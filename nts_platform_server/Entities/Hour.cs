using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class DocHour : BaseEntity  //Сами почасовки со всем данными из эксельки
    {
        //public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Weekday { get; set; }
        public int ProjectNumber { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityCodeTravel { get; set; }
        public float TraverTimeG { get; set; }
        public string Destination { get; set; }
        public string WorkingTime { get; set; }
        public float WTHour { get; set; }
        public float TravelTimeC { get; set; }
        public string Info { get; set; }

        //public int WeekId { get; set; }
        public  Week Week { get; set; }

    }
}
