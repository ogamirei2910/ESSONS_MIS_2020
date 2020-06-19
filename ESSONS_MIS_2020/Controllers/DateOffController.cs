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
                        em.dateoffStartTime = sdr["dateoffStartTime"].ToString();
                        em.dateoffEndTime = sdr["dateoffEndTime"].ToString();
                        em.dateoffNumber = Double.Parse(sdr["dateoffNumber"].ToString());
                        switch (sdr["dateoffType"].ToString())
                        {
                            case "1": em.dateoffType = "Phép năm"; break;
                            case "2": em.dateoffType = "Nghỉ bệnh"; break;
                            case "3": em.dateoffType = "Nghỉ thai sản"; break;
                            case "4": em.dateoffType = "Việc riêng"; break;
                            case "10": em.dateoffType = "Nghỉ vợ sinh/khám/dưỡng"; break;
                            case "11": em.dateoffType = "Nghỉ kết hôn"; break;
                            case "12": em.dateoffType = "Nghỉ tang gia"; break;
                            case "13": em.dateoffType = "Nghỉ con bệnh"; break;
                            case "14": em.dateoffType = "Trực biệt thự"; break;
                            case "15": em.dateoffType = "Đi công tác"; break;
                        }
                        em.username = sdr["username"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<DateOffModel> GetAllEmp()
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
                        new SqlParameter("@type", "GetAllEmp"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DateOffModel em = new DateOffModel();
                        em.empID = sdr["empID"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                        em.dateoffID = sdr["dateoffID"].ToString();
                        em.dateoffStart = sdr["dateoffStart"].ToString();
                        em.dateoffEnd = sdr["dateoffEnd"].ToString();
                        em.dateoffStartTime = sdr["dateoffStartTime"].ToString();
                        em.dateoffEndTime = sdr["dateoffEndTime"].ToString();
                        em.dateoffNumber = Double.Parse(sdr["dateoffNumber"].ToString());
                        switch (sdr["dateoffType"].ToString())
                        {
                            case "1": em.dateoffType = "Phép năm"; break;
                            case "2": em.dateoffType = "Nghỉ bệnh"; break;
                            case "3": em.dateoffType = "Nghỉ thai sản"; break;
                            case "4": em.dateoffType = "Việc riêng"; break;
                            case "10": em.dateoffType = "Nghỉ vợ sinh/khám/dưỡng"; break;
                            case "11": em.dateoffType = "Nghỉ kết hôn"; break;
                            case "12": em.dateoffType = "Nghỉ tang gia"; break;
                            case "13": em.dateoffType = "Nghỉ con bệnh"; break;
                            case "14": em.dateoffType = "Trực biệt thự"; break;
                            case "15": em.dateoffType = "Đi công tác"; break;
                        }
                        em.username = sdr["username"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        [HttpGet("{empID}")]
        public DateOffInfoModel GetDateOffInfo(string empID)
        {
            DateOffInfoModel em = new DateOffInfoModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoffinfo", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", empID));
                    SqlParameter result = new SqlParameter("@phepnam", SqlDbType.Decimal, 5);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    result = new SqlParameter("@phepnamhienco", SqlDbType.Decimal, 5);
                    result.Scale = 2;
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    result = new SqlParameter("@phepnamdadung", SqlDbType.Decimal, 5);
                    result.Scale = 2;
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    result = new SqlParameter("@BHXH", SqlDbType.Decimal, 5);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    result = new SqlParameter("@pheprieng", SqlDbType.Decimal, 5);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    result = new SqlParameter("@khongphep", SqlDbType.Decimal, 5);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);
                    sc.ExecuteNonQuery();

                    em.pheprieng = double.Parse(sc.Parameters["@pheprieng"].Value.ToString());
                    em.phepnam = double.Parse(sc.Parameters["@phepnam"].Value.ToString());
                    em.phepnamhienco = double.Parse(sc.Parameters["@phepnamhienco"].Value.ToString());
                    em.phepnamdadung = double.Parse(sc.Parameters["@phepnamdadung"].Value.ToString());
                    em.BHXH = double.Parse(sc.Parameters["@BHXH"].Value.ToString());
                    em.khongphep = double.Parse(sc.Parameters["@khongphep"].Value.ToString());
                }
                return em;
            }
        }

        [HttpGet]
        public DateOffModel Get()
        {
            DateOffModel em = new DateOffModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoff", sql))
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

        [HttpGet("{empID}")]
        public List<DateOffModel> GetEmpConfirm(string empID)
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
                        new SqlParameter("@type", "GetEmpConfirm"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DateOffModel em = new DateOffModel();
                        em.empID = sdr["empID"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.dateoffID = sdr["dateoffID"].ToString();
                        em.dateoffStart = sdr["dateoffStart"].ToString();
                        em.dateoffEnd = sdr["dateoffEnd"].ToString();
                        em.dateoffStartTime = sdr["dateoffStartTime"].ToString();
                        em.dateoffEndTime = sdr["dateoffendTime"].ToString();
                        em.dateoffNumber = Double.Parse(sdr["dateoffNumber"].ToString());
                        switch(sdr["dateoffType"].ToString())
                        {
                            case "1": em.dateoffType = "Phép năm"; break;
                            case "2": em.dateoffType = "Nghỉ ốm đau (BHXH)"; break;
                            case "3": em.dateoffType = "Nghỉ thai sản (BHXH)"; break;
                            case "4": em.dateoffType = "Việc riêng"; break;
                            case "5": em.dateoffType = "Nghỉ việc"; break;
                        }
                        
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<DateOffExceptionModel> DateOffException()
        {
            List<DateOffExceptionModel> lem = new List<DateOffExceptionModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoffexception", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DateOffExceptionModel em = new DateOffExceptionModel();
                        em.empid = sdr["empID"].ToString();
                        var date = sdr["datework"].ToString();
                        var parsedDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        var formattedDate = parsedDate.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        em.datework = formattedDate;
                        em.dateoffExName = sdr["dateoffExName"].ToString();
                        em.comment = sdr["comment"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());

                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public DateOffExceptionModel DateOffExceptionID(string empid, string workdate)
        {
            DateOffExceptionModel em = new DateOffExceptionModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoffexception", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                       new SqlParameter("@workdate", workdate));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Get"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {                      
                        em.empid = sdr["empID"].ToString();
                        var date = sdr["datework"].ToString();
                        var parsedDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        var formattedDate = parsedDate.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        em.datework = formattedDate;
                        em.dateoffExName = sdr["dateoffExName"].ToString();
                        em.comment = sdr["comment"].ToString();
                        em.status = int.Parse(sdr["status"].ToString());
                    }
                }
                return em;
            }
        }

        public List<TimeWorkModel> GetTimeWork(string empid, string workdate)
        {
            List<TimeWorkModel> lem = new List<TimeWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_timework", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                       new SqlParameter("@datework", workdate));
                    sc.Parameters.Add(
                       new SqlParameter("@type", "BuPhep"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        TimeWorkModel em = new TimeWorkModel();
                        em.depID = sdr["depID"].ToString();
                        em.shiftName = sdr["ShiftName"].ToString();
                        em.timestart = sdr["timestart"].ToString();
                        em.timeend = sdr["timeend"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        public List<TimeWorkModel> GetTimeWorkRequest(string empid)
        {
            List<TimeWorkModel> lem = new List<TimeWorkModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_timework", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                       new SqlParameter("@type", "RequestOT"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        TimeWorkModel em = new TimeWorkModel();
                        em.depID = sdr["depID"].ToString();
                        em.shiftName = sdr["ShiftName"].ToString();
                        em.timestart = sdr["timestart"].ToString();
                        em.timeend = sdr["timeend"].ToString();
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
        [HttpPost]
        public DateOffModel BuPhep([FromBody]DateOffModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_BuPhep", sql))
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
                       new SqlParameter("@dateoffStartTime", model.dateoffStartTime));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffEndTime", model.dateoffEndTime));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffNumber", model.dateoffNumber));
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffType", model.dateoffType));
                    sc.Parameters.Add(
                        new SqlParameter("@username", model.username));
                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar,50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    model.result = sc.Parameters["@result"].Value.ToString();
                    return model;
                }
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
                using (SqlCommand sc = new SqlCommand("sp_dateoff", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffID", value.dateoffID));
                    sc.Parameters.Add(
                        new SqlParameter("@status", value.status));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();

                }
            }
        }

        public IActionResult Delete([FromBody]DateOffModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_dateoff", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@dateoffID", value.dateoffID));
                    sc.Parameters.Add(
                        new SqlParameter("@status", value.status));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Delete"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.RecordsAffected > 0)
                        return Ok();
                    else return NotFound();

                }
            }
        }
    }
}