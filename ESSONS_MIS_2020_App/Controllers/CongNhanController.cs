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
    public class CongNhanController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public IActionResult Index()
        {
            return View();
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
                HttpContext.Session.SetString("empid", em.empID);
                return RedirectToAction("dateoff_Detail_Emp", "CongNhan");
            }

            ViewBag.empID = "";
            ViewBag.message = "Số thẻ không tồn tại";
            return View("Index");
        }

        public IActionResult dateoff_Request()
        {
            ViewBag.EmpName = HttpContext.Session.GetString("EmpName");
            ViewBag.EmpID = HttpContext.Session.GetString("empid");
            ViewBag.EmpImage = HttpContext.Session.GetString("EmpImage");
            ViewBag.EmpRole = HttpContext.Session.GetString("EmpRole");
            return View();
        }

        public async Task<IActionResult> dateoff_Confirm()
        {
            ViewBag.EmpID = HttpContext.Session.GetString("empid");
            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpConfirm/{ViewBag.EmpID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }

            ViewBag.EmpName = HttpContext.Session.GetString("EmpName");
            ViewBag.EmpImage = HttpContext.Session.GetString("EmpImage");
            ViewBag.EmpRole = HttpContext.Session.GetString("EmpRole");
            return View(um);
        }

        [HttpPost]
        public IActionResult dateoff_Request(DateOffModel um)
        {
            HttpClient hc = _api.Initial();

            if (um.dateoffStart == null)
            {
                ViewBag.Error = "Chưa chọn ngày nghỉ";
                return PartialView("DisplayError");
            }

            double number = 0;
            if (um.dateoffType == "5" || um.dateoffType == "6")
            {
                um.dateoffEnd = um.dateoffStart;

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
                    number = Math.Ceiling(Total.TotalHours / 2) * 2;
                }
                else
                {
                    ViewBag.Error = "Kiểm tra lại giờ xin nghỉ";
                    return PartialView("DisplayError");
                }
            }
            else
            {
                if (um.dateoffEnd == null)
                {
                    ViewBag.Error = "Chưa chọn ngày kết thúc nghỉ";
                    return PartialView("DisplayError");
                }

                CultureInfo provider = CultureInfo.InvariantCulture;
                string datestart = um.dateoffStart;
                string dateend = um.dateoffEnd;
                TimeSpan Total = DateTime.ParseExact(dateend, "dd-MM-yyyy", provider) - DateTime.ParseExact(datestart, "dd-MM-yyyy", provider);
                number = (Total.TotalDays + 1) * 8;
            }
            um.dateoffNumber = number;

            um.empName = ViewBag.message;
            var res = hc.PostAsJsonAsync<DateOffModel>("api/dateoff/Create", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Đăng kí phép thành công");
                return RedirectToAction("dateoff_Detail_Emp", "CongNhan");
            }

            ViewBag.Error = "Lỗi kết nối hệ thống. Liên hệ IT";
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

            return RedirectToAction("dateoff_Detail_Emp", new { empID = HttpContext.Session.GetString("empid") });
        }


        public async Task<IActionResult> dateoff_Detail_Emp()
        {
            string empID = HttpContext.Session.GetString("empid");
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DateOffModel> um = new List<DateOffModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }

            res = await hc.GetAsync($"api/dateoff/GetDateOffInfo/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.InfoList = JsonConvert.DeserializeObject<DateOffInfoModel>(results);
            }

            EmpModel em = new EmpModel();
            res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<EmpModel>(results);
                ViewBag.EmpInfo = em;
            }

            List<UserRoleModel> urm = new List<UserRoleModel>();
            res = await hc.GetAsync($"api/user/GetRole/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                urm = JsonConvert.DeserializeObject<List<UserRoleModel>>(results);
            }

            HttpContext.Session.SetString("EmpName", em.empName);
            HttpContext.Session.SetString("EmpImage", em.empImage);
            HttpContext.Session.SetObjectAsJson("EmpRole", urm.Find(x => x.folderChildID == 2));

            ViewBag.EmpRole = urm.Find(x => x.folderChildID == 2);
            ViewBag.EmpID = em.empID;
            ViewBag.EmpName = em.empName;
            ViewBag.EmpImage = em.empImage;
            return View(um);
        }
    }
}