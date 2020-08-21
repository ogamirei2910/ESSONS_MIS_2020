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
        //Lấy đường dẫn đến kết tới API
        EssonsApi _api = new EssonsApi();

        //Lấy thông tin cơ bản của nhân viên và quyền truy cập thư mục
        public void getRole()
        {
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

        //Giao diện mặc định quản lý nhân viên
        public async Task<IActionResult> Index()
        {
            //Kiểm tra quyền truy cập nếu chưa lấy danh sách mục quản lý thì trở về trang đăng nhập
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            //Lấy nội dung thông báo trả về từ các Controller khác và set session về rỗng
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            //Lấy thông tin cơ bản của tất cả nhân viên đang còn làm việc 
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

        //Giao diện mặc định xuất danh sách vị trí của nhân viên qua từng thời gian
        public async Task<IActionResult> emp_Position()
        {
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<PositionDepEmpModel> um = new List<PositionDepEmpModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetAllPositionDepEmp");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<PositionDepEmpModel>>(results);
            }
            return View(um);
        }
        [HttpGet]
        public async Task<IActionResult> emp_Detail(string empID)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

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

        //Load thông tin cho giao diện tạo mới nhân viên
        public async Task<IActionResult> emp_Create()
        {
            //Kiểm tra quyền truy cập nếu chưa lấy danh sách mục quản lý thì trở về trang đăng nhập
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            //Lấy danh sách Nhóm bộ phận
            List<DepartmentModel> em = new List<DepartmentModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/emp/GetDepartment");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<DepartmentModel>>(results);
            }

            //Lấy danh sách nhóm bộ phận định hình => Để thêm người quản lý nhóm bộ phận này
            List<PositionDepEmpModel> am = new List<PositionDepEmpModel>();
            res = await hc.GetAsync("api/emp/GetGroupID");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                am = JsonConvert.DeserializeObject<List<PositionDepEmpModel>>(results);
            }

            //Lấy số thứ tự tiếp theo của nhân viên
            string empid = "";
            res = await hc.GetAsync("api/emp/GetEmpSTT");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                empid = JsonConvert.DeserializeObject<string>(results);
                ViewBag.STT = (int.Parse(empid) + 1).ToString("D5");
            }

            ViewBag.departmentList = em;
            ViewBag.groupList = am;

            //Lấy danh sách những mục được phép truy cập
            getRole();

            return View();
        }

        //Giao diện quản lý bộ phận chức vụ
        public async Task<IActionResult> emp_SetPhongBan()
        {
            //Kiểm tra quyền truy cập nếu chưa lấy danh sách mục quản lý thì trở về trang đăng nhập
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            HttpClient hc = _api.Initial();

            //Lấy danh sách phòng ban 
            var res = await hc.GetAsync("api/emp/GetAllDepartment");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.departmentList = JsonConvert.DeserializeObject<List<DepartmentModel>>(results);
            }

            res = await hc.GetAsync("api/emp/GetAllDepartmentChild");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.departmentchildList = JsonConvert.DeserializeObject<List<DepartmentChildModel>>(results);
            }

            res = await hc.GetAsync("api/emp/GetAllPosition");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.positionList = JsonConvert.DeserializeObject<List<PositionModel>>(results);
            }
            getRole();
            return View();
        }
        //Chỉnh kích thước ảnh phù hợp với khung hình thẻ nhân viên
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        //Gửi thông tin đến api để tiến hành thêm mới nhân viên
        [HttpPost]
        public IActionResult emp_Create(EmpModel um)
        {
            getRole();
            HttpClient hc = _api.Initial(); //thiết lập đường dẫn đến web api
            um.empID = int.Parse(um.empID).ToString("D5"); //Chuyển số thứ tự theo mẫu 00001 00002 00003

            //Xử lý hình ảnh của nhân viên và lưu trữ trên IIS
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
                    g.DrawString(um.positionName, new Font("Calibri", 9, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                else
                {
                    g.DrawString(um.empName, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.positionName, new Font("Calibri", 8, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                g.DrawString("Số hiệu: " + um.empID, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(65, 500));

                //Create QRcode
                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode(um.empID, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(_qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                //-----------------------------------------------------
                g.DrawImage(qrCodeImage, 790, 300, 220, 220);

                img.Save($"wwwroot/images/NhanVien/Card_{um.empImage}");

            }
            //------------------------------------------------------------------------------ End xử lý hình ảnh

            um.username = ViewBag.empid;
            um.status = 1;
            um.indat = DateTime.Now.ToString("dd-mm-yyyy");
            um.intime = DateTime.Now.ToString("HH:mm:ss");

            //Chuyển dữ liệu đến web API để xử lý
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

        //Dùng để in QR tất cả nhân viên
        [HttpGet]
        public async Task<IActionResult> emp_Create_QR()
        {
            getRole();
            List<EmpModel> um = new List<EmpModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync("api/emp/Get");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }

            var i = 1;
            var j = 1;
            var x = 20;
            var x1 = 40;
            var y = 10;
            var y1 = 100;
            Image img = Image.FromFile("wwwroot\\images\\layoutQR.png"); 
            Graphics g = Graphics.FromImage(img);
            foreach (var item in um)
            {
                if (i == 1)
                {
                    img = Image.FromFile("wwwroot\\images\\layoutQR.png");
                    g = Graphics.FromImage(img);
                }
                //Create QRcode
                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode(item.empID, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(_qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                //-----------------------------------------------------
                g.DrawImage(qrCodeImage, x, y, 100, 100);
                g.DrawString(item.empID, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), new Point(x1, y1));  

                if (i % 14 == 0)
                {
                    y += 140;
                    y1 += 140;
                    x = 20;
                    x1 = 40;
                }
                else
                {
                    x += 110;
                    x1 += 110;
                }

                if (i == 70 || j == um.Count )
                {
                    string path = $"wwwroot/images/NhanVien/test{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png";
                    img.Save(path);
                    i = 1;
                    x = 20;
                    x1 = 40;
                    y = 10;
                    y1 = 100;
                }
                else
                    i++;
                                
                j++;
            }


            return View();
        } 

        //Lấy thông tin của nhân viên để sửa
        [HttpGet]
        public async Task<IActionResult> emp_Update(string empID)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            EmpModel um = new EmpModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpID/{empID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<EmpModel>(results);
            }

            List<PositionDepEmpModel> am = new List<PositionDepEmpModel>();
            res = await hc.GetAsync("api/emp/GetGroupID");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                am = JsonConvert.DeserializeObject<List<PositionDepEmpModel>>(results);
            }
            ViewBag.groupList = am;

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

                List<EmpModel> group = new List<EmpModel>();
                hc = _api.Initial();
                res = await hc.GetAsync($"api/Emp/GetEmpManager?groupID={pm.groupID}");
                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStringAsync().Result;
                    group = JsonConvert.DeserializeObject<List<EmpModel>>(results);
                }
                ViewBag.empList = group;
            }

            getRole();
            return View(um);
        }


        //Gửi thông tin đến api để tiền hành cập nhật nhân viên
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
                    g.DrawString(um.positionName, new Font("Calibri", 9, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
                }
                else
                {
                    g.DrawString(um.empName, new Font("Calibri", 9, FontStyle.Bold), new SolidBrush(Color.Black), rect1, stringFormat);
                    g.DrawString(um.positionName, new Font("Calibri", 8, FontStyle.Regular), new SolidBrush(Color.Black), rect2, stringFormat);
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
                HttpContext.Session.SetString("notice", "Đã cập nhật nhân viên " + um.empID);
                return RedirectToAction("emp_Detail", "Emp", new { empID = um.empID });
            }

            ViewBag.Message = "Lỗi kết nối hệ thống. Liên hệ IT";
            return View(um.empID);
        }

        //Lấy thông tin nhân viên đã khóa trước đó
        [HttpGet]
        public async Task<IActionResult> emp_UnBlock()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<EmpModel> um = new List<EmpModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpBlock/");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }
            getRole();
            return View(um);
        }


        //Khóa nhân viên
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

        //Mở khóa nhân viên
        public IActionResult UnBlock(string empID)
        {
            EmpModel em = new EmpModel();
            em.empID = empID;
            HttpClient hc = _api.Initial();
            var res = hc.PostAsJsonAsync<EmpModel>($"api/emp/UnBlock", em);
            res.Wait();
            var results = res.Result;
            if (results.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Đã mở khóa nhân viên " + empID);
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("notice", "Lỗi chưa mở khóa nhân viên " + empID);
            return RedirectToAction("Index");
        }

        //Lấy thông tin bộ phận con cho Nhóm bộ phận truyền đến combobox của View
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

        //Lấy thông tin người quản lý cho Nhóm bộ phận định hình truyền đến combobox của View
        public async Task<IActionResult> EmpManager(string groupID)
        {
            List<EmpModel> em = new List<EmpModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpManager?groupID={groupID}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }
            ViewBag.empManagerList = em;
            return PartialView("DisplayEmpManager");
        }

        //Lấy thông tin chức vụ của nhân viên truyền đến combobox của View
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