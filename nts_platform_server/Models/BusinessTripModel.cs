using System;
using Microsoft.AspNetCore.Http;
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

    public class BusinessTripEditModel
    {
        public BusinessTripModel old { get; set; }
        public BusinessTripModel edit { get; set; }
    }

    public class ReportCheckNewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; set; }

        public DateTime? Date { get; set; } // дата оплаты
        public int Value { get; set; }
        public string Descriptions { get; set; }
        public string CheckBankPhotoName { get; set; } //Название файла для чека который пришел от банка
                                                       // public IFormFile CheckBankPhotoByte { get; set; } //Файл в байтах

        public int BusinessTripId { get; set; }
        public virtual BusinessTrip BusinessTrip { get; set; }

        public string TicketPhotoName { get; set; } //Название файла с его типом  билет бронирования на самолет
       // public IFormFile TicketPhotoByte { get; set; } //Файл в байтах

        public string BorderTicketPhotoName { get; set; } //Название файла с его типом  посадочного билета на самолет
        //public IFormFile BorderTicketPhotoByte { get; set; } //Файл в байтах


        public string BillPhotoName { get; set; } //Название файла с его типом  счет из отеля
       // public IFormFile BillPhotoByte { get; set; } //Файл в байтах
    }

    public class ReportCheckEditModel
    {
        public ReportCheckNewModel old { get; set; }
        public ReportCheckNewModel edit { get; set; }
    }
}

