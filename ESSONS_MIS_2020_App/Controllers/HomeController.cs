using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using ESSONS_MIS_2020_App.Helper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Management;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class HomeController : Controller
    {
        EssonsApi _api = new EssonsApi();

        public async Task<IActionResult> Index()
        {
            //string[] lstPrinterList = new string[10];
            //ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
            //objScope.Connect();

            //SelectQuery selectQuery = new SelectQuery();
            //selectQuery.QueryString = "Select * from win32_Printer";
            //ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
            //ManagementObjectCollection MOC = MOS.Get();
            //int i = 0;
            //foreach (ManagementObject mo in MOC)
            //{
            //    lstPrinterList[i] = mo["Name"].ToString();
            //    i++;
            //}
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            var urm = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            HttpClient hc = _api.Initial();

            List<DateOffModel> um = new List<DateOffModel>();
            HttpResponseMessage res = await hc.GetAsync($"api/dateoff/GetEmpID/{urm.First().empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateOffModel>>(results);
            }

            res = await hc.GetAsync($"api/dateoff/GetDateOffInfo/{urm.First().empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.InfoList = JsonConvert.DeserializeObject<DateOffInfoModel>(results);
            }

            EmpModel em = new EmpModel();
            res = await hc.GetAsync($"api/emp/GetEmpID/{urm.First().empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<EmpModel>(results);
                ViewBag.EmpInfo = em;
            }

            //Role

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
            //------------------------------------------------- 

            return View(um);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            //Hệ thống
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
