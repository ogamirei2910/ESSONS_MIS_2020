using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class CongNhanController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public IActionResult Index()
        {
            return View();
        }

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

        [HttpPost]
        public IActionResult Login(EmpModel em)
        {
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<EmpModel>("api/emp/LoginCongNhan", em);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                List<UserRoleModel> urm = new List<UserRoleModel>();
                hc = _api.Initial();
                res = hc.GetAsync($"api/user/GetRoleCN/{em.empID}");
                var results3 = res.Result;
                if (results3.IsSuccessStatusCode)
                {
                    var results2 = results3.Content.ReadAsStringAsync().Result;
                    urm = JsonConvert.DeserializeObject<List<UserRoleModel>>(results2);

                    HttpContext.Session.SetObjectAsJson("folderList", urm);
                    HttpContext.Session.SetString("notice", "Người dùng " + urm.First().empName + " đã đăng nhập thành công");
                    return RedirectToAction("dateoff_Detail_Emp", "CongNhan");           
                }
            }

            ViewBag.message = "Số thẻ không tồn tại";
            return View("Index");
        }

        public IActionResult dateoff_Request()
        {
            getRole();
            return View();
        }

        public async Task<IActionResult> dateoff_Confirm()
        {
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpConfirm/{ViewBag.EmpID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }
            return View(um);
        }

        [HttpPost]
        public IActionResult dateoff_Request(DateOffModel um)
        {
            HttpClient hc = _api.Initial();
          

            if (um.dateoffType == "0" || um.dateoffType == null)
            {
                ViewBag.Error = "Chưa chọn loại nghỉ phép";
                return PartialView("DisplayError");
            }

            if (um.dateoffStart == null)
            {
                ViewBag.Error = "Chưa chọn ngày nghỉ";
                return PartialView("DisplayError");
            }

            if (um.dateoffEnd == null)
            {
                ViewBag.Error = "Chưa chọn ngày kết thúc nghỉ";
                return PartialView("DisplayError");
            }

            double number = 0;
            CultureInfo provider = CultureInfo.InvariantCulture;
            string datestart = um.dateoffStart;
            string dateend = um.dateoffEnd;
            TimeSpan Total = DateTime.ParseExact(dateend, "dd-MM-yyyy", provider) - DateTime.ParseExact(datestart, "dd-MM-yyyy", provider);
            number = (Total.TotalDays + 1) * 8;
            if (um.dateoffType == "4" || um.dateoffType == "1")
            {
                if (um.dateoffEndTime != null && um.dateoffStartTime != null && um.dateoffStart == um.dateoffEnd)
                {
                    int yearS = int.Parse(um.dateoffStart.Substring(6, 4));
                    int monthS = int.Parse(um.dateoffStart.Substring(3, 2));
                    int dayS = int.Parse(um.dateoffStart.Substring(0, 2));
                    int hourS = int.Parse(um.dateoffStartTime.Substring(0, 2));
                    int minuteS = int.Parse(um.dateoffStartTime.Substring(3, 2));
                    DateTime dtStart = new DateTime(yearS, monthS, dayS, hourS, minuteS, 0);

                    int yearE = int.Parse(um.dateoffEnd.Substring(6, 4));
                    int monthE = int.Parse(um.dateoffEnd.Substring(3, 2));
                    int dayE = int.Parse(um.dateoffEnd.Substring(0, 2));
                    int hourE = int.Parse(um.dateoffEndTime.Substring(0, 2));
                    int minuteE = int.Parse(um.dateoffEndTime.Substring(3, 2));
                    DateTime dtEnd = new DateTime(yearE, monthE, dayE, hourE, minuteE, 0);

                    TimeSpan Total2 = dtEnd - dtStart;
                    var numbertest = Math.Floor(Total2.TotalHours);
                    var numbertest2 = numbertest + 0.5;
                    number = Total2.TotalHours;
                    if (Total2.TotalHours > numbertest && Total2.TotalHours <= numbertest2)
                        number = numbertest2;
                    if (Total2.TotalHours > numbertest2)
                        number = Math.Ceiling(Total2.TotalHours);
                    if (number > 8)
                        number = 8;

                }
            }
            DateTime dt;
            DateTime.TryParseExact(um.dateoffStart,
                            "dd-MM-yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dt);

            if (dt.Date < DateTime.Now.Date)
            {
                ViewBag.Error = "Không thể đăng kí nghỉ phép ngày làm việc đã kết thúc";
                return PartialView("DisplayError");
            }

            if (number <= 4)
            {
                if (dt.Date < DateTime.Now.Date)
                {
                    ViewBag.Error = "Không thể đăng kí nghỉ phép ngày làm việc đã kết thúc";
                    return PartialView("DisplayError");
                }
            }

            if (number >= 8 && number <= 16)
            {
                if (dt.Date <= DateTime.Now.Date)
                {
                    ViewBag.Error = "Đăng kí phép trước 1 ngày với trường hợp nghỉ 1-2 ngày";
                    return PartialView("DisplayError");
                }
            }

            if (number > 16 && number <= 24)
            {
                if (dt.Date <= DateTime.Now.AddDays(13).Date && dt.Date.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
                {
                    ViewBag.Error = "Đăng kí phép trước 2 tuần với trường hợp nghỉ 3 ngày";
                    return PartialView("DisplayError");
                }
            }

            if (number >= 32)
            {
                if (dt.Date <= DateTime.Now.AddDays(29).Date)
                {
                    ViewBag.Error = "Đăng kí phép trước 1 tháng với trường hợp nghỉ 4 ngày";
                    return PartialView("DisplayError");
                }
            }
            um.dateoffNumber = number;

            um.username = ViewBag.empid;
            var res = hc.PostAsJsonAsync<DateOffModel>("api/dateoff/Create", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                string dateoffName = "";
                switch (um.dateoffType)
                {
                    case "1": dateoffName = "Phép năm"; break;
                    case "2": dateoffName = "Nghỉ bệnh (BHXH)"; break;
                    case "3": dateoffName = "Nghỉ thai sản"; break;
                    case "4": dateoffName = "Việc riêng"; break;
                    case "10": dateoffName = "Nghỉ vợ sinh/khám/dưỡng"; break;
                    case "11": dateoffName = "Nghỉ kết hôn"; break;
                    case "12": dateoffName = "Nghỉ tang gia"; break;
                    case "13": dateoffName = "Nghỉ con bệnh"; break;
                    case "16": dateoffName = "Dưỡng sức"; break;
                }

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
                        //SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                        //client.UseDefaultCredentials = false;
                        //client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                        //MailMessage mailMessage = new MailMessage();
                        //mailMessage.From = new MailAddress("noreply@essons.vn");
                        //mailMessage.To.Add(am.empEmail);
                        //mailMessage.IsBodyHtml = true;
                        //mailMessage.Body = "Nhân viên " + ViewBag.message + "(" + ViewBag.empid + ")" + " xin nghỉ " + dateoffName +
                        //    "<br /> Từ ngày: " + um.dateoffStart + " Đến ngày: " + um.dateoffEnd + @" <br /> Đang chờ bạn xác nhận http://portal.essons.vn:456/DateOff/dateoff_Confirm";
                        //mailMessage.Subject = "Nhân viên xin nghỉ phép";
                        //try { client.Send(mailMessage); }
                        //catch { }
                    }
                }
                HttpContext.Session.SetString("notice", "Đăng kí phép thành công");
                ViewBag.Error = "OK";
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Không đủ phép năm để nghỉ phép. Vui lòng kiểm tra lại phép năm.";
            return PartialView("DisplayError");
        }

        public IActionResult dateoff_Delete(string dateoffID)
        {
            DateOffModel em = new DateOffModel();
            em.dateoffID = dateoffID;
            em.status = 3;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<DateOffModel>($"api/dateoff/Delete", em);
            res.Wait();

            return RedirectToAction("dateoff_Detail_Emp");
        }

        public IActionResult dateoff_Update(string dateoffID)
        {
            getRole();
            DateOffModel em = new DateOffModel();
            em.status = 1;
            em.dateoffID = dateoffID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<DateOffModel>($"api/dateoff/Update", em);
            res.Wait();

            EmpModel am = new EmpModel();
            res = hc.GetAsync($"api/emp/GetEmailEmpInManager?dateoffID={dateoffID}");
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
                    mailMessage.Body = "Mã phép " + dateoffID + " đã được duyệt" + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/User/Login";
                    mailMessage.Subject = "Phản hồi nhân viên xin nghỉ phép";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }
            HttpContext.Session.SetString("notice", "Đã duyệt phép thành công");

            return RedirectToAction("dateoff_Confirm");
        }
        public async Task<IActionResult> dateoff_Detail_Emp()
        {
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            var urm = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpID/{ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }

            res = await hc.GetAsync($"api/dateoff/GetDateOffInfo/{ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.InfoList = JsonConvert.DeserializeObject<DateOffInfoModel>(results);
            }

            EmpModel em = new EmpModel();
            res = await hc.GetAsync($"api/emp/GetEmpID/{ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<EmpModel>(results);
                ViewBag.EmpInfo = em;
            }

            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            ViewBag.message = urm.First().empName.ToString();
            ViewBag.roleID = urm.First().roleID.ToString();
            ViewBag.empid = urm.First().empID.ToString();
            if (urm.First().empImage != null)
                ViewBag.empImage = urm.First().empImage.ToString();
            else
                ViewBag.empImage = "";
            var DistinctItems = urm.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = urm;

            return View(um);
        }
    }
}