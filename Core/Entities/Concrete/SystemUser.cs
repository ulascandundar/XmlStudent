using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class SystemUser:IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Fav { get; set; }
    }
}
