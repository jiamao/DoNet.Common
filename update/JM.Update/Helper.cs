using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace JM.Update
{
    static class Helper
    {
        /// <summary>
        /// 获取文件的MD5码
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static string GetFileMD5(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))//打开文件
            {
                MD5 md5 = MD5.Create();
                byte[] bsre = md5.ComputeHash(fs);
                fs.Close();
                return BitConverter.ToString(bsre).Replace("-", "");
            }
        }

        /// <summary>
        /// 打包单个文件
        /// </summary>
        /// <param name="TargetFile"></param>
        /// <param name="sourceFile"></param>
        internal static void SerFileZip(string TargetFile, string sourceFile)
        {
            FileStream SourceFileStream = null;
            GZipStream CompressedStream = null;//压缩流  
            FileStream SerFile = new FileStream(TargetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                Console.WriteLine("目标文件：" + TargetFile);

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);

                Console.Write("打包:" + System.IO.Path.GetFileName(sourceFile) + "           ");
                SourceFileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                long packedsize = 0;
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int re = SourceFileStream.Read(buffer, 0, buffer.Length);//读取源文件 
                    if (re > 0)
                    {
                        CompressedStream.Write(buffer, 0, re);//压缩 
                        packedsize += re;
                        float per = (float)packedsize / SourceFileStream.Length * 100;
                        Console.Write("\b\b\b\b{0,4:G}", per.ToString("###") + "%");
                    }
                    else
                    {
                        Console.WriteLine("\b\b\b\b{0,4:G}", "100%");
                        break;
                    }
                }
                SourceFileStream.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SourceFileStream != null)
                    SourceFileStream.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();

                if (SerFile != null)
                    SerFile.Close();
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        internal static string DerFileZip(string sourcefile, string filepath)
        {                
            string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名

            using (var sourcestream = System.IO.File.OpenRead(sourcefile))
            {
                var CompressedStream = new GZipStream(sourcestream, CompressionMode.Decompress, true);
                var bs = new List<byte>();
                var b = CompressedStream.ReadByte();
                while(b != -1)
                {
                    bs.Add((byte)b);
                    b = CompressedStream.ReadByte();
                }
                using (var file = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    file.Write(bs.ToArray(), 0,bs.Count);
                }
            }
            return "成功";           
        }

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
    }
}
