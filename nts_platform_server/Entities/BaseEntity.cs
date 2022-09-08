using System;
using System.Text.Json.Serialization;

namespace nts_platform_server.Entities
{
    public class BaseEntity
    {
       // [JsonIgnore]
        public virtual int Id { get; set; }
    }
}
