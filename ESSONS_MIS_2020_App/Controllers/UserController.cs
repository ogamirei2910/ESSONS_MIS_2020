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
using Novell.Directory.Ldap.Extensions;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class UserController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public void getRole()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
                RedirectToAction("User", "Login");

            var role = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            ViewBag.message = role.First().empName.ToString();
            ViewBag.roleID = role.First().roleID.ToString();
            ViewBag.empid = role.First().empID.ToString();
            ViewBag.empImage = role.First().empImage.ToString();
            var DistinctItems = role.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = role;
        }
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("isLogin") == "true")
                return RedirectToAction("Index", "Home");

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
                        connection.Connect("10.0.11.2", LdapConnection.DEFAULT_PORT); 
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
  
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserModel>("api/user/login", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", um.username);
                HttpContext.Session.SetString("username", um.username);
                HttpContext.Session.SetString("isLogin", "true");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Tài khoản chưa đăng kí trên hệ thống. Liên hệ IT";
            return View();
        }

        public async Task<IActionResult> SetFolder()
        {
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<UserRoleModel> um = new List<UserRoleModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/user/GetAllRole");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<UserRoleModel>>(results);
                ViewBag.perList = um;
            }

            getRole();         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetFolder(UserRoleModel urm)
        {
            if (urm.empID == null)
            {
                ViewBag.Error = "Chưa nhập số thẻ";
                return PartialView("DisplayError");
            }
            
            HttpClient hc = _api.Initial();
            var res = await hc.PostAsJsonAsync<UserRoleModel>("api/user/setfolder", urm);

            urm.empID = int.Parse(urm.empID).ToString("D5");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;

                if (results == "OK")
                    HttpContext.Session.SetString("notice", "Thêm quyền thành công");

                ViewBag.Error = results;
                return PartialView("DisplayError");
            }
            else
            {
                ViewBag.Error = "Lỗi kết nối mạng.";
                return PartialView("DisplayError");
            }
        }

        public IActionResult user_Block(string empid)
        {
            ViewBag.Error = "";
            UserRoleModel em = new UserRoleModel();
            em.empID = empid;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserRoleModel>($"api/user/Block", em);
            res.Wait();

            var result = res.Result;
            if (result.IsSuccessStatusCode)
                HttpContext.Session.SetString("notice", "Xóa quyền thành công");
            else
                HttpContext.Session.SetString("notice", "Xóa không thành công");

            return RedirectToAction("SetFolder", "User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("isLogin", "");
            return RedirectToAction("Index", "MainPage");
        }
    }
}