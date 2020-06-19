using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class DateWorkController : Controller
    {
        private readonly IConfiguration _configuration;

        public DateWorkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string RequestOT([FromBody]EmpDateWorkModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_datework", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@requestID", model.requestID));
                    sc.Parameters.Add(
                        new SqlParameter("@empID", model.empid));
                    sc.Parameters.Add(
                        new SqlParameter("@datework", model.datework));
                    sc.Parameters.Add(
                        new SqlParameter("@dateworkend", model.dateworkend));
                    sc.Parameters.Add(
                       new SqlParameter("@isOT", model.isOT));
                    sc.Parameters.Add(
                        new SqlParameter("@shiftName", model.shiftName));
                    sc.Parameters.Add(
                       new SqlParameter("@intime", DateTime.Now.ToString("HH:mm:ss")));
                    sc.Parameters.Add(
                        new SqlParameter("@indat", model.indat));
                    sc.Parameters.Add(
                        new SqlParameter("@username", model.username));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Insert"));
                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    string result2 = sc.Parameters["@result"].Value.ToString();
                    return result2;
                }
            }
        }

        public List<EmpDateWorkModel> GetDateWork(string empid)
        {
            List<EmpDateWorkModel> lem = new List<EmpDateWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateworkinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetDateWork"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        EmpDateWorkModel em = new EmpDateWorkModel();
                        em.requestID = sdr["requestID"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.dateworkend = sdr["dateworkend"].ToString();
                        em.shiftName = sdr["shiftName"].ToString();
                        em.depName = sdr["depID"].ToString();
                        em.isOT = int.Parse(sdr["isOT"].ToString());
                        em.status = sdr["status"].ToString();
                        em.indat = sdr["indat"].ToString();
                        em.username = sdr["username"].ToString();
                        em.number = int.Parse(sdr["number"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<EmpDateWorkModel> GetDateWorkDetail(string requestID, int isOT, string shiftName)
        {
            List<EmpDateWorkModel> lem = new List<EmpDateWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateworkinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@requestID", requestID));
                    sc.Parameters.Add(
                        new SqlParameter("@isOT", isOT));
                    sc.Parameters.Add(
                        new SqlParameter("@shiftName", shiftName));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetDateWorkDetail"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        EmpDateWorkModel em = new EmpDateWorkModel();
                        em.requestID = sdr["requestID"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.empid = sdr["empid"].ToString();
                        em.dateworkend = sdr["dateworkend"].ToString();
                        em.shiftName = sdr["shiftName"].ToString();
                        em.depName = sdr["depID"].ToString();
                        em.isOT = int.Parse(sdr["isOT"].ToString());
                        em.status = sdr["status"].ToString();
                        em.indat = sdr["indat"].ToString();
                        em.username = sdr["username"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<DateWorkPrintModel> GetDateWorkPrint(string requestID, string shiftName)
        {
            List<DateWorkPrintModel> lem = new List<DateWorkPrintModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateworkinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@requestID", requestID));
                    sc.Parameters.Add(
                        new SqlParameter("@shiftName", shiftName));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetDateWorkPrint"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DateWorkPrintModel em = new DateWorkPrintModel();
                        em.empid = sdr["empid"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.depchildName = sdr["depchildName"].ToString();
                        em.depName = sdr["depName"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.dateworkend = sdr["dateworkend"].ToString();
                        em.shiftName = sdr["shiftName"].ToString();
                        em.timeStart = sdr["timeStart"].ToString();
                        em.timeEnd = sdr["timeEnd"].ToString();              
                        em.username = sdr["username"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<EmpDateWorkModel> GetAllDateWork(string empid)
        {
            List<EmpDateWorkModel> lem = new List<EmpDateWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateworkinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetAllDateWork"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        EmpDateWorkModel em = new EmpDateWorkModel();
                        em.requestID = sdr["requestID"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.dateworkend = sdr["dateworkend"].ToString();
                        em.shiftName = sdr["shiftName"].ToString();
                        em.depName = sdr["depID"].ToString();
                        em.isOT = int.Parse(sdr["isOT"].ToString());
                        em.status = sdr["status"].ToString();
                        em.indat = sdr["indat"].ToString();
                        em.username = sdr["username"].ToString();
                        em.number = int.Parse(sdr["number"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public string GetDateWorkSTT()
        {
            List<EmpDateWorkModel> lem = new List<EmpDateWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");
            string result = "";
            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateworkinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetDateWorkSTT"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        result = sdr[0].ToString();
                    }
                }
                return result;
            }
        }

        public string Update([FromBody]EmpDateWorkModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_datework", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@requestID", value.requestID ));
                    sc.Parameters.Add(
                        new SqlParameter("@isOT", value.isOT));
                    sc.Parameters.Add(
                        new SqlParameter("@shiftName", value.shiftName));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Confirm"));

                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    string result2 = sc.Parameters["@result"].Value.ToString();
                    return result2;

                }
            }
        }

        public string Delete([FromBody]EmpDateWorkModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_datework", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@requestID", value.requestID));
                    sc.Parameters.Add(
                        new SqlParameter("@isOT", value.isOT));
                    sc.Parameters.Add(
                       new SqlParameter("@shiftName", value.shiftName));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Delete"));

                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    string result2 = sc.Parameters["@result"].Value.ToString();
                    return result2;

                }
            }
        }
    }
}