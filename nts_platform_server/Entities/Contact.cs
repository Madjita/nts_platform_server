using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nts_platform_server.Entities
{
    public class Contact : BaseEntity  //Контакты лиц напрямую не работающих в компании, а цепляемых к проекту
    {
       // public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Info { get; set; }
    }
}
