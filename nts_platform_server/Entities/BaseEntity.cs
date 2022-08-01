using System;
using System.Text.Json.Serialization;

namespace nts_platform_server.Entities
{
    public class BaseEntity
    {
       // [JsonIgnore]
        public long Id { get; set; }
    }
}
