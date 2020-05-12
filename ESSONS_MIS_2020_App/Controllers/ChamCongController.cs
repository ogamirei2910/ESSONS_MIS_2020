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
    public class ChamCongController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public void getRole()
        {
            var role = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            ViewBag.message = role.First().empName.ToString();
            ViewBag.roleID = role.First().roleID.ToString();
            var DistinctItems = role.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = role;
        }
        public async Task<IActionResult> Index()
        {
            List<ChamCongModel> um = new List<ChamCongModel>();
            HttpClient hc = _api.Initial();
            var date = "2020/02/03";
            HttpResponseMessage res = await hc.GetAsync($"api/chamcong/GetChamCongTheoNgay?date={date}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<ChamCongModel>>(results);
            }
            //Role
            getRole();
            //-----------------------------------

            return View(um);
        }

        [HttpGet]
        public async Task<IActionResult> chamcong_theongay(string date)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/chamcong/GetChamCongTheoNgay?date={date}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }
            getRole();
            return View(um);
        }

        [HttpGet]
        public async Task<IActionResult> chamcong_theothang(string date)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/chamcong/GetNgayCongTheoThang?date={date}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }
            getRole();
            return View(um);
        }
    }
}