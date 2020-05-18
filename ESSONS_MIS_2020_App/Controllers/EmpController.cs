﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class EmpController : Controller
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
            var DistinctItems = role.Select(x => x.folderID).Distinct().ToList();
            ViewBag.folder = DistinctItems;
            ViewBag.folderList = role;
        }
        public async Task<IActionResult> Index()
        {
            List<EmpModel> um = new List<EmpModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/emp/Get");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }
            //Role
            getRole();
            //-----------------------------------

            return View(um);
        }

        [HttpGet]
        public async Task<IActionResult> emp_Detail(string empID)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }
            getRole();
            return View(um);
        }

        public async Task<IActionResult> emp_Create()
        {
            List<DepartmentModel> em = new List<DepartmentModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/emp/GetDepartment");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<DepartmentModel>>(results);
            }
            ViewBag.departmentList = em;
            getRole();

            return View();
        }

        [HttpPost]
        public IActionResult emp_Create(EmpModel um)
        {
            HttpClient hc = _api.Initial();
            var image = um.ProfileImage;
            if (image != null)
            {
                //Saving Image on Server
                if (image.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images/NhanVien", image.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }

                um.empImage = um.ProfileImage.FileName;
                um.ProfileImage = null;
            }

            um.username = HttpContext.Session.GetString("username");
            um.status = 1;
            um.indat = DateTime.Now.ToString("dd/MM/yyyy");
            um.intime = DateTime.Now.ToString("HH:mm:ss");

            var res = hc.PostAsJsonAsync<EmpModel>("api/emp/Create", um);

            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                ViewBag.Message = "Đã thêm nhân viên mới";
                return RedirectToAction("emp_Detail", "Emp", new { empID = um.empID });
            }

            ViewBag.Message = "Lỗi kết nối hệ thống. Liên hệ IT";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> emp_Update(string empID)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }

            if (um.empImage != null && um.empImage != "")
                HttpContext.Session.SetString("empImage", um.empImage);
            else
                HttpContext.Session.SetString("empImage", um.empImage);

            List<DepartmentModel> em = new List<DepartmentModel>();
            hc = _api.Initial();
            res = await hc.GetAsync("api/emp/GetDepartment");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<DepartmentModel>>(results);
            }
            ViewBag.departmentList = em;


            PositionDepEmpModel pm = new PositionDepEmpModel();
            hc = _api.Initial();
            res = await hc.GetAsync($"api/emp/GetPositionDepEmp/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                pm = JsonConvert.DeserializeObject<PositionDepEmpModel>(results);
            }
            ViewBag.positiondepList = pm;

            List<DepartmentChildModel> dtm = new List<DepartmentChildModel>();
            hc = _api.Initial();
            res = await hc.GetAsync($"api/emp/GetDepartmentChild?depID={pm.depID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                dtm = JsonConvert.DeserializeObject<List<DepartmentChildModel>>(results);
            }
            ViewBag.departmentchildList = dtm;

            if (pm.depchildID != null && pm.depchildID != "")
            {
                List<PositionModel> ptm = new List<PositionModel>();
                hc = _api.Initial();
                res = await hc.GetAsync($"api/emp/GetPosition?depchildID={pm.depchildID}");
                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStringAsync().Result;
                    ptm = JsonConvert.DeserializeObject<List<PositionModel>>(results);
                }
                ViewBag.positionList = ptm;
            }
            getRole();
            return View(um);
        }

        [HttpPost]
        public IActionResult emp_Update(EmpModel um)
        {
            HttpClient hc = _api.Initial();
            var image = um.ProfileImage;
            //Saving Image on Server
            if (image != null)
            {
                if (image.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images/NhanVien", image.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }
                um.empImage = um.ProfileImage.FileName;
                um.ProfileImage = null;
            }

            if (um.empImage == null)
                um.empImage = HttpContext.Session.GetString("empImage");
            um.username = HttpContext.Session.GetString("username");
            um.indat = DateTime.Now.ToString("dd-MM-yyyy");
            um.status = 1;
            var res = hc.PostAsJsonAsync<EmpModel>("api/emp/Update", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                ViewBag.Message = "Đã cập nhật nhân viên";
                return RedirectToAction("emp_Detail", "Emp", new { empID = um.empID });
            }

            ViewBag.Message = "Lỗi kết nối hệ thống. Liên hệ IT";
            return View(um.empID);
        }

        public IActionResult emp_Block(string empID)
        {
            EmpModel em = new EmpModel();
            em.empID = empID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<EmpModel>($"api/emp/Block", em);
            res.Wait();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DepartmentChild(string depID)
        {
            List<DepartmentChildModel> em = new List<DepartmentChildModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetDepartmentChild?depID={depID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<DepartmentChildModel>>(results);
            }
            ViewBag.DepartmentCList = em;
            return PartialView("DisplayDEpartmentChild");
        }


        public async Task<IActionResult> Position(string depchildID)
        {
            List<PositionModel> em = new List<PositionModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetPosition?depchildID={depchildID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<PositionModel>>(results);
            }
            ViewBag.PositionList = em;
            return PartialView("DisplayPosition");
        }
    }
}