using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class EmpController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{empID}")]
        public EmpModel GetEmpID(int empID)
        {
            EmpModel em = new EmpModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
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
                        em.empID = int.Parse(sdr["empID"].ToString());
                        em.empName = sdr["empName"].ToString();
                        em.empIDTemp = sdr["empIDTemp"].ToString();
                        em.empIdentityCard = sdr["empIdentityCard"].ToString();
                        em.depID = sdr["depID"].ToString();
                        em.empAddress = sdr["empAddress"].ToString();
                        em.empBirthDay = sdr["empBirthDay"].ToString();
                        em.empMarriage = sdr["empMarriage"].ToString();
                        em.empTel = sdr["empTel"].ToString();
                        em.empSex = sdr["empSex"].ToString();
                        em.empEdu = sdr["empEdu"].ToString();
                        em.empInDate = sdr["empInDate"].ToString();
                        em.empStandardDate = sdr["empStandardDate"].ToString();
                        em.empLeaveDate = sdr["empLeaveDate"].ToString();
                        em.positionID = sdr["positionID"].ToString();
                    }
                }
                return em;
            }
        }

        [HttpGet]
        [Route("")]
        public List<EmpModel> Get()
        {
            List<EmpModel> em = new List<EmpModel>();
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
                        EmpModel emc = new EmpModel();
                        emc.empID = int.Parse(sdr["empID"].ToString());
                        emc.empName = sdr["empName"].ToString();
                        emc.depID = sdr["depID"].ToString();
                        emc.empIdentityCard = sdr["empIdentityCard"].ToString();

                        em.Add(emc);
                    }
                }
                return em;
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]EmpModel model)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", model.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@empIDTemp", model.empIDTemp));
                    sc.Parameters.Add(
                        new SqlParameter("@empName", model.empName));
                    sc.Parameters.Add(
                        new SqlParameter("@empIdentityCard", model.empIdentityCard));
                    sc.Parameters.Add(
                        new SqlParameter("@empDepno", model.depID));
                    sc.Parameters.Add(
                        new SqlParameter("@empAddress", model.empAddress));
                    sc.Parameters.Add(
                        new SqlParameter("@empBirthDay", model.empBirthDay));
                    sc.Parameters.Add(
                        new SqlParameter("@empMarriage", model.empMarriage));
                    sc.Parameters.Add(
                        new SqlParameter("@empTel", model.empTel));
                    sc.Parameters.Add(
                        new SqlParameter("@empSex", model.empSex));
                    sc.Parameters.Add(
                        new SqlParameter("@empEdu", model.empEdu));
                    sc.Parameters.Add(
                        new SqlParameter("@empInDate", model.empInDate));
                    sc.Parameters.Add(
                        new SqlParameter("@positionID", model.positionID));
                    sc.Parameters.Add(
                        new SqlParameter("@empImage", model.empImage));
                    sc.Parameters.Add(
                       new SqlParameter("@status", model.status));
                    sc.Parameters.Add(
                        new SqlParameter("@indat", model.indat));
                    sc.Parameters.Add(
                        new SqlParameter("@intime", model.intime));
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

        [HttpPost]
        public IActionResult Update([FromBody]EmpModel value)
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
                        new SqlParameter("@empName", value.empName));
                    sc.Parameters.Add(
                        new SqlParameter("@empIdentityCard", value.empIdentityCard));
                    sc.Parameters.Add(
                        new SqlParameter("@empDepno", value.depID));
                    sc.Parameters.Add(
                        new SqlParameter("@empAddress", value.empAddress));
                    sc.Parameters.Add(
                        new SqlParameter("@empBirthDay", value.empBirthDay));
                    sc.Parameters.Add(
                        new SqlParameter("@empMarriage", value.empMarriage));
                    sc.Parameters.Add(
                        new SqlParameter("@empTel", value.empTel));
                    sc.Parameters.Add(
                        new SqlParameter("@empSex", value.empSex));
                    sc.Parameters.Add(
                        new SqlParameter("@empEdu", value.empEdu));
                    sc.Parameters.Add(
                        new SqlParameter("@empInDate", value.empInDate));
                    sc.Parameters.Add(
                        new SqlParameter("@positionID", value.positionID));
                    sc.Parameters.Add(
                        new SqlParameter("@empImage", value.empImage));
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

        [HttpPost]
        public IActionResult Delete([FromBody]EmpModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@username", value.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Block"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.HasRows == true)
                        return Ok();
                    else return NotFound();

                }
            }
        }

    }
}