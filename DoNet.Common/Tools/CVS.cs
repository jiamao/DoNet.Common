//////////////////////////////////////////////////
// Author   : 丁峰峰
// Date     : 2010/09/15
// Usage    : CVS操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace DoNet.Common.Tool
{
    public class CVS
    {
        //文件编码
        static Encoding _curEncoding = Encoding.GetEncoding("gb2312");

        public CVS()
        {
        }
        public CVS(string encodingString)
        {
            _curEncoding = Encoding.GetEncoding(encodingString);
        }

        const string _cvsFileName = "cvs";

        /// <summary>
        /// 获取当前文件的最版本号
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileLastVer(string fileName)
        {
            List<string> vers = GetFileVerList(fileName);
            string lastver = "";
            foreach (string ver in vers)
            {
                if (lastver == "") lastver = ver;
                else
                {
                    if (string.Compare(ver, lastver) > 0)
                        lastver = ver;
                }
            }
            return lastver;
        }

        /// <summary>
        /// 获取文件版本列表
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<string> GetFileVerList(string filename)
        {
            //获取版本列表
            string LogFileResult =Process.Helper.RunCMDProcess(_cvsFileName, "log " + 
                System.IO.Path.GetFileName(filename), System.IO.Path.GetDirectoryName(filename));
            Regex reg = new Regex(@"revision (?<Ver>\d+(\.\d+)+)");

            List<string> verlist = new List<string>();
            foreach (Match m in reg.Matches(LogFileResult))
            {
                string ver = m.Result("${Ver}").ToString();
                verlist.Add(ver);
            }

            return verlist;
        }

        /// <summary>
        /// 获取文件的当前版本
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetFileCurVer(string filename)
        {
            //获取当前版本信息
            string CurFileResult = Process.Helper.RunCMDProcess(_cvsFileName, "status " + Path.GetFileName(filename), Path.GetDirectoryName(filename));
            Regex reg = new Regex(@"Working revision:\t(?<Ver>\d+(\.\d+)+)");
            if (reg.IsMatch(CurFileResult))
            {
                string curver = reg.Match(CurFileResult).Result("${Ver}").ToString();
                return curver;
            }
            return "";
        }

        /// <summary>
        /// 判断是否存在些文件夹在CVS中
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsRightCVSDir(string dir)
        {
            List<string> alldirver = GetFileVerList(dir);
            if (alldirver == null || alldirver.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// 判断是否是正确的CVS路径
        /// </summary>
        /// <param name="parentDir"></param>
        /// <param name="lastPath"></param>
        /// <returns></returns>
        public bool IsRightCVSPath(string parentDir, string lastPath)
        {
            //获取当前版本信息
            string Result = Process.Helper.RunCMDProcess(_cvsFileName, "log -R \"" + lastPath + "\"", parentDir);
            return !Result.Contains("cvs server: nothing known about");
        }

        /// <summary>
        /// 检查文件路径
        /// 如果不存在则生成其父路径和CVS信息
        /// 此方法避免了只获取一个当前不存在的文件或其父目录都不存在而update整个目录，大大节省时间
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool CheckCVSFilePath(string filename, bool chkCvsPath)
        {
            string parentRespository = "";
            string RootInfo = "";
            string ParentCVSDir = "";
            List<string> CreatedDirs = new List<string>();
            Dictionary<string, string> WritedLines = new Dictionary<string, string>();
            if (CheckCVSDirPath(filename, ref parentRespository, ref RootInfo, ref ParentCVSDir, CreatedDirs, WritedLines))
            {
                if (CheckCVSFileExistsDirInfo(filename, ref parentRespository, ref RootInfo, false, CreatedDirs, WritedLines))
                {
                    string lastPath = filename.Substring(ParentCVSDir.Length).Trim('\\');
                    if ((CreatedDirs.Count > 0 || WritedLines.Count > 0) && chkCvsPath && !IsRightCVSPath(ParentCVSDir, lastPath))
                    {
                        foreach (string cd in CreatedDirs)
                        {
                           Common.IO.DirectoryHelper.DeleteDirectory(cd);
                        }
                        ClearWritedLines(WritedLines);//清除已改写的文件
                    }
                    else return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 回滚生成的信息
        /// </summary>
        /// <param name="WritedLines"></param>
        private void ClearWritedLines(Dictionary<string, string> WritedLines)
        {
            foreach (string key in WritedLines.Keys)
            {
                if (!File.Exists(key)) continue;
                try
                {
                    List<string> OldInfos = new List<string>();
                    FileStream fs = new FileStream(key, FileMode.Open, FileAccess.ReadWrite);
                    StreamReader sr = new StreamReader(fs, _curEncoding);
                    for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
                    {
                        if (line.ToLower().Trim() != WritedLines[key])
                        {
                            OldInfos.Add(line);
                        }
                    }
                    sr.Close();
                    fs.Close();
                    Common.IO.FileHelper.DeleteFile(key);
                    fs = new FileStream(key, FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs, _curEncoding);
                    foreach (string line in OldInfos)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 检查CVS路径的正确性        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool CheckCVSDirPath(string filename, ref string parentRespository, ref string RootInfo, ref string ParentCVSDir, List<string> CreatedDirs, Dictionary<string, string> WritedLines)
        {
            string dir = Path.GetDirectoryName(filename);
            ParentCVSDir = CheckCVSDirExists(dir);

            if (ParentCVSDir.Length == dir.Length) return true;
            else if (string.IsNullOrEmpty(ParentCVSDir))
            {
                return false;
            }
            else
            {
                string dirlast = dir.Substring(ParentCVSDir.Length).Trim().Trim('\\');
                string[] lastlist = dirlast.Split('\\');
                string curdir = ParentCVSDir;
                if (!CheckCVSFileExistsDirInfo(curdir, ref parentRespository, ref RootInfo, true, CreatedDirs, WritedLines))
                    return false;
                foreach (string l in lastlist)
                {
                    curdir = Path.Combine(curdir, l);
                    if (!CheckCVSFileExistsDirInfo(curdir, ref parentRespository, ref RootInfo, true, CreatedDirs, WritedLines))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断CVS文件中的信息
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <param name="parentRespository"></param>
        /// <param name="RootInfo"></param>
        /// <param name="isDir"></param>
        /// <param name="CreatedDir"></param>
        /// <param name="WritedLines"></param>
        /// <returns></returns>
        public bool CheckCVSFileExistsDirInfo(string TargetPath, ref string parentRespository, ref string RootInfo, bool isDir, List<string> CreatedDir, Dictionary<string, string> WritedLines)
        {
            try
            {
                TargetPath = TargetPath.Trim('\\');
                string ParentDir = Path.GetDirectoryName(TargetPath);
                string cvsdir = isDir ? Path.Combine(TargetPath, "CVS") : Path.Combine(ParentDir, "CVS");
                string parentCVSdir = Path.Combine(ParentDir, "CVS");
                string resposeFile = Path.Combine(cvsdir, "Repository");
                string EntriesFile = Path.Combine(cvsdir, "Entries");
                string ExtraFile = Path.Combine(cvsdir, "Entries.Extra");
                string RootFile = Path.Combine(cvsdir, "Root");
                string targetName = Path.GetFileName(TargetPath);

                if (!string.IsNullOrEmpty(parentRespository) && isDir) parentRespository += "/" + Path.GetFileName(TargetPath);

                if (isDir && !Directory.Exists(TargetPath))
                {
                    Common.IO.DirectoryHelper.CreateDirectory(TargetPath);
                    CreatedDir.Add(TargetPath);
                }
                if (isDir && !Directory.Exists(cvsdir))
                {
                    Common.IO.DirectoryHelper.CreateDirectory(cvsdir);
                    Common.IO.FileHelper.SetFileAttribute(cvsdir, FileAttributes.Hidden);
                    CreatedDir.Add(cvsdir);
                }

                if (!File.Exists(resposeFile) && !string.IsNullOrEmpty(parentRespository))
                {
                    using (StreamWriter sw = new StreamWriter(resposeFile, false, _curEncoding))
                    {
                        sw.WriteLine(parentRespository);
                    }
                }
                else if (string.IsNullOrEmpty(parentRespository) && File.Exists(resposeFile))
                {
                    using (StreamReader sr = new StreamReader(resposeFile, _curEncoding))
                    {
                        parentRespository = sr.ReadLine();
                    }
                }

                if (!File.Exists(RootFile) && !string.IsNullOrEmpty(RootInfo))
                {
                    using (StreamWriter sw = new StreamWriter(RootFile, false, _curEncoding))
                    {
                        sw.WriteLine(RootInfo);
                    }
                }
                else if (string.IsNullOrEmpty(RootInfo) && File.Exists(RootFile))
                {
                    using (StreamReader sr = new StreamReader(RootFile, _curEncoding))
                    {
                        RootInfo = sr.ReadLine();
                    }
                }

                CheckCVSFileInfoInCvs(Path.Combine(parentCVSdir, "Entries"), targetName, isDir, WritedLines);
                CheckCVSFileInfoInCvs(Path.Combine(parentCVSdir, "Entries.Extra"), targetName, isDir, WritedLines);

                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 检查CVS信息中是否存在文件或文件夹信息
        /// </summary>
        /// <param name="cvsFileName"></param>
        /// <param name="TargetName"></param>
        /// <param name="isDir"></param>
        /// <returns></returns>
        public bool CheckCVSFileInfoInCvs(string cvsFileName, string TargetName, bool isDir, Dictionary<string, string> WritedLines)
        {
            try
            {
                if (string.IsNullOrEmpty(cvsFileName) || string.IsNullOrEmpty(TargetName)) return true;
                FileStream fs = new FileStream(cvsFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(fs, _curEncoding);
                bool existsinfo = false;
                string info = isDir ? "D/" + TargetName + "////" : "/" + TargetName + "//";//文件为二个。。文件夹为四个
                for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
                {
                    if (isDir)
                    {
                        if (line.ToLower().StartsWith(info.ToLower())) { existsinfo = true; break; }
                    }
                    else
                    {
                        if (line.ToLower().StartsWith(info.ToLower())) { existsinfo = true; break; }
                    }
                }
                if (!existsinfo)
                {
                    StreamWriter sw = new StreamWriter(fs, _curEncoding);
                    sw.WriteLine(info);
                    sw.Close();
                    WritedLines.Add(cvsFileName, info.ToLower().Trim());
                }
                fs.Close();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 检查CVS文件夹是否正确
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string CheckCVSDirExists(string dir)
        {
            string curdircvs = Path.Combine(dir, "cvs");
            if (Directory.Exists(dir) && Directory.Exists(curdircvs))
            {
                return dir;
            }
            else
            {
                string parentdir = Path.GetDirectoryName(dir);
                if (parentdir.Length <= 3) return "";
                return CheckCVSDirExists(parentdir);
            }
        }

        /// <summary>
        /// 获取ＣＶＳ的目录与文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="files"></param>
        /// <param name="dirs"></param>
        public static void GetCvsDetail(string dir, List<string> files, List<string> dirs, Encoding en, bool IsAllDirs)
        {
            string cvsdir = Path.Combine(dir, "cvs");
            if (en == null) en = _curEncoding;
            List<string> curchiddirs = new List<string>();
            if (Directory.Exists(cvsdir))
            {
                string cvsfile = Path.Combine(cvsdir, "Entries");
                if (!File.Exists(cvsfile)) return;
                StreamReader sr = new StreamReader(cvsfile, en);
                for (string line = sr.ReadLine(); !string.IsNullOrEmpty(line); line = sr.ReadLine())
                {
                    line = line.ToLower().Trim();
                    if (line.StartsWith("d") && line.IndexOf('/') > 0 && line.Length > 3)
                    {
                        line = line.Substring(2);
                        line = line.Substring(0, line.IndexOf('/'));
                        curchiddirs.Add(Path.Combine(dir, line));
                    }
                    else if (line.StartsWith("/"))
                    {
                        line = line.Substring(1);
                        line = line.Substring(0, line.IndexOf('/'));
                        files.Add(Path.Combine(dir, line));
                    }
                }
                sr.Close();
            }
            dirs.AddRange(curchiddirs);
            if (IsAllDirs)
            {
                foreach (string cdir in curchiddirs)
                {
                    GetCvsDetail(cdir, files, dirs, en, IsAllDirs);
                }
            }
        }

        /// <summary>
        /// 检查文件是否存在此标签
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string CheckCVSFileTag(string filename, string tag)
        {
            filename = Common.IO.PathMg.CheckPath(filename);
            string parentdir = Path.GetDirectoryName(filename);
            filename = Path.GetFileName(filename);
            string tagresult = Process.Helper.RunCMDProcess(parentdir, 0, "cvs log -T -r \"" + tag + "\" \"" + filename + "\"");
            int startindex = tagresult.IndexOf("symbolic names:");
            if (startindex > -1)
            {
                tagresult = tagresult.Substring(startindex);
                tag = "\t" + tag + ":";
                int tagindex = tagresult.IndexOf(tag);
                if (tagindex > -1)
                {
                    tagresult = tagresult.Substring(tagindex);
                    int splitindex = tagresult.IndexOf(":");
                    tagresult = tagresult.Substring(splitindex + 1, tagresult.IndexOf("\r\n", splitindex) - splitindex - 1);
                    return tagresult;
                }
            }
            return "";
        }

        /// <summary>
        /// 获取CVS路径信息
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string GetResponseInfo(string dir)
        {
            string resposeFile = Path.Combine(Path.Combine(dir, "CVS"), "Repository");
            if (!File.Exists(resposeFile)) return "";
            string response = "";
            using (StreamReader sr = new StreamReader(resposeFile, _curEncoding))
            {
                response = sr.ReadLine();
            }
            return response;
        }

        /// <summary>
        /// 检查文件是否存在此标签
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public List<string> CheckCVSDirTag(string dir, string tag)
        {
            dir = Common.IO.PathMg.CheckPath(dir);

            List<string> FilesInTags = new List<string>();
            string responseinfo = GetResponseInfo(dir);
            responseinfo = responseinfo.Trim().Trim('/').ToLower();
            Process.Helper.delegateErrorInfoReceived showerr = null;
            string fileInTag = "";
            Process.Helper.delegateNormalInfoReceived showinfo = new Process.Helper.delegateNormalInfoReceived(delegate(string info)
            {
                if (string.IsNullOrEmpty(info)) return;
                string infotmp = info.ToLower();
                if (infotmp.StartsWith("rcs file:"))
                {
                    fileInTag = infotmp.Substring(10, info.Length - 12).Trim().Trim('/');
                    int responseindex = fileInTag.IndexOf(responseinfo);
                    fileInTag = fileInTag.Substring(responseindex + responseinfo.Length + 1).Trim('/').Replace("/", "\\");
                }
                else if (info.StartsWith("\t" + tag + ":") && fileInTag != "")
                {
                    fileInTag += "|" + info.Substring(info.IndexOf(":") + 1).Trim();
                    FilesInTags.Add(fileInTag);
                    fileInTag = "";
                }
            });
            Process.Helper.RunCMDProcess(dir, 0, showinfo, showerr, "cvs log -T -r\"" + tag + "\"");

            return FilesInTags;
        }

        /// <summary>
        /// 为单个文件打标签
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="cmds"></param>
        /// <param name="count"></param>
        public bool SetFileTag(string fileName, string tag, ref int count, int CanErrorCount)
        {
            count++;
            string result = Process.Helper.RunCMDProcess(Path.GetDirectoryName(fileName), 0, "cvs tag \"" + tag + "\" \"" + Path.GetFileName(fileName) + "\""); ;
            if (result.ToLower().Contains("[tag aborted]:"))
            {
                if (count >= CanErrorCount)
                {
                    return false;
                }
                return SetFileTag(fileName, tag, ref count, CanErrorCount);
            }
            return true;
        }

        /// <summary>
        /// 签 出文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cvsFilename"></param>
        /// <param name="Addr"></param>
        /// <param name="port"></param>
        /// <param name="rootPath"></param>
        /// <param name="cvspath"></param>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CheckOutDir(string dir, string cvsFilename, string Addr, string port, string rootPath, string cvspath, string userid, string pwd)
        {
            return CheckOutDir(dir, cvsFilename, Addr, port, rootPath, "", cvspath, userid, pwd);
        }
        /// <summary>
        /// 签出文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cvsFilename"></param>
        /// <param name="Addr"></param>
        /// <param name="port"></param>
        /// <param name="rootPath"></param>
        /// <param name="tag"></param>
        /// <param name="cvspath"></param>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CheckOutDir(string dir, string cvsFilename, string Addr, string port, string rootPath, string tag, string cvspath, string userid, string pwd)
        {
            string root = ":pserver:" + userid + "@" + Addr + ":" + port + "/" + rootPath.Trim('/');
            return CheckOutDir(dir, cvsFilename, root, cvspath, pwd);
        }

        /// <summary>
        /// 签出文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cvsFilename"></param>
        /// <param name="root"></param>
        /// <param name="cvspath"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CheckOutDir(string dir, string cvsFilename, string root, string cvspath, string pwd)
        {
            if (string.IsNullOrEmpty(cvsFilename)) cvsFilename = "cvs";
            string runResult = Process.Helper.RunCMDProcess(dir, 5000, cvsFilename + " -d" + root + " login", pwd);
            if (string.IsNullOrEmpty(runResult.Trim()) || runResult.Contains("[login aborted]") || runResult.Contains("超时"))
            {
                return false;
            }
            runResult = Process.Helper.RunCMDProcess(dir, 0, cvsFilename + " -d" + root + " checkout " + cvspath);
            if (runResult.Contains("[checkout aborted]")) return false;
            return true;
        }

        /// <summary>
        /// 更新文件夹
        /// </summary>
        /// <param name="dir"></param>
        public void UpdateDirectory(string dir)
        {
            Process.Helper.RunCMDProcess(_cvsFileName, "update -d ", dir);
        }

        /// <summary>
        /// 更新文件为最新版
        /// </summary>
        /// <param name="filename"></param>
        public void UpdateFile(string filename, string ver)
        {
            string re = Process.Helper.RunCMDProcess(_cvsFileName, "up " + (string.IsNullOrEmpty(ver) ? "" : "-r" + ver + " ") + "\"" + Path.GetFileName(filename) + "\"", Path.GetDirectoryName(filename));
            // Console.WriteLine(re);
        }

        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="filename"></param>
        public bool CheckUpdateFile(string filename)
        {
            string lastver = GetFileLastVer(filename);//获取最新版本
            string curver = GetFileCurVer(filename);//获取当前版本

            if (string.IsNullOrEmpty(lastver))
            {
                Console.WriteLine("无法获取文件的信息，跳过：" + filename);
                return true;
            }
            if (!string.IsNullOrEmpty(lastver) && !string.IsNullOrEmpty(curver) && string.Compare(lastver, curver) > 0)//如果最新版本大于当前版本
            {
                Console.WriteLine(filename + "当前版本不是最新版本，重新获取！");
                UpdateFile(filename, lastver);
                //lastver = GetFileLastVer(filename);//获取最新版本
                curver = GetFileCurVer(filename);//获取当前版本
            }
            if (string.IsNullOrEmpty(lastver) && string.IsNullOrEmpty(curver)) return false;
            else if (string.IsNullOrEmpty(curver) && !string.IsNullOrEmpty(lastver))
            {
                UpdateFile(filename, lastver);
            }
            return string.Compare(lastver, curver) <= 0;
        }

        /// <summary>
        /// 复制CVS信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void CopyCvsInfo(string source, string target)
        {
            source = Common.IO.PathMg.CheckPath(source, "CVS");
            target = Common.IO.PathMg.CheckPath(target, "CVS");
            Common.IO.DirectoryHelper.CopyDirectory(source, target);
            Common.IO.FileHelper.SetFileAttribute(source, FileAttributes.Hidden);
            Common.IO.FileHelper.SetFileAttribute(target, FileAttributes.Hidden);
        }
    }
}
