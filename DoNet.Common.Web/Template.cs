using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.Web
{
    /// <summary>
    /// 模板处理，主要是include文件
    /// </summary>
    public class Template
    {
        const string TemplateCacheKey = "DoNet.Common.Web.Template.";

        /// <summary>
        /// 引入模板文件，加到script标签 中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Include(string file, string id="")
        {
            var key = TemplateCacheKey + file;
            var cache = DoNet.Common.Cache.Cache.Get(key) as TemplateCacheItem;
            var path = DoNet.Common.IO.PathMg.CheckWebPath(file);
            if (System.IO.File.Exists(path))
            {
                var sttime = System.IO.File.GetLastWriteTime(path);
                if (cache != null && cache.LastTime == sttime)
                {
                    return cache.Content;
                }
                else
                {
                    cache = cache ?? new TemplateCacheItem();
                    cache.LastTime = sttime;
                    var content = System.IO.File.ReadAllText(path);
                    if (string.IsNullOrWhiteSpace(id)) id = file;
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        content = content.Replace("{id}", id);
                    }
                    cache.Content = string.Format("<script id=\"" + id + "\" type=\"text/html\">{0}</script>",content);
                    DoNet.Common.Cache.Cache.Insert(key, cache);
                }
            }
            return cache != null ? cache.Content : "<script id=\"" + id + "\" type=\"text/html\"></script>";
        }
    }

    /// <summary>
    /// 模板缓存项
    /// </summary>
    [Serializable]
    public class TemplateCacheItem
    {
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
