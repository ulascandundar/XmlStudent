using AspNetCoreHero.ToastNotification.Abstractions;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        IStudentService _studentService;
        readonly INotyfService _notyfService;

        public StudentController(IStudentService studentService, INotyfService notyfService)
        {
            _studentService = studentService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("StudentPanel/Index");
            }

            _notyfService.Success("Sayfa Yüklendi");
            return View(_studentService.GetAll().Data); 
        }
        [HttpGet]
        public IActionResult StudentAdd()
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            ViewBag.error1 = false;
            return View();
        }
        [HttpPost]
        public IActionResult StudentAdd(StudentAdd studentAdd)
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            var result=_studentService.Add(studentAdd);
            if (result.Success==false)
            {
                ViewBag.error1 = true;
                
                return View();
            }
            _notyfService.Success("Öğrenci Eklendi");
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public IActionResult StudentUpdate(string id)
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
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
            _notyfService.Success("Öğrenci Güncellendi");
            return RedirectToAction("Index");
        }
        public IActionResult DeleteStudent(string id)
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            _studentService.Delete(id);
            return RedirectToAction("Index"); 
        }
    }
}
