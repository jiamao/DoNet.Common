//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 类型操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace DoNet.Common.Reflection
{
    /// <summary>
    /// 类操作
    /// </summary>
    public class ClassHelper
    {
        /// <summary>
        /// 从DLL中获取类
        /// </summary>
        /// <param name="dllPath">DLL路径</param>
        /// <param name="className">类名</param>
        /// <param name="nameSpace">空间名，默认为空</param>
        /// <returns></returns>
        public static Type GetClassType(string dllPath, string className,string nameSpace = "")
        {
            Assembly ass = null;
            //如果驱动文件存在
            if (System.IO.File.Exists(dllPath))
            {
                ass = Assembly.LoadFrom(dllPath);                
            }
            else if(!string.IsNullOrWhiteSpace(nameSpace))
            {
                ass = System.Reflection.Assembly.Load(nameSpace);                
            }
            if (ass != null)
            {
                if (!string.IsNullOrWhiteSpace(className))
                {
                    return ass.GetType(className);
                }
                else
                {
                    Type[] ts = ass.GetTypes();
                    if (ts.Length > 0) return ts[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 加载对象实例
        /// </summary>
        /// <param name="dllPath">DLL路径</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static object GetClassObject(string dllPath, string className)
        {
            Type t = GetClassType(dllPath, className);//获取类型

            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// 生成对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="className">对象类路径</param>
        /// <returns></returns>
        public static T LoadInstance<T>(string namespaceName, string className)
        {
            return (T)Assembly.Load(namespaceName).CreateInstance(className, true,
                BindingFlags.Default, null, null, System.Globalization.CultureInfo.CurrentCulture, null);
        }

        /// <summary>
        /// 生成对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="className">对象类路径</param>
        /// <returns></returns>
        public static T LoadInstance<T>(string namespaceName, string className, params object[] pars)
        {
            return (T)Assembly.Load(namespaceName).CreateInstance(className, true,
                BindingFlags.Default, null, pars, System.Globalization.CultureInfo.CurrentCulture, null);
        }

        /// <summary>
        /// 获取对象属性的值
        /// </summary>
        /// <param name="Instance">对象实体</param>
        /// <param name="ClassName">所在类</param>
        /// <param name="PropertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(object Instance, string PropertyName, bool isStatic=false)
        {
            Type t = Instance.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            if (pi != null)
            {
                return GetPropertyValue(Instance, pi, isStatic);
            }
            return null;
        }

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="property"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object Instance, PropertyInfo property, bool isStatic = false)
        {
            if (isStatic) return property.GetValue(Instance, BindingFlags.Static, null, null, null);
            else return property.GetValue(Instance, null);
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="instance">对象</param>
        /// <param name="PropertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public static void SetPropertyValue(object instance, string PropertyName, object value, object[] indexs)
        {
            if (value == null || value.ToString() == "") return;

            Type t = instance.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            if (pi != null)
            {
                SetPropertyValue(instance, pi, value, indexs);
            }
        }

        /// <summary>
        /// 配置属性的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pi"></param>
        /// <param name="obj"></param>
        public static void SetPropertyValue(object sender, System.Reflection.PropertyInfo pi, object obj, object[] indexs)
        {
            if (pi.CanWrite == false || obj == null || obj.ToString() == "") return;
            var setmethod = pi.GetSetMethod();
            if (setmethod == null || setmethod.IsPublic == false) return;

            if (pi.PropertyType != typeof(String))
            {
                if (pi.PropertyType == typeof(Enum) || pi.PropertyType.BaseType == typeof(Enum))
                {
                    pi.SetValue(sender,obj , indexs);//Convert.ChangeType(obj, typeof(int))
                }
                else if (pi.PropertyType == typeof(Boolean) || pi.PropertyType.BaseType == typeof(Boolean))
                {
                    pi.SetValue(sender, "true".Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase) || "1".Equals(obj.ToString()), indexs);
                }
                else
                {
                    try
                    {
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var t = pi.PropertyType.GetGenericArguments()[0];
                            obj = Convert.ChangeType(obj, t);
                        }
                        else
                        {
                            obj = Convert.ChangeType(obj, pi.PropertyType);
                        }

                        pi.SetValue(sender, obj, indexs);

                        pi.SetValue(sender,
                            Convert.ChangeType(obj, pi.PropertyType),
                            indexs);
                    }
                    catch (Exception ex)
                    {
                        DoNet.Common.IO.Logger.Write(ex.ToString());
                        DoNet.Common.IO.Logger.Write("convert obj[" + obj.ToString() + "] to " + pi.PropertyType.ToString());
                    }
                }
            }
            else
            {
                if (obj.GetType() == typeof(byte[]))
                {
                    var v = System.Text.Encoding.UTF8.GetString((byte[])obj);
                    pi.SetValue(sender, v, indexs);
                }
                else
                {
                    pi.SetValue(sender, obj == DBNull.Value ? "" : obj.ToString(),
                                indexs);
                }
            }
        }

        /// <summary>
        /// 获取静态字段的值
        /// </summary>
        /// <param name="sender">对象实体</param>
        /// <param name="ClassName">所在类</param>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public static object GetStaticFieldValue(Type senderTyle, string fieldName)
        {
            var f = senderTyle.GetField(fieldName);
            if (f != null)
            {
                return f.GetValue(null);
            }
            return null;
        }

        /// <summary>
        /// 配置属性的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fi"></param>
        /// <param name="obj"></param>
        public static void SetFieldValue(object sender, System.Reflection.FieldInfo fi, object obj)
        {
            if (fi.IsPublic == false || obj == null || obj.ToString() == "") return;

            if (fi.FieldType != typeof(String))
            {
                if (fi.FieldType == typeof(Enum) || fi.FieldType.BaseType == typeof(Enum))
                {
                    fi.SetValue(sender, obj);//Convert.ChangeType(obj, typeof(int))
                }
                else if (fi.FieldType == typeof(Boolean) || fi.FieldType.BaseType == typeof(Boolean))
                {
                    fi.SetValue(sender, "true".Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase) || "1".Equals(obj.ToString()));
                }
                else
                {
                    fi.SetValue(sender, Convert.ChangeType(obj, fi.FieldType));
                }
            }
            else
            {
                if (obj.GetType() == typeof(byte[]))
                {
                    var v = System.Text.Encoding.UTF8.GetString((byte[])obj);
                    fi.SetValue(sender, v);
                }
                else
                {
                    fi.SetValue(sender, obj == DBNull.Value ? "" : obj.ToString());
                }
            }
        }

        /// <summary>  
        /// 快速设置属性  
        /// </summary>  
        /// <param name="sender">对象</param>  
        /// <param name="pi">属性</param>  
        /// <param name="obj">值</param>  
        /// <param name="indexs">索引</param>  
        public static void FastSetPropertyValue(object sender, System.Reflection.PropertyInfo pi, object obj, object[] indexs)
        {
            if (pi.CanWrite == false) return;
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                if (pi.PropertyType == typeof(DateTime)) obj = DateTime.Parse("1900-01-01 00:00:00");
                else return;
            }

            //获取属性的赋值方法  
            var setMethod = pi.GetSetMethod();
            if (setMethod == null || setMethod.IsPublic == false) return;

            var invoke = FastInvoke.GetMethodInvoker(setMethod);
            if (invoke == null) return;

            if (pi.PropertyType != typeof(String))
            {
                if (obj != DBNull.Value || obj.ToString() != "")
                {
                    if (pi.PropertyType == typeof(Enum) || pi.PropertyType.BaseType == typeof(Enum))
                    {
                        invoke(sender, new object[] { Convert.ChangeType(obj, typeof(int)), indexs });
                    }
                    else if (pi.PropertyType == typeof(Boolean) || pi.PropertyType.BaseType == typeof(Boolean))
                    {
                        var strv = obj.ToString();
                        var v = 0;
                        var bv = false;
                        if (int.TryParse(strv, out v))
                        {
                            bv = v != 0;
                        }
                        else
                        {
                            bv = "true".Equals(strv.Trim(), StringComparison.OrdinalIgnoreCase);
                        }
                        invoke(sender, new object[] { bv, indexs });
                    }
                    else
                    {
                        try
                        {
                            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var t = pi.PropertyType.GetGenericArguments()[0];
                                obj = Convert.ChangeType(obj, t);
                            }
                            else
                            {
                                obj = Convert.ChangeType(obj, pi.PropertyType);
                            }

                            invoke(sender, new object[] { obj,indexs });
                        }
                        catch (Exception ex)
                        {
                            DoNet.Common.IO.Logger.Write(ex.ToString());
                            DoNet.Common.IO.Logger.Write("convert obj[" + obj.ToString() + "] to " + pi.PropertyType.ToString());
                        }
                    }
                }
            }
            else
            {
                invoke(sender, new object[] { obj == DBNull.Value ? "" : obj.ToString(), indexs });
            }
        }  

        /// <summary>
        /// 获取对象的所有属性        
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetTypePropertys(Type type)
        {
            var ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            return ps;
        }

        /// <summary>
        /// 获取对象的所有字段      
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public static FieldInfo[] GetTypeFields(Type type)
        {
            var ps = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            return ps;
        }

        /// <summary>
        /// 执行一个方法
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="methodName"></param>
        public static object RunMethod(object obj, string methodName, params object[] paras)
        {
            Type t = obj.GetType();

            MethodInfo mi = t.GetMethod(methodName);

            if (mi.IsStatic)
            {
                return mi.Invoke(null, paras);
            }
            else
            {
                return mi.Invoke(obj, paras);
            }
        }

        /// <summary>
        /// 执行一个方法
        /// </summary>
        /// <param name="dllpath">DLL路径</param>
        /// <param name="className">所在类名</param>
        /// <param name="methodName">方法名</param>
        public static object RunMethod(string dllpath, string className, string methodName, params object[] paras)
        {
            if (string.IsNullOrEmpty(dllpath) || string.IsNullOrEmpty(methodName)) return null;
            Type t = GetClassType(dllpath, className);

            MethodInfo mi = t.GetMethod(methodName);

            if (mi.IsStatic)
            {
                return mi.Invoke(null, paras);
            }
            else
            {
                object obj = Activator.CreateInstance(t);
                return mi.Invoke(obj, paras);
            }
        }

        /// <summary>  
        /// 获取ＤＬＬ中一个方法的委托  
        /// 常用于调用C++等其它语言的库
        /// </summary>  
        /// <param name="dllpath">DLL的路径</param>  
        /// <param name="methodname">方法名</param>  
        /// <param name="methodtype">委托类型</param>  
        /// <returns></returns>  
        public Delegate GetMethod(string dllpath, string methodname, Type methodtype)
        {
            var DllLib = ClassApi.LoadLibrary(dllpath);
            IntPtr MethodPtr = ClassApi.GetProcAddress(DllLib, methodname);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(MethodPtr, methodtype);
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static string GetSystemVersion(Assembly asm)
        {
            //获取版本信息
            string VersionType = "";
            string VersionNum = "";
            string version = "";
            //Assembly asm = this.GetType().Assembly;
            Type aType = typeof(AssemblyDescriptionAttribute);
            object[] objarray = asm.GetCustomAttributes(aType, true);

            if (objarray.Length > 0)
            {
                VersionType = ((AssemblyDescriptionAttribute)objarray[0]).Description;

            }
            //获取版本号
            Type bType = typeof(AssemblyFileVersionAttribute);
            objarray = asm.GetCustomAttributes(bType, true);
            if (objarray.Length == 1)
            {
                VersionNum = ((AssemblyFileVersionAttribute)objarray[0]).Version;
            }

            version = VersionType + "V " + VersionNum;

            return version;
        }

        /// <summary>
        /// 拷贝一个对象
        /// </summary>
        /// <typeparam name="T">拷贝的类型</typeparam>
        /// <param name="obj">原对象</param>
        /// <returns></returns>
        public static T CloneObject<T>(T obj)           
        {
            return CloneObject<T, T>(obj);
        }

        /// <summary>
        /// 拷贝一个对象
        /// </summary>
        /// <typeparam name="T">拷贝的类型</typeparam>
        /// <param name="obj">原对象</param>
        /// <returns></returns>
        public static Result CloneObject<Source, Result>(Source obj)
        {
            return CloneObject<Result>(obj, null);
        }

        /// <summary>
        /// 拷贝属性
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Result CloneObject<Result>(object obj, Func<PropertyInfo, PropertyInfo> fun=null)
        {
            var t = obj.GetType();
            var propertys = GetTypePropertys(t);//获取其下的所有属性
            var objpropertys = GetTypePropertys(typeof(Result));
            var fields = GetTypeFields(t);
            var objfields = GetTypeFields(typeof(Result));

            var newobj = Activator.CreateInstance<Result>();//生成新的实例

            foreach (var p in propertys)
            {
                var pvalue = GetPropertyValue(obj, p);//获取原对象的值

                PropertyInfo pi = fun != null ? fun(p) : null;//通过外部映射
                if (pi == null)
                {
                    foreach (var rp in objpropertys)
                    {
                        if (rp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            pi = rp;
                            break;
                        }
                    }
                }
                if (pi == null)
                {
                    foreach (var rp in objfields)
                    {
                        if (rp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            //把它赋给新对象
                            SetFieldValue(newobj, rp, pvalue);
                            break;
                        }
                    }
                }
                else if (pi.CanWrite)
                {
                    //把它赋给新对象
                    FastSetPropertyValue(newobj, pi, pvalue, null);
                }
            }

            foreach (var f in fields)
            {
                var pvalue = f.GetValue(obj);//获取原对象的值

                FieldInfo pi = null;//通过外部映射

                foreach (var rp in objfields)
                {
                    if (rp.Name.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        pi = rp;
                        break;
                    }
                }
                if (pi != null)
                {
                    //把它赋给新对象
                    SetFieldValue(newobj, pi, pvalue);
                }
                else
                {
                    foreach (var rp in objpropertys)
                    {
                        if (rp.Name.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            FastSetPropertyValue(newobj, rp, pvalue, null);
                            break;
                        }
                    }
                }
            }
            return newobj;
        }
    }
}
