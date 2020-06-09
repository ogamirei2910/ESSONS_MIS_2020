using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class DateWorkController : Controller
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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RequestOT()
        {
            getRole();
            ViewBag.Error = "";
            List<EmpModel> um = new List<EmpModel>();
            HttpClient hc = _api.Initial();

            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpInManager?empid=03757");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }

            List<TimeWorkModel> em = new List<TimeWorkModel>();
            HttpResponseMessage res2 = await hc.GetAsync($"api/dateoff/GetTimeWorkRequest?empid=03757&&workdate=0");
            if (res2.IsSuccessStatusCode)
            {
                var results = res2.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<TimeWorkModel>>(results);
            }

            ViewBag.timeworkList = em;
            ViewBag.emp = um;
            return View();
        }
    }
}