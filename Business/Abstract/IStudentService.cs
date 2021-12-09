using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IStudentService
    {
        IDataResult<List<Student>> GetAll();

        IResult Add(StudentAdd student);

        IResult Update(StudentAdd student);

        IResult Delete(string id);

        public IDataResult<Student> Get(string id);
    }
}
