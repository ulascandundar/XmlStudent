using AspNetCoreHero.ToastNotification.Abstractions;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class StudentPanelController : Controller
    {
        readonly INotyfService _notyfService;
        private IStudentService _studentService;

        public StudentPanelController(INotyfService notyfService,IStudentService studentService)
        {
            _notyfService = notyfService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "User")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index","Student");
            }
            var result = _studentService.Get(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).Data;
            return View(result);
        }
    }
}
