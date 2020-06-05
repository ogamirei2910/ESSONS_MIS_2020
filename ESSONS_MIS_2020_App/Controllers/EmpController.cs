using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;


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
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

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
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

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

            string empid = "";
            res = await hc.GetAsync("api/emp/GetEmpSTT");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                empid = JsonConvert.DeserializeObject<string>(results);
                ViewBag.STT = (int.Parse(empid) +1).ToString("D5");
            }

            ViewBag.departmentList = em;
            getRole();

            return View();
        }

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        [HttpPost]
        public IActionResult emp_Create(EmpModel um)
        {
            getRole();
            HttpClient hc = _api.Initial();
            um.empID = int.Parse(um.empID).ToString("D5");
            var image = um.ProfileImage;
            if (image != null)
            {
                string[] NameAndType = new string[2];
                //Saving Image on Server
                if (image.Length > 0)
                {
                    NameAndType = image.FileName.Split(".");
                    var filePath = Path.Combine("wwwroot/images/NhanVien", um.empID + "." + NameAndType[1]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }

                um.empImage = um.empID + "." + NameAndType[1];
                um.ProfileImage = null;

                Image img = Image.FromFile("wwwroot\\images\\layout.jpg");
                Graphics g = Graphics.FromImage(img);

                Image imageHinhThe = Image.FromFile("wwwroot\\images\\NhanVien\\" + um.empID + "." + NameAndType[1]);
                Bitmap bmDatabaseImage = (Bitmap)imageHinhThe;
                g.DrawImage(bmDatabaseImage, 20, 20, 319, 470);

                Rectangle rect1 = new Rectangle(340, 332, 460, 50);
                Rectangle rect2 = new Rectangle(340, 390, 460, 50);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Draw the text and the surrounding rectangle.
                if (um.empName.Length < 20)
                {
                    g.DrawString(um.empName, new Font("Calibri", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.depchildName, new Font("Calibri", 9, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                else
                {
                    g.DrawString(um.empName, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.depchildName, new Font("Calibri", 8, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                g.DrawString("Số hiệu: " + um.empID, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(65, 500));

                //Create QRcode
                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode(um.empID, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(_qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                //var filePath2 = Path.Combine("wwwroot/images/NhanVien", int.Parse(um.empID).ToString("D5") + "_QR.png");
                //qrCodeImage.Save(filePath2, System.Drawing.Imaging.ImageFormat.Png);
                //-----------------------------------------------------
                g.DrawImage(qrCodeImage, 790, 300, 220, 220);

                img.Save($"wwwroot/images/NhanVien/Card_{um.empImage}");

            }

            //Save excel file
            //string folder = "wwwroot/images/NhanVien";
            //string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            //FileInfo file = new FileInfo(Path.Combine(folder, excelName));
            //if (file.Exists)
            //{
            //    file.Delete();
            //    file = new FileInfo(Path.Combine(folder, excelName));
            //}

            //// query data from database  
            //using (var package = new ExcelPackage(file))
            //{
            //    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

            //    package.Save();
            //}

            um.username = ViewBag.empid;
            um.status = 1;
            um.indat = DateTime.Now.ToString("dd-mm-yyyy");
            um.intime = DateTime.Now.ToString("HH:mm:ss");

            var res = hc.PostAsJsonAsync<EmpModel>("api/emp/Create", um);

            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Đã thêm nhân viên mới");
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
            getRole();
            HttpClient hc = _api.Initial();
            var image = um.ProfileImage;
            string[] NameAndType = new string[2];
            //Saving Image on Server
            if (image != null)
            {           
                if (image.Length > 0)
                {
                    NameAndType = image.FileName.Split(".");
                    var filePath = Path.Combine("wwwroot/images/NhanVien", um.empID + "." + NameAndType[1]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }
                um.empImage = um.empID + "." + NameAndType[1];
                um.ProfileImage = null;           
            }

            if (um.empImage == null)
                um.empImage = HttpContext.Session.GetString("empImage");

            if (um.empImage != null && um.empImage != "")
            {
                NameAndType = um.empImage.Split(".");
                Image img = Image.FromFile("wwwroot\\images\\layout.jpg");
                Graphics g = Graphics.FromImage(img);

                Image imageHinhThe = Image.FromFile("wwwroot\\images\\NhanVien\\" + um.empID + "." + NameAndType[1]);
                Bitmap bmDatabaseImage = (Bitmap)imageHinhThe;
                g.DrawImage(bmDatabaseImage, 20, 20, 319, 480);

                Rectangle rect1 = new Rectangle(340, 332, 410, 50);
                Rectangle rect2 = new Rectangle(340, 390, 410, 50);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Draw the text and the surrounding rectangle.
                if (um.empName.Length < 20)
                {
                    g.DrawString(um.empName, new Font("Calibri", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.depchildName, new Font("Calibri", 9, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                else
                {
                    g.DrawString(um.empName, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.depchildName, new Font("Calibri", 8, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                g.DrawString("Số hiệu: " + um.empID, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(65, 500));

                //Create QRcode
                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode(um.empID, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(_qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                //var filePath2 = Path.Combine("wwwroot/images/NhanVien", int.Parse(um.empID).ToString("D5") + "_QR.png");
                //qrCodeImage.Save(filePath2, System.Drawing.Imaging.ImageFormat.Png);
                //-----------------------------------------------------
                g.DrawImage(qrCodeImage, 760, 300, 220, 220);

                img.Save($"wwwroot/images/NhanVien/Card_{um.empImage}");
            }
           

            um.username = ViewBag.empid;
            um.indat = DateTime.Now.ToString("dd-MM-yyyy");
            um.status = 1;
            var res = hc.PostAsJsonAsync<EmpModel>("api/emp/Update", um);
            res.Wait();

            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Đã cập nhật nhân viên " + um.empID );
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
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Đã khóa nhân viên " + empID);
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("notice", "Lỗi chưa khóa nhân viên " + empID);
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