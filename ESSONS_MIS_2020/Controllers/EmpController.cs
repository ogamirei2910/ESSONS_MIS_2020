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
        public EmpModel GetEmpID(string empID)
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
                        em.empID = sdr["empID"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.empIDTemp = sdr["empIDTemp"].ToString();
                        em.empIdentityCard = sdr["empIdentityCard"].ToString();
                        em.depID = sdr["depName"].ToString();
                        em.empAddress = sdr["empAddress"].ToString();
                        em.empBirthDay = sdr["empBirthDay"].ToString();
                        em.empMarriage = sdr["empMarriage"].ToString();
                        em.empTel = sdr["empTel"].ToString();
                        em.empSex = sdr["empSex"].ToString();
                        em.empEdu = sdr["empEdu"].ToString();
                        em.empInDate = sdr["empInDate"].ToString();
                        em.empStandardDate = sdr["empStandardDate"].ToString();
                        em.empLeaveDate = sdr["empLeaveDate"].ToString();
                        em.positionID = sdr["positionName"].ToString();
                        em.empImage = sdr["empImage"].ToString();

                        if(sdr["groupName"] != null)
                            em.empIDManager = sdr["groupName"].ToString();
                        em.depchildName = sdr["depchildName"].ToString();
                        em.positionName = sdr["positionName"].ToString();
                        em.depchildID = sdr["depchildName"].ToString();
                        em.empAddressTemp = sdr["empAddressTemp"].ToString();
                        em.empBankNo = sdr["empBankNo"].ToString();
                        em.empBankName = sdr["empBankName"].ToString();
                        em.empBirthCertificate = int.Parse(sdr["empBirthCertificate"].ToString());
                        em.empBorn = sdr["empBorn"].ToString();
                        em.empChild = int.Parse(sdr["empChild"].ToString());
                        em.empComputer = sdr["empComputer"].ToString();
                        em.empCultural = int.Parse(sdr["empCultural"].ToString());
                        em.empDomicile = sdr["empDomicile"].ToString();
                        em.empIdentityPlace = sdr["empIdentityPlace"].ToString();
                        em.empIdentityDate = sdr["empIdentityDate"].ToString();
                        em.empHouseHold = sdr["empHouseHold"].ToString();
                        em.empHouseHoldOwn = sdr["empHouseHoldOwn"].ToString();
                        em.empLanguage = sdr["empLanguage"].ToString();
                        em.empNation = sdr["empNation"].ToString();
                        em.empProvince = int.Parse(sdr["empProvince"].ToString());
                        em.empReligion = sdr["empReligion"].ToString();
                        //em.empYearOff = int.Parse(sdr["empYearOff"].ToString());
                    }
                }
                return em;
            }
        }

        public string GetEmpSTT()
        {
            string empID = "";
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetEmpSTT"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        empID = sdr["STT"].ToString();
                    }
                }
                return empID;
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
                        emc.empID = sdr["empID"].ToString();
                        if(sdr["empIDTemp"] != null)
                            emc.empIDTemp = sdr["empIDTemp"].ToString();
                        emc.empName = sdr["empName"].ToString();
                        emc.positionName = sdr["positionName"].ToString();
                        emc.empIdentityCard = sdr["empIdentityCard"].ToString();

                        em.Add(emc);
                    }
                }
                return em;
            }
        }

        [HttpGet("{empID}")]
        public PositionDepEmpModel GetPositionDepEmp(string empid)
        {
            PositionDepEmpModel dm = new PositionDepEmpModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_positiondepemp", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        dm.empID = sdr["empID"].ToString();
                        dm.positionID = sdr["positionID"].ToString();
                        dm.positionName = sdr["positionName"].ToString();
                        dm.depID = sdr["depID"].ToString();
                        dm.depchildID = sdr["depchildID"].ToString();
                        dm.depchildName = sdr["depchildName"].ToString();
                        dm.comment = sdr["comment"].ToString();
                        dm.confirmDate = sdr["confirmDate"].ToString();
                        dm.status = int.Parse(sdr["status"].ToString());
                        dm.typeName = sdr["typeName"].ToString();
                        dm.username = sdr["username"].ToString();
                    }
                }
            }
            return dm;
        }

        [HttpGet]
        public List<DepartmentModel> GetDepartment()
        {
            List<DepartmentModel> ldm = new List<DepartmentModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_department", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DepartmentModel dm = new DepartmentModel();
                        dm.depID = sdr["depID"].ToString();
                        dm.depName = sdr["depName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }
            return ldm;
        }

        [HttpGet]
        public List<EmpModel> GetEmpManager(string depID)
        {
            List<EmpModel> ldm = new List<EmpModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@depID", depID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetEmpManager"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        EmpModel dm = new EmpModel();
                        dm.empIDManager = sdr["empID"].ToString();
                        dm.empNameManager = sdr["empName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }
            return ldm;
        }

        [HttpGet]
        public List<EmpModel> GetEmpInManager(string empid)
        {
            List<EmpModel> ldm = new List<EmpModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@empID", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetEmpInManager"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        EmpModel dm = new EmpModel();
                        dm.empID = sdr["empID"].ToString();
                        dm.empIDTemp = sdr["empIDTemp"].ToString();
                        dm.empName = sdr["empName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }
            return ldm;
        }

        [HttpGet]
        public List<DepartmentModel> GetDepartmentID(string depID)
        {
            List<DepartmentModel> ldm = new List<DepartmentModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_department", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@depID", depID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetDepID"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DepartmentModel dm = new DepartmentModel();
                        dm.depID = sdr["depID"].ToString();
                        dm.depName = sdr["depName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }
            return ldm;
        }

        [HttpGet]
        public IEnumerable<DepartmentChildModel> GetDepartmentChild(string depID)
        {
            List<DepartmentChildModel> ldm = new List<DepartmentChildModel>();

            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_departmentchild", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@depID", depID));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        DepartmentChildModel dm = new DepartmentChildModel();
                        dm.DepChildID = sdr["depchildID"].ToString();
                        dm.DepChildName = sdr["depchildName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }

            return ldm;
        }

        [HttpGet]
        public List<PositionModel> GetPosition(string depchildID)
        {
            List<PositionModel> ldm = new List<PositionModel>();

            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_position", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@depchildID", depchildID));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        PositionModel dm = new PositionModel();
                        dm.positionID = sdr["positionID"].ToString();
                        dm.positionName = sdr["positionName"].ToString();
                        ldm.Add(dm);
                    }
                }
            }

            return ldm;
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
                        new SqlParameter("@depID", model.depID));
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
                        new SqlParameter("@empYearOff", model.empYearOff));
                    sc.Parameters.Add(
                       new SqlParameter("@empIDManager", model.empIDManager));

                    sc.Parameters.Add(
                        new SqlParameter("@depchildID", model.depchildID));
                    sc.Parameters.Add(
                       new SqlParameter("@depchildName", model.depchildName));
                    sc.Parameters.Add(
                       new SqlParameter("@positionName", model.positionName));
                    sc.Parameters.Add(
                       new SqlParameter("@empIdentityPlace", model.empIdentityPlace));
                    sc.Parameters.Add(
                       new SqlParameter("@empIdentityDate", model.empIdentityDate));
                    sc.Parameters.Add(
                       new SqlParameter("@empNation", model.empNation));
                    sc.Parameters.Add(
                       new SqlParameter("@empReligion", model.empReligion));
                    sc.Parameters.Add(
                       new SqlParameter("@empBorn", model.empBorn));
                    sc.Parameters.Add(
                       new SqlParameter("@empDomicile", model.empDomicile));
                    sc.Parameters.Add(
                       new SqlParameter("@empAddressTemp", model.empAddressTemp));
                    sc.Parameters.Add(
                       new SqlParameter("@empCultural", model.empCultural));
                    sc.Parameters.Add(
                       new SqlParameter("@empLanguage", model.empLanguage));
                    sc.Parameters.Add(
                       new SqlParameter("@empComputer", model.empComputer));
                    sc.Parameters.Add(
                       new SqlParameter("@empChild", model.empChild));
                    sc.Parameters.Add(
                       new SqlParameter("@empBirthCertificate", model.empBirthCertificate));
                    sc.Parameters.Add(
                       new SqlParameter("@empHouseHold", model.empHouseHold));
                    sc.Parameters.Add(
                       new SqlParameter("@empHouseHoldOwn", model.empHouseHoldOwn));
                    sc.Parameters.Add(
                       new SqlParameter("@empProvince", model.empProvince));
                    sc.Parameters.Add(
                       new SqlParameter("@empBankNo", model.empBankNo));
                    sc.Parameters.Add(
                       new SqlParameter("@empBankName", model.empBankName));

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
                        new SqlParameter("@empIDTemp", value.empIDTemp));
                    sc.Parameters.Add(
                        new SqlParameter("@empName", value.empName));
                    sc.Parameters.Add(
                        new SqlParameter("@empIdentityCard", value.empIdentityCard));
                    sc.Parameters.Add(
                        new SqlParameter("@depID", value.depID));
                    sc.Parameters.Add(
                       new SqlParameter("@depchildID", value.depchildID));
                    sc.Parameters.Add(
                      new SqlParameter("@depchildName", value.depchildName));
                    sc.Parameters.Add(
                       new SqlParameter("@positionName", value.positionName));
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
                        new SqlParameter("@empStandardDate", value.empStandardDate));
                    sc.Parameters.Add(
                        new SqlParameter("@positionID", value.positionID));
                    sc.Parameters.Add(
                        new SqlParameter("@empImage", value.empImage));
                    sc.Parameters.Add(
                       new SqlParameter("@indat", value.indat));
                    sc.Parameters.Add(
                        new SqlParameter("@username", value.username));
                    sc.Parameters.Add(
                        new SqlParameter("@empYearOff", value.empYearOff));
                    sc.Parameters.Add(
                        new SqlParameter("@empIDManager", value.empIDManager));

                    sc.Parameters.Add(
                       new SqlParameter("@empIdentityPlace", value.empIdentityPlace));
                    sc.Parameters.Add(
                       new SqlParameter("@empIdentityDate", value.empIdentityDate));
                    sc.Parameters.Add(
                       new SqlParameter("@empNation", value.empNation));
                    sc.Parameters.Add(
                       new SqlParameter("@empReligion", value.empReligion));
                    sc.Parameters.Add(
                       new SqlParameter("@empBorn", value.empBorn));
                    sc.Parameters.Add(
                       new SqlParameter("@empDomicile", value.empDomicile));
                    sc.Parameters.Add(
                       new SqlParameter("@empAddressTemp", value.empAddressTemp));
                    sc.Parameters.Add(
                       new SqlParameter("@empCultural", value.empCultural));
                    sc.Parameters.Add(
                       new SqlParameter("@empLanguage", value.empLanguage));
                    sc.Parameters.Add(
                       new SqlParameter("@empComputer", value.empComputer));
                    sc.Parameters.Add(
                       new SqlParameter("@empChild", value.empChild));
                    sc.Parameters.Add(
                       new SqlParameter("@empBirthCertificate", value.empBirthCertificate));
                    sc.Parameters.Add(
                       new SqlParameter("@empHouseHold", value.empHouseHold));
                    sc.Parameters.Add(
                       new SqlParameter("@empHouseHoldOwn", value.empHouseHoldOwn));
                    sc.Parameters.Add(
                       new SqlParameter("@empProvince", value.empProvince));
                    sc.Parameters.Add(
                       new SqlParameter("@empBankNo", value.empBankNo));
                    sc.Parameters.Add(
                       new SqlParameter("@empBankName", value.empBankName));
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
        public IActionResult Block([FromBody]EmpModel em)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", em.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Block"));

                    SqlDataReader sdr = sc.ExecuteReader();
                
                }
                return NoContent();
            }
        }

        [HttpPost]
        public IActionResult LoginCongNhan([FromBody]EmpModel em)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_employee", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", em.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "GetEmpID"));

                    SqlDataReader sdr = sc.ExecuteReader();
                    if (sdr.HasRows == true)
                        return Ok();
                    else
                        return NotFound();
                }
            }
        }

    }
}