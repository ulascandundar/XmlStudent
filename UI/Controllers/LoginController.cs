using AspNetCoreHero.ToastNotification.Abstractions;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        IAuthService _authService;
        readonly INotyfService _notyfService;
        public LoginController(IAuthService authService, INotyfService notyfService)
        {
            _authService = authService;
            _notyfService = notyfService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.error = false;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(UserForLoginDto userForLoginDto)
        {
            var result = _authService.Login(userForLoginDto);
            if (result.Success && result.Data.Type=="Admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userForLoginDto.Name),
                    new Claim(ClaimTypes.NameIdentifier,result.Data.Id),
                    new Claim(ClaimTypes.Role,result.Data.Type)
                };
                
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                ViewBag.user = userForLoginDto.Name;
                HttpContext.Session.SetString("Name",userForLoginDto.Name);
                _notyfService.Success("Hoşgeldiniz");
                return RedirectToAction("Index", "Student");


            }
            ViewBag.deactive = false;
            ViewBag.error = true;
            _notyfService.Error("Bilgileriniz yanlış");
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> StudentLogin(UserForLoginDto userForLoginDto)
        {
            var result = _authService.Login(userForLoginDto);
            if (result.Success&&result.Data.Type=="User")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userForLoginDto.Name),
                    new Claim(ClaimTypes.NameIdentifier,result.Data.Id),
                    new Claim(ClaimTypes.Role,result.Data.Type)
                };

                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                ViewBag.user = userForLoginDto.Name;
                HttpContext.Session.SetString("Name", userForLoginDto.Name);
                _notyfService.Success("Hoşgeldiniz");
                return RedirectToAction("Index", "StudentPanel");


            }
            _notyfService.Error("Bilgileriniz yanlış");
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordReset()
        {
            ViewBag.Result2 = false;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult PasswordReset(string email, string fav)
        {
            var result= _authService.PasswordRefresh(email, fav);
            if (result.Success)
            {
                TempData["mail"] = email;
                ViewBag.Result2 = false;
                return RedirectToAction("ConfirmMail", "Login");
            }
            ViewBag.Result2 = true;
            ViewBag.Result1 = "Böyle Bir Mail yok.";
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ConfirmMail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
