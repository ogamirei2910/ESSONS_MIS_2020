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
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class UserController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public void getRole()
        {
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
            if (HttpContext.Session.GetString("isLogin") == "true")
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


            string username = um.username + "@essons.vn";
            var mail = "";
            var empid = "";
            //using (var DE = new DirectoryEntry("LDAP://OU=Essons Amata,DC=essons,DC=vn", "Administrator", "Csi@dvtk2020")) P@ssw0rd123
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect("10.0.11.2", LdapConnection.DEFAULT_PORT);
                    connection.Bind(username, um.password);
                    if (connection.Bound)
                    {
                        String[] attrs = new String[2];
                        attrs[0] = "postOfficeBox";
                        attrs[1] = "mail";

                        LdapSearchResults lsc = connection.Search("OU=Essons Amata,DC=essons,DC=vn",
                           LdapConnection.SCOPE_SUB,
                           "sAMAccountName=" + um.username,
                           attrs,
                           false);

                        while (lsc.HasMore())
                        {
                            LdapEntry nextEntry = null;
                            try
                            {
                                nextEntry = lsc.Next();
                            }
                            catch (LdapException e)
                            {
                                // Exception is thrown, go for next entry
                                continue;
                            }
                            LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                            System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                            
                            while (ienum.MoveNext())
                            {
                                LdapAttribute attribute = (LdapAttribute)ienum.Current;
                                if (attribute.Name == "mail")                        
                                    mail = attribute.StringValue;
                                else
                                    empid = attribute.StringValue;
                            }
                        }
                        connection.Disconnect();
                    }
                }
            }
            catch (LdapException ex)
            {
                ViewBag.Message = "Kiểm tra lại tài khoản hoặc mật khẩu";
                return View();
            }
            HttpClient hc = _api.Initial();
            EmpModel em = new EmpModel();
            hc = _api.Initial();
            em.empID = empid;
            em.empEmail = mail;
            var res = hc.PostAsJsonAsync<EmpModel>($"api/emp/UpdateMail/", em);
            res.Wait();
            //empid = "03905";

            List<UserRoleModel> urm = new List<UserRoleModel>();
            hc = _api.Initial();
            res = hc.GetAsync($"api/user/GetRole/{empid}");
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                var results2 = results.Content.ReadAsStringAsync().Result;
                urm = JsonConvert.DeserializeObject<List<UserRoleModel>>(results2);

                HttpContext.Session.SetObjectAsJson("folderList", urm);

                HttpContext.Session.SetString("notice", "Người dùng " + urm.First().empName + " đã đăng nhập thành công");
                if (HttpContext.Session.GetString("resultPage") is null || HttpContext.Session.GetString("resultPage") == "")
                {
                    HttpContext.Session.SetString("isLogin", "true");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string url = HttpContext.Session.GetString("resultPage");
                    HttpContext.Session.SetString("resultPage", "");
                    return Redirect(url);
                }
            }

            ViewBag.Message = "Lỗi đăng nhập. Liên hệ IT";
            return View();
        }

        public async Task<IActionResult> SetFolder()
        {
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<UserFolderModel> um = new List<UserFolderModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/user/GetAllPer");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<UserFolderModel>>(results);
                ViewBag.perList = um;
            }

            res = await hc.GetAsync("api/emp/Get");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.emp = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }

            res = await hc.GetAsync("api/emp/GetDepartment");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.departmentList = JsonConvert.DeserializeObject<List<DepartmentModel>>(results);
            }

            getRole();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetFolder(UserFolderModel urm)
        {
            if (urm.empID == null)
            {
                ViewBag.Error = "Chưa nhập số thẻ";
                return PartialView("DisplayError");
            }

            if (urm.depName == null)
            {
                ViewBag.Error = "Chưa chọn bộ phận";
                return PartialView("DisplayError");
            }

            HttpClient hc = _api.Initial();
            var res = await hc.PostAsJsonAsync<UserFolderModel>("api/user/setper", urm);

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
            UserFolderModel em = new UserFolderModel();
            em.empID = empid;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<UserFolderModel>($"api/user/Block", em);
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
            HttpContext.Session.SetObjectAsJson("folderList", null);
            return RedirectToAction("Index", "MainPage");
        }
    }
}