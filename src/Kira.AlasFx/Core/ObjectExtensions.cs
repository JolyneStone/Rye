using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
