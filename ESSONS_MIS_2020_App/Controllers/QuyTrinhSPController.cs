using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetObjectFromJson<List<UserRoleModel>>("folderList") is null)
            {
                string path = Request.Scheme.ToString() + @"://" + Request.Host.Value + Request.Path.ToString() + Request.QueryString.ToString();
                HttpContext.Session.SetString("resultPage", path);
                return RedirectToAction("Login", "User");
            }
            getRole();
            ChildQuyTrinh um = new ChildQuyTrinh();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.PostAsJsonAsync($"api/QuyTrinhSP/QuyTrinh_Insert", um);
            if (res.IsSuccessStatusCode)
            {
                ViewBag.Error = "OK";          
            }
            else
                ViewBag.Error = "Lỗi mã chứng từ và Code SP này đã tồn tại.";
            return RedirectToAction("Detail", new { um.SoDonKhuon, um.CodeSP });
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
            QuyTrinhModel um = new QuyTrinhModel();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/QuyTrinh_Get?SoDonKhuon={SoDonKhuon}&&codeSP={codeSP}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<QuyTrinhModel>(results);
            }
            return View(um);
        }

        public async Task<IActionResult> Export(CancellationToken cancellationToken)
        {
            getRole();
            // query data from database  
            List<DateWorkPrintModel> um = new List<DateWorkPrintModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/QuyTrinhSP/GetALL");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateWorkPrintModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP QUI TRÌNH SẢN XUẤT PHÁT TRIỂN SẢN PHẨM MỚI";
                worksheet.Cells["A3"].Value = "KINH DOANH";
                worksheet.Cells["A4"].Value = "Số đơn khuôn";
                worksheet.Cells["B4"].Value = "Ngày duyệt";
                worksheet.Cells["C4"].Value = "Ngày phát hành";
                worksheet.Cells["D4"].Value = "Biên bản";
                worksheet.Cells["E4"].Value = "Số lỗ khuôn (lỗ)";
                worksheet.Cells["F4"].Value = "% Co rút theo ĐK (%)";
                worksheet.Cells["G4"].Value = "KLSP (theo ĐK) (g)";
                worksheet.Cells["H4"].Value = "Time hoàn tất (LT)";
                worksheet.Cells["I4"].Value = "Loại khuôn";


                worksheet.Cells["A1"].Style.Font.Size = 22;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A3:I3"].Merge = true;
                using (var range = worksheet.Cells["A3:I3"])
                {
                    range.Style.Font.Size = 18;
                    range.Style.Font.Bold = true;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(32,55,100));
                    range.Style.Font.Color.SetColor(Color.White);
                    var border = range.Style.Border;
                    border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
                using (var range = worksheet.Cells["A4:E4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 12;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var border = range.Style.Border;
                    border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = worksheet.Cells["F4:I4"])
                {
                    range.Style.WrapText = true;
                    range.Style.Font.Size = 12;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(142, 169, 219));
                    range.Style.Font.Color.SetColor(Color.Black);
                    var border = range.Style.Border;
                    border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                        OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                worksheet.Cells["A3:I4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:I4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Row(4).Height = 78.00;
                worksheet.Column(1).Width = 16;
                worksheet.Column(2).Width = 16;
                worksheet.Column(3).Width = 16;
                worksheet.Column(4).Width = 16;
                worksheet.Column(5).Width = 16;
                worksheet.Column(6).Width = 16;
                worksheet.Column(7).Width = 16;
                worksheet.Column(8).Width = 16;
                worksheet.Column(9).Width = 16;


                worksheet.View.FreezePanes(1, 10);
                worksheet.Cells["A4:I4"].AutoFilter = true;
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"QUYTRINHSP-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}