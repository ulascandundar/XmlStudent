using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class StudentAdd
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Exam1 { get; set; }
        public string Exam2 { get; set; }
        public string Exam3 { get; set; }
        public string Tc { get; set; }
        public string Date { get; set; }
    }
}
