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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
        public async Task<IActionResult> Index()
        {
            getRole();
            HttpContext.Session.SetString("requestID", "");
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            List<EmpDateWorkModel> um = new List<EmpDateWorkModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/datework/GetDateWork?empid={ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpDateWorkModel>>(results);
            }

            ViewBag.requestList = um;
            return View();
        }

        public async Task<IActionResult> Detail(string requestID, int isOT, string shiftName)
        {
            getRole();
            HttpContext.Session.SetString("requestID", "");
            List<EmpDateWorkModel> um = new List<EmpDateWorkModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/datework/GetDateWorkDetail?requestID={requestID}&&isOT={isOT}&&shiftName={shiftName}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpDateWorkModel>>(results);
            }

            ViewBag.requestList = um;
            return View();
        }

        public async Task<IActionResult> Confirm()
        {
            getRole();
            HttpContext.Session.SetString("requestID", "");
            ViewBag.notice = HttpContext.Session.GetString("notice");
            HttpContext.Session.SetString("notice", "");
            List<EmpDateWorkModel> um = new List<EmpDateWorkModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/datework/GetAllDateWork?empid={ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpDateWorkModel>>(results);
            }

            ViewBag.requestList = um;
            return View();
        }

        public async Task<IActionResult> ExportV2(CancellationToken cancellationToken, string requestID, string shiftName)
        {
            getRole();
            // query data from database  
            List<DateWorkPrintModel> um = new List<DateWorkPrintModel>();
            HttpClient hc = _api.Initial();
            HttpResponseMessage res = await hc.GetAsync($"api/datework/GetDateWorkPrint?requestID={requestID}&&shiftName={shiftName}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<DateWorkPrintModel>>(results);
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "MẪU SỐ 1";
                worksheet.Cells["A2"].Value = "(Ban hành kèm theo thông tư số 15/2003/TT-BLNTBXH, ngày 03/06/2003)";
                worksheet.Cells["A3"].Value = "Của bộ lao động-Thương binh và xã hội";
                worksheet.Cells["A4"].Value = "DOANH NGHIỆP CÔNG TY:TNHH CÔNG NGHIỆP TOÀN CẦU ESSONS";
                worksheet.Cells["A5"].Value = "BIÊN BẢN THỎA THUẬN CỦA NGƯỜI LÀM THÊM GIỜ";
                worksheet.Cells["A6"].Value = "Thời gian làm thêm: Từ ngày " +um.First().datework.Substring(8,2)+ " tháng  "+ um.First().datework.Substring(5, 2) + "  " +
                    "đến ngày  " + um.First().dateworkend.Substring(8, 2) + "  tháng " + um.First().dateworkend.Substring(5, 2) + "  năm " +um.First().datework.Substring(0,4);
                //worksheet.Cells["A7"].Value = "Địa điểm làm thêm BỘ PHẬN: CHẾ BIẾN NL";
                ExcelRange rg = worksheet.Cells["A7"];
                rg.IsRichText = true;
                ExcelRichText text1 = rg.RichText.Add("Địa điểm làm thêm BỘ PHẬN: ");
                text1.Size = 14;
                text1.FontName = "Times New Roman";
                ExcelRichText text2 = rg.RichText.Add(um.First().depName.ToString());
                text1.Size = 14;
                text1.FontName = "Times New Roman";
                text2.Bold = true;
                worksheet.Cells["A8"].Value = "STT";
                worksheet.Cells["B8"].Value = "SỐ HIỆU";
                worksheet.Cells["C8"].Value = "HỌ VÀ TÊN";
                worksheet.Cells["D8"].Value = "CÔNG VIỆC ĐANG LÀM(CHỨC VỤ)";
                worksheet.Cells["D8"].Style.WrapText = true;
                worksheet.Cells["E8"].Value = "SỐ GIỜ LÀM VIỆC TRONG NGÀY(giờ)";
                worksheet.Cells["E8"].Style.WrapText = true;
                worksheet.Cells["F8"].Value = "SỐ GIỜ LÀM THÊM TRONG NGÀY(giờ)";
                worksheet.Cells["F8"].Style.WrapText = true;
                worksheet.Cells["G8"].Value = "TỔNG SỐ GIỜ LÀM THÊM";
                worksheet.Cells["G8"].Style.WrapText = true;
                worksheet.Cells["H8"].Value = "LÝ DO LÀM THÊM GIỜ";
                worksheet.Cells["H8"].Style.WrapText = true;
                worksheet.Cells["I8"].Value = "CHỮ KÍ CỦA NGƯỜI LAO ĐỘNG";
                worksheet.Cells["I8"].Style.WrapText = true;

                int cell = 9;
                int i = 1;
                foreach(var item in um)
                {
                    worksheet.Cells["A" + cell.ToString()].Value = i.ToString();
                    worksheet.Cells["B" + cell.ToString()].Value = item.empid;
                    worksheet.Cells["C" + cell.ToString()].Value = item.empName;
                    worksheet.Cells["D" + cell.ToString()].Value = item.depchildName;
                    worksheet.Cells["E" + cell.ToString()].Value = item.timeStart + "-" + item.timeEnd;
                    worksheet.Cells["F" + cell.ToString()].Value = "";
                    worksheet.Cells["G" + cell.ToString()].Value = "";
                    worksheet.Cells["H" + cell.ToString()].Value = "";
                    worksheet.Cells["I" + cell.ToString()].Value = "";
                    cell += 1;
                    i++;
                }

                var date = DateTime.Now.ToString("ddMMyyyy");
                worksheet.Cells["A" + (cell + 2).ToString()].Value = "Essons, ngày " + date.Substring(0, 2) + "  tháng " + date.Substring(2, 2) + " năm " + date.Substring(4, 4);
                worksheet.Cells["A" + (cell + 3).ToString()].Value = "Lập biểu";
                worksheet.Cells["C" + (cell + 3).ToString()].Value = "Tổ trưởng";
                worksheet.Cells["E" + (cell + 3).ToString()].Value = "Trưởng bộ phận";
                worksheet.Cells["H" + (cell + 3).ToString()].Value = "Tổng giám đốc";
                worksheet.Cells["A" + (cell + 6).ToString()].Value = ViewBag.message;
                //Set font
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A2"].Style.Font.Size = 14;
                worksheet.Cells["A3"].Style.Font.Size = 14;
                worksheet.Cells["A4"].Style.Font.Size = 14;
                worksheet.Cells["A5"].Style.Font.Size = 14;
                worksheet.Cells["A6"].Style.Font.Size = 14;
                worksheet.Cells["A7"].Style.Font.Size = 14;
                worksheet.Cells["A8"].Style.Font.Size = 14;
                worksheet.Cells["B8"].Style.Font.Size = 13;
                worksheet.Cells["C8"].Style.Font.Size = 13;
                worksheet.Cells["A" + (cell + 6).ToString()].Style.Font.Size = 10;
                var cellHeader = worksheet.Cells["D8:I8"];
                cellHeader.Style.Font.Size = 10;

                //Merge cell
                worksheet.Cells["A1:I1"].Merge = true;
                worksheet.Cells["A2:I2"].Merge = true;
                worksheet.Cells["A3:I3"].Merge = true;
                worksheet.Cells["A4:I4"].Merge = true;
                worksheet.Cells["A5:I5"].Merge = true;
                worksheet.Cells["A6:I6"].Merge = true;
                worksheet.Cells["A7:I7"].Merge = true;
                worksheet.Cells["A"+ (cell + 3).ToString() +":B" + (cell + 3).ToString()].Merge = true;
                worksheet.Cells["C"+ (cell + 3).ToString() +":D" + (cell + 3).ToString()].Merge = true;
                worksheet.Cells["E" + (cell + 3).ToString() +":F" + (cell + 3).ToString()].Merge = true;
                worksheet.Cells["H" + (cell + 3).ToString() +":I" + (cell + 3).ToString()].Merge = true;

                //Set cao rong
                worksheet.Row(8).Height = 111.50;

                worksheet.Column(1).Width = 5.55;
                worksheet.Column(2).Width = 9.91;
                worksheet.Column(3).Width = 24.64;
                worksheet.Column(4).Width = 11.00;
                worksheet.Column(5).Width = 12.09;
                worksheet.Column(6).Width = 12.09;
                worksheet.Column(7).Width = 7.18;
                worksheet.Column(8).Width = 6.82;
                worksheet.Column(9).Width = 10.91;

                //Bold
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A2"].Style.Font.Bold = true;
                worksheet.Cells["A4"].Style.Font.Bold = true;
                worksheet.Cells["A5"].Style.Font.Bold = true;
                worksheet.Cells["A39"].Style.Font.Bold = true;
                worksheet.Cells["A" + (cell + 2).ToString()].Style.Font.Bold = true;
                worksheet.Cells["A" + (cell + 3).ToString()].Style.Font.Bold = true;
                worksheet.Cells["C" + (cell + 3).ToString()].Style.Font.Bold = true;
                worksheet.Cells["E" + (cell + 3).ToString()].Style.Font.Bold = true;
                worksheet.Cells["H" + (cell + 3).ToString()].Style.Font.Bold = true;
                worksheet.Cells["A" + (cell + 6).ToString()].Style.Font.Bold = true;

                // Formatting style all
                using (var range = worksheet.Cells["A1:I" + (cell + 6).ToString()])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Font.Name = "Times New Roman";
                    range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                //Border
                worksheet.Cells["A8:I" + cell.ToString()].Style.Font.Size = 10;
                worksheet.Cells["A8:I" + cell.ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                var cellData = worksheet.Cells["A8:I" + cell.ToString()];
                cellData.Style.Font.Bold = true;
                var border = cellData.Style.Border;
                border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Align
                worksheet.Cells["A1:I8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:I8"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["A" + (cell + 3).ToString() + ":I" + (cell + 3).ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + (cell + 3).ToString() + ":I" + (cell + 3).ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                worksheet.PrinterSettings.Orientation = eOrientation.Portrait;
                worksheet.PrinterSettings.Scale = 90;
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"YCTangCa-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet]
        public async Task<IActionResult> RequestOT()
        {
            getRole();
            ViewBag.Error = "";
            List<EmpModel> um = new List<EmpModel>();
            HttpClient hc = _api.Initial();

            HttpResponseMessage res = await hc.GetAsync($"api/emp/GetEmpInManager?empid={ViewBag.empid}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                um = JsonConvert.DeserializeObject<List<EmpModel>>(results);
            }

            List<TimeWorkModel> em = new List<TimeWorkModel>();
            HttpResponseMessage res2 = await hc.GetAsync($"api/dateoff/GetTimeWorkRequest?empid={ViewBag.empid}&&workdate=0");
            if (res2.IsSuccessStatusCode)
            {
                var results = res2.Content.ReadAsStringAsync().Result;
                em = JsonConvert.DeserializeObject<List<TimeWorkModel>>(results);
            }

            string requestID = "";
            res = await hc.GetAsync($"api/datework/GetDateWorkSTT");
            if (res.IsSuccessStatusCode)
            {
                requestID = res.Content.ReadAsStringAsync().Result;
                if (HttpContext.Session.GetString("requestID") == null || HttpContext.Session.GetString("requestID") == "")
                    HttpContext.Session.SetString("requestID", requestID);
            }

            ViewBag.timeworkList = em;
            ViewBag.emp = um;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestOT(EmpDateWorkModel um)
        {
            getRole();
            HttpClient hc = _api.Initial();
            um.requestID = HttpContext.Session.GetString("requestID");
            um.indat = DateTime.Now.ToString("dd-MM-yyyy");
            um.username = ViewBag.empid;
            var res = await hc.PostAsJsonAsync<EmpDateWorkModel>("api/DateWork/RequestOT", um);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;

                ViewBag.Error = results;
                return PartialView("DisplayError");
            }

            ViewBag.Error = "Lỗi kết nối. Gọi IT";
            return PartialView("DisplayError");
        }

        public async Task<IActionResult> Delete(string requestID, int isOT, string shiftName, int page)
        {
            getRole();
            EmpDateWorkModel em = new EmpDateWorkModel();
            em.requestID = requestID;
            em.isOT = isOT;
            em.shiftName = shiftName;
            HttpClient hc = _api.Initial();
            var res = await hc.PostAsJsonAsync<EmpDateWorkModel>($"api/DateWork/Delete", em);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                if (results == "OK")
                {
                    HttpContext.Session.SetString("notice", "Hủy yêu cầu tăng ca thành công");
                    if (page == 1)
                        return RedirectToAction("Index", "DateWork");
                    else
                        return RedirectToAction("Confirm", "DateWork");
                }
            }
            if (page == 1)
                return RedirectToAction("Index", "DateWork");
            else
                return RedirectToAction("Confirm", "DateWork");
        }

        public async Task<IActionResult> Update(string requestID, int isOT, string shiftName, int page)
        {
            HttpClient hc = _api.Initial();
            EmpDateWorkModel em = new EmpDateWorkModel();
            em.requestID = requestID;
            em.isOT = isOT;
            em.shiftName = shiftName;
            var res = await hc.PostAsJsonAsync<EmpDateWorkModel>($"api/DateWork/Update", em);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                if (results == "OK")
                {
                    HttpContext.Session.SetString("notice", "Cập nhật loại tăng ca thành công");
                    if (page == 1)
                        return RedirectToAction("Index", "DateWork");
                    else
                        return RedirectToAction("Confirm", "DateWork");
                }
            }

            return RedirectToAction("Confirm", "DateWork");
        }
    }
}