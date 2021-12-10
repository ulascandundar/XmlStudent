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

        public LoginController(IAuthService authService)
        {
            _authService = authService;

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
            var result = _authService.Login(userForLoginDto).Success;
            if (result)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userForLoginDto.Name)
                };
                
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                ViewBag.user = userForLoginDto.Name;
                HttpContext.Session.SetString("Name",userForLoginDto.Name);
                return RedirectToAction("Index", "Student");


            }
            ViewBag.deactive = false;
            ViewBag.error = true;
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
