using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class DateOffController : Controller
    {
        //Lấy đường dẫn đến kết tới API
        EssonsApi _api = new EssonsApi();

        //Lấy thông tin cơ bản của nhân viên và quyền truy cập thư mục
        public void getRole()
        {
            var role = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            ViewBag.message = role.First().empName.ToString();
            ViewBag.empid = role.First().empID.ToString();
            ViewBag.roleID = role.First().roleID.ToString();
            ViewBag.empImage = role.First().empImage.ToString();
            var DistinctItems = role.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = role;
        }

        //Giao diện yêu cầu nghỉ phép
        public IActionResult dateoff_Request()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            return View();
        }

        //Yêu cầu nghỉ phép
        [HttpPost]
        public IActionResult dateoff_Request(DateOffModel um)
        {
            
            getRole();
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

            /* Tính khoảng thời gian xin nghỉ phép */
            double number = 0;
            CultureInfo provider = CultureInfo.InvariantCulture;
            string datestart = um.dateoffStart;
            string dateend = um.dateoffEnd;
            TimeSpan Total = DateTime.ParseExact(dateend, "dd-MM-yyyy", provider) - DateTime.ParseExact(datestart, "dd-MM-yyyy", provider);
            number = (Total.TotalDays + 1) * 8;
            /*----------------------------------------------------*/

            /*Kiểm tra có phải phép việc riêng hoặc phép năm không */
            if (um.dateoffType == "4" || um.dateoffType == "1")
            {
                if (um.dateoffEndTime != null && um.dateoffStartTime != null && um.dateoffStart == um.dateoffEnd && number == 8)
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

            /*Kiểm tra thời gian xin nghỉ phép */
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

            if(um.dateoffType == "1" || um.dateoffType == "4")
            {
                if (number < 8)
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
            }

          
            /*Gửi dữ liệu đến API */
            um.dateoffNumber = number;
            um.username = ViewBag.empid;
            var res = hc.PostAsJsonAsync<DateOffModel>("api/dateoff/Create", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {

                /*Gửi mail đến người quản lý trực tiếp */
                string dateoffName = "";
                switch(um.dateoffType)
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

                //Lấy danh sách người quản lý
                List<EmpModel> am = new List<EmpModel>();
                res = hc.GetAsync($"api/emp/GetEmailEmpManager?empid={ViewBag.empid}");
                res.Wait();
                results = res.Result;
                if (results.IsSuccessStatusCode)
                {
                    //Gửi mail 
                    var results2 = results.Content.ReadAsStringAsync().Result;
                    am = JsonConvert.DeserializeObject<List<EmpModel>>(results2);
                    if (am.Count > 0)
                    {
                        SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("noreply@essons.vn");
                        foreach (var item in am)
                            mailMessage.To.Add(item.empEmail);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = "Nhân viên " + ViewBag.message + "(" + ViewBag.empid + ")" + " xin nghỉ " + dateoffName +
                            "<br /> Từ ngày: " + um.dateoffStart + " Đến ngày: " + um.dateoffEnd + @" <br /> Đang chờ bạn xác nhận http://portal.essons.vn:456/DateOff/dateoff_Confirm";
                        mailMessage.Subject = "Nhân viên xin nghỉ phép";
                        try { client.Send(mailMessage); }
                        catch { }
                    }
                }
                HttpContext.Session.SetString("notice", "Đăng kí phép thành công");
                ViewBag.Error = "OK";
                return PartialView("DisplayError");
            }
           
            ViewBag.Error = "Không đủ phép năm để nghỉ phép. Vui lòng kiểm tra lại phép năm.";
            return PartialView("DisplayError");
        }


        //Giao diện xác nhận nghỉ phép
        public async Task<IActionResult> dateoff_Confirm()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            //Lấy danh sách nhân viên thuộc quản lý
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpConfirm/{ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }
            return View(um);
        }

        //Xác nhận cho phép nghỉ
        public IActionResult dateoff_Update(string dateoffID)
        {
            getRole();
            DateOffModel em = new DateOffModel();
            em.status = 1;
            em.dateoffID = dateoffID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<DateOffModel>($"api/dateoff/Update", em);
            res.Wait();

            //Gửi thông tin đên web api
            EmpModel am = new EmpModel();
            res = hc.GetAsync($"api/emp/GetEmailEmpInManager?dateoffID={dateoffID}");
            res.Wait();
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                /* lấy email người đăng kí phép */
                var results2 = results.Content.ReadAsStringAsync().Result;
                am = JsonConvert.DeserializeObject<EmpModel>(results2);
                if (am.empEmail != "")
                {
                    SmtpClient client = new SmtpClient("SRV-mgt-01.essons.vn", 587);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("noreply@essons.vn", "P@ssw0rd123");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("noreply@essons.vn");
                    mailMessage.To.Add(am.empEmail);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = "Mã phép "  + dateoffID + " đã được duyệt" + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/User/Login";
                    mailMessage.Subject = "Phản hồi nhân viên xin nghỉ phép";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }
            HttpContext.Session.SetString("notice", "Đăng kí phép thành công");
            return RedirectToAction("dateoff_Confirm");
        }

        //Cập nhật phép BHXH
        public IActionResult dateoff_UpdatePaper(string dateoffID)
        {
            getRole();
            DateOffModel em = new DateOffModel();
            em.status = 1;
            em.dateoffID = dateoffID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<DateOffModel>($"api/dateoff/UpdatePaper", em);
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
                    mailMessage.Body = "Mã phép "  + dateoffID + " đã bổ sung giấy tờ." + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/User/Login";
                    mailMessage.Subject = "Phản hồi nhân viên xin nghỉ phép";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }
            return RedirectToAction("dateoff_Detail");
        }

        //Hủy phép
        public IActionResult dateoff_Delete(string dateoffID, int page)
        {
            getRole();
            DateOffModel em = new DateOffModel();
            em.dateoffID = dateoffID;
            em.status = 3;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<DateOffModel>($"api/dateoff/Delete", em);
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
                    mailMessage.Body = "Mã phép " + dateoffID + " đã bị hủy." + @" <br /> Kiểm tra thông tin xác nhận http://portal.essons.vn:456/User/Login";
                    mailMessage.Subject = "Phản hồi nhân viên xin nghỉ phép";
                    try { client.Send(mailMessage); }
                    catch { }
                }
            }


            if (page == 1)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("dateoff_Confirm");
        }

        public async Task<IActionResult> dateoff_Detail()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetAllEmp");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }
            return View(um);
        }

        //Lấy thông tin toàn bộ phép năm nhân viên công ty trừ thời vụ
        public async Task<IActionResult> dateoff_yearoff()
        {
            //Kiểm tra quyền truy cập nếu chưa lấy danh sách mục quản lý thì trở về trang đăng nhập
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            //Lấy danh sách những mục được phép truy cập
            getRole();

            //Lấy nội dung thông báo trả về từ các Controller khác và set session về rỗng
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            //Lấy danh sách nhân viên trong công ty trừ thời vụ
            List<YearOffModel> um = new List<YearOffModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetAllYearOff");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<YearOffModel>>(results);
            }
            return View(um);
        }

        public async Task<IActionResult> dateoff_Detail_Emp()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
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
            return View(um);
        }
    }
}