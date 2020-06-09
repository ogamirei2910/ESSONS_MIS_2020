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
    public class OverTimeController : Controller
    {
        private readonly IConfiguration _configuration;

        public OverTimeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public string UpdateOT([FromBody]OverTimeModel um)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");
            string result2 = "";
            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_overtime", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@datework", um.datework));
                    sc.Parameters.Add(
                        new SqlParameter("@overtimeID", um.overtimeID));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "UpdateOT"));

                    SqlParameter result = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                    result.Direction = ParameterDirection.Output;
                    sc.Parameters.Add(result);

                    sc.ExecuteNonQuery();
                    result2 = sc.Parameters["@result"].Value.ToString();

                }
                return result2;
            }
        }
    }
}