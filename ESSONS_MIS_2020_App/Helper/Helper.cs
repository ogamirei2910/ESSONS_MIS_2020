using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Helper
{
    public class EssonsApi
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("http://192.168.1.86:4444");
            return Client;
        }
    }
}
