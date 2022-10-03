using System;
using nts_platform_server.Entities;

namespace nts_platform_server.Models
{
    public class BusinessTripModel
    {
        public int id { get; set; }
        public string Name { get; set; }            // навзвание командировки
        public int Spent { get; set; }
        public string Descriptions { get; set; }    // дополнительная информация
        public DateTime DateStart { get; set; }    // дата начала командировки
        public DateTime? DateEnd { get; set; }    // дата начала командировки

        public UserProjectOnlyModel UserProject { get; set; }
    }
}

