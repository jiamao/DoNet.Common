using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 映射到表的属性
    /// </summary>
    [Serializable]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        {
            
        }

        /// <summary>
        /// 表名，用来映射到数据库的表名
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// model和数据库映射关系信息
    /// </summary>
    public class DBMappingInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表中的字段集合
        /// </summary>
        public Dictionary<string, string> Fields { get; set; }

        /// <summary>
        /// 主健字段
        /// </summary>
        public List<string> Primaries { get; set; }

        /// <summary>
        /// 字段映射
        /// key=model字段，value=DB字段名
        /// </summary>
        public Dictionary<string, string> Mapping { get; set; }

        static Dictionary<Type, DBMappingInfo> dbMappingInfoCache = new Dictionary<Type, DBMappingInfo>();
        /// <summary>
        /// 从model中获取数据库信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DBMappingInfo Get<T>() where T:class
        {
            var type = typeof(T);
            return Get(type);
        }

        /// <summary>
        /// 从model中获取数据库信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DBMappingInfo Get(Type type)
        {
            DBMappingInfo result = null;
            if (!dbMappingInfoCache.ContainsKey(type))
            {
                lock (dbMappingInfoCache)
                {
                    if (!dbMappingInfoCache.ContainsKey(type))
                    {
                        result = new DBMappingInfo() { Fields = new Dictionary<string, string>(), Primaries = new List<string>() };
                        result.TableName = type.Name;

                        var attrobjs = type.GetCustomAttributes(typeof(TableAttribute), false);
                        if (attrobjs != null && attrobjs.Length > 0)
                        {
                            var tbattr = attrobjs[0] as TableAttribute;
                            if (!string.IsNullOrWhiteSpace(tbattr.Name)) result.TableName = tbattr.Name;
                        }

                        var fields = type.GetProperties();
                        foreach (var f in fields)
                        {
                            var fieldattrobjs = f.GetCustomAttributes(typeof(TableFieldAttribute), false);
                            if (fieldattrobjs != null && fieldattrobjs.Length > 0)
                            {
                                if (result.Mapping == null) result.Mapping = new Dictionary<string, string>();
                                var attr = fieldattrobjs[0] as TableFieldAttribute;
                                if (!string.IsNullOrWhiteSpace(attr.Name))
                                {
                                    result.Fields.Add(attr.Name,f.Name);
                                    result.Mapping.Add(f.Name, attr.Name);
                                }
                                else
                                {
                                    result.Fields.Add(f.Name,f.Name);
                                    result.Mapping.Add(f.Name, f.Name);
                                }

                                //如果为主健
                                if (true == attr.IsPrimary)
                                {
                                    result.Primaries.Add(attr.Name);
                                }
                            }
                        }
                        dbMappingInfoCache.Add(type, result);
                    }
                    else
                    {
                        result = dbMappingInfoCache[type];
                    }
                }
            }
            else
            {
                result = dbMappingInfoCache[type];
            }
            return result;
        }
    }
}
