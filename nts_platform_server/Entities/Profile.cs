using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class Profile : BaseEntity
    {
       // [Key]
      //  [ForeignKey("User")]
      //  [JsonIgnore]
      //  public override int Id { get; set; }

        public User User { get; set; }
        public bool Sex { get; set; }
        public DateTime Date { get; set; }
        public string PrfSeries { get; set; }
        public string PrfNumber { get; set; }
        public DateTime PrfDateTaked { get; set; }
        public DateTime PrfDateBack { get; set; }
        public string PrfCode { get; set; }
        public string PrfTaked { get; set; }
        public string PrfPlaceBorned { get; set; }
        public string PrfPlaceRegistration { get; set; }
        public string PrfPlaceLived { get; set; }
        public string IpNumber { get; set; }
        public DateTime IpDateTaked { get; set; }
        public DateTime IpDateBack { get; set; }
        public string IpCode { get; set; }
        public string IpTaked { get; set; }
        public string IpPlaceBorned { get; set; }
        public string UlmNumber { get; set; }
        public DateTime UlmDateTaked { get; set; }
        public DateTime UlmDateBack { get; set; }
        public string UlmTaked { get; set; }
        public string UlmPlaceBorned { get; set; }
        public string Snils { get; set; }
        public string Inn { get; set; }
        public string Phone { get; set; }
        public string PhotoName { get; set; }
        public byte[] PhotoByte { get; set; }

    }
}
