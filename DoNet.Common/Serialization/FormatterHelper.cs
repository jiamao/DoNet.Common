//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace DoNet.Common.Serialization
{
    /// <summary>
    /// 序列化
    /// </summary>
    public class FormatterHelper
    {
        /// <summary>
        /// 序例化对象为XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XMLSerObjectToString(object obj)
        {
            return Encoding.UTF8.GetString(XMLSerObjectToBytes(obj));
        }

        /// <summary>
        /// 序例化对象为XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] XMLSerObjectToBytes(object obj)
        {
            XmlSerializer xmlsers = new XmlSerializer(obj.GetType());

            MemoryStream ms = new MemoryStream();
            xmlsers.Serialize(ms, obj);

            return ms.ToArray();
        }

        /// <summary>
        /// XML反序列化为class对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">序列化字符串</param>
        /// <returns></returns>
        public static T XMLDerObjectFromString<T>(string source) where T:class
        {
            return XMLDerObjectFromString(typeof(T), source) as T;
        }

        /// <summary>
        /// 从字符串反序列化对象
        /// </summary>
        /// <param name="t"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object XMLDerObjectFromString(Type t, string source)
        {
            return XMLDerObjectFromBytes(t, Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// 
        /// 从字节流反序列化对象
        /// </summary>
        /// <param name="t"></param>
        /// <param name="BS"></param>
        /// <returns></returns>
        public static object XMLDerObjectFromBytes(Type t, byte[] BS)
        {
            XmlSerializer xmlsers = new XmlSerializer(t);

            object obj = xmlsers.Deserialize(new MemoryStream(BS));

            return obj;
        }

        /// <summary>
        /// XML序列化对象
        /// </summary>
        /// <param name="obj"></param>
        public static void XMLSerObject(object obj, string filename)
        {
            XmlSerializer xmlsers = new XmlSerializer(obj.GetType());
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (File.Exists(filename)) File.Delete(filename);

            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);

            try
            {
                xmlsers.Serialize(fs, obj);
            }
            catch { throw; }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// XML反序列化对象
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object XMLDerObject(Type t, string filename)
        {
            XmlSerializer xmlsers = new XmlSerializer(t);

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
            try
            {
                object obj = xmlsers.Deserialize(fs);
                return obj;
            }
            catch { throw; }
            finally
            {
                fs.Close();
            }            
        }

        /// <summary>
        /// 二进制系列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void BinarySerObject(object obj, string filename)
        {
            try
            {
                string dir = Path.GetDirectoryName(filename);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (File.Exists(filename)) File.Delete(filename);

                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                MemoryStream stream = BinarySerObjectToStream(obj);
                fs.Write(stream.ToArray(), 0, (int)stream.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 二进制系列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static MemoryStream BinarySerObjectToStream(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms;
        }

        /// <summary>
        /// 进制制反序列化
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object BinaryDerObject(string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            object obj = bf.Deserialize(fs);

            fs.Close();
            return obj;
        }

        /// <summary>
        /// 二进制反序列化流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object BinaryDerStream(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();

            object obj = bf.Deserialize(stream);

            return obj;
        }

    }
}
