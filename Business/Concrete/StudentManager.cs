using Business.Abstract;
using Core.Aspects.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class StudentManager : IStudentService
    {
        private IAuthDal _studentDal;

        public StudentManager(IAuthDal studentDal)
        {
            _studentDal = studentDal;
        }

        [CacheRemoveAspect("IStudentService.Get")]
        public IResult Add(StudentAdd student)
        {
            List<string> datas = new List<string> { student.Exam1, student.Exam2, student.Exam3 };
            var results = _studentDal.GetAll();
            var result = results.FirstOrDefault(s => s.Id == student.Id);
            if (result!=null)
            {
                return new ErrorResult("Tc zaten var");
            }
            Student real = new Student()
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                Exams=datas,
                Date=student.Date
                
            };
            
             _studentDal.Add(real);
            return new SuccessResult("Eklendi");
        }
        [CacheRemoveAspect("IStudentService.Get")]
        public IResult Delete(string id)
        {
            _studentDal.Delete(id);
            return new SuccessResult("Silindi");

        }
        [CacheAspect]
        public IDataResult<List<Student>> GetAll()
        {
            List<Student> students=_studentDal.GetAll();
            return new SuccessDataResult<List<Student>>(students);
        }
        [CacheRemoveAspect("IStudentService.Get")]
        public IResult Update(StudentAdd student)
        {
            List<string> datas = new List<string> { student.Exam1, student.Exam2, student.Exam3 };
            Student real = new Student()
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                Exams = datas,
                Tc=student.Tc,
                Date=student.Date

            };
            _studentDal.Update(real);
            return new SuccessResult("Güncellendi");
        }

        public IDataResult<Student> Get(string id)
        {
           var data= GetAll().Data;
            Student student= data.FirstOrDefault(s => s.Id == id);
            return new SuccessDataResult<Student>(student);
        }

        public IResult AddAdress(string id, string enlem, string boylam)
        {
            var user = Get(id);
            if (user.Data==null)
            {
                return new ErrorResult("Böyle bir kullanıcı yok");
            }
            _studentDal.AddAdress(id, enlem, boylam);
            return new SuccessResult("Başarıyla eklendi");
        }



       
    }
}
