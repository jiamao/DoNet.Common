//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : JSON序列化
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization.Json;

namespace DoNet.Common.Serialization
{
    /// <summary>
    /// JSON序列化
    /// </summary>
    public class JSon
    {
        /// <summary>
        /// 把对象序列化成JSON字符
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <returns></returns>
        public static string ModelToJson(object obj,params Type[] knowTypes)
        {
            if (obj == null) return "null";
            var ser = new DataContractJsonSerializer(obj.GetType(), knowTypes);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ser.WriteObject(ms, obj);
                string js = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                return js;
            }            
        }

        /// <summary>
        /// 把JSON反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="js"></param>
        /// <returns></returns>
        public static T JsonToModel<T>(string js)
        {            
            var obj = JsonToModel(typeof(T), js);
            return (T)obj;
        }

        /// <summary>
        /// 把JSON反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="js"></param>
        /// <returns></returns>
        public static T JsonToModel<T>(byte[] data)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }

        /// <summary>
        /// 把JSON反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="js"></param>
        /// <returns></returns>
        public static object JsonToModel(Type t, string js)
        {
            if (js == "null") return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(js)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
                var obj = ser.ReadObject(ms);
                return obj;
            }
        }

        /// <summary>
        /// JSON字符串解析
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static System.Json.JsonValue Parse(string source)
        {
            var obj = System.Json.JsonObject.Parse(source);
            return obj;
        }
    }
}
