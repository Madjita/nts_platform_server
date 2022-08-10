using System;
namespace nts_platform_server.Models
{
    public class DownloadProjectUserWeekExelModel
    {
        public string ProjectCode { get; set; }
        public string UserEmail { get; set; }
        public int YearWeek { get; set; }
        public int NumberWeek { get; set; }
    }
}