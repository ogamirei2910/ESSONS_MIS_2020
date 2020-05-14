﻿using System;
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
    public class ChamCongController : Controller
    {
        private readonly IConfiguration _configuration;

        public ChamCongController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public List<ChamCongModel> GetChamCong(string date)
        {
            List<ChamCongModel> lem = new List<ChamCongModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_chamcong", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@date", date));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "All"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        ChamCongModel em = new ChamCongModel();
                        em.empID = sdr["empID"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.depName = sdr["depchildName"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.hours = double.Parse(sdr["giocong"].ToString());
                        em.OT = double.Parse(sdr["OT"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
        [HttpGet]
        public List<ChamCongModel> GetChamCongTheoNgay(string date)
        {
            List<ChamCongModel> lem = new List<ChamCongModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_chamcong", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@date", date));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "day"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        ChamCongModel em = new ChamCongModel();
                        em.empID = sdr["empID"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.depName = sdr["depchildName"].ToString();
                        em.datework = sdr["datework"].ToString();
                        em.hours = double.Parse(sdr["giocong"].ToString());
                        em.OT = double.Parse(sdr["OT"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }

        [HttpGet]
        public List<ChamCongModel> GetChamCongTheoThang(string date)
        {
            List<ChamCongModel> lem = new List<ChamCongModel>();
            string connection = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection sql = new SqlConnection(connection))
            {
                using (SqlCommand sc = new SqlCommand("sp_chamcong", sql))
                {
                    sql.Open();
                    sc.CommandType = System.Data.CommandType.StoredProcedure;
                    sc.Parameters.Add(
                        new SqlParameter("@date", date));
                    sc.Parameters.Add(
                        new SqlParameter("@type", "month"));
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        ChamCongModel em = new ChamCongModel();
                        em.empID = sdr["empID"].ToString();
                        em.empName = sdr["empName"].ToString();
                        em.depName = sdr["depchildName"].ToString();
                        em.datework = sdr["loaicong"].ToString();
                        em.hours = double.Parse(sdr["giocong"].ToString());
                        em.OT = double.Parse(sdr["OT"].ToString());
                        lem.Add(em);
                    }
                }
                return lem;
            }
        }
    }
    
}