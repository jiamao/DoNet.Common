using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace DoNet.Common.Reflection
{
    /// <summary>
    /// 属性处理类
    /// </summary>
    public class PropertyManager
    {
        /// <summary>
        /// 获取枚举注释说明
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(T val)
        {
            var t = typeof(T);
            string enumvalue = Enum.GetName(t, val);
            if (String.IsNullOrEmpty(enumvalue))
                return "";

            return GetFieldDescription(t, enumvalue);
        }

        /// <summary>
        /// 获取当前枚举全部说明
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICollection<string> GetAllEnumDescriptions<T>()
        {
            var t = typeof(T);
            var enumvalues = Enum.GetNames(t);
            //说明集合
            var names =new List<string>();
            foreach (var n in enumvalues)
            {
                var v = GetFieldDescription(t, n);
                if (!string.IsNullOrWhiteSpace(n)) names.Add(v);
            }
            return names;
        }

        /// <summary>
        /// 获取指定枚举的说明
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetFieldDescription(Type t, string name)
        {
            var finfo = t.GetField(name);
            var cAttr = finfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (cAttr.Length > 0)
            {
                var desc = cAttr[0] as DescriptionAttribute;
                if (desc != null)
                {
                    return desc.Description;
                }
            }
            return name;
        }
    }
}
