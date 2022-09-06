using System;
using System.Collections.Generic;

namespace nts_platform_server.Entities
{
    public class Company : BaseEntity  //Модель компании, пока что бесполезен, так как у нас одна компания, зато можно посчитать количество сотрудников
    {
        public string Name { get; set; }
        public string Place { get; set; }
        public int PersonalCount
        {
            get { return this.Users.Count; }
        }
        public virtual List<User> Users { get; set; }

        public Company()
        {
            Users = new List<User>();
        }
    }
}
