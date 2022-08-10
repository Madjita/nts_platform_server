using System;
using System.Collections.Generic;
using System.Globalization;
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
                float accum = 0;
                if(MoHour != null)
                {
                    if(MoHour.WorkingTime != null)
                    {
                        accum += float.Parse(MoHour.WorkingTime);
                    }
                    
                }
                if (TuHour != null)
                {
                    if (TuHour.WorkingTime != null)
                        accum += float.Parse(TuHour.WorkingTime);
                }
                if (WeHour != null)
                {
                    if (WeHour.WorkingTime != null)
                        accum += float.Parse(WeHour.WorkingTime);
                }
                if (ThHour != null)
                {
                    if (ThHour.WorkingTime != null)
                        accum += float.Parse(ThHour.WorkingTime);
                }
                if (FrHour != null)
                {
                    if (FrHour.WorkingTime != null)
                        accum += float.Parse(FrHour.WorkingTime);
                }
                if (SaHour != null)
                {
                    if (SaHour.WorkingTime != null)
                        accum += float.Parse(SaHour.WorkingTime);
                }
                if (SuHour != null)
                {
                    if (SuHour.WorkingTime != null)
                        accum += float.Parse(SuHour.WorkingTime);
                }

                return accum;
               /* return this.MoHour.WTHour +
                       this.TuHour.WTHour +
                       this.WeHour.WTHour +
                       this.ThHour.WTHour +
                       this.FrHour.WTHour +
                       this.SaHour.WTHour +
                       this.SuHour.WTHour;*/
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


        public virtual User User { get; set; } // информация кто добавил данную почасовку на неделю


    }
}
