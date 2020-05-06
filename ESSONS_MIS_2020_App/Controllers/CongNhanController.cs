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
                return RedirectToAction("dateoff_Detail_Emp", "CongNhan",new { empid = em.empID});
            }

            ViewBag.empID = "";
            ViewBag.message = "Số thẻ không tồn tại";
            return View("Index");
        }

        public IActionResult dateoff_Request()
        {
            return View();
        }

        [HttpPost]
        public IActionResult dateoff_Request(DateOffModel um)
        {
            HttpClient hc = _api.Initial();

            double number = 0;
            if (um.dateoffStart == um.dateoffEnd)
            {
                if (um.dateoffStartTime is null || um.dateoffStartTime is null)
                {
                    ViewBag.Error = "Chưa chọn giờ nghỉ";
                    return View();
                }

                string timestart = um.dateoffStartTime;
                string timeend = um.dateoffEndTime;
                TimeSpan timeS = new TimeSpan(int.Parse(timestart.Substring(0, 2)), int.Parse(timestart.Substring(3, 2)), 0);
                TimeSpan timeE = new TimeSpan(int.Parse(timeend.Substring(0, 2)), int.Parse(timeend.Substring(3, 2)), 0);
                TimeSpan Total = timeE - timeS;
                number = Total.TotalHours;
            }
            else
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string datestart = um.dateoffStart;
                string dateend = um.dateoffEnd;
                TimeSpan Total = DateTime.ParseExact(dateend, "dd/MM/yyyy", provider) - DateTime.ParseExact(datestart, "dd/MM/yyyy", provider);
                number = (Total.TotalDays + 1) * 8;
            }
            um.dateoffNumber = number;

            um.empName = ViewBag.message;
            var res = hc.PostAsJsonAsync<DateOffModel>("api/dateoff/Create", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                return RedirectToAction("dateoff_Detail_Emp", "CongNhan", new { empID = um.empID });
            }

            ViewBag.Error = "Lỗi kết nối hệ thống. Liên hệ IT";
            return View();
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

        public async Task<IActionResult> dateoff_Detail_Emp(string empID)
        {

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
                ViewBag.EmpInfo = JsonConvert.DeserializeObject<EmpModel>(results);
            }

            return View(um);
        }
    }
}