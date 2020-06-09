using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class ChamCongController : Controller
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
        public async Task<IActionResult> Index(string date, string type)
        {
            List<ChamCongModel> um = new List<ChamCongModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = new HttpResponseMessage();
            if (type == "All")
                res = await hc.GetAsync($"api/chamcong/GetChamCong?date={date}");
            if (type == "Day")
                res = await hc.GetAsync($"api/chamcong/GetChamCongTheoNgay?date={date}");
            if (type == "SumMonth")
                res = await hc.GetAsync($"api/chamcong/GetChamCongTheoThang?date={date}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<ChamCongModel>>(results);
            }

            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            //Role
            getRole();
            //-----------------------------------

            ViewBag.type = type;
            ViewBag.chamcongList = um;
            return View();
        }

        public async Task<IActionResult> GetDate(string date)
        {
            List<ChamCongModel> em = new List<ChamCongModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/chamcong/GetChamCongTheoNgay?date={date}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<ChamCongModel>>(results);
            }
            ViewBag.chamcongList = em;
            return PartialView("DisplayChamCong");
        }

        public async Task<IActionResult> DateOffException()
        {
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DateOffExceptionModel> um = new List<DateOffExceptionModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/DateOffException");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffExceptionModel>>(results);
            }
            //Role
            getRole();
            //-----------------------------------
            ViewBag.dateoffList = um;
            return View();
        }


        public async Task<IActionResult> BuPhep(string empid, string workdate)
        {
            getRole();
            ViewBag.Error = "";
            DateOffExceptionModel um = new DateOffExceptionModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/DateOffExceptionID?empid={empid}&&workdate={workdate}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<DateOffExceptionModel>(results);
            }

            List<TimeWorkModel> em = new List<TimeWorkModel>();
            HttpResponseMessage res2 = await hc.GetAsync($"api/dateoff/GetTimeWork?empid={empid}&&workdate={workdate}");
            if (res2.IsSuccessStatusCode)
            {
                var results = res2.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<TimeWorkModel>>(results);
            }

            ViewBag.buphep = um;
            ViewBag.timework = em;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuPhep(DateOffModel um)
        {
            getRole();
            HttpClient hc = _api.Initial();

            if (um.dateoffType == null)
            {
                ViewBag.Error = "Chưa chọn loại bù phép";
                return PartialView("DisplayError");
            }

            if (um.dateoffStart == null)
            {
                ViewBag.Error = "Chưa chọn ngày bắt đầu xin phép";
                return PartialView("DisplayError");
            }


            um.username = ViewBag.empid;

            double number = 0;
            if (um.dateoffEndTime != null && um.dateoffStartTime != null)
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

                TimeSpan Total = dtEnd - dtStart;
                number = Math.Floor(Total.TotalMinutes / 30) / 2;

            }
            else
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string datestart = um.dateoffStart;
                string dateend = um.dateoffEnd;
                if (datestart != null && dateend != null)
                {
                    TimeSpan Total = DateTime.ParseExact(dateend, "dd-MM-yyyy", provider) - DateTime.ParseExact(datestart, "dd-MM-yyyy", provider);
                    number = (Total.TotalDays + 1) * 8;
                }
            }

            um.dateoffNumber = number;
            var res = await hc.PostAsJsonAsync<DateOffModel>("api/DateOff/BuPhep", um);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<DateOffModel>(results);

                if (um.result == "OK")
                    HttpContext.Session.SetString("notice", "Bù phép thành công");

                ViewBag.Error = um.result;
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kết nối. Gọi IT";
            return PartialView("DisplayError");
        }

        public async Task<IActionResult> LayLieuChamCong(string date)
        {
            //HttpClient hc = _api.Initial();
            //var res = await hc.GetAsync($"api/ChamCong/LayLieuChamCong?date={date}");

            //if (res.IsSuccessStatusCode)
            //{
            //    var results = res.Content.ReadAsStringAsync().Result;
            //    if (results == "OK")
            //    {
            //        ViewBag.Error = "Lấy dữ liệu thành công";
            //        return PartialView("DisplayError");
            //    }
            //    else
            //    {
            //        ViewBag.Error = results;
            //        return PartialView("DisplayError");
            //    }
            //}

            DateTime dt;
            DateTime.TryParseExact(date,
                            "dd-MM-yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dt);

            if (dt < DateTime.Now)
            {
                HttpClient hc = _api.Initial();
                var res = await hc.GetAsync($"api/ChamCong/LayLieuChamCong?date={date}");

                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStringAsync().Result;
                    if (results == "OK")
                    {
                        ViewBag.Error = "Lấy dữ liệu thành công";
                        return PartialView("DisplayError");
                    }
                    else
                    {
                        ViewBag.Error = results;
                        return PartialView("DisplayError");
                    }
                }
            }
            else
            {
                ViewBag.Error = "Ngày làm việc chưa kết thúc";
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kết nối. Gọi IT";
            return PartialView("DisplayError");
        }

        public async Task<IActionResult> UpdateOT(OverTimeModel um)
        {
            HttpClient hc = _api.Initial();
            var res = await hc.PostAsJsonAsync<OverTimeModel>($"api/OverTime/UpdateOT",um);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                if (results == "OK")
                {
                    HttpContext.Session.SetString("notice", "Cập nhật loại tăng ca thành công");
                    ViewBag.Error = "Cập nhật loại tăng ca thành công";
                    return PartialView("DisplayError");
                }
                else
                {
                    ViewBag.Error = results;
                    return PartialView("DisplayError");
                }
            }

            ViewBag.Error = "Lỗi kết nối. Gọi IT";
            return PartialView("DisplayError");
        }
    }
}