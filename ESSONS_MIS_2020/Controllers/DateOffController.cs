using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class DateOffController : Controller
    {
        private readonly IConfiguration _configuration;

        public DateOffController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{empID}")]
        public List<DateOffModel> GetEmpID(string empID)
        {
            List<DateOffModel> lem = new List<DateOffModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoff", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", empID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetEmpID"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DateOffModel em = new DateOffModel();
                        em.empID = sdr["empID"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                        em.dateoffID = sdr["dateoffID"].ToString();
                        em.dateoffStart = sdr["dateoffStart"].ToString();
                        em.dateoffEnd = sdr["dateoffEnd"].ToString();
                        em.dateoffNumber = Double.Parse(sdr["dateoffNumber"].ToString());
                        em.dateoffType = sdr["dateoffType"].ToString();
                        em.username = sdr["username"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        [HttpGet]
        public DateOffModel Get()
        {
            DateOffModel em = new DateOffModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        em.empID = sdr["empID"].ToString();
                        em.dateoffStart = sdr["dateoffStart"].ToString();
                        em.dateoffEnd = sdr["dateoffEnd"].ToString();
                        em.dateoffNumber = Double.Parse(sdr["dateoffNumber"].ToString());
                        em.dateoffType = sdr["dateoffType"].ToString();
                        em.username = sdr["username"].ToString();
                    }
                }
                return em;
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]DateOffModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoff", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", model.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffStart", model.dateoffStart));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffEnd", model.dateoffEnd));
                    sc.Parameters.Add(
                       new SqlParameter("@dateoffTimeStart", model.dateoffStartTime));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffTimeEnd", model.dateoffEndTime));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffNumber", model.dateoffNumber));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffType", model.dateoffType));
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

        public IActionResult Update([FromBody]DateOffModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", value.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@status", value.status));
                    sc.Parameters.Add(
                        new SqlParameter("@username", value.username));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();

                }
            }
        }
    }
}