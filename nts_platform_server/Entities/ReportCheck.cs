using System;
namespace nts_platform_server.Entities
{
    // новое добавил "Sergei" для отчетов по чекам на проект по конкретному пользователю
    public class ReportCheck : BaseEntity
    {
        public DateTime Date { get; set; } // дата оплаты
        public string Value { get; set; }
        public string Descriptions { get; set; }
        public string CheckBankPhotoName { get; set; } //Название файла для чека который пришел от банка
        public byte[] CheckBankPhotoByte { get; set; } //Файл в байтах
    }

    public class CheckPlane : ReportCheck
    {
        public string TicketPhotoName { get; set; } //Название файла с его типом  билет бронирования на самолет
        public byte[] TicketPhotoByte { get; set; } //Файл в байтах

        public string BorderTicketPhotoName { get; set; } //Название файла с его типом  посадочного билета на самолет
        public byte[] BorderTicketPhotoByte { get; set; } //Файл в байтах
    }

    public class CheckTrain : ReportCheck
    {
        public string BorderTicketPhotoName { get; set; } //Название файла с его типом  посадочного билета на поезд
        public byte[] BorderTicketPhotoByte { get; set; } //Файл в байтах
    }

    public class CheckHostel : ReportCheck
    {
        public string BillPhotoName { get; set; } //Название файла с его типом  счет из отеля
        public byte[] BillPhotoByte { get; set; } //Файл в байтах
    }

    /*
    public class CheckAnother : ReportCheck
    {
        public string CheckPhotoName { get; set; } //Название файла с его типом  любой чек из магазина
        public byte[] CheckPhotoByte { get; set; } //Файл в байтах
    }*/
}

