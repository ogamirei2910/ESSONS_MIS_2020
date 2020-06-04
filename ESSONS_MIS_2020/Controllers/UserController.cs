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

        [HttpGet("{username}")]
        public List<UserRoleModel> GetRole(string username)
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
                        new SqlParameter("@username", username));
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

        public List<UserRoleModel> GetAllRole()
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
                       new SqlParameter("@type", "GetAll"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        UserRoleModel urm = new UserRoleModel();
                        urm.empName = sdr["empName"].ToString();
                        urm.depName = sdr["depID"].ToString();
                        urm.empID = sdr["empID"].ToString();
                        em.Add(urm);
                    }
                }
                return em;
            }
        }

        [HttpGet]
        public UserRoleModel GetFolder()
        {
            UserRoleModel em = new UserRoleModel();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_folder", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        FolderModel fm = new FolderModel();
                        fm.folderID = int.Parse(sdr["folderID"].ToString());
                        fm.folderName = sdr["folderName"].ToString();
                        em.folderList.Add(fm);
                    }
                    sdr.Close();
                    sql.Close();               
                }

                using (SqlCommand sc = new SqlCommand("sp_folderchild", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        FolderChildModel fm = new FolderChildModel();
                        fm.folderID = int.Parse(sdr["folderID"].ToString());
                        fm.folderChildID = int.Parse(sdr["folderName"].ToString());
                        fm.folderChildName = sdr["folderChildName"].ToString();
                        em.folderchildList.Add(fm);
                    }
                    sdr.Close();
                    sql.Close();
                }
                return em;
            }
        }

        [HttpGet]
        public List<UserModel> GetUser()
        {
            List<UserModel> lum = new List<UserModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_createuser", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                       new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        UserModel um = new UserModel();
                        um.empID = sdr["empID"].ToString();
                        um.username = sdr["username"].ToString();
                        lum.Add(um);
                    }
                }
                return lum;
            }
        }

        [HttpPost]
        public string SetFolder([FromBody]UserRoleModel value)
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

        [HttpPost]
        public string Block([FromBody]UserRoleModel value)
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