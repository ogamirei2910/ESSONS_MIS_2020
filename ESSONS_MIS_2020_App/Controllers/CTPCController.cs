using System;
using System.Collections.Generic;
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
    public class CTPCController : Controller
    {
        EssonsApi _api = new EssonsApi();
        public void getRole()
        {
            var role = HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList");
            ViewBag.message = role.First().empName.ToString();
            ViewBag.roleID = role.First().roleID.ToString();
            ViewBag.empid = role.First().empID.ToString();
            ViewBag.depName = role.First().depName.ToString();
            if (role.First().empImage != null)
                ViewBag.empImage = role.First().empImage.ToString();
            else
                ViewBag.empImage = "";
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
            List<CongThucPhaChe> um = new List<CongThucPhaChe>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/CTPC/GetAllCTPC");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<CongThucPhaChe>>(results);
            }
            ViewBag.ctpcList = um;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.Error = "";
            CongThucPhaChe um = new CongThucPhaChe();
            HttpClient hc = _api.Initial();

            HttpResponseMessage res = await hc.GetAsync($"api/CTPC/GetSoThe");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.SoThe = (int.Parse(results) + 1).ToString("D3");
            }

            res = await hc.GetAsync($"api/QuyTrinhSP/HoaChatSP_GetALL");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.HoaChat = JsonConvert.DeserializeObject<List<HoaChatSPModel>>(results);
            }

            List<CongThucPhaCheE> le = new List<CongThucPhaCheE>();
            List<CongThucPhaCheK> lk = new List<CongThucPhaCheK>();
            for (int i = 0; i < 14; i ++)
            {
                CongThucPhaCheE e = new CongThucPhaCheE();
                CongThucPhaCheK k = new CongThucPhaCheK();

                le.Add(e);
                lk.Add(k);
            }
            um.ctpce = le;
            um.ctpck = lk;
            return View(um);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CongThucPhaChe model)
        {
            getRole();
            ViewBag.Error = "";
            CongThucPhaChe um = new CongThucPhaChe();
            HttpClient hc = _api.Initial();

            var res = await hc.PostAsJsonAsync($"api/CTPC/CTPC_Insert",model);
            if (res.IsSuccessStatusCode)
            {
                ViewBag.Error = "Thêm mới thành công số thẻ " + model.sothe;
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kiểm tra lại dữ liệu đã nhập";
            return PartialView("DisplayError");
        }

        public async Task<IActionResult> Detail(string SoThe)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.Error = "";
            CongThucPhaChe um = new CongThucPhaChe();
            HttpClient hc = _api.Initial();

            HttpResponseMessage res = await hc.GetAsync($"api/CTPC/CTPC_Detail?SoThe={SoThe}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<CongThucPhaChe>(results);
            }

            res = await hc.GetAsync($"api/QuyTrinhSP/HoaChatSP_GetALL");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.HoaChat = JsonConvert.DeserializeObject<List<HoaChatSPModel>>(results);
            }

            return View(um);
        }
        

        [HttpPost]
        public async Task<IActionResult> Update(CongThucPhaChe model)
        {
            getRole();
            ViewBag.Error = "";
            CongThucPhaChe um = new CongThucPhaChe();
            HttpClient hc = _api.Initial();

            var res = await hc.PostAsJsonAsync($"api/CTPC/CTPC_Update", model);
            if (res.IsSuccessStatusCode)
            {
                ViewBag.Error = "Cập nhật thành công số thẻ " + model.sothe;
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kiểm tra lại dữ liệu đã nhập";
            return PartialView("DisplayError");
        }
    }
}