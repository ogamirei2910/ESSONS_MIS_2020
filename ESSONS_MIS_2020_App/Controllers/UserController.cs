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
using System.Net;
using System.Runtime.InteropServices;
using Novell.Directory.Ldap;

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
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Vui lòng nhập tài khoản và mật khẩu";
                return View();
            }

            if (um.username != "nam")
            {
                string username = um.username + "@essons.vn";
                //using (var DE = new DirectoryEntry("LDAP://OU=Essons Amata,DC=essons,DC=vn", "Administrator", "Csi@dvtk2020")) P@ssw0rd123
                try
                {
                    using (var connection = new LdapConnection { SecureSocketLayer = false })
                    {
                        connection.Connect("10.0.11.2", LdapConnection.DEFAULT_PORT); ;
                        connection.Bind(username, um.password);
                        if (connection.Bound)
                        {

                        }
                    }
                }
                catch (LdapException ex)
                {
                    ViewBag.Message = "Kiểm tra lại tài khoản hoặc mật khẩu";
                    return View();
                }
            }

            if (um.username != "nam" || um.password != "P@ssw0rd123")
            {
                ViewBag.Message = "Kiểm tra lại tài khoản hoặc mật khẩu";
                return View();
            }    

            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserModel>("api/user/login", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", um.username);
                HttpContext.Session.SetString("username", um.username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Tài khoản chưa đăng kí trên hệ thống. Liên hệ IT";
            return View();
        }

        [HttpPost]
        public IActionResult SetFolder(UserRoleModel urm )
        {
            if(urm.empID == "" || urm.empID is null)
            {
                ViewBag.Error = "Vui lòng nhập username";
                return View();
            }

            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserRoleModel>("api/user/setfolder", urm);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                ViewBag.Error = "Thêm quyền thành công";
                return View();
            }

            ViewBag.Error = "Username không tồn tại";
            return View();
        }

        public async Task<IActionResult> CreateUser()
        {
            List<UserModel> um = new List<UserModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/user/GetUser");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<UserModel>>(results);
            }

            return View(um);
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel um)
        {
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserModel>("api/user/CreateUser", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                ViewBag.Error = "Thêm quyền thành công";
                return View();
            }

            ViewBag.Error = "Username không tồn tại";
            return View();
        }

        public IActionResult user_Block(string username)
        {
            UserModel em = new UserModel();
            em.username = username;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserModel>($"api/emp/Block", em);
            res.Wait();

            return RedirectToAction("CreateUser");
        }
    }
}