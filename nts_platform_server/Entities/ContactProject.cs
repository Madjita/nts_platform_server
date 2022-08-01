using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class ContactProject : BaseEntity  //Предположительно одно лицо может быть прикреплено к нескольким проектам и так же на одном проекте есть несколько контактов, это соеденительный класс
    {
        //public int Id { get; set; }
       // public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        //public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
