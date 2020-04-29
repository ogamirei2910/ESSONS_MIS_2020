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
using Newtonsoft.Json;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class EmpController : Controller
    {
        EssonsApi _api = new EssonsApi();
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
            return View(um);
        }

        [HttpGet]
        public async Task<IActionResult> emp_Detail(int empID)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }
            return View(um);
        }

        public IActionResult emp_Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult emp_Create(EmpModel um)
        {
            HttpClient hc = _api.Initial();
            var image = um.ProfileImage;
            if(image != null)
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
            
            um.status = 1;
            um.indat = DateTime.Now.ToString("dd/MM/yyyy");
            um.intime = DateTime.Now.ToString("HH:mm:ss");
            um.username = HttpContext.Session.GetString("username");

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
        public async Task<IActionResult> emp_Update(int empID)
        {
            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }
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
                um.status = 1;
                um.ProfileImage = null;
            }

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

        public async Task<IActionResult> emp_Block(int empID)
        {
            HttpClient hc = _api.Initial();
            var res = await hc.DeleteAsync($"api/emp/Block/{empID}");

            return RedirectToAction("Index");
        }
    }
}