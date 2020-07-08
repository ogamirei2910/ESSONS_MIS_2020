using System;
using System.Collections.Generic;
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
        public IActionResult QuyTrinh_Insert([FromBody]QuyTrinhModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_quytrinhphattrien", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoChungTu", model.SoChungTu));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayDuyet", model.NgayDuyet));
                    sc.Parameters.Add(
                        new SqlParameter("@NgayPhatHanh", model.NgayPhatHanh));
                    sc.Parameters.Add(
                        new SqlParameter("@Noi", model.Noi));
                    sc.Parameters.Add(
                       new SqlParameter("@CodeKH", model.CodeKH));
                    sc.Parameters.Add(
                        new SqlParameter("@CodeSP", model.CodeSP));
                    sc.Parameters.Add(
                        new SqlParameter("@TenQU", model.TenQU));
                    sc.Parameters.Add(
                        new SqlParameter("@MaKhuonTW", model.MaKhuonTW));
                    sc.Parameters.Add(
                        new SqlParameter("@QuyCachKH", model.QuyCachKH));
                    sc.Parameters.Add(
                       new SqlParameter("@QuyCachEssons", model.QuyCachEssons ));
                    sc.Parameters.Add(
                        new SqlParameter("@ChatKeo", model.ChatKeo));
                    sc.Parameters.Add(
                        new SqlParameter("@Mau", model.Mau));
                    sc.Parameters.Add(
                       new SqlParameter("@MaKeo", model.MaKeo));
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
                        new SqlParameter("@type", "Insert"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        [HttpGet]
        public QuyTrinhModel QuyTrinh_Get(string codesp)
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
                        new SqlParameter("@CodeSP", codesp));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Get"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {                    
                        em.SoChungTu = sdr["SoChungTu"].ToString();
                        em.NgayDuyet = sdr["NgayDuyet"].ToString();
                        em.NgayPhatHanh = sdr["NgayPhatHanh"].ToString();
                        em.Noi = sdr["Noi"].ToString();
                        em.CodeKH = sdr["CodeKH"].ToString();
                        em.CodeSP = sdr["CodeSP"].ToString();
                        em.TenQU = sdr["TenQU"].ToString();
                        em.MaKhuonTW = sdr["MaKhuonTW"].ToString();
                        em.QuyCachKH = sdr["QuyCachKH"].ToString();
                        em.QuyCachEssons = sdr["QuyCachEssons"].ToString();
                        em.ChatKeo = sdr["ChatKeo"].ToString();
                        em.Mau = sdr["Mau"].ToString();
                        em.MaKeo = sdr["MaKeo"].ToString();
                        em.SoLoKhuonKH = sdr["SoLoKhuonKH"].ToString();
                        em.CoRutTheoDonKhuon = sdr["CoRutTheoDonKhuon"].ToString();
                        em.UocTinhKLSP = sdr["UocTinhKLSP"].ToString();
                        em.ThoiGianHoanTat1k = sdr["ThoiGianHoanTat1k"].ToString();
                        em.LoaiKhuonMau = sdr["LoaiKhuonMau"].ToString();
                        em.NgayGiaoMau = sdr["NgayGiaoMau"].ToString();
                        em.GhiChuKHSX = sdr["GhiChuKHSX"].ToString();
                        em.NgayPheDuyetMua = sdr["NgayPheDuyetMua"].ToString();
                        em.LoaiThepKhuon = sdr["LoaiThepKhuon"].ToString();
                        em.KichThuocKhuon = sdr["KichThuocKhuon"].ToString();
                        em.SoTam = sdr["SoTam"].ToString();
                        em.CoRutThucTe = sdr["%CoRutThucTe"].ToString();
                        em.BLBNHCDK = sdr["BLBNHCDK"].ToString();
                        em.BocTach = sdr["BocTach"].ToString();
                        em.NgaySXKhuon = sdr["NgaySXKhuon"].ToString();
                        em.NgayHoanThanhDuKien = sdr["NgayHoanThanhDuKien"].ToString();
                        em.NgayHoanThanh = sdr["NgayHoanThanh"].ToString();
                        em.RacBot = sdr["RacBot"].ToString();
                        em.TronHoaChat = sdr["TronHoaChat"].ToString();
                        em.TongThoiGianTronCan = sdr["TongThoiGianTronCan"].ToString();
                        em.CatSoiEpDun = sdr["CatSoiEpDun"].ToString();
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
                        em.DoDay5 = sdr["DoDay5"].ToString();
                        em.ChieuDaiMin5 = sdr["ChieuDaiMin5"].ToString();
                        em.ChieuDaiMax5 = sdr["ChieuDaiMax5"].ToString();
                        em.TLSoiMin5 = sdr["TLSoiMin5"].ToString();
                        em.TLSoiMax5 = sdr["TLSoiMax5"].ToString();
                        em.SoSoi5 = sdr["SoSoi5"].ToString();
                        em.PPDatKeo = sdr["PPDatKeo"].ToString();
                        em.CachDat = sdr["CachDat"].ToString();
                        em.SoLoKhuon = sdr["SoLoKhuon"].ToString();
                        em.SoLoThucLam = sdr["SoLoThucLam"].ToString();
                        em.Tren5 = sdr["Tren5"].ToString();
                        em.Duoi5 = sdr["Duoi5"].ToString();
                        em.LucEp = sdr["LucEp"].ToString();
                        em.SoLanThoatKhi = sdr["SoLanThoatKhi"].ToString();
                        em.TongTLSat = sdr["TongTLSat"].ToString();
                        em.KLSPThucTe = sdr["KLSPThucTe"].ToString();
                        em.ThoiGianLuuHoa = sdr["ThoiGianLuuHoa"].ToString();
                        em.ThoiGianHoanTat = sdr["ThoiGianHoanTat"].ToString();
                        em.ChatLuongKhuon = sdr["ChatLuongKhuon"].ToString();
                        em.RacBotSilicon = sdr["RacBotSilicon"].ToString();
                        em.PPBocTachKiemTra = sdr["PPBocTachKiemTra"].ToString();
                        em.DungCuHoTro = sdr["DungCuHoTro"].ToString();
                        em.ViTriKhuon = sdr["ViTriKhuon"].ToString();
                        em.VitriBanKeo = sdr["VitriBanKeo"].ToString();
                        em.SoTamTrenKhuon = sdr["SoTamTrenKhuon"].ToString();
                        em.NgayHoanThanhKhuon = sdr["NgayHoanThanhKhuon"].ToString();
                        em.KichThuocTruocLH = sdr["KichThuocTruocLH"].ToString();
                        em.KichThuocSauLH = sdr["KichThuocSauLH"].ToString();
                        em.KichThuocLonTieuChuan = sdr["KichThuocLonTieuChuan"].ToString();
                        em.KichThuocNhoTieuChuan = sdr["KichThuocNhoTieuChuan"].ToString();
                        em.KichThuocKhongDongNhat = sdr["KichThuocKhongDongNhat"].ToString();
                        em.PPLamSach = sdr["PPLamSach"].ToString();
                        em.RacBotSiliconLH = sdr["RacBotSiliconLH"].ToString();
                        em.LH2NhietDo = sdr["LH2NhietDo"].ToString();
                        em.LH2ThoiGian = sdr["LH2ThoiGian"].ToString();
                        em.PPBocTach = sdr["PPBocTach"].ToString();
                        em.DungCuBocTach = sdr["DungCuBocTach"].ToString();
                        em.RacBotSiliconBT = sdr["RacBotSiliconBT"].ToString();
                        em.MNThoiGian = sdr["MNThoiGian"].ToString();
                        em.MNTocDo = sdr["MNTocDo"].ToString();
                        em.MNKL1LanBan = sdr["MNKL1LanBan"].ToString();
                        em.NNhietDo = sdr["NNhietDo"].ToString();
                        em.NCB = sdr["NCB"].ToString();
                        em.BocTachDTK = sdr["BocTachDTK"].ToString();
                        em.BanNito = sdr["BanNito"].ToString();
                        em.MaiHang = sdr["MaiHang"].ToString();
                        em.ThoiGianMai = sdr["ThoiGianMai"].ToString();
                        em.LoaiDaMai = sdr["LoaiDaMai"].ToString();
                        em.GanLinhKien = sdr["GanLinhKien"].ToString();
                        em.BaviaKH = sdr["BaviaKH"].ToString();
                        em.BaviaCaiDat = sdr["BaviaCaiDat"].ToString();
                        em.KHYCKM = sdr["KHYCKM"].ToString();
                        em.RacBotSiliconKM = sdr["RacBotSiliconKM"].ToString();
                        em.KTDungCu = sdr["KTDungCu"].ToString();
                        em.KiemPham = sdr["KiemPham"].ToString();
                        em.TyLeNG = sdr["TyLeNG"].ToString();
                        em.GhiChuThayDoiBT = sdr["GhiChuThayDoiBT"].ToString();
                        em.RacBotSiliconDG = sdr["RacBotSiliconDG"].ToString();
                        em.TrongLuong1SP = sdr["TrongLuong1SP"].ToString();
                        em.TrongLuongLinhKien = sdr["TrongLuongLinhKien"].ToString();
                        em.SL1boc = sdr["SL1boc"].ToString();
                        em.KichThuocBoc = sdr["KichThuocBoc"].ToString();
                        em.SL1hop = sdr["SL1hop"].ToString();
                        em.KichThuocThung = sdr["KichThuocThung"].ToString();
                        em.SL1thung = sdr["SL1thung"].ToString();
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
                        em.SoChungTu = sdr["SoChungTu"].ToString();
                        em.NgayDuyet = sdr["NgayDuyet"].ToString();
                        em.NgayPhatHanh = sdr["NgayPhatHanh"].ToString();
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