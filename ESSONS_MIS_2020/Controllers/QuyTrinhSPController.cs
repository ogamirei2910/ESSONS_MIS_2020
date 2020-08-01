using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class QuyTrinhSPController : Controller
    {
        private readonly IConfiguration _configuration;

        public QuyTrinhSPController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ActionResult QuyTrinh_Insert([FromBody]ChildQuyTrinh model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Insert"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        public ActionResult QuyTrinh_DieuDong([FromBody]MaKeoQuyTrinh model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@MaKeo1", model.MaKeo1));
                    sc.Parameters.Add(
                        new SqlParameter("@Batchno", model.BatchNo));
                    sc.Parameters.Add(
                        new SqlParameter("@Weight", model.Weight));
                    sc.Parameters.Add(
                        new SqlParameter("@indat", DateTime.Now.ToString("yyyy-MM-dd")));
                    sc.Parameters.Add(
                       new SqlParameter("@intime", DateTime.Now.ToString("HH:mm:ss")));
                    sc.Parameters.Add(
                     new SqlParameter("@username", model.username));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "DieuDong"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_KinhDoanh([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayDuyet", model.NgayDuyet));
                    sc.Parameters.Add(
                        new SqlParameter("@PhienBan1", model.PhienBan1));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayPhatHanh1", model.NgayPhatHanh1));
                    sc.Parameters.Add(
                       new SqlParameter("@PhienBan2", model.PhienBan2));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayPhatHanh2", model.NgayPhatHanh2));
                    sc.Parameters.Add(
                       new SqlParameter("@PhienBan3", model.PhienBan3));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayPhatHanh3", model.NgayPhatHanh3));
                    sc.Parameters.Add(
                       new SqlParameter("@SoLoKhuonKH", model.SoLoKhuonKH));
                    sc.Parameters.Add(
                       new SqlParameter("@CoRutTheoDonKhuon", model.CoRutTheoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@UocTinhKLSP", model.UocTinhKLSP));
                    sc.Parameters.Add(
                        new SqlParameter("@ThoiGianHoanTat", model.ThoiGianHoanTat));
                    sc.Parameters.Add(
                       new SqlParameter("@LoaiKhuonMau", model.LoaiKhuonMau));

                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "KinhDoanh"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_KHSX([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@CodeKH", model.CodeKH));
                    sc.Parameters.Add(
                        new SqlParameter("@TenQU", model.TenQU));
                    sc.Parameters.Add(
                        new SqlParameter("@MaKhuonTW", model.MaKhuonTW));
                    sc.Parameters.Add(
                        new SqlParameter("@QuyCachKH", model.QuyCachKH));
                    sc.Parameters.Add(
                       new SqlParameter("@QuyCachEssons", model.QuyCachEssons));
                    sc.Parameters.Add(
                        new SqlParameter("@ChatKeo", model.ChatKeo));
                    sc.Parameters.Add(
                        new SqlParameter("@Mau", model.Mau));
                    sc.Parameters.Add(
                        new SqlParameter("@MaKeo1", model.MaKeo1));
                    sc.Parameters.Add(
                        new SqlParameter("@MaKeo2", model.MaKeo2));
                    sc.Parameters.Add(
                       new SqlParameter("@PantoneMau", model.PantoneMau));
                    sc.Parameters.Add(
                       new SqlParameter("@GhiChuKHSX", model.GhiChuKHSX));
                    sc.Parameters.Add(
                       new SqlParameter("@GocNL", model.GocNL));
                    sc.Parameters.Add(
                       new SqlParameter("@SoPCSMoiBo", model.SoPCSMoiBo));
                    sc.Parameters.Add(
                       new SqlParameter("@PhaKeo", model.PhaKeo));
                    sc.Parameters.Add(
                      new SqlParameter("@NgayGiaoMau", model.NgayGiaoMau));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayKHApprovedMau", model.NgayKHApprovedMau));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "KHSX"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_Kho([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat1MK1", model.HoaChat1MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC1MK1", model.KhoiLuongHC1MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat2MK1", model.HoaChat2MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC2MK1", model.KhoiLuongHC2MK1));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat3MK1", model.HoaChat3MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC3MK1", model.KhoiLuongHC3MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat4MK1", model.HoaChat4MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC4MK1", model.KhoiLuongHC4MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat5MK1", model.HoaChat5MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC5MK1", model.KhoiLuongHC5MK1));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat6MK1", model.HoaChat6MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC6MK1", model.KhoiLuongHC6MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat7MK1", model.HoaChat7MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC7MK1", model.KhoiLuongHC7MK1));
                    sc.Parameters.Add(
                      new SqlParameter("@HoaChat8MK1", model.HoaChat8MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC8MK1", model.KhoiLuongHC8MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat9MK1", model.HoaChat9MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC9MK1", model.KhoiLuongHC9MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat10MK1", model.HoaChat10MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC10MK1", model.KhoiLuongHC10MK1));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat11MK1", model.HoaChat11MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC11MK1", model.KhoiLuongHC11MK1));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat12MK1", model.HoaChat12MK1));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC12MK1", model.KhoiLuongHC12MK1));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat1MK1KH", model.HoaChat1MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC1MK1KH", model.KhoiLuongHC1MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat2MK1KH", model.HoaChat2MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC2MK1KH", model.KhoiLuongHC2MK1KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat3MK1KH", model.HoaChat3MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC3MK1KH", model.KhoiLuongHC3MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat4MK1KH", model.HoaChat4MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC4MK1KH", model.KhoiLuongHC4MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat5MK1KH", model.HoaChat5MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC5MK1KH", model.KhoiLuongHC5MK1KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat6MK1KH", model.HoaChat6MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC6MK1KH", model.KhoiLuongHC6MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat7MK1KH", model.HoaChat7MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC7MK1KH", model.KhoiLuongHC7MK1KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat8MK1KH", model.HoaChat8MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC8MK1KH", model.KhoiLuongHC8MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat9MK1KH", model.HoaChat9MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC9MK1KH", model.KhoiLuongHC9MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat10MK1KH", model.HoaChat10MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuong10HC5MK1KH", model.KhoiLuongHC10MK1KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat11MK1KH", model.HoaChat11MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC11MK1KH", model.KhoiLuongHC11MK1KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat12MK1KH", model.HoaChat12MK1KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC12MK1KH", model.KhoiLuongHC12MK1KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat1MK2", model.HoaChat1MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC1MK2", model.KhoiLuongHC1MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat2MK2", model.HoaChat2MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC2MK2", model.KhoiLuongHC2MK2));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat3MK2", model.HoaChat3MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC3MK2", model.KhoiLuongHC3MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat4MK2", model.HoaChat4MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC4MK2", model.KhoiLuongHC4MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat5MK2", model.HoaChat5MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC5MK2", model.KhoiLuongHC5MK2));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat6MK2", model.HoaChat6MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC6MK2", model.KhoiLuongHC6MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat7MK2", model.HoaChat7MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC7MK2", model.KhoiLuongHC7MK2));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat8MK2", model.HoaChat8MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC8MK2", model.KhoiLuongHC8MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat9MK2", model.HoaChat9MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC9MK2", model.KhoiLuongHC9MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat10MK2", model.HoaChat10MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC10MK2", model.KhoiLuongHC10MK2));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat11MK2", model.HoaChat11MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC11MK2", model.KhoiLuongHC11MK2));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat12MK2", model.HoaChat12MK2));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC12MK2", model.KhoiLuongHC12MK2));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat1MK2KH", model.HoaChat1MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC1MK2KH", model.KhoiLuongHC1MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat2MK2KH", model.HoaChat2MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC2MK2KH", model.KhoiLuongHC2MK2KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat3MK2KH", model.HoaChat3MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC3MK2KH", model.KhoiLuongHC3MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat4MK2KH", model.HoaChat4MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC4MK2KH", model.KhoiLuongHC4MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat5MK2KH", model.HoaChat5MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC5MK2KH", model.KhoiLuongHC5MK2KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat6MK2KH", model.HoaChat6MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC6MK2KH", model.KhoiLuongHC6MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat7MK2KH", model.HoaChat7MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC7MK2KH", model.KhoiLuongHC7MK2KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat8MK2KH", model.HoaChat8MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC8MK2KH", model.KhoiLuongHC8MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat9MK2KH", model.HoaChat9MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC9MK2KH", model.KhoiLuongHC9MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat10MK2KH", model.HoaChat10MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC10MK2KH", model.KhoiLuongHC10MK2KH));
                    sc.Parameters.Add(
                        new SqlParameter("@HoaChat11MK2KH", model.HoaChat11MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC11MK2KH", model.KhoiLuongHC11MK2KH));
                    sc.Parameters.Add(
                         new SqlParameter("@HoaChat12MK2KH", model.HoaChat12MK2KH));
                    sc.Parameters.Add(
                       new SqlParameter("@KhoiLuongHC12MK2KH", model.KhoiLuongHC12MK2KH));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "Kho"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_ThuMua([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayDuyetPhoiThep", model.NgayDuyetPhoiThep));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayMuaPhoiThep", model.NgayMuaPhoiThep));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayMuaPhoiThep2", model.NgayMuaPhoiThep2));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayMuaPhoiThep3", model.NgayMuaPhoiThep3));
                    sc.Parameters.Add(
                        new SqlParameter("@TenQU", model.TenQU));
                    sc.Parameters.Add(
                        new SqlParameter("@SoCTMuaPhoi", model.SoCTMuaPhoi));
                    sc.Parameters.Add(
                        new SqlParameter("@NCCPhoiThep", model.NCCPhoiThep));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "ThuMua"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_CTK([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@LoaiThepKhuon", model.LoaiThepKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@KichThuocKhuon", model.KichThuocKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@SoTam", model.SoTam));
                    sc.Parameters.Add(
                        new SqlParameter("@CoRutThucTe", model.CoRutThucTe));
                    sc.Parameters.Add(
                        new SqlParameter("@LoaiKhuon", model.LoaiKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@PhuKienCTK", model.PhuKienCTK));
                    sc.Parameters.Add(
                        new SqlParameter("@PPXuLyBTP", model.PPXuLyBTP));
                    sc.Parameters.Add(
                        new SqlParameter("@NgaySXKhuon", model.NgaySXKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayHoanThanhDuKien", model.NgayHoanThanhDuKien));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayHoanThanh", model.NgayHoanThanh));
                    sc.Parameters.Add(
                        new SqlParameter("@SuaChiTietLan1", model.SuaChiTietLan1));
                    sc.Parameters.Add(
                        new SqlParameter("@SuaChiTietLan2", model.SuaChiTietLan2));
                    sc.Parameters.Add(
                       new SqlParameter("@SuaChiTietLan3", model.SuaChiTietLan3));
                    sc.Parameters.Add(
                       new SqlParameter("@SuaChiTietLan4", model.SuaChiTietLan4));
                    sc.Parameters.Add(
                        new SqlParameter("@NgaySuaChiTietLan1", model.NgaySuaChiTietLan1));
                    sc.Parameters.Add(
                        new SqlParameter("@NgaySuaChiTietLan2", model.NgaySuaChiTietLan2));
                    sc.Parameters.Add(
                       new SqlParameter("@NgaySuaChiTietLan3", model.NgaySuaChiTietLan3));
                    sc.Parameters.Add(
                       new SqlParameter("@NgaySuaChiTietLan4", model.NgaySuaChiTietLan4));
                    sc.Parameters.Add(
                       new SqlParameter("@GhiChuKHSX", model.GhiChuKHSX));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "CTK"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_BTK([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayNhanKhuon", model.NgayNhanKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@ViTriKhuon", model.ViTriKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@SoKhuonHienCo", model.SoKhuonHienCo));
                    sc.Parameters.Add(
                        new SqlParameter("@ViTriPhuKien", model.ViTriPhuKien));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "BTK"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_CBNL([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@XuLyBeMat", model.XuLyBeMat));
                    sc.Parameters.Add(
                        new SqlParameter("@PPCheBienNL", model.PPCheBienNL));
                    sc.Parameters.Add(
                       new SqlParameter("@ThoiGianTron", model.ThoiGianTron));
                    sc.Parameters.Add(
                        new SqlParameter("@TongThoiGianCan", model.TongThoiGianCan));
                    sc.Parameters.Add(
                        new SqlParameter("@ChiTietCaiTien", model.ChiTietCaiTien));
                    sc.Parameters.Add(
                       new SqlParameter("@PPCat", model.PPCat));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "CBNL"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_DHE([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@DoDay", model.DoDay));
                    sc.Parameters.Add(
                       new SqlParameter("@DoDay2", model.DoDay2));
                    sc.Parameters.Add(
                        new SqlParameter("@DoDay3", model.DoDay3));
                    sc.Parameters.Add(
                        new SqlParameter("@DoDay4", model.DoDay4));
                    sc.Parameters.Add(
                        new SqlParameter("@ChieuDaiMin", model.ChieuDaiMin));
                    sc.Parameters.Add(
                       new SqlParameter("@ChieuDaiMin2", model.ChieuDaiMin2));
                    sc.Parameters.Add(
                        new SqlParameter("@ChieuDaiMin3", model.ChieuDaiMin3));
                    sc.Parameters.Add(
                        new SqlParameter("@ChieuDaiMin4", model.ChieuDaiMin4));
                    sc.Parameters.Add(
                       new SqlParameter("@ChieuDaiMax", model.ChieuDaiMax));
                    sc.Parameters.Add(
                        new SqlParameter("@ChieuDaiMax2", model.ChieuDaiMax2));
                    sc.Parameters.Add(
                        new SqlParameter("@ChieuDaiMax3", model.ChieuDaiMax3));
                    sc.Parameters.Add(
                       new SqlParameter("@ChieuDaiMax4", model.ChieuDaiMax4));
                    sc.Parameters.Add(
                       new SqlParameter("@TLSoiMin", model.TLSoiMin));
                    sc.Parameters.Add(
                     new SqlParameter("@TLSoiMin2", model.TLSoiMin2));
                    sc.Parameters.Add(
                     new SqlParameter("@TLSoiMin3", model.TLSoiMin3));
                    sc.Parameters.Add(
                     new SqlParameter("@TLSoiMin4", model.TLSoiMin4));
                    sc.Parameters.Add(
                       new SqlParameter("@TLSoiMax", model.TLSoiMax));
                    sc.Parameters.Add(
                       new SqlParameter("@TLSoiMax2", model.TLSoiMax2));
                    sc.Parameters.Add(
                       new SqlParameter("@TLSoiMax3", model.TLSoiMax3));
                    sc.Parameters.Add(
                       new SqlParameter("@TLSoiMax4", model.TLSoiMax4));
                    sc.Parameters.Add(
                      new SqlParameter("@SoSoi", model.SoSoi));
                    sc.Parameters.Add(
                      new SqlParameter("@SoSoi2", model.SoSoi2));
                    sc.Parameters.Add(
                      new SqlParameter("@SoSoi3", model.SoSoi3));
                    sc.Parameters.Add(
                      new SqlParameter("@SoSoi4", model.SoSoi4));
                    sc.Parameters.Add(
                      new SqlParameter("@PPDatKeo", model.PPDatKeo));
                    sc.Parameters.Add(
                      new SqlParameter("@TheTichBom", model.TheTichBom));
                    sc.Parameters.Add(
                      new SqlParameter("@CachDat", model.CachDat));
                    sc.Parameters.Add(
                      new SqlParameter("@SoLoKhuon", model.SoLoKhuon));
                    sc.Parameters.Add(
                      new SqlParameter("@SoLoThucLam", model.SoLoThucLam));
                    sc.Parameters.Add(
                      new SqlParameter("@Tren5", model.Tren5));
                    sc.Parameters.Add(
                      new SqlParameter("@Duoi5", model.Duoi5));
                    sc.Parameters.Add(
                      new SqlParameter("@LucEp", model.LucEp));
                    sc.Parameters.Add(
                      new SqlParameter("@SoLanThoatKhi", model.SoLanThoatKhi));
                    sc.Parameters.Add(
                      new SqlParameter("@TongTLSat", model.TongTLSat));
                    sc.Parameters.Add(
                      new SqlParameter("@KLSPThucTe", model.KLSPThucTe));
                    sc.Parameters.Add(
                      new SqlParameter("@ThoiGianLuuHoa", model.ThoiGianLuuHoa));
                    sc.Parameters.Add(
                      new SqlParameter("@ThoiGianHoanTat1k", model.ThoiGianHoanTat1k));
                    sc.Parameters.Add(
                      new SqlParameter("@ChatLuong", model.ChatLuong));
                    sc.Parameters.Add(
                      new SqlParameter("@XuLyDinhDH", model.XuLyDinhDH));
                    sc.Parameters.Add(
                      new SqlParameter("@XuLyBTPTaiDH", model.XuLyBTPTaiDH));
                    sc.Parameters.Add(
                      new SqlParameter("@DCBocTay", model.DCBocTay));
                    sc.Parameters.Add(
                     new SqlParameter("@DCXuLyBeMat", model.DCXuLyBeMat));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "DHE"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_QCKN([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayKTMau", model.NgayKTMau));
                    sc.Parameters.Add(
                       new SqlParameter("@KTTruocLH", model.KTTruocLH));
                    sc.Parameters.Add(
                       new SqlParameter("@KTSauLH", model.KTSauLH));
                    sc.Parameters.Add(
                      new SqlParameter("@KTXuatMau", model.KTXuatMau));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "QCKN"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_BT([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@PPBocTachBTM", model.PPBocTachBTM));
                    sc.Parameters.Add(
                       new SqlParameter("@XuLyBeMatBTM", model.XuLyBeMatBTM));
                    sc.Parameters.Add(
                        new SqlParameter("@ThoiGian", model.ThoiGian));
                    sc.Parameters.Add(
                        new SqlParameter("@TocDo", model.TocDo));
                    sc.Parameters.Add(
                        new SqlParameter("@KL1LanBan", model.KL1LanBan));
                    sc.Parameters.Add(
                       new SqlParameter("@LoaiCatBi", model.LoaiCatBi));
                    sc.Parameters.Add(
                        new SqlParameter("@NhietDo", model.NhietDo));
                    sc.Parameters.Add(
                        new SqlParameter("@ChatLuongBT", model.ChatLuongBT));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "BT"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_LH2([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@PPLamSach", model.PPLamSach));
                    sc.Parameters.Add(
                       new SqlParameter("@XuLyBeMatLH", model.XuLyBeMatLH));
                    sc.Parameters.Add(
                        new SqlParameter("@NhietDoLH", model.NhietDoLH));
                    sc.Parameters.Add(
                        new SqlParameter("@ThoiGianLH", model.ThoiGianLH));

                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "LH2"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_MH([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@KL1LanMai", model.KL1LanMai));
                    sc.Parameters.Add(
                       new SqlParameter("@ThoiGianMai", model.ThoiGianMai));
                    sc.Parameters.Add(
                      new SqlParameter("@LoaiDaMai", model.LoaiDaMai));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "MH"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_QTK([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@TronBot", model.TronBot));
                    sc.Parameters.Add(
                       new SqlParameter("@ChamSon", model.ChamSon));
                    sc.Parameters.Add(
                        new SqlParameter("@GanLinhKien", model.GanLinhKien));
                    sc.Parameters.Add(
                        new SqlParameter("@PhuTeflon", model.PhuTeflon));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "QTK"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_KM([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@BaviaKH", model.BaviaKH));
                    sc.Parameters.Add(
                       new SqlParameter("@BaviaCaiDat", model.BaviaCaiDat));
                    sc.Parameters.Add(
                        new SqlParameter("@YeuCauKiemMay", model.YeuCauKiemMay));
                    sc.Parameters.Add(
                        new SqlParameter("@XuLyBeMatKM", model.XuLyBeMatKM));
                    sc.Parameters.Add(
                        new SqlParameter("@NgoaiQuanKM", model.NgoaiQuanKM));
                    sc.Parameters.Add(
                       new SqlParameter("@TyLeNG", model.TyLeNG));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "KM"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_DG([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@TrongLuong1SP", model.TrongLuong1SP));
                    sc.Parameters.Add(
                       new SqlParameter("@TrongLuongLinhKien", model.TrongLuongLinhKien));
                    sc.Parameters.Add(
                        new SqlParameter("@SL1boc", model.SL1boc));
                    sc.Parameters.Add(
                        new SqlParameter("@KichThuocBoc", model.KichThuocBoc));
                    sc.Parameters.Add(
                        new SqlParameter("@SL1hop", model.SL1hop));
                    sc.Parameters.Add(
                       new SqlParameter("@KichThuocThung", model.KichThuocThung));
                    sc.Parameters.Add(
                        new SqlParameter("@SL1thung", model.SL1thung));
                    sc.Parameters.Add(
                        new SqlParameter("@PPDongBoc", model.PPDongBoc));
                    sc.Parameters.Add(
                        new SqlParameter("@PPDongHoi", model.PPDongHoi));
                    sc.Parameters.Add(
                       new SqlParameter("@GhiChuKhacDG", model.GhiChuKhacDG));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "DG"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }
        public ActionResult QuyTrinh_Update_YCCL([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", model.SoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@KichThuoc", model.KichThuoc));
                    sc.Parameters.Add(
                       new SqlParameter("@NgoaiQuan", model.NgoaiQuan));
                    sc.Parameters.Add(
                        new SqlParameter("@DongGoi", model.DongGoi));
                    sc.Parameters.Add(
                        new SqlParameter("@Khac", model.Khac));
                    sc.Parameters.Add(
                        new SqlParameter("@GhiChu", model.GhiChu));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", "YCCL"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        [HttpGet]
        public QuyTrinhModel QuyTrinh_Get(string SoDonKhuon, string codeSP)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            QuyTrinhModel em = new QuyTrinhModel();
            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoDonKhuon", SoDonKhuon));
                    sc.Parameters.Add(
                       new SqlParameter("@CodeSP", codeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetSoDonKhuon"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        em.SoDonKhuon = sdr["SoDonKhuon"].ToString();
                        if (sdr["NgayDuyet"] is null || sdr["NgayDuyet"].ToString() == "")
                            em.NgayDuyet = "";
                        else
                            em.NgayDuyet = DateTime.ParseExact(sdr["NgayDuyet"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan1 = sdr["PhienBan1"].ToString();
                        if (sdr["NgayPhatHanh1"] is null || sdr["NgayPhatHanh1"].ToString() == "")
                            em.NgayPhatHanh1 = "";                       
                        else
                            em.NgayPhatHanh1 = DateTime.ParseExact(sdr["NgayPhatHanh1"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan2 = sdr["PhienBan2"].ToString();
                        if (sdr["NgayPhatHanh2"] is null || sdr["NgayPhatHanh2"].ToString() == "")
                            em.NgayPhatHanh2 = "";
                        else
                            em.NgayPhatHanh2 = DateTime.ParseExact(sdr["NgayPhatHanh2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan3 = sdr["PhienBan3"].ToString();
                        if (sdr["NgayPhatHanh3"] is null || sdr["NgayPhatHanh3"].ToString() == "")
                            em.NgayPhatHanh3 = "";
                        else
                            em.NgayPhatHanh3 = DateTime.ParseExact(sdr["NgayPhatHanh3"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                     .ToString("MM-dd-yyyy");
                        if (sdr["SoLoKhuonKH"] is null || sdr["SoLoKhuonKH"].ToString() == "")
                            em.SoLoKhuonKH = 0;
                        else
                            em.SoLoKhuonKH = int.Parse(sdr["SoLoKhuonKH"].ToString());
                        em.CoRutTheoDonKhuon = sdr["CoRutTheoDonKhuon"].ToString();
                        em.UocTinhKLSP = sdr["UocTinhKLSP"].ToString();
                        em.ThoiGianHoanTat1k = sdr["ThoiGianHoanTat1k"].ToString();
                        em.LoaiKhuonMau = sdr["LoaiKhuonMau"].ToString();
                        em.CodeKH = sdr["CodeKH"].ToString();
                        em.CodeSP = sdr["CodeSP"].ToString();
                        em.TenQU = sdr["TenQU"].ToString();
                        em.MaKhuonTW = sdr["MaKhuonTW"].ToString();
                        em.QuyCachKH = sdr["QuyCachKH"].ToString();
                        em.QuyCachEssons = sdr["QuyCachEssons"].ToString();
                        em.ChatKeo = sdr["ChatKeo"].ToString();
                        em.Mau = sdr["Mau"].ToString();
                        em.MaKeo1 = sdr["MaKeo1"].ToString();
                        em.MaKeo2 = sdr["MaKeo2"].ToString();
                        em.PantoneMau = sdr["PantoneMau"].ToString();
                        em.GhiChuKHSX = sdr["GhiChuKHSX"].ToString();
                        em.GocNL = sdr["GocNL"].ToString();
                        if (sdr["SoPCSMoiBo"] is null || sdr["SoPCSMoiBo"].ToString() == "")
                            em.SoPCSMoiBo = 0;
                        else
                            em.SoPCSMoiBo = int.Parse(sdr["SoPCSMoiBo"].ToString());
                        em.PhaKeo = sdr["PhaKeo"].ToString();
                        if (sdr["NgayGiaoMau"] is null || sdr["NgayGiaoMau"].ToString() == "")
                            em.NgayGiaoMau = "";                        
                        else
                            em.NgayGiaoMau = DateTime.ParseExact(sdr["NgayGiaoMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        if (sdr["NgayKHApprovedMau"] is null || sdr["NgayKHApprovedMau"].ToString() == "")
                            em.NgayKHApprovedMau = "";                       
                        else
                            em.NgayKHApprovedMau = DateTime.ParseExact(sdr["NgayKHApprovedMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.MaKeoKho1 = sdr["MaKeo1"].ToString();
                        em.HoaChat1MK1 = sdr["HoaChat1MK1"].ToString();
                        em.KhoiLuongHC1MK1 = sdr["KhoiLuongHC1MK1"].ToString();
                        em.HoaChat2MK1 = sdr["HoaChat2MK1"].ToString();
                        em.KhoiLuongHC2MK1 = sdr["KhoiLuongHC2MK1"].ToString();
                        em.HoaChat3MK1 = sdr["HoaChat3MK1"].ToString();
                        em.KhoiLuongHC3MK1 = sdr["KhoiLuongHC3MK1"].ToString();
                        em.HoaChat4MK1 = sdr["HoaChat4MK1"].ToString();
                        em.KhoiLuongHC4MK1 = sdr["KhoiLuongHC4MK1"].ToString();
                        em.HoaChat5MK1 = sdr["HoaChat5MK1"].ToString();
                        em.KhoiLuongHC5MK1 = sdr["KhoiLuongHC5MK1"].ToString();
                        em.HoaChat6MK1 = sdr["HoaChat6MK1"].ToString();
                        em.KhoiLuongHC6MK1 = sdr["KhoiLuongHC6MK1"].ToString();
                        em.HoaChat7MK1 = sdr["HoaChat7MK1"].ToString();
                        em.KhoiLuongHC7MK1 = sdr["KhoiLuongHC7MK1"].ToString();
                        em.HoaChat8MK1 = sdr["HoaChat8MK1"].ToString();
                        em.KhoiLuongHC8MK1 = sdr["KhoiLuongHC8MK1"].ToString();
                        em.HoaChat9MK1 = sdr["HoaChat9MK1"].ToString();
                        em.KhoiLuongHC9MK1 = sdr["KhoiLuongHC9MK1"].ToString();
                        em.HoaChat10MK1 = sdr["HoaChat10MK1"].ToString();
                        em.KhoiLuongHC10MK1 = sdr["KhoiLuongHC10MK1"].ToString();
                        em.HoaChat11MK1 = sdr["HoaChat11MK1"].ToString();
                        em.KhoiLuongHC11MK1 = sdr["KhoiLuongHC11MK1"].ToString();
                        em.HoaChat12MK1 = sdr["HoaChat12MK1"].ToString();
                        em.KhoiLuongHC12MK1 = sdr["KhoiLuongHC12MK1"].ToString();
                        em.HoaChat1MK1KH = sdr["HoaChat1MK1KH"].ToString();
                        em.KhoiLuongHC1MK1KH = sdr["KhoiLuongHC1MK1KH"].ToString();
                        em.HoaChat2MK1KH = sdr["HoaChat2MK1KH"].ToString();
                        em.KhoiLuongHC2MK1KH = sdr["KhoiLuongHC2MK1KH"].ToString();
                        em.HoaChat3MK1KH = sdr["HoaChat3MK1KH"].ToString();
                        em.KhoiLuongHC3MK1KH = sdr["KhoiLuongHC3MK1KH"].ToString();
                        em.HoaChat4MK1KH = sdr["HoaChat4MK1KH"].ToString();
                        em.KhoiLuongHC4MK1KH = sdr["KhoiLuongHC4MK1KH"].ToString();
                        em.HoaChat5MK1KH = sdr["HoaChat5MK1KH"].ToString();
                        em.KhoiLuongHC5MK1KH = sdr["KhoiLuongHC5MK1KH"].ToString();
                        em.HoaChat6MK1KH = sdr["HoaChat6MK1KH"].ToString();
                        em.KhoiLuongHC6MK1KH = sdr["KhoiLuongHC6MK1KH"].ToString();
                        em.HoaChat7MK1KH = sdr["HoaChat7MK1KH"].ToString();
                        em.KhoiLuongHC7MK1KH = sdr["KhoiLuongHC7MK1KH"].ToString();
                        em.HoaChat8MK1KH = sdr["HoaChat8MK1KH"].ToString();
                        em.KhoiLuongHC8MK1KH = sdr["KhoiLuongHC8MK1KH"].ToString();
                        em.HoaChat9MK1KH = sdr["HoaChat9MK1KH"].ToString();
                        em.KhoiLuongHC9MK1KH = sdr["KhoiLuongHC9MK1KH"].ToString();
                        em.HoaChat10MK1KH = sdr["HoaChat10MK1KH"].ToString();
                        em.KhoiLuongHC10MK1KH = sdr["KhoiLuongHC10MK1KH"].ToString();
                        em.HoaChat11MK1KH = sdr["HoaChat11MK1KH"].ToString();
                        em.KhoiLuongHC11MK1KH = sdr["KhoiLuongHC11MK1KH"].ToString();
                        em.HoaChat12MK1KH = sdr["HoaChat12MK1KH"].ToString();
                        em.KhoiLuongHC12MK1KH = sdr["KhoiLuongHC12MK1KH"].ToString();
                        em.MaKeoKho2 = sdr["MaKeo2"].ToString();
                        em.HoaChat1MK2 = sdr["HoaChat1MK2"].ToString();
                        em.KhoiLuongHC1MK2 = sdr["KhoiLuongHC1MK2"].ToString();
                        em.HoaChat2MK2 = sdr["HoaChat2MK2"].ToString();
                        em.KhoiLuongHC2MK2 = sdr["KhoiLuongHC2MK2"].ToString();
                        em.HoaChat3MK2 = sdr["HoaChat3MK2"].ToString();
                        em.KhoiLuongHC3MK2 = sdr["KhoiLuongHC3MK2"].ToString();
                        em.HoaChat4MK2 = sdr["HoaChat4MK2"].ToString();
                        em.KhoiLuongHC4MK2 = sdr["KhoiLuongHC4MK2"].ToString();
                        em.HoaChat5MK2 = sdr["HoaChat5MK2"].ToString();
                        em.KhoiLuongHC5MK2 = sdr["KhoiLuongHC5MK2"].ToString();
                        em.HoaChat6MK2 = sdr["HoaChat6MK2"].ToString();
                        em.KhoiLuongHC6MK2 = sdr["KhoiLuongHC6MK2"].ToString();
                        em.HoaChat7MK2 = sdr["HoaChat7MK2"].ToString();
                        em.KhoiLuongHC7MK2 = sdr["KhoiLuongHC7MK2"].ToString();
                        em.HoaChat8MK2 = sdr["HoaChat8MK2"].ToString();
                        em.KhoiLuongHC8MK2 = sdr["KhoiLuongHC8MK2"].ToString();
                        em.HoaChat9MK2 = sdr["HoaChat9MK2"].ToString();
                        em.KhoiLuongHC9MK2 = sdr["KhoiLuongHC9MK2"].ToString();
                        em.HoaChat10MK2 = sdr["HoaChat10MK2"].ToString();
                        em.KhoiLuongHC10MK2 = sdr["KhoiLuongHC10MK2"].ToString();
                        em.HoaChat11MK2 = sdr["HoaChat11MK2"].ToString();
                        em.KhoiLuongHC11MK2 = sdr["KhoiLuongHC11MK2"].ToString();
                        em.HoaChat12MK2 = sdr["HoaChat12MK2"].ToString();
                        em.KhoiLuongHC12MK2 = sdr["KhoiLuongHC12MK2"].ToString();
                        em.HoaChat1MK2KH = sdr["HoaChat1MK2KH"].ToString();
                        em.KhoiLuongHC1MK2KH = sdr["KhoiLuongHC1MK2KH"].ToString();
                        em.HoaChat2MK2KH = sdr["HoaChat2MK2KH"].ToString();
                        em.KhoiLuongHC2MK2KH = sdr["KhoiLuongHC2MK2KH"].ToString();
                        em.HoaChat3MK2KH = sdr["HoaChat3MK2KH"].ToString();
                        em.KhoiLuongHC3MK2KH = sdr["KhoiLuongHC3MK2KH"].ToString();
                        em.HoaChat4MK2KH = sdr["HoaChat4MK2KH"].ToString();
                        em.KhoiLuongHC4MK2KH = sdr["KhoiLuongHC4MK2KH"].ToString();
                        em.HoaChat5MK2KH = sdr["HoaChat5MK2KH"].ToString();
                        em.KhoiLuongHC5MK2KH = sdr["KhoiLuongHC5MK2KH"].ToString();
                        em.HoaChat6MK2KH = sdr["HoaChat6MK2KH"].ToString();
                        em.KhoiLuongHC6MK2KH = sdr["KhoiLuongHC6MK2KH"].ToString();
                        em.HoaChat7MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC7MK2KH"].ToString();
                        em.HoaChat8MK2KH = sdr["HoaChat8MK2KH"].ToString();
                        em.KhoiLuongHC8MK2KH = sdr["KhoiLuongHC8MK2KH"].ToString();
                        em.HoaChat9MK2KH = sdr["HoaChat9MK2KH"].ToString();
                        em.KhoiLuongHC9MK2KH = sdr["KhoiLuongHC9MK2KH"].ToString();
                        em.HoaChat10MK2KH = sdr["HoaChat10MK2KH"].ToString();
                        em.KhoiLuongHC10MK2KH = sdr["KhoiLuongHC10MK2KH"].ToString();
                        em.HoaChat11MK2KH = sdr["HoaChat11MK2KH"].ToString();
                        em.KhoiLuongHC11MK2KH = sdr["KhoiLuongHC11MK2KH"].ToString();
                        em.HoaChat12MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC12MK2KH"].ToString();
                        if (sdr["NgayDuyetPhoiThep"] is null || sdr["NgayDuyetPhoiThep"].ToString() == "")
                            em.NgayDuyetPhoiThep = "";
                        else
                            em.NgayDuyetPhoiThep = DateTime.ParseExact(sdr["NgayDuyetPhoiThep"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayMuaPhoiThep"] is null || sdr["NgayMuaPhoiThep"].ToString() == "")
                            em.NgayMuaPhoiThep = "";
                        else
                            em.NgayMuaPhoiThep = DateTime.ParseExact(sdr["NgayMuaPhoiThep"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayMuaPhoiThep2"] is null || sdr["NgayMuaPhoiThep2"].ToString() == "")
                            em.NgayMuaPhoiThep2 = "";
                        else
                            em.NgayMuaPhoiThep2 = DateTime.ParseExact(sdr["NgayMuaPhoiThep2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayMuaPhoiThep3"] is null || sdr["NgayMuaPhoiThep3"].ToString() == "")
                            em.NgayMuaPhoiThep3 = "";
                        else
                            em.NgayMuaPhoiThep3 = DateTime.ParseExact(sdr["NgayMuaPhoiThep3"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        em.SoCTMuaPhoi = sdr["SoCTMuaPhoi"].ToString();
                        em.NCCPhoiThep = sdr["NCCPhoiThep"].ToString();
                        em.LoaiThepKhuon = sdr["LoaiThepKhuon"].ToString();
                        em.KichThuocKhuon = sdr["KichThuocKhuon"].ToString();
                        if (sdr["SoTam"] is null || sdr["SoTam"].ToString() == "")
                            em.SoTam = 0;
                        else
                            em.SoTam = int.Parse(sdr["SoTam"].ToString());
                        em.CoRutThucTe = sdr["%CoRutThucTe"].ToString();
                        em.LoaiKhuon = sdr["LoaiKhuon"].ToString();
                        em.PhuKienCTK = sdr["PhuKienCTK"].ToString();
                        em.PPXuLyBTP = sdr["PPXuLyBTP"].ToString();
                        if (sdr["NgaySXKhuon"] is null || sdr["NgaySXKhuon"].ToString() == "")
                            em.NgaySXKhuon = "";
                        else
                            em.NgaySXKhuon = DateTime.ParseExact(sdr["NgaySXKhuon"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayHoanThanhDuKien"] is null || sdr["NgayHoanThanhDuKien"].ToString() == "")
                            em.NgayHoanThanhDuKien = "";
                        else
                            em.NgayHoanThanhDuKien = DateTime.ParseExact(sdr["NgayHoanThanhDuKien"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayHoanThanh"] is null || sdr["NgayHoanThanh"].ToString() == "")
                            em.NgayHoanThanh = "";
                        else
                            em.NgayHoanThanh = DateTime.ParseExact(sdr["NgayHoanThanh"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan1 = sdr["SuaChiTietLan1"].ToString();
                        if (sdr["NgaySuaChiTietLan1"] is null || sdr["NgaySuaChiTietLan1"].ToString() == "")
                            em.NgaySuaChiTietLan1 = "";
                        else
                            em.NgaySuaChiTietLan1 = DateTime.ParseExact(sdr["NgaySuaChiTietLan1"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan2 = sdr["SuaChiTietLan2"].ToString();
                        if (sdr["NgaySuaChiTietLan2"] is null || sdr["NgaySuaChiTietLan2"].ToString() == "")
                            em.NgaySuaChiTietLan2 = "";
                        else
                            em.NgaySuaChiTietLan2 = DateTime.ParseExact(sdr["NgaySuaChiTietLan2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan3 = sdr["SuaChiTietLan3"].ToString();
                        if (sdr["NgaySuaChiTietLan3"] is null || sdr["NgaySuaChiTietLan3"].ToString() == "")
                            em.NgaySuaChiTietLan3 = "";
                        else
                            em.NgaySuaChiTietLan3 = DateTime.ParseExact(sdr["NgaySuaChiTietLan3"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan4 = sdr["SuaChiTietLan4"].ToString();
                        if (sdr["NgaySuaChiTietLan4"] is null || sdr["NgaySuaChiTietLan4"].ToString() == "")
                            em.NgaySuaChiTietLan4 = "";
                        else
                            em.NgaySuaChiTietLan4 = DateTime.ParseExact(sdr["NgaySuaChiTietLan4"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        if (sdr["NgayNhanKhuon"] is null || sdr["NgayNhanKhuon"].ToString() == "")
                            em.NgayNhanKhuon = "";                       
                        else
                            em.NgayNhanKhuon = DateTime.ParseExact(sdr["NgayNhanKhuon"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.ViTriKhuon = sdr["ViTriKhuon"].ToString();
                        if (sdr["SoKhuonHienCo"] is null || sdr["SoKhuonHienCo"].ToString() == "")
                            em.SoKhuonHienCo = 0;
                        else
                            em.SoKhuonHienCo = int.Parse(sdr["SoKhuonHienCo"].ToString());
                        em.ViTriPhuKien = sdr["ViTriPhuKien"].ToString();
                        em.XuLyBeMat = sdr["XuLyBeMat"].ToString();
                        em.PPCheBienNL = sdr["PPCheBienNL"].ToString();
                        em.PPCat = sdr["PPCat"].ToString();
                        if (sdr["ThoiGianTron"] is null || sdr["ThoiGianTron"].ToString() == "")
                            em.ThoiGianTron = 0;
                        else
                            em.ThoiGianTron = int.Parse(sdr["ThoiGianTron"].ToString());
                        if (sdr["TongThoiGianCan"] is null || sdr["TongThoiGianCan"].ToString() == "")
                            em.TongThoiGianCan = 0;
                        else
                            em.TongThoiGianCan = int.Parse(sdr["TongThoiGianCan"].ToString());
                        em.ChiTietCaiTien = sdr["ChiTietCaiTien"].ToString();
                        em.DoDay = sdr["DoDay"].ToString();
                        em.ChieuDaiMin = sdr["ChieuDaiMin"].ToString();
                        em.ChieuDaiMax = sdr["ChieuDaiMax"].ToString();
                        em.TLSoiMin = sdr["TLSoiMin"].ToString();
                        em.TLSoiMax = sdr["TLSoiMax"].ToString();
                        em.SoSoi = sdr["SoSoi"].ToString();
                        em.DoDay2 = sdr["DoDay2"].ToString();
                        em.ChieuDaiMin2 = sdr["ChieuDaiMin2"].ToString();
                        em.ChieuDaiMax2 = sdr["ChieuDaiMax2"].ToString();
                        em.TLSoiMin2 = sdr["TLSoiMin2"].ToString();
                        em.TLSoiMax2 = sdr["TLSoiMax2"].ToString();
                        em.SoSoi2 = sdr["SoSoi2"].ToString();
                        em.DoDay3 = sdr["DoDay3"].ToString();
                        em.ChieuDaiMin3 = sdr["ChieuDaiMin3"].ToString();
                        em.ChieuDaiMax3 = sdr["ChieuDaiMax3"].ToString();
                        em.TLSoiMin3 = sdr["TLSoiMin3"].ToString();
                        em.TLSoiMax3 = sdr["TLSoiMax3"].ToString();
                        em.SoSoi3 = sdr["SoSoi3"].ToString();
                        em.DoDay4 = sdr["DoDay4"].ToString();
                        em.ChieuDaiMin4 = sdr["ChieuDaiMin4"].ToString();
                        em.ChieuDaiMax4 = sdr["ChieuDaiMax4"].ToString();
                        em.TLSoiMin4 = sdr["TLSoiMin4"].ToString();
                        em.TLSoiMax4 = sdr["TLSoiMax4"].ToString();
                        em.SoSoi4 = sdr["SoSoi4"].ToString();
                        em.PPDatKeo = sdr["PPDatKeo"].ToString();
                        em.TheTichBom = sdr["TheTichBom"].ToString();
                        em.CachDat = sdr["CachDat"].ToString();
                        if (sdr["SoLoKhuon"] is null || sdr["SoLoKhuon"].ToString() == "")
                            em.SoLoKhuon = 0;
                        else
                            em.SoLoKhuon = int.Parse(sdr["SoLoKhuon"].ToString());
                        if (sdr["SoLoThucLam"] is null || sdr["SoLoThucLam"].ToString() == "")
                            em.SoLoThucLam = 0;
                        else
                            em.SoLoThucLam = int.Parse(sdr["SoLoThucLam"].ToString());
                        if (sdr["Tren5"] is null || sdr["Tren5"].ToString() == "")
                            em.Tren5 = 0;
                        else
                            em.Tren5 = int.Parse(sdr["Tren5"].ToString());
                        if (sdr["Duoi5"] is null || sdr["Duoi5"].ToString() == "")
                            em.Duoi5 = 0;
                        else
                            em.Duoi5 = int.Parse(sdr["Duoi5"].ToString());
                        if (sdr["LucEp"] is null || sdr["LucEp"].ToString() == "")
                            em.LucEp = 0;
                        else
                            em.LucEp = int.Parse(sdr["LucEp"].ToString());
                        if (sdr["SoLanThoatKhi"] is null || sdr["SoLanThoatKhi"].ToString() == "")
                            em.TongThoiGianCan = 0;
                        else
                            em.SoLanThoatKhi = int.Parse(sdr["SoLanThoatKhi"].ToString());
                        em.TongTLSat = sdr["TongTLSat"].ToString();
                        em.KLSPThucTe = sdr["KLSPThucTe"].ToString();
                        em.ThoiGianLuuHoa = sdr["ThoiGianLuuHoa"].ToString();
                        em.ThoiGianHoanTat = sdr["ThoiGianHoanTat"].ToString();
                        em.ChatLuong = sdr["ChatLuong"].ToString();
                        em.XuLyDinhDH = sdr["XuLyDinhDH"].ToString();
                        em.XuLyBTPTaiDH = sdr["XuLyBTPTaiDH"].ToString();
                        em.DCBocTay = sdr["DCBocTay"].ToString();
                        em.DCXuLyBeMat = sdr["DCXuLyBeMat"].ToString();
                        if (sdr["NgayKTMau"] is null || sdr["NgayKTMau"].ToString() == "")
                            em.NgayKTMau = "";                       
                        else
                            em.NgayKTMau = DateTime.ParseExact(sdr["NgayKTMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.KTTruocLH = sdr["KTTruocLH"].ToString();
                        em.KTSauLH = sdr["KTSauLH"].ToString();
                        em.KTXuatMau = sdr["KTXuatMau"].ToString();
                        em.PPBocTachBTM = sdr["PPBocTachBTM"].ToString();
                        em.XuLyBeMatBTM = sdr["XuLyBeMatBTM"].ToString();
                        em.ThoiGian = sdr["ThoiGian"].ToString();
                        em.TocDo = sdr["TocDo"].ToString();
                        em.KL1LanBan = sdr["KL1LanBan"].ToString();
                        em.LoaiCatBi = sdr["LoaiCatBi"].ToString();
                        em.NhietDo = sdr["NhietDo"].ToString();
                        em.ChatLuongBT = sdr["ChatLuongBT"].ToString();
                        em.PPLamSach = sdr["PPLamSach"].ToString();
                        em.XuLyBeMatLH = sdr["XuLyBeMatLH"].ToString();
                        em.NhietDoLH = sdr["NhietDoLH"].ToString();
                        em.ThoiGianLH = sdr["ThoiGianLH"].ToString();
                        em.KL1LanMai = sdr["KL1LanMai"].ToString();
                        em.ThoiGianMai = sdr["ThoiGianMai"].ToString();
                        em.LoaiDaMai = sdr["LoaiDaMai"].ToString();
                        em.TronBot = sdr["TronBot"].ToString();
                        em.ChamSon = sdr["ChamSon"].ToString();
                        em.GanLinhKien = sdr["GanLinhKien"].ToString();
                        em.PhuTeflon = sdr["PhuTeflon"].ToString();
                        em.BaviaKH = sdr["BaviaKH"].ToString();
                        em.BaviaCaiDat = sdr["BaviaCaiDat"].ToString();
                        em.YeuCauKiemMay = sdr["YeuCauKiemMay"].ToString();
                        em.XuLyBeMatKM = sdr["XuLyBeMatKM"].ToString();
                        em.NgoaiQuanKM = sdr["NgoaiQuanKM"].ToString();
                        em.TyLeNG = sdr["TyLeNG"].ToString();
                        em.TrongLuong1SP = sdr["TrongLuong1SP"].ToString();
                        em.TrongLuongLinhKien = sdr["TrongLuongLinhKien"].ToString();
                        if (sdr["SL1boc"] is null || sdr["SL1boc"].ToString() == "")
                            em.SL1boc = 0;
                        else
                            em.SL1boc = int.Parse(sdr["SL1boc"].ToString());
                        em.KichThuocBoc = sdr["KichThuocBoc"].ToString();
                        if (sdr["SL1hop"] is null || sdr["SL1hop"].ToString() == "")
                            em.SL1hop = 0;
                        else
                            em.SL1hop = int.Parse(sdr["SL1hop"].ToString());
                        em.KichThuocThung = sdr["KichThuocThung"].ToString();
                        if (sdr["SL1thung"] is null || sdr["SL1thung"].ToString() == "")
                            em.SL1thung = 0;
                        else
                            em.SL1thung = int.Parse(sdr["SL1thung"].ToString());
                        em.PPDongBoc = sdr["PPDongBoc"].ToString();
                        em.PPDongHoi = sdr["PPDongHoi"].ToString();
                        em.GhiChuKhacDG = sdr["GhiChuKhacDG"].ToString();
                        em.KichThuoc = sdr["KichThuoc"].ToString();
                        em.NgoaiQuan = sdr["NgoaiQuan"].ToString();
                        em.DongGoi = sdr["DongGoi"].ToString();
                        em.Khac = sdr["Khac"].ToString();
                        em.GhiChu = sdr["GhiChu"].ToString();
                    }
                }
                return em;
            }
        }

        [HttpGet]
        public List<ChildQuyTrinh> QuyTrinh_GetALL()
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            List<ChildQuyTrinh> lem = new List<ChildQuyTrinh>();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetAll"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        ChildQuyTrinh em = new ChildQuyTrinh();
                        em.SoDonKhuon = sdr["SoDonKhuon"].ToString();
                        em.NgayDuyet = sdr["NgayDuyet"].ToString();
                        em.CodeKH = sdr["CodeKH"].ToString();
                        em.MaKhuonTW = sdr["MaKhuonTW"].ToString();
                        em.CodeSP = sdr["CodeSP"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        [HttpGet]
        public List<QuyTrinhModel> QuyTrinh_GetALLQuyTrinh()
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            List<QuyTrinhModel> lem = new List<QuyTrinhModel>();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetAllQuyTrinh"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        QuyTrinhModel em = new QuyTrinhModel();
                        em.SoDonKhuon = sdr["SoDonKhuon"].ToString();
                        if (sdr["NgayDuyet"] is null || sdr["NgayDuyet"].ToString() == "")
                            em.NgayDuyet = "";
                        else
                            em.NgayDuyet = DateTime.ParseExact(sdr["NgayDuyet"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan1 = sdr["PhienBan1"].ToString();
                        if (sdr["NgayPhatHanh1"] is null || sdr["NgayPhatHanh1"].ToString() == "")
                            em.NgayPhatHanh1 = "";
                        else
                            em.NgayPhatHanh1 = DateTime.ParseExact(sdr["NgayPhatHanh1"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan2 = sdr["PhienBan2"].ToString();
                        if (sdr["NgayPhatHanh2"] is null || sdr["NgayPhatHanh2"].ToString() == "")
                            em.NgayPhatHanh2 = "";
                        else
                            em.NgayPhatHanh2 = DateTime.ParseExact(sdr["NgayPhatHanh2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                    .ToString("MM-dd-yyyy");
                        em.PhienBan3 = sdr["PhienBan3"].ToString();
                        if (sdr["NgayPhatHanh3"] is null || sdr["NgayPhatHanh3"].ToString() == "")
                            em.NgayPhatHanh3 = "";
                        else
                            em.NgayPhatHanh3 = DateTime.ParseExact(sdr["NgayPhatHanh3"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                     .ToString("MM-dd-yyyy");
                        if (sdr["SoLoKhuonKH"] is null || sdr["SoLoKhuonKH"].ToString() == "")
                            em.SoLoKhuonKH = 0;
                        else
                            em.SoLoKhuonKH = int.Parse(sdr["SoLoKhuonKH"].ToString());
                        em.CoRutTheoDonKhuon = sdr["CoRutTheoDonKhuon"].ToString();
                        em.UocTinhKLSP = sdr["UocTinhKLSP"].ToString();
                        em.ThoiGianHoanTat1k = sdr["ThoiGianHoanTat1k"].ToString();
                        em.LoaiKhuonMau = sdr["LoaiKhuonMau"].ToString();
                        em.CodeKH = sdr["CodeKH"].ToString();
                        em.CodeSP = sdr["CodeSP"].ToString();
                        em.TenQU = sdr["TenQU"].ToString();
                        em.MaKhuonTW = sdr["MaKhuonTW"].ToString();
                        em.QuyCachKH = sdr["QuyCachKH"].ToString();
                        em.QuyCachEssons = sdr["QuyCachEssons"].ToString();
                        em.ChatKeo = sdr["ChatKeo"].ToString();
                        em.Mau = sdr["Mau"].ToString();
                        em.MaKeo1 = sdr["MaKeo1"].ToString();
                        em.MaKeo2 = sdr["MaKeo2"].ToString();
                        em.PantoneMau = sdr["PantoneMau"].ToString();
                        em.GhiChuKHSX = sdr["GhiChuKHSX"].ToString();
                        em.GocNL = sdr["GocNL"].ToString();
                        if (sdr["SoPCSMoiBo"] is null || sdr["SoPCSMoiBo"].ToString() == "")
                            em.SoPCSMoiBo = 0;
                        else
                            em.SoPCSMoiBo = int.Parse(sdr["SoPCSMoiBo"].ToString());
                        em.PhaKeo = sdr["PhaKeo"].ToString();
                        if (sdr["NgayGiaoMau"] is null || sdr["NgayGiaoMau"].ToString() == "")
                            em.NgayGiaoMau = "";
                        else
                            em.NgayGiaoMau = DateTime.ParseExact(sdr["NgayGiaoMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        if (sdr["NgayKHApprovedMau"] is null || sdr["NgayKHApprovedMau"].ToString() == "")
                            em.NgayKHApprovedMau = "";
                        else
                            em.NgayKHApprovedMau = DateTime.ParseExact(sdr["NgayKHApprovedMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.MaKeoKho1 = sdr["MaKeo1"].ToString();
                        em.HoaChat1MK1 = sdr["HoaChat1MK1"].ToString();
                        em.KhoiLuongHC1MK1 = sdr["KhoiLuongHC1MK1"].ToString();
                        em.HoaChat2MK1 = sdr["HoaChat2MK1"].ToString();
                        em.KhoiLuongHC2MK1 = sdr["KhoiLuongHC2MK1"].ToString();
                        em.HoaChat3MK1 = sdr["HoaChat3MK1"].ToString();
                        em.KhoiLuongHC3MK1 = sdr["KhoiLuongHC3MK1"].ToString();
                        em.HoaChat4MK1 = sdr["HoaChat4MK1"].ToString();
                        em.KhoiLuongHC4MK1 = sdr["KhoiLuongHC4MK1"].ToString();
                        em.HoaChat5MK1 = sdr["HoaChat5MK1"].ToString();
                        em.KhoiLuongHC5MK1 = sdr["KhoiLuongHC5MK1"].ToString();
                        em.HoaChat6MK1 = sdr["HoaChat6MK1"].ToString();
                        em.KhoiLuongHC6MK1 = sdr["KhoiLuongHC6MK1"].ToString();
                        em.HoaChat7MK1 = sdr["HoaChat7MK1"].ToString();
                        em.KhoiLuongHC7MK1 = sdr["KhoiLuongHC7MK1"].ToString();
                        em.HoaChat8MK1 = sdr["HoaChat8MK1"].ToString();
                        em.KhoiLuongHC8MK1 = sdr["KhoiLuongHC8MK1"].ToString();
                        em.HoaChat9MK1 = sdr["HoaChat9MK1"].ToString();
                        em.KhoiLuongHC9MK1 = sdr["KhoiLuongHC9MK1"].ToString();
                        em.HoaChat10MK1 = sdr["HoaChat10MK1"].ToString();
                        em.KhoiLuongHC10MK1 = sdr["KhoiLuongHC10MK1"].ToString();
                        em.HoaChat11MK1 = sdr["HoaChat11MK1"].ToString();
                        em.KhoiLuongHC11MK1 = sdr["KhoiLuongHC11MK1"].ToString();
                        em.HoaChat12MK1 = sdr["HoaChat12MK1"].ToString();
                        em.KhoiLuongHC12MK1 = sdr["KhoiLuongHC12MK1"].ToString();
                        em.HoaChat1MK1KH = sdr["HoaChat1MK1KH"].ToString();
                        em.KhoiLuongHC1MK1KH = sdr["KhoiLuongHC1MK1KH"].ToString();
                        em.HoaChat2MK1KH = sdr["HoaChat2MK1KH"].ToString();
                        em.KhoiLuongHC2MK1KH = sdr["KhoiLuongHC2MK1KH"].ToString();
                        em.HoaChat3MK1KH = sdr["HoaChat3MK1KH"].ToString();
                        em.KhoiLuongHC3MK1KH = sdr["KhoiLuongHC3MK1KH"].ToString();
                        em.HoaChat4MK1KH = sdr["HoaChat4MK1KH"].ToString();
                        em.KhoiLuongHC4MK1KH = sdr["KhoiLuongHC4MK1KH"].ToString();
                        em.HoaChat5MK1KH = sdr["HoaChat5MK1KH"].ToString();
                        em.KhoiLuongHC5MK1KH = sdr["KhoiLuongHC5MK1KH"].ToString();
                        em.HoaChat6MK1KH = sdr["HoaChat6MK1KH"].ToString();
                        em.KhoiLuongHC6MK1KH = sdr["KhoiLuongHC6MK1KH"].ToString();
                        em.HoaChat7MK1KH = sdr["HoaChat7MK1KH"].ToString();
                        em.KhoiLuongHC7MK1KH = sdr["KhoiLuongHC7MK1KH"].ToString();
                        em.HoaChat8MK1KH = sdr["HoaChat8MK1KH"].ToString();
                        em.KhoiLuongHC8MK1KH = sdr["KhoiLuongHC8MK1KH"].ToString();
                        em.HoaChat9MK1KH = sdr["HoaChat9MK1KH"].ToString();
                        em.KhoiLuongHC9MK1KH = sdr["KhoiLuongHC9MK1KH"].ToString();
                        em.HoaChat10MK1KH = sdr["HoaChat10MK1KH"].ToString();
                        em.KhoiLuongHC10MK1KH = sdr["KhoiLuongHC10MK1KH"].ToString();
                        em.HoaChat11MK1KH = sdr["HoaChat11MK1KH"].ToString();
                        em.KhoiLuongHC11MK1KH = sdr["KhoiLuongHC11MK1KH"].ToString();
                        em.HoaChat12MK1KH = sdr["HoaChat12MK1KH"].ToString();
                        em.KhoiLuongHC12MK1KH = sdr["KhoiLuongHC12MK1KH"].ToString();
                        em.MaKeoKho2 = sdr["MaKeo2"].ToString();
                        em.HoaChat1MK2 = sdr["HoaChat1MK2"].ToString();
                        em.KhoiLuongHC1MK2 = sdr["KhoiLuongHC1MK2"].ToString();
                        em.HoaChat2MK2 = sdr["HoaChat2MK2"].ToString();
                        em.KhoiLuongHC2MK2 = sdr["KhoiLuongHC2MK2"].ToString();
                        em.HoaChat3MK2 = sdr["HoaChat3MK2"].ToString();
                        em.KhoiLuongHC3MK2 = sdr["KhoiLuongHC3MK2"].ToString();
                        em.HoaChat4MK2 = sdr["HoaChat4MK2"].ToString();
                        em.KhoiLuongHC4MK2 = sdr["KhoiLuongHC4MK2"].ToString();
                        em.HoaChat5MK2 = sdr["HoaChat5MK2"].ToString();
                        em.KhoiLuongHC5MK2 = sdr["KhoiLuongHC5MK2"].ToString();
                        em.HoaChat6MK2 = sdr["HoaChat6MK2"].ToString();
                        em.KhoiLuongHC6MK2 = sdr["KhoiLuongHC6MK2"].ToString();
                        em.HoaChat7MK2 = sdr["HoaChat7MK2"].ToString();
                        em.KhoiLuongHC7MK2 = sdr["KhoiLuongHC7MK2"].ToString();
                        em.HoaChat8MK2 = sdr["HoaChat8MK2"].ToString();
                        em.KhoiLuongHC8MK2 = sdr["KhoiLuongHC8MK2"].ToString();
                        em.HoaChat9MK2 = sdr["HoaChat9MK2"].ToString();
                        em.KhoiLuongHC9MK2 = sdr["KhoiLuongHC9MK2"].ToString();
                        em.HoaChat10MK2 = sdr["HoaChat10MK2"].ToString();
                        em.KhoiLuongHC10MK2 = sdr["KhoiLuongHC10MK2"].ToString();
                        em.HoaChat11MK2 = sdr["HoaChat11MK2"].ToString();
                        em.KhoiLuongHC11MK2 = sdr["KhoiLuongHC11MK2"].ToString();
                        em.HoaChat12MK2 = sdr["HoaChat12MK2"].ToString();
                        em.KhoiLuongHC12MK2 = sdr["KhoiLuongHC12MK2"].ToString();
                        em.HoaChat1MK2KH = sdr["HoaChat1MK2KH"].ToString();
                        em.KhoiLuongHC1MK2KH = sdr["KhoiLuongHC1MK2KH"].ToString();
                        em.HoaChat2MK2KH = sdr["HoaChat2MK2KH"].ToString();
                        em.KhoiLuongHC2MK2KH = sdr["KhoiLuongHC2MK2KH"].ToString();
                        em.HoaChat3MK2KH = sdr["HoaChat3MK2KH"].ToString();
                        em.KhoiLuongHC3MK2KH = sdr["KhoiLuongHC3MK2KH"].ToString();
                        em.HoaChat4MK2KH = sdr["HoaChat4MK2KH"].ToString();
                        em.KhoiLuongHC4MK2KH = sdr["KhoiLuongHC4MK2KH"].ToString();
                        em.HoaChat5MK2KH = sdr["HoaChat5MK2KH"].ToString();
                        em.KhoiLuongHC5MK2KH = sdr["KhoiLuongHC5MK2KH"].ToString();
                        em.HoaChat6MK2KH = sdr["HoaChat6MK2KH"].ToString();
                        em.KhoiLuongHC6MK2KH = sdr["KhoiLuongHC6MK2KH"].ToString();
                        em.HoaChat7MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC7MK2KH"].ToString();
                        em.HoaChat8MK2KH = sdr["HoaChat8MK2KH"].ToString();
                        em.KhoiLuongHC8MK2KH = sdr["KhoiLuongHC8MK2KH"].ToString();
                        em.HoaChat9MK2KH = sdr["HoaChat9MK2KH"].ToString();
                        em.KhoiLuongHC9MK2KH = sdr["KhoiLuongHC9MK2KH"].ToString();
                        em.HoaChat10MK2KH = sdr["HoaChat10MK2KH"].ToString();
                        em.KhoiLuongHC10MK2KH = sdr["KhoiLuongHC10MK2KH"].ToString();
                        em.HoaChat11MK2KH = sdr["HoaChat11MK2KH"].ToString();
                        em.KhoiLuongHC11MK2KH = sdr["KhoiLuongHC11MK2KH"].ToString();
                        em.HoaChat12MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC12MK2KH"].ToString();
                        if (sdr["NgayDuyetPhoiThep"] is null || sdr["NgayDuyetPhoiThep"].ToString() == "")
                            em.NgayDuyetPhoiThep = "";
                        else
                            em.NgayDuyetPhoiThep = DateTime.ParseExact(sdr["NgayDuyetPhoiThep"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayMuaPhoiThep"] is null || sdr["NgayMuaPhoiThep"].ToString() == "")
                            em.NgayMuaPhoiThep = "";
                        else
                            em.NgayMuaPhoiThep = DateTime.ParseExact(sdr["NgayMuaPhoiThep"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        em.SoCTMuaPhoi = sdr["SoCTMuaPhoi"].ToString();
                        em.NCCPhoiThep = sdr["NCCPhoiThep"].ToString();
                        em.LoaiThepKhuon = sdr["LoaiThepKhuon"].ToString();
                        em.KichThuocKhuon = sdr["KichThuocKhuon"].ToString();
                        if (sdr["SoTam"] is null || sdr["SoTam"].ToString() == "")
                            em.SoTam = 0;
                        else
                            em.SoTam = int.Parse(sdr["SoTam"].ToString());
                        em.CoRutThucTe = sdr["%CoRutThucTe"].ToString();
                        em.LoaiKhuon = sdr["LoaiKhuon"].ToString();
                        em.PhuKienCTK = sdr["PhuKienCTK"].ToString();
                        em.PPXuLyBTP = sdr["PPXuLyBTP"].ToString();
                        if (sdr["NgaySXKhuon"] is null || sdr["NgaySXKhuon"].ToString() == "")
                            em.NgaySXKhuon = "";
                        else
                            em.NgaySXKhuon = DateTime.ParseExact(sdr["NgaySXKhuon"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayHoanThanhDuKien"] is null || sdr["NgayHoanThanhDuKien"].ToString() == "")
                            em.NgayHoanThanhDuKien = "";
                        else
                            em.NgayHoanThanhDuKien = DateTime.ParseExact(sdr["NgayHoanThanhDuKien"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        if (sdr["NgayHoanThanh"] is null || sdr["NgayHoanThanh"].ToString() == "")
                            em.NgayHoanThanh = "";
                        else
                            em.NgayHoanThanh = DateTime.ParseExact(sdr["NgayHoanThanh"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                 .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan1 = sdr["SuaChiTietLan1"].ToString();
                        if (sdr["NgaySuaChiTietLan1"] is null || sdr["NgaySuaChiTietLan1"].ToString() == "")
                            em.NgaySuaChiTietLan1 = "";
                        else
                            em.NgaySuaChiTietLan1 = DateTime.ParseExact(sdr["NgaySuaChiTietLan1"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan2 = sdr["SuaChiTietLan2"].ToString();
                        if (sdr["NgaySuaChiTietLan2"] is null || sdr["NgaySuaChiTietLan2"].ToString() == "")
                            em.NgaySuaChiTietLan2 = "";
                        else
                            em.NgaySuaChiTietLan2 = DateTime.ParseExact(sdr["NgaySuaChiTietLan2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan3 = sdr["SuaChiTietLan3"].ToString();
                        if (sdr["NgaySuaChiTietLan3"] is null || sdr["NgaySuaChiTietLan3"].ToString() == "")
                            em.NgaySuaChiTietLan3 = "";
                        else
                            em.NgaySuaChiTietLan3 = DateTime.ParseExact(sdr["NgaySuaChiTietLan3"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.SuaChiTietLan4 = sdr["SuaChiTietLan4"].ToString();
                        if (sdr["NgaySuaChiTietLan4"] is null || sdr["NgaySuaChiTietLan4"].ToString() == "")
                            em.NgaySuaChiTietLan4 = "";
                        else
                            em.NgaySuaChiTietLan4 = DateTime.ParseExact(sdr["NgaySuaChiTietLan4"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        if (sdr["NgayNhanKhuon"] is null || sdr["NgayNhanKhuon"].ToString() == "")
                            em.NgayNhanKhuon = "";
                        else
                            em.NgayNhanKhuon = DateTime.ParseExact(sdr["NgayNhanKhuon"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.ViTriKhuon = sdr["ViTriKhuon"].ToString();
                        if (sdr["SoKhuonHienCo"] is null || sdr["SoKhuonHienCo"].ToString() == "")
                            em.SoKhuonHienCo = 0;
                        else
                            em.SoKhuonHienCo = int.Parse(sdr["SoKhuonHienCo"].ToString());
                        em.ViTriPhuKien = sdr["ViTriPhuKien"].ToString();
                        em.XuLyBeMat = sdr["XuLyBeMat"].ToString();
                        em.PPCheBienNL = sdr["PPCheBienNL"].ToString();
                        em.PPCat = sdr["PPCat"].ToString();
                        if (sdr["ThoiGianTron"] is null || sdr["ThoiGianTron"].ToString() == "")
                            em.ThoiGianTron = 0;
                        else
                            em.ThoiGianTron = int.Parse(sdr["ThoiGianTron"].ToString());
                        if (sdr["TongThoiGianCan"] is null || sdr["TongThoiGianCan"].ToString() == "")
                            em.TongThoiGianCan = 0;
                        else
                            em.TongThoiGianCan = int.Parse(sdr["TongThoiGianCan"].ToString());
                        em.ChiTietCaiTien = sdr["ChiTietCaiTien"].ToString();
                        em.DoDay = sdr["DoDay"].ToString();
                        em.ChieuDaiMin = sdr["ChieuDaiMin"].ToString();
                        em.ChieuDaiMax = sdr["ChieuDaiMax"].ToString();
                        em.TLSoiMin = sdr["TLSoiMin"].ToString();
                        em.TLSoiMax = sdr["TLSoiMax"].ToString();
                        em.SoSoi = sdr["SoSoi"].ToString();
                        em.DoDay2 = sdr["DoDay2"].ToString();
                        em.ChieuDaiMin2 = sdr["ChieuDaiMin2"].ToString();
                        em.ChieuDaiMax2 = sdr["ChieuDaiMax2"].ToString();
                        em.TLSoiMin2 = sdr["TLSoiMin2"].ToString();
                        em.TLSoiMax2 = sdr["TLSoiMax2"].ToString();
                        em.SoSoi2 = sdr["SoSoi2"].ToString();
                        em.DoDay3 = sdr["DoDay3"].ToString();
                        em.ChieuDaiMin3 = sdr["ChieuDaiMin3"].ToString();
                        em.ChieuDaiMax3 = sdr["ChieuDaiMax3"].ToString();
                        em.TLSoiMin3 = sdr["TLSoiMin3"].ToString();
                        em.TLSoiMax3 = sdr["TLSoiMax3"].ToString();
                        em.SoSoi3 = sdr["SoSoi3"].ToString();
                        em.DoDay4 = sdr["DoDay4"].ToString();
                        em.ChieuDaiMin4 = sdr["ChieuDaiMin4"].ToString();
                        em.ChieuDaiMax4 = sdr["ChieuDaiMax4"].ToString();
                        em.TLSoiMin4 = sdr["TLSoiMin4"].ToString();
                        em.TLSoiMax4 = sdr["TLSoiMax4"].ToString();
                        em.SoSoi4 = sdr["SoSoi4"].ToString();
                        em.PPDatKeo = sdr["PPDatKeo"].ToString();
                        em.TheTichBom = sdr["TheTichBom"].ToString();
                        em.CachDat = sdr["CachDat"].ToString();
                        if (sdr["SoLoKhuon"] is null || sdr["SoLoKhuon"].ToString() == "")
                            em.SoLoKhuon = 0;
                        else
                            em.SoLoKhuon = int.Parse(sdr["SoLoKhuon"].ToString());
                        if (sdr["SoLoThucLam"] is null || sdr["SoLoThucLam"].ToString() == "")
                            em.SoLoThucLam = 0;
                        else
                            em.SoLoThucLam = int.Parse(sdr["SoLoThucLam"].ToString());
                        if (sdr["Tren5"] is null || sdr["Tren5"].ToString() == "")
                            em.Tren5 = 0;
                        else
                            em.Tren5 = int.Parse(sdr["Tren5"].ToString());
                        if (sdr["Duoi5"] is null || sdr["Duoi5"].ToString() == "")
                            em.Duoi5 = 0;
                        else
                            em.Duoi5 = int.Parse(sdr["Duoi5"].ToString());
                        if (sdr["LucEp"] is null || sdr["LucEp"].ToString() == "")
                            em.LucEp = 0;
                        else
                            em.LucEp = int.Parse(sdr["LucEp"].ToString());
                        if (sdr["SoLanThoatKhi"] is null || sdr["SoLanThoatKhi"].ToString() == "")
                            em.TongThoiGianCan = 0;
                        else
                            em.SoLanThoatKhi = int.Parse(sdr["SoLanThoatKhi"].ToString());
                        em.TongTLSat = sdr["TongTLSat"].ToString();
                        em.KLSPThucTe = sdr["KLSPThucTe"].ToString();
                        em.ThoiGianLuuHoa = sdr["ThoiGianLuuHoa"].ToString();
                        em.ThoiGianHoanTat = sdr["ThoiGianHoanTat"].ToString();
                        em.ChatLuong = sdr["ChatLuong"].ToString();
                        em.XuLyDinhDH = sdr["XuLyDinhDH"].ToString();
                        em.XuLyBTPTaiDH = sdr["XuLyBTPTaiDH"].ToString();
                        em.DCBocTay = sdr["DCBocTay"].ToString();
                        em.DCXuLyBeMat = sdr["DCXuLyBeMat"].ToString();
                        if (sdr["NgayKTMau"] is null || sdr["NgayKTMau"].ToString() == "")
                            em.NgayKTMau = "";
                        else
                            em.NgayKTMau = DateTime.ParseExact(sdr["NgayKTMau"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                .ToString("MM-dd-yyyy");
                        em.KTTruocLH = sdr["KTTruocLH"].ToString();
                        em.KTSauLH = sdr["KTSauLH"].ToString();
                        em.KTXuatMau = sdr["KTXuatMau"].ToString();
                        em.PPBocTachBTM = sdr["PPBocTachBTM"].ToString();
                        em.XuLyBeMatBTM = sdr["XuLyBeMatBTM"].ToString();
                        em.ThoiGian = sdr["ThoiGian"].ToString();
                        em.TocDo = sdr["TocDo"].ToString();
                        em.KL1LanBan = sdr["KL1LanBan"].ToString();
                        em.LoaiCatBi = sdr["LoaiCatBi"].ToString();
                        em.NhietDo = sdr["NhietDo"].ToString();
                        em.ChatLuongBT = sdr["ChatLuongBT"].ToString();
                        em.PPLamSach = sdr["PPLamSach"].ToString();
                        em.XuLyBeMatLH = sdr["XuLyBeMatLH"].ToString();
                        em.NhietDoLH = sdr["NhietDoLH"].ToString();
                        em.ThoiGianLH = sdr["ThoiGianLH"].ToString();
                        em.KL1LanMai = sdr["KL1LanMai"].ToString();
                        em.ThoiGianMai = sdr["ThoiGianMai"].ToString();
                        em.LoaiDaMai = sdr["LoaiDaMai"].ToString();
                        em.TronBot = sdr["TronBot"].ToString();
                        em.ChamSon = sdr["ChamSon"].ToString();
                        em.GanLinhKien = sdr["GanLinhKien"].ToString();
                        em.PhuTeflon = sdr["PhuTeflon"].ToString();
                        em.BaviaKH = sdr["BaviaKH"].ToString();
                        em.BaviaCaiDat = sdr["BaviaCaiDat"].ToString();
                        em.YeuCauKiemMay = sdr["YeuCauKiemMay"].ToString();
                        em.XuLyBeMatKM = sdr["XuLyBeMatKM"].ToString();
                        em.NgoaiQuanKM = sdr["NgoaiQuanKM"].ToString();
                        em.TyLeNG = sdr["TyLeNG"].ToString();
                        em.TrongLuong1SP = sdr["TrongLuong1SP"].ToString();
                        em.TrongLuongLinhKien = sdr["TrongLuongLinhKien"].ToString();
                        if (sdr["SL1boc"] is null || sdr["SL1boc"].ToString() == "")
                            em.SL1boc = 0;
                        else
                            em.SL1boc = int.Parse(sdr["SL1boc"].ToString());
                        em.KichThuocBoc = sdr["KichThuocBoc"].ToString();
                        if (sdr["SL1hop"] is null || sdr["SL1hop"].ToString() == "")
                            em.SL1hop = 0;
                        else
                            em.SL1hop = int.Parse(sdr["SL1hop"].ToString());
                        em.KichThuocThung = sdr["KichThuocThung"].ToString();
                        if (sdr["SL1thung"] is null || sdr["SL1thung"].ToString() == "")
                            em.SL1thung = 0;
                        else
                            em.SL1thung = int.Parse(sdr["SL1thung"].ToString());
                        em.PPDongBoc = sdr["PPDongBoc"].ToString();
                        em.PPDongHoi = sdr["PPDongHoi"].ToString();
                        em.GhiChuKhacDG = sdr["GhiChuKhacDG"].ToString();
                        em.KichThuoc = sdr["KichThuoc"].ToString();
                        em.NgoaiQuan = sdr["NgoaiQuan"].ToString();
                        em.DongGoi = sdr["DongGoi"].ToString();
                        em.Khac = sdr["Khac"].ToString();
                        em.GhiChu = sdr["GhiChu"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
        [HttpGet]
        public ChildQuyTrinh QuyTrinh_GetCodeSP(string codeSP)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            ChildQuyTrinh em = new ChildQuyTrinh();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", codeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetCodeSP"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        em.CodeSP = sdr["CodeSP"].ToString();
                    }
                }
                return em;
            }
        }

        [HttpGet]
        public List<ChildQuyTrinh> QuyTrinh_GetCodeSPList(string codeSP)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            List<ChildQuyTrinh> lem = new List<ChildQuyTrinh>();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", codeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetCodeSPList"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        ChildQuyTrinh em = new ChildQuyTrinh();
                        em.CodeSP = sdr["CodeSP"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
        [HttpGet]
        public QuyTrinhModel QuyTrinh_GetMaKeo(string codeSP)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            QuyTrinhModel em = new QuyTrinhModel();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", codeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetMaKeo"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        em.MaKeo1 = sdr["MaKeo1"].ToString();
                        em.MaKeo2 = sdr["MaKeo2"].ToString();
                        em.MaKeoKho1 = sdr["MaKeo1"].ToString();
                        em.HoaChat1MK1 = sdr["HoaChat1MK1"].ToString();
                        em.KhoiLuongHC1MK1 = sdr["KhoiLuongHC1MK1"].ToString();
                        em.HoaChat2MK1 = sdr["HoaChat2MK1"].ToString();
                        em.KhoiLuongHC2MK1 = sdr["KhoiLuongHC2MK1"].ToString();
                        em.HoaChat3MK1 = sdr["HoaChat3MK1"].ToString();
                        em.KhoiLuongHC3MK1 = sdr["KhoiLuongHC3MK1"].ToString();
                        em.HoaChat4MK1 = sdr["HoaChat4MK1"].ToString();
                        em.KhoiLuongHC4MK1 = sdr["KhoiLuongHC4MK1"].ToString();
                        em.HoaChat5MK1 = sdr["HoaChat5MK1"].ToString();
                        em.KhoiLuongHC5MK1 = sdr["KhoiLuongHC5MK1"].ToString();
                        em.HoaChat6MK1 = sdr["HoaChat6MK1"].ToString();
                        em.KhoiLuongHC6MK1 = sdr["KhoiLuongHC6MK1"].ToString();
                        em.HoaChat7MK1 = sdr["HoaChat7MK1"].ToString();
                        em.KhoiLuongHC7MK1 = sdr["KhoiLuongHC7MK1"].ToString();
                        em.HoaChat8MK1 = sdr["HoaChat8MK1"].ToString();
                        em.KhoiLuongHC8MK1 = sdr["KhoiLuongHC8MK1"].ToString();
                        em.HoaChat9MK1 = sdr["HoaChat9MK1"].ToString();
                        em.KhoiLuongHC9MK1 = sdr["KhoiLuongHC9MK1"].ToString();
                        em.HoaChat10MK1 = sdr["HoaChat10MK1"].ToString();
                        em.KhoiLuongHC10MK1 = sdr["KhoiLuongHC10MK1"].ToString();
                        em.HoaChat11MK1 = sdr["HoaChat11MK1"].ToString();
                        em.KhoiLuongHC11MK1 = sdr["KhoiLuongHC11MK1"].ToString();
                        em.HoaChat12MK1 = sdr["HoaChat12MK1"].ToString();
                        em.KhoiLuongHC12MK1 = sdr["KhoiLuongHC12MK1"].ToString();
                        em.HoaChat1MK1KH = sdr["HoaChat1MK1KH"].ToString();
                        em.KhoiLuongHC1MK1KH = sdr["KhoiLuongHC1MK1KH"].ToString();
                        em.HoaChat2MK1KH = sdr["HoaChat2MK1KH"].ToString();
                        em.KhoiLuongHC2MK1KH = sdr["KhoiLuongHC2MK1KH"].ToString();
                        em.HoaChat3MK1KH = sdr["HoaChat3MK1KH"].ToString();
                        em.KhoiLuongHC3MK1KH = sdr["KhoiLuongHC3MK1KH"].ToString();
                        em.HoaChat4MK1KH = sdr["HoaChat4MK1KH"].ToString();
                        em.KhoiLuongHC4MK1KH = sdr["KhoiLuongHC4MK1KH"].ToString();
                        em.HoaChat5MK1KH = sdr["HoaChat5MK1KH"].ToString();
                        em.KhoiLuongHC5MK1KH = sdr["KhoiLuongHC5MK1KH"].ToString();
                        em.HoaChat6MK1KH = sdr["HoaChat6MK1KH"].ToString();
                        em.KhoiLuongHC6MK1KH = sdr["KhoiLuongHC6MK1KH"].ToString();
                        em.HoaChat7MK1KH = sdr["HoaChat7MK1KH"].ToString();
                        em.KhoiLuongHC7MK1KH = sdr["KhoiLuongHC7MK1KH"].ToString();
                        em.HoaChat8MK1KH = sdr["HoaChat8MK1KH"].ToString();
                        em.KhoiLuongHC8MK1KH = sdr["KhoiLuongHC8MK1KH"].ToString();
                        em.HoaChat9MK1KH = sdr["HoaChat9MK1KH"].ToString();
                        em.KhoiLuongHC9MK1KH = sdr["KhoiLuongHC9MK1KH"].ToString();
                        em.HoaChat10MK1KH = sdr["HoaChat10MK1KH"].ToString();
                        em.KhoiLuongHC10MK1KH = sdr["KhoiLuongHC10MK1KH"].ToString();
                        em.HoaChat11MK1KH = sdr["HoaChat11MK1KH"].ToString();
                        em.KhoiLuongHC11MK1KH = sdr["KhoiLuongHC11MK1KH"].ToString();
                        em.HoaChat12MK1KH = sdr["HoaChat12MK1KH"].ToString();
                        em.KhoiLuongHC12MK1KH = sdr["KhoiLuongHC12MK1KH"].ToString();
                        em.MaKeoKho2 = sdr["MaKeo2"].ToString();
                        em.HoaChat1MK2 = sdr["HoaChat1MK2"].ToString();
                        em.KhoiLuongHC1MK2 = sdr["KhoiLuongHC1MK2"].ToString();
                        em.HoaChat2MK2 = sdr["HoaChat2MK2"].ToString();
                        em.KhoiLuongHC2MK2 = sdr["KhoiLuongHC2MK2"].ToString();
                        em.HoaChat3MK2 = sdr["HoaChat3MK2"].ToString();
                        em.KhoiLuongHC3MK2 = sdr["KhoiLuongHC3MK2"].ToString();
                        em.HoaChat4MK2 = sdr["HoaChat4MK2"].ToString();
                        em.KhoiLuongHC4MK2 = sdr["KhoiLuongHC4MK2"].ToString();
                        em.HoaChat5MK2 = sdr["HoaChat5MK2"].ToString();
                        em.KhoiLuongHC5MK2 = sdr["KhoiLuongHC5MK2"].ToString();
                        em.HoaChat6MK2 = sdr["HoaChat6MK2"].ToString();
                        em.KhoiLuongHC6MK2 = sdr["KhoiLuongHC6MK2"].ToString();
                        em.HoaChat7MK2 = sdr["HoaChat7MK2"].ToString();
                        em.KhoiLuongHC7MK2 = sdr["KhoiLuongHC7MK2"].ToString();
                        em.HoaChat8MK2 = sdr["HoaChat8MK2"].ToString();
                        em.KhoiLuongHC8MK2 = sdr["KhoiLuongHC8MK2"].ToString();
                        em.HoaChat9MK2 = sdr["HoaChat9MK2"].ToString();
                        em.KhoiLuongHC9MK2 = sdr["KhoiLuongHC9MK2"].ToString();
                        em.HoaChat10MK2 = sdr["HoaChat10MK2"].ToString();
                        em.KhoiLuongHC10MK2 = sdr["KhoiLuongHC10MK2"].ToString();
                        em.HoaChat11MK2 = sdr["HoaChat11MK2"].ToString();
                        em.KhoiLuongHC11MK2 = sdr["KhoiLuongHC11MK2"].ToString();
                        em.HoaChat12MK2 = sdr["HoaChat12MK2"].ToString();
                        em.KhoiLuongHC12MK2 = sdr["KhoiLuongHC12MK2"].ToString();
                        em.HoaChat1MK2KH = sdr["HoaChat1MK2KH"].ToString();
                        em.KhoiLuongHC1MK2KH = sdr["KhoiLuongHC1MK2KH"].ToString();
                        em.HoaChat2MK2KH = sdr["HoaChat2MK2KH"].ToString();
                        em.KhoiLuongHC2MK2KH = sdr["KhoiLuongHC2MK2KH"].ToString();
                        em.HoaChat3MK2KH = sdr["HoaChat3MK2KH"].ToString();
                        em.KhoiLuongHC3MK2KH = sdr["KhoiLuongHC3MK2KH"].ToString();
                        em.HoaChat4MK2KH = sdr["HoaChat4MK2KH"].ToString();
                        em.KhoiLuongHC4MK2KH = sdr["KhoiLuongHC4MK2KH"].ToString();
                        em.HoaChat5MK2KH = sdr["HoaChat5MK2KH"].ToString();
                        em.KhoiLuongHC5MK2KH = sdr["KhoiLuongHC5MK2KH"].ToString();
                        em.HoaChat6MK2KH = sdr["HoaChat6MK2KH"].ToString();
                        em.KhoiLuongHC6MK2KH = sdr["KhoiLuongHC6MK2KH"].ToString();
                        em.HoaChat7MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC7MK2KH"].ToString();
                        em.HoaChat8MK2KH = sdr["HoaChat8MK2KH"].ToString();
                        em.KhoiLuongHC8MK2KH = sdr["KhoiLuongHC8MK2KH"].ToString();
                        em.HoaChat9MK2KH = sdr["HoaChat9MK2KH"].ToString();
                        em.KhoiLuongHC9MK2KH = sdr["KhoiLuongHC9MK2KH"].ToString();
                        em.HoaChat10MK2KH = sdr["HoaChat10MK2KH"].ToString();
                        em.KhoiLuongHC10MK2KH = sdr["KhoiLuongHC10MK2KH"].ToString();
                        em.HoaChat11MK2KH = sdr["HoaChat11MK2KH"].ToString();
                        em.KhoiLuongHC11MK2KH = sdr["KhoiLuongHC11MK2KH"].ToString();
                        em.HoaChat12MK2KH = sdr["HoaChat7MK2KH"].ToString();
                        em.KhoiLuongHC7MK2KH = sdr["KhoiLuongHC12MK2KH"].ToString();
                    }
                }
                return em;
            }
        }
        [HttpGet]
        public List<HoaChatSPModel> HoaChatSP_GetALL()
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            List<HoaChatSPModel> lem = new List<HoaChatSPModel>();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_HoaChatQT", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetAllHoaChat"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        HoaChatSPModel em = new HoaChatSPModel();
                        em.TenHoaChat = sdr["TenHoaChat"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
    }
}