using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http;

namespace ESSONS_MIS_2020.Controllers
{
    [Route("api/[Controller]")]
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
                    sc.Parameters.Add(
                        new SqlParameter("@password", value.password));
                    SqlDataReader sdr = sc.ExecuteReader();
                    if(sdr.HasRows == true)
                        return Ok();
                    else return NotFound();

                }
            }
        }
    }
}