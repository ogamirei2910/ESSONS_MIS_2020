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
        public ActionResult QuyTrinh_Insert([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

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

        public ActionResult QuyTrinh_Update_KHSX([FromBody]QuyTrinhModel model, string typeUpdate)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@NgayDuyet", model.NgayDuyet));
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
                        new SqlParameter("@SoLoKhuonKH", model.SoLoKhuonKH));
                    sc.Parameters.Add(
                       new SqlParameter("@CoRutTheoDonKhuon", model.CoRutTheoDonKhuon));
                    sc.Parameters.Add(
                        new SqlParameter("@UocTinhKLSP", model.UocTinhKLSP));
                    sc.Parameters.Add(
                        new SqlParameter("@ThoiGianHoanTat1k", model.ThoiGianHoanTat1k));
                    sc.Parameters.Add(
                       new SqlParameter("@LoaiKhuonMau", model.LoaiKhuonMau));
                    sc.Parameters.Add(
                       new SqlParameter("@NgayGiaoMau", model.NgayGiaoMau));
                    sc.Parameters.Add(
                       new SqlParameter("@GhiChuKHSX", model.GhiChuKHSX));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    sc.Parameters.Add(
                        new SqlParameter("@typeUpdate", typeUpdate));
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
            string connection = _configuration.GetConnectionString("DefaultConnection");
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
                        em.NgayDuyet = sdr["NgayDuyet"].ToString();
                        em.PhienBan1 = sdr["PhienBan1"].ToString();
                        em.NgayPhatHanh1 = sdr["NgayPhatHanh1"].ToString();
                        em.PhienBan2 = sdr["PhienBan1"].ToString();
                        em.NgayPhatHanh2 = sdr["NgayPhatHanh1"].ToString();
                        em.PhienBan3 = sdr["PhienBan1"].ToString();
                        em.NgayPhatHanh3 = sdr["NgayPhatHanh1"].ToString();
                        
                        if(sdr["SoLoKhuonKH"] is null || sdr["SoLoKhuonKH"].ToString() == "")
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
                        em.NgayGiaoMau = sdr["NgayGiaoMau"].ToString();
                        em.NgayKHApprovedMau = sdr["NgayGiaoMau"].ToString();
                        em.NgayPheDuyetMua = sdr["NgayGiaoMau"].ToString();
                        em.MaKeoKho1 = sdr["MaKeoKho1"].ToString();
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
                        em.MaKeoKho2 = sdr["MaKeoKho2"].ToString();
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
                        em.NgayDuyetPhoiThep = sdr["NgayDuyetPhoiThep"].ToString();
                        em.NgayMuaPhoiThep = sdr["NgayMuaPhoiThep"].ToString();
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
                        em.NgaySXKhuon = sdr["NgaySXKhuon"].ToString();
                        em.SuaChiTietLan1 = sdr["SuaChiTietLan1"].ToString();
                        em.NgaySuaChiTietLan1 = sdr["NgaySuaChiTietLan1"].ToString();
                        em.SuaChiTietLan2 = sdr["SuaChiTietLan2"].ToString();
                        em.NgaySuaChiTietLan2 = sdr["NgaySuaChiTietLan2"].ToString();
                        em.SuaChiTietLan3 = sdr["SuaChiTietLan3"].ToString();
                        em.NgaySuaChiTietLan3 = sdr["NgaySuaChiTietLan3"].ToString();
                        em.SuaChiTietLan4 = sdr["SuaChiTietLan4"].ToString();
                        em.NgaySuaChiTietLan4 = sdr["NgaySuaChiTietLan4"].ToString();
                        em.NgayNhanKhuon = sdr["NgayNhanKhuon"].ToString();
                        em.ViTriKhuon = sdr["ViTriKhuon"].ToString();
                        if (sdr["SoKhuonHienCo"] is null || sdr["SoKhuonHienCo"].ToString() == "")
                            em.SoKhuonHienCo = 0;
                        else
                            em.SoKhuonHienCo = int.Parse(sdr["SoKhuonHienCo"].ToString());
                        em.ViTriPhuKien = sdr["ViTriPhuKien"].ToString();
                        em.XuLyBeMat = sdr["SuaChiTietLan3"].ToString();
                        em.PPCheBienNL = sdr["NgaySuaChiTietLan3"].ToString();
                        if (sdr["ThoiGianTron"] is null || sdr["ThoiGianTron"].ToString() == "")
                            em.ThoiGianTron = 0;
                        else
                            em.ThoiGianTron = int.Parse(sdr["ThoiGianTron"].ToString());
                        if(sdr["TongThoiGianCan"] is null || sdr["TongThoiGianCan"].ToString() == "")
                            em.TongThoiGianCan = 0;
                        else
                            em.TongThoiGianCan = int.Parse(sdr["TongThoiGianCan"].ToString());
                        em.ChiTietCaiTien = sdr["NgaySuaChiTietLan4"].ToString();
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
                        em.ChieuDaiMin3= sdr["ChieuDaiMin3"].ToString();
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
                        em.NgayKTMau = sdr["NgayKTMau"].ToString();
                        em.KTTruocLH = sdr["KTTruocLH"].ToString();
                        em.KTSauLH = sdr["KTSauLH"].ToString();
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
            string connection = _configuration.GetConnectionString("DefaultConnection");
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
    }
}