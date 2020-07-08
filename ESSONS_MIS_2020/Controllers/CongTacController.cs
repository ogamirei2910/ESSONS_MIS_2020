using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESSONS_MIS_2020.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Newtonsoft.Json;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class CongTacController : Controller
    {
        private readonly IConfiguration _configuration;

        public CongTacController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult CongTac_Request([FromBody]CongTacModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model.empmodel);
            DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CongTac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@planName", model.planName));
                    sc.Parameters.Add(
                        new SqlParameter("@planDescription", model.planDescription));
                    sc.Parameters.Add(
                        new SqlParameter("@planNumber", model.planNumber));
                    sc.Parameters.Add(
                        new SqlParameter("@udt", pDt));
                    sc.Parameters.Add(
                       new SqlParameter("@planType", model.planType));
                    sc.Parameters.Add(
                        new SqlParameter("@planEstimatedBudget", model.planEstimatedBudget));
                    sc.Parameters.Add(
                        new SqlParameter("@planSpentBudget", model.planSpentBudget));
                    sc.Parameters.Add(
                        new SqlParameter("@planStart", model.planStart));
                    sc.Parameters.Add(
                        new SqlParameter("@planEnd", model.planEnd));
                    sc.Parameters.Add(
                       new SqlParameter("@intime", DateTime.Now.ToString("HH:mm:ss")));
                    sc.Parameters.Add(
                        new SqlParameter("@indat", model.indat));
                    sc.Parameters.Add(
                        new SqlParameter("@username", model.username));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Insert"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        public IActionResult Edit([FromBody]CongTacModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model.empmodel);
            DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CongTac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@congtacID", model.congtacID));
                    sc.Parameters.Add(
                        new SqlParameter("@planName", model.planName));
                    sc.Parameters.Add(
                        new SqlParameter("@planDescription", model.planDescription));
                    sc.Parameters.Add(
                        new SqlParameter("@planNumber", model.planNumber));
                    sc.Parameters.Add(
                        new SqlParameter("@udt", pDt));
                    sc.Parameters.Add(
                       new SqlParameter("@planType", model.planType));
                    sc.Parameters.Add(
                        new SqlParameter("@planEstimatedBudget", model.planEstimatedBudget));
                    sc.Parameters.Add(
                        new SqlParameter("@planSpentBudget", model.planSpentBudget));
                    sc.Parameters.Add(
                        new SqlParameter("@planStart", model.planStart));
                    sc.Parameters.Add(
                        new SqlParameter("@planEnd", model.planEnd));
                    sc.Parameters.Add(
                       new SqlParameter("@intime", DateTime.Now.ToString("HH:mm:ss")));
                    sc.Parameters.Add(
                        new SqlParameter("@indat", DateTime.Now.ToString("dd-MM-yyyy")));
                    sc.Parameters.Add(
                        new SqlParameter("@username", model.username));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Edit"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        public IActionResult Update([FromBody]CongTacModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CongTac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@congtacID", model.congtacID));
                    sc.Parameters.Add(
                        new SqlParameter("@status", model.status));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        public IActionResult Delete([FromBody]CongTacModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_CongTac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@congtacID", model.congtacID));
                    sc.Parameters.Add(
                        new SqlParameter("@status", model.status));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Delete"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();
                }
            }
        }

        public List<CongTacModel> GetCongTac(string empid)
        {
            List<CongTacModel> lem = new List<CongTacModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_congtac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        CongTacModel em = new CongTacModel();
                        em.congtacID = sdr["congtacID"].ToString();
                        em.depName = sdr["depName"].ToString();
                        em.planName = sdr["planName"].ToString();
                        em.planStart = sdr["planStart"].ToString();
                        em.planEnd = sdr["planEnd"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<CongTacModel> GetAllCongTac(string empid)
        {
            List<CongTacModel> lem = new List<CongTacModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_congtac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "SelectAll"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        CongTacModel em = new CongTacModel();
                        em.congtacID = sdr["congtacID"].ToString();
                        em.depName = sdr["depName"].ToString();
                        em.planName = sdr["planName"].ToString();
                        em.planStart = sdr["planStart"].ToString();
                        em.planEnd = sdr["planEnd"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public CongTacModel GetCongTacDetail(string congtacID)
        {
            CongTacModel em = new CongTacModel();
            List<ChildCongTac> lcm = new List<ChildCongTac>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_congtac", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@congtacID", congtacID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "SelectCongTac"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {                  
                        em.congtacID = sdr["congtacID"].ToString();
                        em.depName = sdr["depName"].ToString();
                        em.planName = sdr["planName"].ToString();
                        em.planDescription = sdr["planDescription"].ToString();
                        em.planType = sdr["planType"].ToString();
                        em.planNumber = int.Parse(sdr["planNumber"].ToString());
                        em.planEstimatedBudget = decimal.Parse(sdr["planEstimatedBudget"].ToString());
                        em.planSpentBudget = decimal.Parse(sdr["planSpentBudget"].ToString());
                        em.planKM = int.Parse(sdr["planKM"].ToString());
                        em.planStart = sdr["planStart"].ToString();
                        em.planEnd = sdr["planEnd"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                        em.username = sdr["username"].ToString();
                        ChildCongTac cm = new ChildCongTac();
                        cm.empid = sdr["empid"].ToString();
                        lcm.Add(cm);
                    }
                    em.empmodel = lcm;
                }
                return em;
            }
        }
    }
}