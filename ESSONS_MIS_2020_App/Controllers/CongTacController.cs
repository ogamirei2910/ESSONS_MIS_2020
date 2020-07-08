using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class CongTacController : Controller
    {
        EssonsApi _api = new EssonsApi();

        public void getRole()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
                RedirectToAction("User", "Login");

            var role = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            ViewBag.message = role.First().empName.ToString();
            ViewBag.empid = role.First().empID.ToString();
            ViewBag.roleID = role.First().roleID.ToString();
            ViewBag.empImage = role.First().empImage.ToString();
            var DistinctItems = role.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = role;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            getRole();
            HttpClient hc = _api.Initial();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            var res = await hc.GetAsync($"api/CongTac/GetCongTac?empid={ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.requestList = JsonConvert.DeserializeObject<List<CongTacModel>>(results);
            }
            return View();
        }

        public async Task<IActionResult> YeuCauCongTac()
        {
            getRole();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/emp/Get");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.emp = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }
            return View();
        }

        [HttpPost]
        public IActionResult YeuCauCongTac(CongTacModel um)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            HttpClient hc = _api.Initial();
            var test = um.empid.Split(',');
            um.planNumber = test.Length;
            List<ChildCongTac> lem = new List<ChildCongTac>();            
            for (int i = 0; i < test.Length; i++)
            {
                ChildCongTac em = new ChildCongTac();
                em.empid = test[i];
                lem.Add(em);
            }
            um.empmodel = lem;
            var res = hc.PostAsJsonAsync<CongTacModel>("api/CongTac/CongTac_Request", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                EmpModel am = new EmpModel();
                res = hc.GetAsync($"api/emp/GetEmailEmpManager?empid={ViewBag.empid}");
                res.Wait();
                results = res.Result;
                if (results.IsSuccessStatusCode)
                {
                    var results2 = results.Content.ReadAsStringAsync().Result;
                    am = JsonConvert.DeserializeObject<EmpModel>(results2);
                    if (am.empEmail != "" && am.empEmail != null)
                    {
                        SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("noreply@essons.vn");
                        mailMessage.To.Add(am.empEmail);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = "Nhân viên " + ViewBag.message + "(" + ViewBag.empid + ")" + " xin đi công tác " + um.planName +
                            "<br /> Từ ngày: " + um.planStart + " Đến ngày: " + um.planEnd + @" <br /> Đang chờ bạn xác nhận http://portal.essons.vn:456/CongTac/XacNhanCongTac";
                        mailMessage.Subject = "Nhân viên xin đi công tác";
                        try { client.Send(mailMessage); }
                        catch { }
                    }
                }
                HttpContext.Session.SetString("notice", "Đăng kí công tác thành công");
                return RedirectToAction("Index", "CongTac");
            }
            return View(um);
        }

        public async Task<IActionResult> XacNhanCongTac()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            getRole();
            HttpClient hc = _api.Initial();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            var res = await hc.GetAsync($"api/CongTac/GetAllCongTac?empid={ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.requestList = JsonConvert.DeserializeObject<List<CongTacModel>>(results);
            }
            return View();
        }

        public IActionResult Update(string congtacID, int page, int status)
        {
            getRole();
            CongTacModel em = new CongTacModel();
            em.status = status;
            em.congtacID = congtacID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<CongTacModel>($"api/congtac/Update", em);
            res.Wait();
            EmpModel am = new EmpModel();
            res = hc.GetAsync($"api/emp/GetEmailEmpInManager?congtacID={congtacID}");
            res.Wait();
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                var results2 = results.Content.ReadAsStringAsync().Result;
                am = JsonConvert.DeserializeObject<EmpModel>(results2);
                if (am.empEmail != "" && am.empEmail != null)
                {
                    SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("noreply@essons.vn");
                    mailMessage.To.Add(am.empEmail);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = "Mã công tác " + congtacID + " đã được cập nhật" + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/CongTac/Index";
                    mailMessage.Subject = "Phản hồi nhân viên xin công tác";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }

            if (page == 1)
                return RedirectToAction("Index");
            else
                return RedirectToAction("XacNhanCongTac");
        }
        public IActionResult Delete(string congtacID, int page, int status)
        {
            getRole();
            CongTacModel em = new CongTacModel();
            em.status = status;
            em.congtacID = congtacID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<CongTacModel>($"api/congtac/Delete", em);
            res.Wait();
            EmpModel am = new EmpModel();
            res = hc.GetAsync($"api/emp/GetEmailEmpInManager?congtacID={congtacID}");
            res.Wait();
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                var results2 = results.Content.ReadAsStringAsync().Result;
                am = JsonConvert.DeserializeObject<EmpModel>(results2);
                if (am.empEmail != "" && am.empEmail != null)
                {
                    SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("noreply@essons.vn");
                    mailMessage.To.Add(am.empEmail);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = "Mã công tác " + congtacID + " đã bị hủy." + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/CongTac/Index";
                    mailMessage.Subject = "Phản hồi nhân viên xin công tác";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }
            if (page == 1)
                return RedirectToAction("Index");
            else
                return RedirectToAction("XacNhanCongTac");
        }

        public async Task<IActionResult> Detail(string congtacID, int page)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            HttpClient hc = _api.Initial();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            HttpResponseMessage res = await hc.GetAsync("api/emp/Get");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.emp = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }

            res = await hc.GetAsync($"api/CongTac/GetCongTacDetail?congtacID={congtacID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.infoList = JsonConvert.DeserializeObject<CongTacModel>(results);
            }
            ViewBag.page = page;
            return View();
        }

        public IActionResult Edit(CongTacModel model)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            HttpClient hc = _api.Initial();
            var test = model.empid.Split(',');
            model.planNumber = test.Length;
            List<ChildCongTac> lem = new List<ChildCongTac>();
            for (int i = 0; i < test.Length; i++)
            {
                ChildCongTac em = new ChildCongTac();
                em.empid = test[i];
                lem.Add(em);
            }
            model.empmodel = lem;
            var res = hc.PostAsJsonAsync<CongTacModel>($"api/congtac/Edit", model);
            res.Wait();
            return RedirectToAction("Index");
        }
    }
}