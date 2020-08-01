using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http;
using System.Data;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody]UserModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");
           
            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_checkLogin", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@username", value.username));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if(sdr.HasRows == true)
                        return Ok();
                    else return NotFound();

                }
            }
        }

        [HttpGet("{empid}")]
        public List<UserRoleModel> GetRole(string empid)
        {
            List<UserRoleModel> em = new List<UserRoleModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_userrole", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empid", empid));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "NEW"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        UserRoleModel urm = new UserRoleModel();
                        urm.empName = sdr["empName"].ToString();
                        urm.depName = sdr["depID"].ToString();
                        urm.empID = sdr["empID"].ToString();
                        urm.empImage = sdr["empImage"].ToString();
                        urm.roleID = int.Parse(sdr["roleID"].ToString());
                        urm.folderID = int.Parse(sdr["folderID"].ToString());
                        urm.folderChildID = int.Parse(sdr["folderChildID"].ToString());
                        em.Add(urm);
                    }
                }
                return em;
            }
        }

        [HttpGet("{empid}")]
        public List<UserRoleModel> GetRoleCN(string empid)
        {
            List<UserRoleModel> em = new List<UserRoleModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_userrole", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empid", empid));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        UserRoleModel urm = new UserRoleModel();
                        urm.empName = sdr["empName"].ToString();
                        urm.empID = sdr["empID"].ToString();
                        urm.empImage = sdr["empImage"].ToString();
                        try
                        {
                            urm.roleID = int.Parse(sdr["roleID"].ToString());
                            urm.folderID = int.Parse(sdr["folderID"].ToString());
                            urm.folderChildID = int.Parse(sdr["folderChildID"].ToString());
                        }
                        catch { }
                        em.Add(urm);
                    }
                }
                return em;
            }
        }

        public List<UserFolderModel> GetAllPer()
        {
            List<UserFolderModel> em = new List<UserFolderModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_userfolder", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        UserFolderModel um = new UserFolderModel();
                        um.empID = sdr["empID"].ToString();
                        um.empName = sdr["empName"].ToString();
                        um.depName = sdr["depName"].ToString();
                        um.depchildName = sdr["depchildName"].ToString();
                        um.requestOT = int.Parse(sdr["requestOT"].ToString());
                        um.confirmOT = int.Parse(sdr["confirmOT"].ToString());
                        um.confirmDO = int.Parse(sdr["confirmDO"].ToString());
                        um.confirmCT = int.Parse(sdr["confirmCT"].ToString());
                        em.Add(um);
                    }
                }
                return em;
            }
        }

        [HttpPost]
        public string SetPer([FromBody]UserFolderModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_userfolder", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", value.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@depID", value.depName));
                    sc.Parameters.Add(
                        new SqlParameter("@depchildID", value.depchildName));
                    sc.Parameters.Add(
                        new SqlParameter("@isRequestOT", value.requestOT));
                    sc.Parameters.Add(
                        new SqlParameter("@isConfirmOT", value.confirmOT));
                    sc.Parameters.Add(
                        new SqlParameter("@isConfirmDO", value.confirmDO));
                    sc.Parameters.Add(
                        new SqlParameter("@isConfirmCT", value.confirmCT));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Update"));
                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    string result2 = sc.Parameters["@result"].Value.ToString();
                    return result2;
                }
            }
        }

        [HttpPost]
        public string Block([FromBody]UserFolderModel value)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_userfolder", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@empID", value.empID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "Block"));
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