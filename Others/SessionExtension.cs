using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Others
{
    public static class SessionExtension
    {
        public static void  SetObj(this ISession sesion, string key, object value)
        {
            sesion.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObj<T>(this ISession sesion, string key)
        {
            var value = sesion.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
