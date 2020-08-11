using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class CTPCController : Controller
    {
        private readonly IConfiguration _configuration;

        public CTPCController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetSoThe()
        {
            string SoThe = "";
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetSoThe"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        SoThe = sdr["SoThe"].ToString();
                    }
                }
                return SoThe;
            }
        }
        public List<CongThucPhaChe> GetSoTheHC()
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            List<CongThucPhaChe> lpc = new List<CongThucPhaChe>();
            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetSoTheHC"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        CongThucPhaChe pc = new CongThucPhaChe();
                        pc.sothe = sdr["SoThe"].ToString();
                        lpc.Add(pc);
                    }
                }
                return lpc;
            }
        }
        public List<CongThucPhaChe> GetAllCTPC()
        {
            List<CongThucPhaChe> ldm = new List<CongThucPhaChe>();
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "SelectAll"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        CongThucPhaChe dm = new CongThucPhaChe();
                        dm.sothe = sdr["sothe"].ToString();
                        dm.chatkeo = sdr["chatkeo"].ToString();
                        dm.MaKeo1 = sdr["MaKeo1"].ToString();
                        dm.MaKeo2 = sdr["MaKeo2"].ToString();
                        ldm.Add(dm);
                    }
                }
            }
            return ldm;
        }
        public CongThucPhaChe CTPC_GetMaKeo(string sothe)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            CongThucPhaChe em = new CongThucPhaChe();

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoThe", sothe));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetMaKeo"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        em.MaKeo1 = sdr["MaKeo1"].ToString();
                        em.MaKeo2 = sdr["MaKeo2"].ToString();
                    }
                }
                return em;
            }
        }
        public CongThucPhaChe CTPC_Detail(string SoThe)
        {
            CongThucPhaChe dm = new CongThucPhaChe();
            List<CongThucPhaCheK> lcptck = new List<CongThucPhaCheK>();
            List<CongThucPhaCheE> lcptce = new List<CongThucPhaCheE>();
            string connection = _configuration.GetConnectionString("DefaultConnection3");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {                    
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoThe", SoThe));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "SelectCTPCK"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        dm.sothe = sdr["sothe"].ToString();
                        dm.chatkeo = sdr["chatkeo"].ToString();
                        CongThucPhaCheK cptck = new CongThucPhaCheK();
                        cptck.MaKeo1 = sdr["MaKeo1"].ToString();
                        cptck.MaKeo2 = sdr["MaKeo2"].ToString();
                        cptck.KhoiLuongMK1 = double.Parse(sdr["KhoiLuongMK1"].ToString());
                        cptck.KhoiLuongMK2 = double.Parse(sdr["KhoiLuongMK2"].ToString());
                        cptck.TenHoaChatMK1 = sdr["TenHoaChatMK1"].ToString();
                        cptck.TenHoaChatMK2 = sdr["TenHoaChatMK2"].ToString();
                        cptck.STT = int.Parse(sdr["STT"].ToString());
                        lcptck.Add(cptck);
                    }
                    sdr.Close();
                }

                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@SoThe", SoThe));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "SelectCTPCE"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        CongThucPhaCheE cptce = new CongThucPhaCheE();
                        cptce.MaKeo1 = sdr["MaKeo1"].ToString();
                        cptce.MaKeo2 = sdr["MaKeo2"].ToString();
                        cptce.KhoiLuongMK1 = double.Parse(sdr["KhoiLuongMK1"].ToString());
                        cptce.KhoiLuongMK2 = double.Parse(sdr["KhoiLuongMK2"].ToString());
                        cptce.TenHoaChatMK1 = sdr["TenHoaChatMK1"].ToString();
                        cptce.TenHoaChatMK2 = sdr["TenHoaChatMK2"].ToString();
                        cptce.STT = int.Parse(sdr["STT"].ToString());
                        lcptce.Add(cptce);
                    }
                }
                sql.Close();


                if (lcptck.Count < 12)
                {
                    for (int i = lcptck.Count; i < 12; i++)
                    {
                        CongThucPhaCheK cptck = new CongThucPhaCheK();
                        cptck.STT = i + 1;
                        lcptck.Add(cptck);
                    }
                }
                if (lcptce.Count < 14)
                {
                    for (int i = lcptce.Count; i < 14; i++)
                    {
                        CongThucPhaCheE cptce = new CongThucPhaCheE();
                        cptce.STT = i + 1;
                        lcptce.Add(cptce);
                    }
                }

                dm.ctpce = lcptce;
                dm.ctpck = lcptck;
            }
            return dm;
        }
        public ActionResult CTPC_Insert([FromBody]CongThucPhaChe model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model.ctpck);
            DataTable ctpck = JsonConvert.DeserializeObject<DataTable>(json);
            json = Newtonsoft.Json.JsonConvert.SerializeObject(model.ctpce);
            DataTable ctpce = JsonConvert.DeserializeObject<DataTable>(json);

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoThe", model.sothe));
                    sc.Parameters.Add(
                        new SqlParameter("@ChatKeo", model.chatkeo));
                    sc.Parameters.Add(
                        new SqlParameter("@CTPCK", ctpck));
                    sc.Parameters.Add(
                        new SqlParameter("@CTPCE", ctpce));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Insert"));
                    try
                    {
                        SqlDataReader sdr = sc.ExecuteReader();
                        if (sdr.RecordsAffected > 0)
                            return Ok();
                        else return NotFound();
                    }
                    catch (Exception ex) { return NotFound(); }                
                }
            }
        }
        public ActionResult CTPC_Update([FromBody]CongThucPhaChe model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection3");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model.ctpck);
            DataTable ctpck = JsonConvert.DeserializeObject<DataTable>(json);
            json = Newtonsoft.Json.JsonConvert.SerializeObject(model.ctpce);
            DataTable ctpce = JsonConvert.DeserializeObject<DataTable>(json);

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CTPC", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@SoThe", model.sothe));
                    sc.Parameters.Add(
                        new SqlParameter("@ChatKeo", model.chatkeo));
                    sc.Parameters.Add(
                        new SqlParameter("@CTPCK", ctpck));
                    sc.Parameters.Add(
                        new SqlParameter("@CTPCE", ctpce));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    try
                    {
                        SqlDataReader sdr = sc.ExecuteReader();
                        if (sdr.RecordsAffected > 0)
                            return Ok();
                        else return NotFound();
                    }
                    catch (Exception ex) { return NotFound(); }
                }
            }
        }
    }
}