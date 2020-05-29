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

namespace ESSONS_MIS_2020_App.Controllers
{
    public class HomeController : Controller
    {
        EssonsApi _api = new EssonsApi();

        public async Task<IActionResult> Index()
        {

            string username = HttpContext.Session.GetString("username");
            List<UserRoleModel> um = new List<UserRoleModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/user/GetRole/{username}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<UserRoleModel>>(results);
            }

            //Role
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            ViewBag.message = um.First().empName.ToString();
            ViewBag.roleID = um.First().roleID.ToString();
            ViewBag.empid = um.First().empID.ToString();
            if (um.First().empImage != null)
                ViewBag.empImage = um.First().empImage.ToString();
            else
                ViewBag.empImage = "";
            var DistinctItems = um.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = um;
            HttpContext.Session.SetObjectAsJson("folderList", um);
            //-------------------------------------------------

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
