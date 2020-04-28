using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class UserController : Controller
    {
        EssonsApi _api = new EssonsApi();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel um)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Message = "Vui lòng nhập tài khoản và mật khẩu";
                return View();
            }

            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserModel>("api/user", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("username", um.username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Kiểm tra lại tài khoản hoặc mật khẩu";
            return View();
        }
    }
}