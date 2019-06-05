using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;

namespace DoNet.Common.Web
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件URL路径</param>
        /// <param name="showFilename"></param>
        /// <returns></returns>
        public static bool DownFile(string filePath, string showFilename)
        {
            try
            {
                string fileName = DoNet.Common.IO.PathMg.CheckPath(filePath);
                if (!showFilename.Contains(".")) showFilename = showFilename + System.IO.Path.GetExtension(fileName);

                if (!File.Exists(fileName))
                {
                    return false; ;
                }
                var f = new FileInfo(fileName);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(showFilename));
                HttpContext.Current.Response.AddHeader("Content-Length", f.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(fileName);
                HttpContext.Current.Response.End();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 下载内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static void DownContent(string fileName, string html)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(html);
            DownData(fileName, data);
        }

        /// <summary>
        /// 下载字节
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void DownData(string fileName, byte[] data)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            HttpContext.Current.Response.AddHeader("Content-Length", data.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(data);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 简单替换xss字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ReplaceXss(string source)
        {
            return source.Replace("\\", "\\\\").Replace("<", "&gt;").Replace(">", "&lt;");
        }

        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeHtml(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return source;
            return source.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;").Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }
    }
}
