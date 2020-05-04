using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESSONS_MIS_2020_App.Models;

namespace ESSONS_MIS_2020_App.Helper
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static List<UserRoleModel> GetObjectFromJson<List>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(List<UserRoleModel>) : JsonConvert.DeserializeObject<List<UserRoleModel>>(value);
        }
    }
}
