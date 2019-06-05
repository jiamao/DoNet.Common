using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace DoNet.Common.Tool
{
    public class SVN
    {
        static SVN _instance = new SVN();
        /// <summary>
        /// SVN帮助类
        /// </summary>
        public static SVN Instance
        {
            get { return _instance; }
        }


        //文件编码
        Encoding _curEncoding = Encoding.GetEncoding("gb2312");

        /// <summary>
        /// info命令
        /// </summary>
        const string SVNInfoCommand = "info";

        /// <summary>
        /// 查找查异命令
        /// </summary>
        const string SVNDiffCommand = "{0} diff --summarize --xml {1} \"{2}\" \"{3}\"";

        /// <summary>
        /// SVN拷贝命令,主要用来创建分支或标签
        /// </summary>
        const string SVNCopyCommand = "{0} copy {1} {2} {3} -m \"{4}\"";
        /// <summary>
        /// 创建目录命令
        /// </summary>
        const string SVNCreateDirCommand = "{0} mkdir {1} \"{2}\" -m\"{3}\"";
        /// <summary>
        /// checkout命令
        /// </summary>
        const string SVNCheckoutCommand = "{0} checkout --force {1} \"{2}\" \"{3}\"";

        /// <summary>
        /// 导出命令
        /// </summary>
        const string SVNExportCommand = "{0} export --force {1} \"{2}\" \"{3}\"";

        public SVN()
        {
            var str = System.Configuration.ConfigurationManager.AppSettings["encoding"];
            if (!string.IsNullOrWhiteSpace(str))
            {
                _curEncoding= Encoding.GetEncoding(str);
            }
        }

        public SVN(string encodingString)
        {
            _curEncoding = Encoding.GetEncoding(encodingString);
        }

        const string _svnFileName = @"svn";

        /// <summary>
        /// 获取文件的当前版本
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetFileCurVer(string filename)
        {
            //获取当前版本信息
            string CurFileResult = Process.Helper.RunCMDProcess(_svnFileName, "info " + Path.GetFileName(filename), Path.GetDirectoryName(filename));
            return GetVersionFromString(CurFileResult);
        }

        /// <summary>
        /// 获取当前SVN:url
        /// </summary>
        /// <param name="path"></param>
        /// <returns>当前路径SVN信息,key值全小写:URL,Repository Root,Repository UUID,Revision,Node Kind,Schedule</returns>
        public Dictionary<string,string> GetSVNUrl(string path)
        {
            //执行info命令
            var result = DoNet.Common.Process.Helper.RunCMDProcess(_svnFileName, SVNInfoCommand, path, 0);

            var stringreader = new StringReader(result);
            var dic = new Dictionary<string, string>();

            //截取url
            for (var line = stringreader.ReadLine(); line != null; line = stringreader.ReadLine())
            {
                var index = line.IndexOf(':');
                if (index > 0)
                {
                    var key = line.Substring(0, index).ToLower().Trim();                    
                    if (!dic.ContainsKey(key)) {
                        var value = line.Substring(index + 1).Trim();
                        dic.Add(key, value); 
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 获取分支
        /// </summary>
        /// <param name="url"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public string GetBranchByUrl(string url,ref string rooturl, ref string response)
        {
            url = url.ToLower().Trim('/') + "/";
            var branch = "";
            var branchindex = url.IndexOf("/trunk/", StringComparison.CurrentCultureIgnoreCase);
            if (branchindex >= 0)
            {
                branch = "trunk";
                rooturl = url.Substring(0, branchindex);
                branchindex += 7;
            }
            else
            {
                branchindex = url.IndexOf("/branches/", StringComparison.CurrentCultureIgnoreCase);
                if (branchindex >= 0)
                {
                    rooturl = url.Substring(0, branchindex);
                    branchindex += 10;
                    var branchend = url.IndexOf('/', branchindex);
                    branchend = branchend < 0 ? branchend = url.Length : branchend;
                    branch = url.Substring(branchindex, branchend - branchindex);//截取分支
                    branchindex = branchend + 1;
                }
                else
                {
                    branchindex = url.IndexOf("/tags/", StringComparison.CurrentCultureIgnoreCase);
                    if (branchindex >= 0)
                    {
                        rooturl = url.Substring(0, branchindex);
                        branchindex += 6;
                        var branchend = url.IndexOf('/', branchindex);
                        branchend = branchend < 0 ? branchend = url.Length : branchend;
                        branch = url.Substring(branchindex, branchend - branchindex);//截取分支
                        branchindex = branchend + 1;
                    }
                }
            }

            if (branchindex >= 0)
            {
                if (branchindex < url.Length)
                    response = url.Substring(branchindex);//获取后面的相对路径                   
            }

            return branch;
        }

        /// <summary>
        /// 拷贝SVN信息
        /// </summary>
        /// <param name="sourdir"></param>
        /// <param name="targetdir"></param>
        public void CopySvnInfo(string sourdir, string targetdir)
        {
            sourdir = IO.PathMg.CheckPath(sourdir, ".svn");
            targetdir = IO.PathMg.CheckPath(targetdir, ".svn");

           IO.DirectoryHelper.CopyDirectory(sourdir, targetdir, false);
        }

        /// <summary>
        /// 签出SVN库
        /// </summary>
        /// <param name="path">签出到的路径</param>
        /// <param name="url">库的URL地址</param>
        /// <param name="username">可选,用户名</param>
        /// <param name="password">可选,密码</param>
        public string Checkout(string path, string url, string username = "", string password = "",Func<string,string> fun = null)
        {
            var command = string.Format(SVNCheckoutCommand, _svnFileName, CreateLoginInfo(username, password), url, path);

            //执行命令
            var resutl = RunSvnCommand(path, command, fun);

            var ver = GetVersionFromString(resutl);//获取版本号

            return ver;
        }

        /// <summary>
        /// 导出库
        /// </summary>
        /// <param name="path">导出路径</param>
        /// <param name="url">地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="fun">消息回调</param>
        /// <returns></returns>
        public string Export(string path, string url, string username = "", string password = "", Func<string, string> fun = null)
        {
            var command = string.Format(SVNExportCommand, _svnFileName, CreateLoginInfo(username, password), url, path);

            //执行命令
            var resutl = RunSvnCommand(path, command, fun);

            var ver = GetVersionFromString(resutl);//获取版本号

            return ver;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">本地路径</param>
        /// <param name="url">创建目录URL</param>
        /// <param name="username">用户名，可选</param>
        /// <param name="password">用户密码，可选</param>
        /// <param name="message">备注，可选</param>
        /// <param name="fun">消息回调，可选</param>
        /// <returns></returns>
        public string CreateDir(string path, string url, string username = "", string password = "", string message = "", Func<string, string> fun = null)
        {
            //组合拷贝命令
            var command = string.Format(SVNCreateDirCommand, _svnFileName, CreateLoginInfo(username, password), url,message);

            //执行命令
            var resutl = RunSvnCommand(path, command, fun);

            var ver = GetVersionFromString(resutl);//获取版本号

            return ver;
        }

        /// <summary>
        /// 拷贝SVN库到分支或标签
        /// </summary>
        /// <param name="path">当前路径</param>
        /// <param name="url1">源URL</param>
        /// <param name="url2">目标URL</param>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="message">备注</param>
        /// <param name="fun">消息处理函数</param>
        /// <returns>返回版本号</returns>
        public string Copy(string path, string url1,string url2, string username = "", string password = "",string message="",Func<string,string> fun = null)
        {
            //组合拷贝命令
            var command = string.Format(SVNCopyCommand, _svnFileName, CreateLoginInfo(username,password), url1, url2, message);

            //执行命令
            var resutl = RunSvnCommand(path, command, fun);

            var ver = GetVersionFromString(resutl);//获取版本号

            return ver;
        }

        /// <summary>
        /// 获取二个SVN库或者标签之间的差异
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public List<DiffFile> GetDiff(string url1, string url2, string username = "", string password = "", string path = "", Func<string, string> fun = null)
        {
            var command = string.Format(SVNDiffCommand, _svnFileName, CreateLoginInfo(username,password),url1, url2);

            //执行查异化命令
            var result = RunSvnCommand(path, command, fun);
            
            var index = result.IndexOf("<?xml");
            var end = result.IndexOf("</diff>") - index + 7;
            var diffs = new List<DiffFile>();//差异集合
            if (index >= 0 && end > 0)
            {
                var xmldoc = result.Substring(index, end);//截取XML
                
                var xmlwr = new IO.XmlHelper();
                xmlwr.LoadXmlString(xmldoc);

                //获取需要的path节点信息
                var nodes = xmlwr.GetNodeListByDelegate(delegate(System.Xml.XmlNode xn)
                {
                    return xn.Name.Equals("path", StringComparison.CurrentCultureIgnoreCase)
                        && xn.ParentNode != null && xn.ParentNode.Name.Equals("paths", StringComparison.CurrentCultureIgnoreCase);
                });

                if (nodes != null)
                {
                    foreach (System.Xml.XmlNode cn in nodes)
                    {
                        var df = new DiffFile();
                        var item = IO.XmlHelper.ReadAttributeValue(cn, "item");//获取类型
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            switch (item.ToLower().Trim())
                            {
                                case "added": { df.DiffType = DiffFile.EnumDiff.Add; break; }
                                case "modified": { df.DiffType = DiffFile.EnumDiff.Update; break; }
                                case "deleted": { df.DiffType = DiffFile.EnumDiff.Delete; break; }                               
                            }
                        }
                        var kind = IO.XmlHelper.ReadAttributeValue(cn, "kind");//获取文件类型file or dir
                        df.FileType = (DiffFile.EnumFileType)Enum.Parse(typeof(DiffFile.EnumFileType), kind, true);
                        df.Url = cn.InnerText;

                        diffs.Add(df);
                    }
                }
            }

            return diffs;
        }

        /// <summary>
        /// 执行SVN命令
        /// </summary>
        /// <param name="path"></param>
        /// <param name="command"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public string RunSvnCommand(string path,string command,Func<string,string> fun = null)
        {
            var infos = new StringBuilder();

            Process.Helper.RunCMDProcess(path, 0, delegate(string info)
            {
                if (info == null) return;

                if (fun != null) fun(info);

                infos.AppendLine(info);

            }, delegate(string error)
            {
                if (error == null) return;
                if (fun != null) fun(error);

                infos.AppendLine(error);

            }, command);

            return infos.ToString();
        }

        /// <summary>
        /// 从信息中提取版本号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetVersionFromString(string info)
        {
            Regex reg = new Regex(@"Revision[:]?\s+(?<Ver>\d+)", RegexOptions.IgnoreCase);
            if (reg.IsMatch(info))
            {
                string curver = reg.Match(info).Result("${Ver}").ToString();
                return curver;
            }
            return "";
        }

        /// <summary>
        /// 合并SVN多路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public string CombinSvnPath(params string[] paths)
        {
            var path = "";
            if (paths != null)
            {
                foreach (var p in paths)
                {
                    if (string.IsNullOrWhiteSpace(p)) continue;
                    path +=  p.Trim('/') + '/';
                }
            }
            return path.Trim('/');
        }

        /// <summary>
        /// 生成登录信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string CreateLoginInfo(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                return string.Format("--username \"{0}\" --password \"{1}\"  --non-interactive ", username, password);
            }
            return "";
        }
    }

    /// <summary>
    /// SVN对比差异文件信息
    /// </summary>
    public class DiffFile
    {
        /// <summary>
        /// 文件变更类型枚举
        /// </summary>
        public enum EnumDiff
        {
            /// <summary>
            /// 未知
            /// </summary>
            None = 0,
            /// <summary>
            /// 新添文件
            /// </summary>
            Add = 1,
            /// <summary>
            /// 更新文件
            /// </summary>
            Update = 2,
            /// <summary>
            /// 被删除文件
            /// </summary>
            Delete = 3
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public enum EnumFileType
        { 
            File=0,
            Dir=1
        }

        /// <summary>
        /// 在库中的绝对路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public EnumDiff DiffType { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public EnumFileType FileType { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(Url);
            }
        }

    }
}
