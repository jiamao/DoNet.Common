using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public static class DataConvert
    {
        //缓存
        private static Dictionary<string,object> thisCache = new Dictionary<string,object>();

        /// <summary>
        /// 转换datatable为对象集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">表</param>
        /// <param name="fieldMapping">字段映射</param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertDataTableToCollection<T>(System.Data.DataTable dt, IDictionary<string, string> fieldMapping) where T : class
        {
            var list = new List<T>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                var obj = ConvertDataToObject<T>(dr, fieldMapping);
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 将数据转为自定义对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public static T ConvertDataToObject<T>(System.Data.IDataReader dr, IDictionary<string, string> fieldMapping) where T : class
        {
            return (T)ConvertDataToObject(dr, fieldMapping, typeof(T));
        }

        /// <summary>
        /// 将数据转为自定义对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dr">行</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public static T ConvertDataToObject<T>(System.Data.DataRow dr, IDictionary<string, string> fieldMapping) where T : class
        {
            return (T)ConvertDataToObject(dr, fieldMapping, typeof(T));
        }

        /// <summary>
        /// 将数据转为自定义对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="dr">数据阅读游标</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        public static object ConvertDataToObject(System.Data.IDataReader dr, IDictionary<string, string> fieldMapping, Type type)
        {
            var data = GetDataByReader(dr);//获取当前索引的数据

            return ConvertDataToObject(data, fieldMapping, type);
        }

        /// <summary>
        /// 将数据转为自定义对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="dr">数据的datarow</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        public static object ConvertDataToObject(System.Data.DataRow dr, IDictionary<string, string> fieldMapping, Type type)
        {
            var data = GetDataByReader(dr);//获取当前索引的数据

            return ConvertDataToObject(data, fieldMapping, type);
        }

        /// <summary>
        /// 将数据转为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">数据源</param>
        /// <param name="fieldMapping">字段映射</param>
        /// <param name="type">对象type</param>
        /// <returns></returns>
        private static object ConvertDataToObject(IDictionary<string, object> data, IDictionary<string, string> fieldMapping, Type type)
        {
            var returnObject = Activator.CreateInstance(type);//生成实例对象

            bool mappingExists = fieldMapping != null && fieldMapping.Count > 0;
            
            foreach (var pi in GetTypePropertys(type))
            {
                //可写
                if (!pi.CanWrite) continue;

                object obj = null;
                if (pi.PropertyType.IsClass && pi.PropertyType != typeof(string))
                {
                    if (pi.PropertyType.IsGenericType || pi.PropertyType.IsArray || pi.PropertyType.IsAbstract || pi.PropertyType.IsInterface)
                    {
                        continue;
                    }
                    var mapping = new Dictionary<string, string>();
                    if (fieldMapping != null)
                    {
                        //把映射 中的当前属性类的下级映射收集
                        foreach (var d in fieldMapping)
                        {
                            if (d.Key.StartsWith(pi.Name + '.', StringComparison.OrdinalIgnoreCase))
                            {
                                mapping.Add(d.Key.Substring(pi.Name.Length + 1), d.Value);
                            }
                        }
                    }
                    obj = ConvertDataToObject(data, mapping, pi.PropertyType);
                }
                else
                {
                    var pn = pi.Name.ToLower();
                    string colName = null;
                    //获取对应的列名
                    if (mappingExists)
                    {
                        foreach (var m in fieldMapping)
                        {
                            if (pn.Equals(m.Key, StringComparison.OrdinalIgnoreCase))
                            {
                                colName = m.Value.ToLower();
                                break;
                            }
                        }
                        //if (string.IsNullOrWhiteSpace(colName)) continue;
                    }
                    if (string.IsNullOrWhiteSpace(colName)) colName = pn;           
                    //读取结果集中包含当前列名。则读取
                    if (data.ContainsKey(colName))
                    {
                        obj = data[colName];
                    }                    
                }
                if ((obj == null || obj == DBNull.Value) && pi.PropertyType.IsValueType) continue; //如果查询字段中没有则下一个

                Reflection.ClassHelper.FastSetPropertyValue(returnObject, pi, obj, null);
            }
            return returnObject;
        }


        /// <summary>
        /// 泛型转换成DataTable
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">泛型</param>
        /// <returns>返回表</returns>
        public static System.Data.DataTable ConvertToDataTable<T>(IList<T> list)
        {
            //参数为空或无记录
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            //指定表名创建表
            var dt = new System.Data.DataTable(typeof(T).Name);
            System.Data.DataColumn dc;
            System.Data.DataRow dr;
            Type tp = typeof(T);
            //获取公共成员或实例成员的公有属性
            System.Reflection.PropertyInfo[] propertyInfo = GetTypePropertys(tp);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                dr = dt.NewRow();
                int j = propertyInfo.Length;
                for (int i = 0; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = propertyInfo[i];
                    string strName = pi.Name;
                    if (dt.Columns[strName] == null)
                    {
                        dc = new System.Data.DataColumn(strName, pi.PropertyType);
                        dt.Columns.Add(dc);
                    }
                    dr[strName] = pi.GetValue(t, null);
                }
                dt.Rows.Add(dt);
            }
            return dt;
        }

        /// <summary>
        /// 解析字段映射
        /// </summary>
        /// <param name="mappingString">字段映射字符串</param>
        /// <returns>KEY为对象属性名,VALUE为数据库中字段名</returns>
        public static Dictionary<string, string> CreateMapping(string mappingString,char fieldsplit=',',char splitchar='|')
        {
            if (string.IsNullOrEmpty(mappingString)) return null;
            mappingString = mappingString.ToLower();//保证最小化

            //从缓存中获取
            var _fieldmapping = GetCache(mappingString) as Dictionary<string, string>;
            //如果缓存中没有则解析，并保存至缓存中
            if (_fieldmapping == null)
            {
                lock (thisCache)
                {
                    _fieldmapping = GetCache(mappingString) as Dictionary<string, string>;
                    if (_fieldmapping == null)
                    {
                        _fieldmapping = new Dictionary<string, string>();
                        //分隔映射
                        string[] mps = mappingString.Split(fieldsplit);
                        foreach (string mp in mps)
                        {
                            if (string.IsNullOrEmpty(mp)) continue;

                            string[] fields = mp.Split(splitchar);
                            if (fields.Length > 1)
                            {
                                var key = fields[0];
                                if (!_fieldmapping.ContainsKey(key)) _fieldmapping.Add(key, fields[1]);
                            }
                        }

                        thisCache.Add(mappingString, _fieldmapping);
                    }
                }
            }
            return _fieldmapping;
        }

        /// <summary>
        /// 获取对象的所有属性
        /// 并存入缓存..所以不需每次都得反射获取
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public static System.Reflection.PropertyInfo[] GetTypePropertys(Type type)
        {
            string typeName = type.FullName;
            //从缓存中获取此对象的属性
            var ps = GetCache(typeName) as System.Reflection.PropertyInfo[];
            if (ps == null)
            {
                lock (thisCache)
                {
                    ps = GetCache(typeName) as System.Reflection.PropertyInfo[];
                    if (ps == null)
                    {
                        ps = Reflection.ClassHelper.GetTypePropertys(type);
                        //将属性添加到缓存中
                        thisCache.Add(typeName, ps);
                    }
                }
            }
            return ps;
        }

        /// <summary>
        /// 从阅读取中获取当前数据行
        /// </summary>
        /// <param name="reader">数据库表游标</param>
        /// <returns>用列名为key。值为value的结果集</returns>
        private static Dictionary<string, object> GetDataByReader(System.Data.IDataReader reader)
        {
            var result = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                //读取列名，并最小化
                var name = reader.GetName(i).ToLower();

                if (!result.ContainsKey(name))
                {
                    var value = reader.GetValue(i);
                    result.Add(name, value);
                }
            }
            return result;
        }

        /// <summary>
        /// 分解每行的记录
        /// </summary>
        /// <param name="row">表行</param>
        /// <returns></returns>
        private static Dictionary<string, object> GetDataByReader(System.Data.DataRow row)
        {
            var result = new Dictionary<string, object>();
            foreach (System.Data.DataColumn c in row.Table.Columns)
            {
                //读取列名，并最小化
                var name = c.ColumnName.ToLower();

                if (!result.ContainsKey(name))
                {
                    var value = row[c];
                    result.Add(name, value);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static object GetCache(string key)
        {
            object obj = null;
            thisCache.TryGetValue(key, out obj);

            return obj;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        static void SetCache(string key, object value)
        {
            lock (thisCache)
            {
                if (thisCache.ContainsKey(key))
                {
                    thisCache[key] = value;
                }
                else
                {
                    thisCache.Add(key, value);
                }
            }
        }
    }
}
