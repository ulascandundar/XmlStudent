using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Student:IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Tc { get; set; }
        public List<string> Exams { get; set; }
        public string Date { get; set; }
        
    }
}
