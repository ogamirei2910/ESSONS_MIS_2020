using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Helper;
using ESSONS_MIS_2020_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESSONS_MIS_2020_App.Controllers
{
    public class QuyTrinhSPController : Controller
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

        public async Task<IActionResult> Create(ChildQuyTrinh um)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Insert", um);
            if (res.IsSuccessStatusCode)
            {
                ChildQuyTrinh em = new ChildQuyTrinh();
                res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetCodeSP?CodeSP={um.CodeSP}");
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<ChildQuyTrinh>(results);
                HttpContext.Session.SetString("notice", "Tạo mới thành công");
                return RedirectToAction("Detail", new { um.SoDonKhuon, em.CodeSP });
            }
            else
            {
                return RedirectToAction("Index");
            }            
        }

        public async Task<IActionResult> Update_KinhDoanh(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if(um.NgayDuyet != null && um.NgayDuyet != "")
                um.NgayDuyet = DateTime.ParseExact(um.NgayDuyet, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayPhatHanh1 != null && um.NgayPhatHanh1 != "")
                um.NgayPhatHanh1 = DateTime.ParseExact(um.NgayPhatHanh1, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayPhatHanh2 != null && um.NgayPhatHanh2 != "")
                um.NgayPhatHanh2 = DateTime.ParseExact(um.NgayPhatHanh2, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayPhatHanh3 != null && um.NgayPhatHanh3 != "")
                um.NgayPhatHanh3 = DateTime.ParseExact(um.NgayPhatHanh3, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_KinhDoanh", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_KHSX(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if (um.NgayGiaoMau != null && um.NgayGiaoMau != "")
                um.NgayGiaoMau = DateTime.ParseExact(um.NgayGiaoMau, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayKHApprovedMau != null && um.NgayKHApprovedMau != "")
                um.NgayKHApprovedMau = DateTime.ParseExact(um.NgayKHApprovedMau, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_KHSX", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_ThuMua(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if (um.NgayDuyetPhoiThep != null && um.NgayDuyetPhoiThep != "")
                um.NgayDuyetPhoiThep = DateTime.ParseExact(um.NgayDuyetPhoiThep, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayMuaPhoiThep != null && um.NgayMuaPhoiThep != "")
                um.NgayMuaPhoiThep = DateTime.ParseExact(um.NgayMuaPhoiThep, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayMuaPhoiThep2 != null && um.NgayMuaPhoiThep2 != "")
                um.NgayMuaPhoiThep2 = DateTime.ParseExact(um.NgayMuaPhoiThep2, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayMuaPhoiThep3 != null && um.NgayMuaPhoiThep3 != "")
                um.NgayMuaPhoiThep3 = DateTime.ParseExact(um.NgayMuaPhoiThep3, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_ThuMua", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_CTK(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if (um.NgaySXKhuon != null && um.NgaySXKhuon != "")
                um.NgaySXKhuon = DateTime.ParseExact(um.NgaySXKhuon, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayHoanThanhDuKien != null && um.NgayHoanThanhDuKien != "")
                um.NgayHoanThanhDuKien = DateTime.ParseExact(um.NgayHoanThanhDuKien, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgayHoanThanh != null && um.NgayHoanThanh != "")
                um.NgayHoanThanh = DateTime.ParseExact(um.NgayHoanThanh, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgaySuaChiTietLan1 != null && um.NgaySuaChiTietLan1 != "")
                um.NgaySuaChiTietLan1 = DateTime.ParseExact(um.NgaySuaChiTietLan1, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgaySuaChiTietLan2 != null && um.NgaySuaChiTietLan2 != "")
                um.NgaySuaChiTietLan2 = DateTime.ParseExact(um.NgaySuaChiTietLan2, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgaySuaChiTietLan3 != null && um.NgaySuaChiTietLan3 != "")
                um.NgaySuaChiTietLan3 = DateTime.ParseExact(um.NgaySuaChiTietLan3, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            if (um.NgaySuaChiTietLan4 != null && um.NgaySuaChiTietLan4!= "")
                um.NgaySuaChiTietLan4 = DateTime.ParseExact(um.NgaySuaChiTietLan4, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_CTK", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_BTK(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if (um.NgayNhanKhuon != null && um.NgayNhanKhuon != "")
                um.NgayNhanKhuon = DateTime.ParseExact(um.NgayNhanKhuon, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_BTK", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_CBNL(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_CBNL", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_DHE(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_DHE", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_QCKN(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            if (um.NgayKTMau != null && um.NgayKTMau != "")
                um.NgayKTMau = DateTime.ParseExact(um.NgayKTMau, "MM-dd-yyyy", CultureInfo.InvariantCulture)
                                        .ToString("yyyy-MM-dd");
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_QCKN", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_BT(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_BT", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_LH2(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_LH2", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_MH(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_MH", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_QTK(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_QTK", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_KM(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_KM", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_DG(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_DG", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
        }
        public async Task<IActionResult> Update_YCCL(QuyTrinhModel um)
        {
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Update_YCCL", um);

            if (res.IsSuccessStatusCode)
                ViewBag.Error = "Cập nhật thành công";
            else
                ViewBag.Error = "Lỗi kết nối server.";

            return PartialView("DisplayError");
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
            List<ChildQuyTrinh> um = new List<ChildQuyTrinh>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALL");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<ChildQuyTrinh>>(results);
            }
            ViewBag.quytrinhList = um;
            return View();
        }

        public async Task<IActionResult> Detail(string SoDonKhuon, string codeSP)
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }

            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            QuyTrinhModel um = new QuyTrinhModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_Get?SoDonKhuon={SoDonKhuon}&&codeSP={codeSP}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<QuyTrinhModel>(results);
            }

            res = await hc.GetAsync($"api/QuyTrinhSP/HoaChatSP_GetALL");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.HoaChat = JsonConvert.DeserializeObject<List<HoaChatSPModel>>(results);
            }
            return View(um);
        }

        public async Task<IActionResult> DieuDong()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<CongThucPhaChe> um = new List<CongThucPhaChe>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/CTPC/GetSoTheHC");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.sothe = JsonConvert.DeserializeObject<List<CongThucPhaChe>>(results);
            }

            List<DieuDongModel> dd = new List<DieuDongModel>();
            res = await hc.GetAsync($"api/CTPC/CTPC_GetAll");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.DieuDong = JsonConvert.DeserializeObject<List<DieuDongModel>>(results);
            }

            return View();
        }
        public async Task<IActionResult> LichSuDieuDong()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");

            List<DieuDongModel> um = new List<DieuDongModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/DieuDong_GetAll");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                ViewBag.dieudongList = JsonConvert.DeserializeObject<List<DieuDongModel>>(results);
            }

            return View();
        }
        public async Task<IActionResult> GetMaKeo(string sothe)
        {
            CongThucPhaChe em = new CongThucPhaChe();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/CTPC/CTPC_GetMaKeo?sothe={sothe}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<CongThucPhaChe>(results);
            }
            ViewBag.makeoList = em;
            return PartialView("DisplayMaKeo");
        }

        public async Task<IActionResult> GetKhoHoaChat1(string codeSP)
        {
            QuyTrinhModel em = new QuyTrinhModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetMaKeo?codeSP={codeSP}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<QuyTrinhModel>(results);
            }
            ViewBag.makeoList = em;
            return PartialView("DisplayKhoHoaChat1");
        }

        public async Task<IActionResult> GetKhoHoaChat2(string codeSP)
        {
            QuyTrinhModel em = new QuyTrinhModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetMaKeo?codeSP={codeSP}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<QuyTrinhModel>(results);
            }
            ViewBag.makeoList = em;
            return PartialView("DisplayKhoHoaChat2");
        }

        [HttpPost]
        public async Task<IActionResult> DieuDong(MaKeoQuyTrinh um)
        {
            getRole();
            HttpClient hc = _api.Initial();

            if (um.sothe == null)
            {
                ViewBag.Error = "Chưa chọn số thẻ";
                return PartialView("DisplayError");
            }

            if (um.MaKeo1 == null || um.MaKeo1 == "")
            {
                ViewBag.Error = "Kiểm tra lại mã keo";
                return PartialView("DisplayError");
            }

            if (um.Weight == 0)
            {
                ViewBag.Error = "Kiểm tra lại số KG";
                return PartialView("DisplayError");
            }

            um.username = ViewBag.empid;
            var res = await hc.PostAsJsonAsync<MaKeoQuyTrinh>("api/QuyTrinhSP/QuyTrinh_DieuDong", um);

            if (res.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Điều động thành công");
                ViewBag.Error = "OK";
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kết nối. Gọi IT";
            return PartialView("DisplayError");
        }

        [HttpPost]
        public async Task<IActionResult> DieuDong_Huy(string MO)
        {
            getRole();
            HttpClient hc = _api.Initial();
            DieuDongModel dd = new DieuDongModel();
            dd.MO = MO;
            var res = await hc.PostAsJsonAsync<DieuDongModel>("api/QuyTrinhSP/DieuDong_Huy", dd);

            if (res.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("notice", "Hủy điều động thành công");
                return RedirectToAction("DieuDong");
            }
            HttpContext.Session.SetString("notice", "Lỗi không hủy được");
            return RedirectToAction("DieuDong");
        }
        public async Task<IActionResult> ExportKinhDoanh(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";
                worksheet.Cells["A3"].Value = "KINH DOANH";
                worksheet.Cells["A4"].Value = "Số đơn khuôn";
                worksheet.Cells["B4"].Value = "Ngày duyệt";
                worksheet.Cells["C4"].Value = "Phiên bản";
                worksheet.Cells["D4"].Value = "Ngày phát hành";
                worksheet.Cells["E4"].Value = "Phiên bản";
                worksheet.Cells["F4"].Value = "Ngày phát hành";
                worksheet.Cells["G4"].Value = "Phiên bản";
                worksheet.Cells["H4"].Value = "Ngày phát hành";
                worksheet.Cells["I4"].Value = "Số lỗ khuôn (lỗ)";
                worksheet.Cells["J4"].Value = "% Co rút theo ĐK (%)";
                worksheet.Cells["K4"].Value = "KLSP (theo ĐK) (g)";
                worksheet.Cells["L4"].Value = "Time hoàn tất (LT)";
                worksheet.Cells["M4"].Value = "Loại khuôn";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:M3"].Merge = true;
                using (var range = worksheet.Cells["A3:M3"])
                {
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(32,55,100));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
                using (var range = worksheet.Cells["A4:M4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 12;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }


                worksheet.Cells["A3:M4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                worksheet.Column(1).Width = 14;
                worksheet.Column(2).Width = 14;
                worksheet.Column(3).Width = 14;
                worksheet.Column(4).Width = 14;
                worksheet.Column(5).Width = 14;
                worksheet.Column(6).Width = 14;
                worksheet.Column(7).Width = 14;
                worksheet.Column(8).Width = 14;
                worksheet.Column(9).Width = 14;
                worksheet.Column(10).Width = 14;
                worksheet.Column(11).Width = 14;
                worksheet.Column(12).Width = 14;
                worksheet.Column(13).Width = 14;
                worksheet.Column(14).Width = 14;

                worksheet.View.FreezePanes(1, 15);
                worksheet.Cells["A4:M4"].AutoFilter = true;

                int i = 0;
                foreach(var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.SoDonKhuon;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.NgayDuyet;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.PhienBan1;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.NgayPhatHanh1;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.PhienBan2;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.NgayPhatHanh2;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.PhienBan3;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.NgayPhatHanh3;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.SoLoKhuonKH;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.CoRutTheoDonKhuon;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.KLSPThucTe;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.ThoiGianHoanTat;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.LoaiKhuonMau;                  
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:M" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:M" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportKHSX(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";
                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code KH";
                worksheet.Cells["B4"].Value = "Code SP";
                worksheet.Cells["C4"].Value = "Tên QU";
                worksheet.Cells["D4"].Value = "Mã khuôn TW";
                worksheet.Cells["E4"].Value = "Qui cách Khách hàng";
                worksheet.Cells["F4"].Value = "Quy cách Essons";
                worksheet.Cells["G4"].Value = "Chất keo";
                worksheet.Cells["H4"].Value = "Màu";
                worksheet.Cells["I4"].Value = "Mã keo 1";
                worksheet.Cells["J4"].Value = "Mã keo 2";
                worksheet.Cells["K4"].Value = "Patone màu";
                worksheet.Cells["L4"].Value = "Ghi chú";
                worksheet.Cells["M4"].Value = "Sô pcs mỗi bộ";
                worksheet.Cells["N4"].Value = "Gốc NL";
                worksheet.Cells["O4"].Value = "Pha keo";
                worksheet.Cells["P4"].Value = "Ngày giao mẫu";
                worksheet.Cells["Q4"].Value = "Ngày KH approved mẫu";



                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:Q3"].Merge = true;
                using (var range = worksheet.Cells["A3:Q3"])
                {
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
                using (var range = worksheet.Cells["A4:Q4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 12;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }


                worksheet.Cells["A3:Q4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:Q4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                worksheet.Column(1).Width = 11;
                worksheet.Column(2).Width = 11;
                worksheet.Column(3).Width = 11;
                worksheet.Column(4).Width = 11;
                worksheet.Column(5).Width = 11;
                worksheet.Column(6).Width = 11;
                worksheet.Column(7).Width = 11;
                worksheet.Column(8).Width = 11;
                worksheet.Column(9).Width = 11;
                worksheet.Column(10).Width = 11;
                worksheet.Column(11).Width = 11;
                worksheet.Column(12).Width = 11;
                worksheet.Column(13).Width = 12;
                worksheet.Column(14).Width = 12;

                worksheet.View.FreezePanes(1, 18);
                worksheet.Cells["A4:Q4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeKH;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.TenQU;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.MaKhuonTW;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.PantoneMau;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.GhiChu;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.GocNL;
                    worksheet.Cells["N" + (4 + i).ToString()].Value = item.SoPCSMoiBo;
                    worksheet.Cells["O" + (4 + i).ToString()].Value = item.PhaKeo;
                    worksheet.Cells["P" + (4 + i).ToString()].Value = item.NgayGiaoMau;
                    worksheet.Cells["Q" + (4 + i).ToString()].Value = item.NgayKHApprovedMau;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:Q" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:Q" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportThuMua(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";
                worksheet.Cells["H3"].Value = "THU MUA";
                worksheet.Cells["H4"].Value = "Ngày duyệt phôi thép";
                worksheet.Cells["I4"].Value = "Ngày mua phôi thép";
                worksheet.Cells["J4"].Value = "Ngày mua phôi thép 2";
                worksheet.Cells["K4"].Value = "Ngày mua phôi thép 3";
                worksheet.Cells["L4"].Value = "Số CT mua phôi";
                worksheet.Cells["M4"].Value = "NCC phôi thép";
                
                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:M3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:M3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 176, 240));
                    range.Style.Font.Color.SetColor(Color.Yellow);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:M4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 226, 225));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:M4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                for (int j = 1; j <= 13; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.NgayDuyetPhoiThep;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.NgayMuaPhoiThep;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.NgayMuaPhoiThep2;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.NgayMuaPhoiThep3;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.SoCTMuaPhoi;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.NCCPhoiThep;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:M" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:M" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportCTK(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "CHẾ TẠO KHUÔN";
                worksheet.Cells["H4"].Value = "Loại thép khuôn";
                worksheet.Cells["I4"].Value = "Kích thước khuôn";
                worksheet.Cells["J4"].Value = "Số tấm";
                worksheet.Cells["K4"].Value = "% co rút thực tế";
                worksheet.Cells["L4"].Value = "Loại khuôn";
                worksheet.Cells["M4"].Value = "Phụ kiện";
                worksheet.Cells["N4"].Value = "PP xử lý BTP";
                worksheet.Cells["O4"].Value = "Ngày SX khuôn";
                worksheet.Cells["P4"].Value = "Ngày dự kiến HT";
                worksheet.Cells["Q4"].Value = "Ngày hoàn thành TT";
                worksheet.Cells["R4"].Value = "Chi tiết sửa lần 1";
                worksheet.Cells["S4"].Value = "Chi tiết sửa lần 2";
                worksheet.Cells["T4"].Value = "Chi tiết sửa lần 3";
                worksheet.Cells["U4"].Value = "Chi tiết sửa lần 4";
                
                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:U3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:U3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.Yellow);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:U4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:U4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:U4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                for (int j = 1; j <= 23; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.LoaiThepKhuon;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.KichThuocKhuon;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.SoTam;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.CoRutThucTe;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.LoaiKhuon;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.PhuKienCTK;
                    worksheet.Cells["N" + (4 + i).ToString()].Value = item.PPXuLyBTP;
                    worksheet.Cells["O" + (4 + i).ToString()].Value = item.NgaySXKhuon;
                    worksheet.Cells["P" + (4 + i).ToString()].Value = item.NgayHoanThanhDuKien;
                    worksheet.Cells["Q" + (4 + i).ToString()].Value = item.NgayHoanThanh;
                    worksheet.Cells["R" + (4 + i).ToString()].Value = item.SuaChiTietLan1;
                    worksheet.Cells["S" + (4 + i).ToString()].Value = item.SuaChiTietLan2;
                    worksheet.Cells["T" + (4 + i).ToString()].Value = item.SuaChiTietLan3;
                    worksheet.Cells["U" + (4 + i).ToString()].Value = item.SuaChiTietLan4;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:U" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:U" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:U" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:U" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:U" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:U" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportBTK(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "BẢO TRÌ KHUÔN";
                worksheet.Cells["H4"].Value = "Ngày nhận khuôn";
                worksheet.Cells["I4"].Value = "Vị trí khuôn";
                worksheet.Cells["J4"].Value = "Số khuôn hiện có";
                worksheet.Cells["K4"].Value = "Vị trí phụ kiện";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:K3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:K3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 96, 0));
                    range.Style.Font.Color.SetColor(Color.Yellow);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:K4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 217, 102));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:K4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:K4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                for (int j = 1; j <= 12; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.NgayNhanKhuon;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.ViTriKhuon;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.SoKhuonHienCo;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.ViTriPhuKien;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:K" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:K" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportCTNL(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "CÁN - TRỘN - CẮT NL";
                worksheet.Cells["H4"].Value = "Xử lý bề mặt";
                worksheet.Cells["I4"].Value = "PP chế biến NL";
                worksheet.Cells["J4"].Value = "Thời gian trộn (phút)";
                worksheet.Cells["K4"].Value = "Tổng thời gian cán (phút)";
                worksheet.Cells["L4"].Value = "Chi tiết cải tiến";
                worksheet.Cells["M4"].Value = "PP cắt";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:M3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:M3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 89, 17));
                    range.Style.Font.Color.SetColor(Color.Yellow);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:M4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:M4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                for (int j = 1; j <= 14; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.XuLyBeMat;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.PPCheBienNL;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.ThoiGianTron;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.TongThoiGianCan;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.ChiTietCaiTien;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.PPCat;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:M" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:M" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportDHE(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "ĐỊNH HÌNH";
                worksheet.Cells["H4"].Value = "Độ dày ±0.5 (1)";
                worksheet.Cells["I4"].Value = "Chiều dài min (1)";
                worksheet.Cells["J4"].Value = "Chiều dài max (1)";
                worksheet.Cells["K4"].Value = "TL sợi min (1)";
                worksheet.Cells["L4"].Value = "TL sợi max (1)";
                worksheet.Cells["M4"].Value = "Số sợi/vòng (1)";
                worksheet.Cells["N4"].Value = "Độ dày ±0.5 (2)";
                worksheet.Cells["O4"].Value = "Chiều dài min (2)";
                worksheet.Cells["P4"].Value = "Chiều dài max (2)";
                worksheet.Cells["Q4"].Value = "TL sợi min (2)";
                worksheet.Cells["R4"].Value = "TL sợi max (2)";
                worksheet.Cells["S4"].Value = "Số sợi/vòng (2)";
                worksheet.Cells["T4"].Value = "Độ dày ±0.5 (3)";
                worksheet.Cells["U4"].Value = "Chiều dài min (3)";
                worksheet.Cells["V4"].Value = "Chiều dài max (3)";
                worksheet.Cells["W4"].Value = "TL sợi min (3)";
                worksheet.Cells["X4"].Value = "TL sợi max (3)";
                worksheet.Cells["Y4"].Value = "Số sợi/vòng (3)";
                worksheet.Cells["Z4"].Value = "Độ dày ±0.5 (4)";
                worksheet.Cells["AA4"].Value = "Chiều dài min (4)";
                worksheet.Cells["AB4"].Value = "Chiều dài max (4)";
                worksheet.Cells["AC4"].Value = "TL sợi min (4)";
                worksheet.Cells["AD4"].Value = "TL sợi max (4)";
                worksheet.Cells["AE4"].Value = "Số sợi/vòng (4)";
                worksheet.Cells["AF4"].Value = "PP đặt keo";
                worksheet.Cells["AG4"].Value = "Thể tích bơm";
                worksheet.Cells["AH4"].Value = "Cách đặt";
                worksheet.Cells["AI4"].Value = "Số lỗ khuôn";
                worksheet.Cells["AJ4"].Value = "Số lỗ thực làm";
                worksheet.Cells["AK4"].Value = "Nhiệt độ khuôn trên ±0.5";
                worksheet.Cells["AL4"].Value = "Nhiệt độ khuôn dưới ±0.5";
                worksheet.Cells["AM4"].Value = "Lực ép (kg/cm2) ±10";
                worksheet.Cells["AN4"].Value = "Số lần thoát khí";
                worksheet.Cells["AO4"].Value = "TL linh kiện (g)";
                worksheet.Cells["AP4"].Value = "KLSP thực tế (g)";
                worksheet.Cells["AQ4"].Value = "Thời gian lưu hóa (s)";
                worksheet.Cells["AR4"].Value = "Thời gian hoàn tất 1k (s)";
                worksheet.Cells["AS4"].Value = "Chất lượng";
                worksheet.Cells["AT4"].Value = "Xử lý dính ĐH";
                worksheet.Cells["AU4"].Value = "Xử lý BTP tại ĐH";
                worksheet.Cells["AV4"].Value = "DC bóc tay";
                worksheet.Cells["AW4"].Value = "DC xử lý bề mặt";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:AW3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:AW3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 143, 0));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219,219,219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:AW4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:AW4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:AW4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 42; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.DoDay;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.ChieuDaiMin;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.ChieuDaiMax;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.TLSoiMin;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.TLSoiMax;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.SoSoi;
                    worksheet.Cells["N" + (4 + i).ToString()].Value = item.DoDay2;
                    worksheet.Cells["O" + (4 + i).ToString()].Value = item.ChieuDaiMin2;
                    worksheet.Cells["P" + (4 + i).ToString()].Value = item.ChieuDaiMax2;
                    worksheet.Cells["Q" + (4 + i).ToString()].Value = item.TLSoiMin2;
                    worksheet.Cells["R" + (4 + i).ToString()].Value = item.TLSoiMax2;
                    worksheet.Cells["S" + (4 + i).ToString()].Value = item.SoSoi2;
                    worksheet.Cells["T" + (4 + i).ToString()].Value = item.DoDay3;
                    worksheet.Cells["U" + (4 + i).ToString()].Value = item.ChieuDaiMin3;
                    worksheet.Cells["V" + (4 + i).ToString()].Value = item.ChieuDaiMax3;
                    worksheet.Cells["W" + (4 + i).ToString()].Value = item.TLSoiMin3; 
                    worksheet.Cells["X" + (4 + i).ToString()].Value = item.TLSoiMax3;
                    worksheet.Cells["Y" + (4 + i).ToString()].Value = item.SoSoi3;
                    worksheet.Cells["Z" + (4 + i).ToString()].Value = item.DoDay4;
                    worksheet.Cells["AA" + (4 + i).ToString()].Value = item.ChieuDaiMin4;
                    worksheet.Cells["AB" + (4 + i).ToString()].Value = item.ChieuDaiMax4;
                    worksheet.Cells["AC" + (4 + i).ToString()].Value = item.TLSoiMin4;
                    worksheet.Cells["AD" + (4 + i).ToString()].Value = item.TLSoiMax4;
                    worksheet.Cells["AE" + (4 + i).ToString()].Value = item.SoSoi4;
                    worksheet.Cells["AF" + (4 + i).ToString()].Value = item.PPDatKeo;
                    worksheet.Cells["AG" + (4 + i).ToString()].Value = item.TheTichBom;
                    worksheet.Cells["AH" + (4 + i).ToString()].Value = item.CachDat;
                    worksheet.Cells["AI" + (4 + i).ToString()].Value = item.SoLoKhuon;
                    worksheet.Cells["AJ" + (4 + i).ToString()].Value = item.SoLoThucLam;
                    worksheet.Cells["AK" + (4 + i).ToString()].Value = item.Tren5;
                    worksheet.Cells["AL" + (4 + i).ToString()].Value = item.Duoi5;
                    worksheet.Cells["AM" + (4 + i).ToString()].Value = item.LucEp;
                    worksheet.Cells["AN" + (4 + i).ToString()].Value = item.SoLanThoatKhi;
                    worksheet.Cells["AO" + (4 + i).ToString()].Value = item.TongTLSat;
                    worksheet.Cells["AP" + (4 + i).ToString()].Value = item.KLSPThucTe;
                    worksheet.Cells["AQ" + (4 + i).ToString()].Value = item.ThoiGianLuuHoa;
                    worksheet.Cells["AR" + (4 + i).ToString()].Value = item.ThoiGianHoanTat1k;
                    worksheet.Cells["AS" + (4 + i).ToString()].Value = item.ChatLuong;
                    worksheet.Cells["AT" + (4 + i).ToString()].Value = item.XuLyDinhDH;
                    worksheet.Cells["AU" + (4 + i).ToString()].Value = item.XuLyBTPTaiDH;
                    worksheet.Cells["AV" + (4 + i).ToString()].Value = item.DCBocTay;
                    worksheet.Cells["AW" + (4 + i).ToString()].Value = item.DCXuLyBeMat;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:AW" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:AW" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:AW" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:AW" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:AW" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:AW" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportQCKN(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "QC KIỂM NGHIỆM";
                worksheet.Cells["H4"].Value = "Ngày KT mẫu";
                worksheet.Cells["I4"].Value = "KT trước lưu hóa 2";
                worksheet.Cells["J4"].Value = "KT sau lưu hóa 2";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:J3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:J3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219,219,219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:J4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(216, 205, 225));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:J4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:J4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                for (int j = 1; j <= 12; j++)
                {
                    worksheet.Column(j).Width = 11;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.NgayKTMau;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.KTTruocLH;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.KTSauLH;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:J" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:J" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportBT(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "BÓC TÁCH MÁY";
                worksheet.Cells["H4"].Value = "PP bóc tách";
                worksheet.Cells["I4"].Value = "Xử lý bề mặt";
                worksheet.Cells["J4"].Value = "Thời gian";
                worksheet.Cells["K4"].Value = "Tốc độ";
                worksheet.Cells["L4"].Value = "KL 1 lần bắn (kg)";
                worksheet.Cells["M4"].Value = "Loại cát, bi";
                worksheet.Cells["N4"].Value = "Nhiệt độ (oC)";
                worksheet.Cells["O4"].Value = "Chất lượng BT";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:O3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:O3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(131, 60, 12));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219,219,219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:O4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:O4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:O4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 16; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.PPBocTachBTM;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.XuLyBeMatBTM;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.ThoiGian;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.TocDo;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.KL1LanBan;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.LoaiCatBi;
                    worksheet.Cells["N" + (4 + i).ToString()].Value = item.NhietDo;
                    worksheet.Cells["O" + (4 + i).ToString()].Value = item.ChatLuongBT;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:O" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:O" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:O" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:O" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:O" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:O" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportLH2(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "LƯU HÓA 2";
                worksheet.Cells["H4"].Value = "PP làm sạch";
                worksheet.Cells["I4"].Value = "Xử lý bề mặt";
                worksheet.Cells["J4"].Value = "Nhiệt độ LH2 (oC)";
                worksheet.Cells["K4"].Value = "Thời gian LH2 (h)";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:K3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:K3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(51, 63, 79));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219, 219, 219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:K4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:K4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:K4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 12; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.PPLamSach;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.XuLyBeMatLH;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.NhietDoLH;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.ThoiGianLH;

                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:K" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:K" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:K" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportMH(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "MÀI HÀNG";
                worksheet.Cells["H4"].Value = "KL 1 lần mài (kg)";
                worksheet.Cells["I4"].Value = "Thời gian mài (phút)";
                worksheet.Cells["J4"].Value = "Loại đá mài";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:J3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:J3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 96, 0));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219, 219, 219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:J4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(172, 185, 202));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:J4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:J4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 11; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.KL1LanMai;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.ThoiGianMai;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.LoaiDaMai;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:J" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:J" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:J" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportQTK(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "QUI TRÌNH KHÁC";
                worksheet.Cells["H4"].Value = "Trộn bột";
                worksheet.Cells["I4"].Value = "Chấm sơn";
                worksheet.Cells["J4"].Value = "Đóng thêm mộc, chữ";
                worksheet.Cells["K4"].Value = "Cắt theo bảng vẽ";
                worksheet.Cells["L4"].Value = "Gắn linh kiện";
                worksheet.Cells["M4"].Value = "Phủ Teflon";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:M3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:M3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(51, 63, 79));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219, 219, 219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:M4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(142, 169, 219));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:M4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 14; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.TronBot;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.ChamSon;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.DongThemMocChu;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.CatTheoBangVe;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.GanLinhKien;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.PhuTeflon;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:M" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:M" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportKM(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "KIỂM MÁY";
                worksheet.Cells["H4"].Value = "Bavia KH cho phép (mm)";
                worksheet.Cells["I4"].Value = "Bavia cài đặt máy (mm)";
                worksheet.Cells["J4"].Value = "Yêu cầu kiểm máy";
                worksheet.Cells["K4"].Value = "Xử lý bề mặt";
                worksheet.Cells["L4"].Value = "Ngoại quan";
                worksheet.Cells["M4"].Value = "Ty lệ NG (%)";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:M3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:M3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(84, 130, 53));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219, 219, 219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:M4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 224, 180));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:M4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 14; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.BaviaKH;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.BaviaCaiDat;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.YeuCauKiemMay;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.XuLyBeMat;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.NgoaiQuanKM;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.TyLeNG;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:M" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:M" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:M" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportDG(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "ĐÓNG GÓI";
                worksheet.Cells["H4"].Value = "TL 1 sản phẩm (g)";
                worksheet.Cells["I4"].Value = "TL linh kiện (g)";
                worksheet.Cells["J4"].Value = "SL 1 bọc (pcs)";
                worksheet.Cells["K4"].Value = "KT bọc (mm)";
                worksheet.Cells["L4"].Value = "SL 1 hộp";
                worksheet.Cells["M4"].Value = "KT thùng (mm)";
                worksheet.Cells["N4"].Value = "SL 1 thùng (pcs)";
                worksheet.Cells["O4"].Value = "PP đóng bọc";
                worksheet.Cells["P4"].Value = "PP đóng hơi";
                worksheet.Cells["Q4"].Value = "Ghi chú";

                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:Q3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:Q3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
                    range.Style.Font.Color.SetColor(Color.FromArgb(219, 219, 219));
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:Q4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:Q4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:Q4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 18; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.TrongLuong1SP;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.TrongLuongLinhKien;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.SL1boc;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.KichThuocBoc;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.SL1hop;
                    worksheet.Cells["M" + (4 + i).ToString()].Value = item.KichThuocThung;
                    worksheet.Cells["N" + (4 + i).ToString()].Value = item.SL1thung;
                    worksheet.Cells["O" + (4 + i).ToString()].Value = item.PPDongBoc;
                    worksheet.Cells["P" + (4 + i).ToString()].Value = item.PPDongHoi;
                    worksheet.Cells["Q" + (4 + i).ToString()].Value = item.GhiChuKhacDG;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:Q" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:Q" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:Q" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> ExportYCCL(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<QuyTrinhModel> um = new List<QuyTrinhModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_GetALLQuyTrinh");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<QuyTrinhModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";

                worksheet.Cells["A3"].Value = "KẾ HOẠCH SẢN XUẤT";
                worksheet.Cells["A4"].Value = "Code SP";
                worksheet.Cells["B4"].Value = "Quy cách Khách hàng";
                worksheet.Cells["C4"].Value = "Quy cách Essons";
                worksheet.Cells["D4"].Value = "Chất keo";
                worksheet.Cells["E4"].Value = "Màu";
                worksheet.Cells["F4"].Value = "Mã keo 1";
                worksheet.Cells["G4"].Value = "Mã keo 2";

                worksheet.Cells["H3"].Value = "YÊU CẦU CL KHÁCH HÀNG";
                worksheet.Cells["H4"].Value = "Kích thước";
                worksheet.Cells["I4"].Value = "Ngoại quan";
                worksheet.Cells["J4"].Value = "Đóng gói";
                worksheet.Cells["K4"].Value = "Khác";
                worksheet.Cells["L4"].Value = "Ghi chú";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:G3"].Merge = true;
                worksheet.Cells["H3:Q3"].Merge = true;

                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(55, 86, 35));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["A4:G4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H3:L3"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(192, 0, 0));
                    range.Style.Font.Color.SetColor(Color.White);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["H4:L4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 14;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 143, 0));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var borderHeader = range.Style.Border;
                    borderHeader.Top.Style = borderHeader.Left.Style = borderHeader.Right.Style = borderHeader.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:L4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:L4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 90.00;
                for (int j = 1; j <= 13; j++)
                {
                    worksheet.Column(j).Width = 12;
                }


                worksheet.View.FreezePanes(1, 8);
                worksheet.Cells["A4:G4"].AutoFilter = true;

                int i = 0;
                foreach (var item in um)
                {
                    i++;
                    worksheet.Cells["A" + (4 + i).ToString()].Value = item.CodeSP;
                    worksheet.Cells["B" + (4 + i).ToString()].Value = item.QuyCachKH;
                    worksheet.Cells["C" + (4 + i).ToString()].Value = item.QuyCachEssons;
                    worksheet.Cells["D" + (4 + i).ToString()].Value = item.ChatKeo;
                    worksheet.Cells["E" + (4 + i).ToString()].Value = item.Mau;
                    worksheet.Cells["F" + (4 + i).ToString()].Value = item.MaKeo1;
                    worksheet.Cells["G" + (4 + i).ToString()].Value = item.MaKeo2;
                    worksheet.Cells["H" + (4 + i).ToString()].Value = item.KichThuoc;
                    worksheet.Cells["I" + (4 + i).ToString()].Value = item.NgoaiQuan;
                    worksheet.Cells["J" + (4 + i).ToString()].Value = item.DongGoi;
                    worksheet.Cells["K" + (4 + i).ToString()].Value = item.Khac;
                    worksheet.Cells["L" + (4 + i).ToString()].Value = item.GhiChu;
                }

                // Formatting style all
                using (var range = worksheet.Cells["A5:L" + (i + 4).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A5:L" + (i + 4).ToString()].Style.Font.Size = 10;
                worksheet.Cells["A5:L" + (i + 4).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A5:L" + (i + 4).ToString()];
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A5:L" + (i + 4).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:L" + (i + 4).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}