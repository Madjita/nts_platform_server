using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class Profile : BaseEntity
    {
        [Key]
        [ForeignKey("User")]
        [JsonIgnore]
        public override int Id { get; set; }

        public User User { get; set; }
        public bool Sex { get; set; }
        public DateTime Date { get; set; }
        public int PRFseries { get; set; }
        public int PRFnumber { get; set; }
        public DateTime PRFdatetaked { get; set; }
        public DateTime PRFdateback { get; set; }
        public int PRFcode { get; set; }
        public string PRFtaked { get; set; }
        public string PRFplaceborned { get; set; }
        public string PRFplaceregistration { get; set; }
        public string PRFplacelived { get; set; }
        public int IPnumber { get; set; }
        public DateTime IPdatetaked { get; set; }
        public DateTime IPdateback { get; set; }
        public int IPcode { get; set; }
        public string IPtaked { get; set; }
        public string IPplaceborned { get; set; }
        public int ULMnumber { get; set; }
        public DateTime ULMdatetaked { get; set; }
        public DateTime ULMdateback { get; set; }
        public int ULMcode { get; set; }
        public string ULMtaked { get; set; }
        public string ULMplaceborned { get; set; }
        public string Snils { get; set; }
        public int INN { get; set; }
        public string Phone { get; set; }
        public string PhotoName { get; set; }
        public byte[] PhotoByte { get; set; }

    }
}
