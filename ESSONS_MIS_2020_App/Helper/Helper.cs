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
            //localhost:50457
            //Client.BaseAddress = new Uri("http://10.0.11.6:457/");
           Client.BaseAddress = new Uri("http://localhost:50457/");
            return Client; 
        }
    }
}
