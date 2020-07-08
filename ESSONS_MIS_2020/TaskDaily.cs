using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020
{
    public class IncomingEthTxService : BackgroundService
    {
        public IncomingEthTxService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    string connection = "Data Source=SRV-DB-02\\SQLEXPRESS;Initial Catalog=MIS;User ID=sa;Password=Es@2020";

                    using (SqlConnection sql = new SqlConnection(connection))
                    {
                        using (SqlCommand sc = new SqlCommand("sp_DailyTask", sql))
                        {
                            sql.Open();
                            sc.CommandType = System.Data.CommandType.StoredProcedure;
                            sc.Parameters.Add(
                                new SqlParameter("@type", "Test"));
                            SqlDataReader sdr = sc.ExecuteReader();
                        }
                    }
                    await Task.Delay(new TimeSpan(1, 0, 0));
                }
            }
        }
    }
}
