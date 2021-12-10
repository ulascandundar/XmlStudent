using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View(_studentService.GetAll().Data); 
        }
        [HttpGet]
        public IActionResult StudentAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult StudentAdd(StudentAdd studentAdd)
        {
            _studentService.Add(studentAdd);
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public IActionResult StudentUpdate(string id)
        {
            var data= _studentService.Get(id).Data;
            StudentAdd studentAdd = new StudentAdd()
            {
                Id = data.Id,
                Date = data.Date,
                Exam1 = data.Exams[0],
                Exam2 = data.Exams[1],
                Exam3 = data.Exams[2],
                Name = data.Name,
                Surname = data.Surname,
                Tc=data.Id,
                
            };
            return View(studentAdd); 

        }

        [HttpPost]
        public IActionResult StudentUpdate(StudentAdd studentAdd)
        {
            _studentService.Update(studentAdd);

            return RedirectToAction("Index");
        }
        public IActionResult DeleteStudent(string id)
        {
            _studentService.Delete(id);
            return RedirectToAction("Index"); 
        }
    }
}
