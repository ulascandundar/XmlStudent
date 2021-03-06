using AspNetCoreHero.ToastNotification.Abstractions;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UI.Data;

namespace UI.Controllers
{
    public class ChartController : Controller
    {
        IStudentService _studentService;
        readonly INotyfService _notyfService;
        public ChartController(IStudentService studentService, INotyfService notyfService)
        {
            this._studentService = studentService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult Index4()
        {
            return View();
        }
        public IActionResult Statistics()
        {
            ViewBag.d1 = _studentService.GetAll().Data.Count();
            List<Class1> cs = new List<Class1>();
            cs = _studentService.GetAll().Data.Select(p => new Class1
            {
                name = p.Name,
                exams = p.Exams.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).Where(n => n.HasValue).Select(n => n.Value).ToList().Average()
            }).ToList();
            ViewBag.d2 = cs.OrderByDescending(s => s.exams).Select(u => u.name).FirstOrDefault();
            ViewBag.d3 = cs.OrderBy(s => s.exams).Select(u => u.name).FirstOrDefault();
            return View();
        }
        public IActionResult All()
        {

            ViewBag.d1 = _studentService.GetAll().Data.Count();
            List<Class1> cs = new List<Class1>();
            cs = _studentService.GetAll().Data.Select(p => new Class1
            {
                name = p.Name,
                exams = p.Exams.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).Where(n => n.HasValue).Select(n => n.Value).ToList().Average()
            }).ToList();
            ViewBag.d2 = cs.OrderByDescending(s => s.exams).Select(u => u.name).FirstOrDefault();
            ViewBag.d3 = cs.OrderBy(s => s.exams).Select(u => u.name).FirstOrDefault();
            return View();
        }

        public IActionResult Map()
        {
            return View();
        }
        public IActionResult Map2()
        {
            ViewBag.error2 = false;
            ViewBag.kullanici = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }
        [HttpPost]
        public IActionResult Map2(string id, string Enlem, string Boylam)
        {
            //string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = _studentService.AddAdress(id, Enlem, Boylam);
            if (result.Success==false)
            {
                ViewBag.error2 = true;
                return View();
            }
            _notyfService.Success("Adres Bilgisi Eklendi");
            return RedirectToAction("Index");
        }
        public IActionResult VisualizeProductResult()
        {
            return Json(ProList());
        }

        public IActionResult VisualizeProductResult2()
        {
            return Json(ProList2());
        }

        public List<Class1> ProList()
        {
            List<Class1> cs = new List<Class1>();
            //var datas = _studentService.GetAll().Data;
            //List<int> ints = null;
            //foreach (var item in datas)
            //{
            //    item.Exams = item.Exams.ForEach(e => Int32.Parse(e));
            //}
            cs = _studentService.GetAll().Data.Select(p => new Class1
            {
                name = p.Name,
                exams = p.Exams.Select(s=>Int32.TryParse(s, out int n) ? n : (int?)null).Where(n=>n.HasValue).Select(n=>n.Value).ToList().Average()
            }).ToList();
            return cs;
        }

        public List<Class1> ProList2()
        {
            List<Class1> cs = new List<Class1>();
            //var datas = _studentService.GetAll().Data;
            //List<int> ints = null;
            //foreach (var item in datas)
            //{
            //    item.Exams = item.Exams.ForEach(e => Int32.Parse(e));
            //}
            var Failed = _studentService.GetAll().Data.Select(s => s.Exams.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).Where(n => n.HasValue).Select(n => n.Value).ToList().Average()).Where(x => x < 60).Count();
            var Passed= _studentService.GetAll().Data.Select(s => s.Exams.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).Where(n => n.HasValue).Select(n => n.Value).ToList().Average()).Where(x => x >= 60).Count();
            cs.Add(new Class1()
            {
                name = "Başarılı Öğrenci Miktarı",
                exams = Passed
            });
            cs.Add(new Class1()
            {
                name = "Başarısız Öğrenci Miktarı",
                exams = Failed
            });
            return cs;
        }
    }
}
