﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities //Хранит данные по всем 7 дням недели(7 элементов таблицы Hour), чтоб сформировать отчет за неделю
{
    public class Week : BaseEntity
    {
        //public int Id { get; set; }
        public float SumHour {
            get
            {
                return this.MoHour.WTHour +
                       this.TuHour.WTHour +
                       this.WeHour.WTHour +
                       this.ThHour.WTHour +
                       this.FrHour.WTHour +
                       this.SaHour.WTHour +
                       this.SuHour.WTHour;
            }
        }
        
        public int Year { get; set; }
        public int Month { get; set; }
        public int NumberWeek { get; set; }

        //public int MoHourId { get; set; }
        public virtual DocHour MoHour { get; set; }

        //public int TuHourId { get; set; }
        public virtual DocHour TuHour { get; set; }

       // public int WeHourId { get; set; }
        public virtual DocHour WeHour { get; set; }

        //public int ThHourId { get; set; }
        public virtual DocHour ThHour { get; set; }

        //public int FrHourId { get; set; }
        public virtual DocHour FrHour { get; set; }

        //public int SaHourId { get; set; }
        public virtual DocHour SaHour { get; set; }

        //public int SuHourId { get; set; }
        public virtual DocHour SuHour { get; set; }

        //public int UserProjectId { get; set; }
        public virtual UserProject UserProject { get; set; }


    }
}