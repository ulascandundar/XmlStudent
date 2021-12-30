using AspNetCoreHero.ToastNotification.Abstractions;
using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private IAuthService _authService;
        readonly INotyfService _notyfService;

        public AdminController(IAuthService authService, INotyfService notyfService)
        {
            _authService = authService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role!="Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var users = _authService.GetAll();
            bool super = users.Data.FirstOrDefault(u => u.Id == a).Super;
            ViewBag.super = super;
            return View(_authService.GetAll().Data.Where(a=>a.Type=="Admin").ToList());
        }

        [HttpGet]
        public IActionResult AdminAdd()
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var users = _authService.GetAll();
            bool super = users.Data.FirstOrDefault(u => u.Id == a).Super;
            if (super==false)
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminAdd(SystemUser systemUser)
        {
            var result =_authService.Add(systemUser);
            if (result.Success == false)
            {
                _notyfService.Error(result.Message);

                return View();
            }
            _notyfService.Success("Admin Eklendi");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AdminUpdate(string id)
        {
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != "Admin")
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var users = _authService.GetAll();
            bool super = users.Data.FirstOrDefault(u => u.Id == a).Super;
            if (super == false)
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            var datas = _authService.GetAll().Data;
            var data = datas.FirstOrDefault(u => u.Id == id);
            SystemUser systemUser = new SystemUser()
            {
                Id = data.Id,
                Fav = data.Fav,
                Mail = data.Mail,
                Name = data.Name,
                Password = data.Password,
                Super = data.Super
            };
            return View(systemUser);
        }
        [HttpPost]
        public IActionResult AdminUpdate(SystemUser systemUser)
        {
            var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var users = _authService.GetAll();
            bool super = users.Data.FirstOrDefault(u => u.Id == a).Super;
            if (super == false)
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            _authService.Update(systemUser);
            _notyfService.Success("Güncellendi");
            return RedirectToAction("Index");
        }
        public IActionResult DeleteAdmin(string id)
        {
            var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var users = _authService.GetAll();
            bool super = users.Data.FirstOrDefault(u => u.Id == a).Super;
            if (super == false)
            {
                _notyfService.Error("Yetkiniz yok");
                return RedirectToAction("Index");
            }
            _authService.Delete(id);
            _notyfService.Success("Admin silindi");
            return RedirectToAction("Index");
        }
    }
}
