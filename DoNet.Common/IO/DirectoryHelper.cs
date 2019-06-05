//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DoNet.Common.IO
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDirectory(string dir)
        {
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDirectory(string dir)
        {
            if (!System.IO.Directory.Exists(dir)) return;           
            //删除所有文件
            string[] files = System.IO.Directory.GetFiles(dir, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                FileHelper.DeleteFile(file);
            }

            System.IO.Directory.Delete(dir, true);
        }


        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void ClearDirectory(string dir, params string[] withoutdirs)
        {
            if (!System.IO.Directory.Exists(dir)) return;

            string[] dirs = System.IO.Directory.GetDirectories(dir, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string d in dirs)
            {
                if (withoutdirs != null && StrInArray(withoutdirs, d, true)) continue;
                ClearDirectory(d);
            }
            //删除所有文件
            string[] files = System.IO.Directory.GetFiles(dir, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                FileHelper.DeleteFile(file);
            }
        }

        /// <summary>
        /// 对文件夹进行拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyDirectory(string source, string target, params string[] withoutdirs)
        {
            source = source.Trim('\\').Trim();
            if (!System.IO.Directory.Exists(source)) return;

            string[] files = System.IO.Directory.GetFiles(source, "*.*");
            foreach (string file in files)
            {
                if (!System.IO.File.Exists(file)) continue;
                string newfile = System.IO.Path.Combine(target, file.Substring(source.Length).Trim('\\'));
                FileHelper.CopyFile(file, newfile);
            }

            string[] dirs = System.IO.Directory.GetDirectories(source, "*.*");
            foreach (string dir in dirs)
            {
                String dirName = System.IO.Path.GetFileName(dir);
                if (withoutdirs != null && StrInArray(withoutdirs, dirName, true)) continue;
                String TargetDir = PathMg.CheckPath(target, dirName);
                CreateDirectory(TargetDir);

                CopyDirectory(dir, TargetDir, withoutdirs);//递归拷贝
            }
        }

        /// <summary>
        /// 对文件夹进行拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyDirectoryNotWithExt(string source, string target, params string[] withoutExt)
        {
            source = source.TrimEnd('\\').Trim();
            if (!System.IO.Directory.Exists(source)) return;

            string[] files = System.IO.Directory.GetFiles(source, "*.*");
            foreach (string file in files)
            {
                string ext = System.IO.Path.GetExtension(file);
                //过滤后缀
                if (!System.IO.File.Exists(file) || (withoutExt != null && StrInArray(withoutExt, ext, true))) continue;

                string newfile = System.IO.Path.Combine(target, file.Substring(source.Length).Trim('\\'));
                FileHelper.CopyFile(file, newfile);
            }

            string[] dirs = System.IO.Directory.GetDirectories(source, "*.*");
            foreach (string dir in dirs)
            {
                String dirName = System.IO.Path.GetFileName(dir);
                String TargetDir = PathMg.CheckPath(target, dirName);
                CreateDirectory(TargetDir);
                CopyDirectoryNotWithExt(dir, TargetDir, withoutExt);//递归拷贝
            }

        }

        /// <summary>
        /// 对文件夹进行拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyDirectoryNWithExt(string source, string target, params string[] withExt)
        {
            source = source.Trim('\\').Trim();
            if (!System.IO.Directory.Exists(source)) return;

            string[] files = System.IO.Directory.GetFiles(source, "*.*");
            foreach (string file in files)
            {
                string ext = System.IO.Path.GetExtension(file);
                //过滤后缀
                if (!System.IO.File.Exists(file) || (withExt != null && !StrInArray(withExt, ext, true))) continue;

                string newfile = System.IO.Path.Combine(target, file.Substring(source.Length).Trim('\\'));
                FileHelper.CopyFile(file, newfile);
            }

            string[] dirs = System.IO.Directory.GetDirectories(source, "*.*");
            foreach (string dir in dirs)
            {
                String dirName = System.IO.Path.GetFileName(dir);
                String TargetDir = PathMg.CheckPath(target, dirName);
                CreateDirectory(TargetDir);
                CopyDirectoryNWithExt(dir, TargetDir, withExt);//递归拷贝
            }

        }


        /// <summary>
        /// 对文件夹进行拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyDirectory(string source, string target, bool setNormal)
        {
            string[] dirs = System.IO.Directory.GetDirectories(source, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                CreateDirectory(dir);
            }
            string[] files = System.IO.Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string newfile = System.IO.Path.Combine(target, file.Replace(source, "").Trim('\\'));

                FileHelper.CopyFile(file, newfile);

                if (setNormal) FileHelper.SetFileNormal(newfile);//去除只读
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="array"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool StrInArray(string[] array, string str, bool tolower)
        {
            if (tolower) str = str.ToLower();
            foreach (string s in array)
            {
                if ((tolower ? s.ToLower() : s) == str) return true;
            }
            return false;
        }
    }
}
