using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataAccess.Abstract
{
    public interface IAuthDal
    {
        public void Add(Student student);


        public List<Student> GetAll();

        public void Delete(string id);

        public void Update(Student student);

        public void AddAdress(string id, string enlem, string boylam);
    }
}
