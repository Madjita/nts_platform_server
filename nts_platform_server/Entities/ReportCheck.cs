using System;
namespace nts_platform_server.Entities
{
    // новое добавил "Sergei" для отчетов по чекам на проект по конкретному пользователю
    public class ReportCheck : BaseEntity
    {
        public string Value { get; set; }
        public string Descriptions { get; set; }
        public string PhotoName { get; set; }
        public byte[] PhotoByte { get; set; }
    }
}

