using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;


namespace JM.Update
{
    public class UpdateApp
    {
        private Action<string> MsgHandler = null;

        delegate void UpdateDelegate(string rootpath);

        /// <summary>
        /// 执行更新程序
        /// </summary>
        /// <param name="msgHandler"></param>
        public void Run(Action<string> msgHandler = null,string rootpath = null)
        {
            MsgHandler = msgHandler;
            var handler = new UpdateDelegate(RunUpdate);
            handler.BeginInvoke(rootpath,null, null);
        }

        private void RunUpdate(string rootpath)
        {
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["updateurl"];
                if (string.IsNullOrWhiteSpace(url))
                {
                    SendMsg("未配置更新地址...");
                    return;
                }
                SendMsg("获取更新信息中...");
                var configs = LoadUpdateConfig(url);//先获取配置信息
                if (configs == null || configs.Items == null || configs.Items.Count == 0)
                {
                    return;
                }
                SendMsg("获取更新信息完成，开始对比更新...");
                var web = new System.Net.WebClient();
                web.Encoding = System.Text.Encoding.UTF8;

                if (string.IsNullOrWhiteSpace(rootpath))
                {
                    rootpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                }
                //更新文件
                foreach (var item in configs.Items)
                {
                    try
                    {
                        var f = System.IO.Path.Combine(rootpath, item.FileName.Trim('/','\\'));

                        if (item.Mode == 1)
                        {
                            if (System.IO.File.Exists(f)) {
                                System.IO.File.Delete(f);
                            }
                        }
                        //如果存在此文件，且版本一致则不下载
                        if (System.IO.File.Exists(f) && Helper.GetFileMD5(f) == item.FileMd5)
                        {
                            writelog("文件:" + item.FileName + " 无需更新");
                            continue;
                        }

                        var itemurl = url + "/" + item.Url;
                        SendMsg("下载更新文件:" + item.FileName);
                       
                        var bs = web.DownloadData(itemurl);

                        System.IO.File.WriteAllBytes(f, bs);
                    }
                    catch (Exception ex)
                    {
                        SendMsg(ex.Message);
                        SendMsg("更新文件：" + item.FileName + " 失败");
                    }
                }
            }
            catch (Exception ex)
            {
                SendMsg(ex.Message);
            }
            finally
            {
                SendComplete();
            }
        }

        /// <summary>
        /// 从服务器上获取更新信息
        /// </summary>
        /// <returns></returns>
        UpdateConfig LoadUpdateConfig(string url)
        {
            var xml = new System.Net.WebClient().DownloadString(url + "/update.xml");
            writelog(xml);
            var configs = (UpdateConfig)Helper.XMLDerObjectFromString(typeof(UpdateConfig), xml);
            return configs;
        }

        private void SendComplete()
        {
            SendMsg("complete");
        }

        private void SendMsg(string msg)
        {
            if (MsgHandler != null)
            {
                MsgHandler(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
            writelog(msg);
        }

        /// <summary>
        /// 生成更新包
        /// </summary>
        /// <param name="path"></param>
        public static void CreateUpdatePack(string path,string target)
        {
            var files = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);
            if (!System.IO.Directory.Exists(target)) System.IO.Directory.CreateDirectory(target);
            var configs = new UpdateConfig() { Items = new List<UpdateItem>()};

            foreach (var f in files)
            {
                var item = new UpdateItem();
                item.FileName = f.Substring(path.Length);
                item.FileMd5 = Helper.GetFileMD5(f);
                item.Url = Guid.NewGuid().ToString("n") + ".zip";
                configs.Items.Add(item);

                var newfile = System.IO.Path.Combine(target, item.Url);
                System.IO.File.Copy(f, newfile, true);
            }

            var xml = System.IO.Path.Combine(target, "update.xml");
            Helper.XMLSerObject(configs, xml);
        }

        public static string LogPath = "";
        internal static void writelog(string msg)
        {
            try
            {
                using (var fs = new System.IO.FileStream(LogPath, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    var bs = System.Text.Encoding.UTF8.GetBytes(msg + "\r\n");
                    fs.Write(bs, 0, bs.Length);
                }
            }
            catch { }
        }
    }
}
