using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JM.Update
{
    [Serializable]
    public class UpdateItem
    {
        public string FileName { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// 0=平常处理,1=移除
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// 当前文件md5码
        /// </summary>
        public string FileMd5 { get; set; }
    }

    [Serializable]
    public class UpdateConfig
    {
        public string Version { get; set; }

        public List<UpdateItem> Items { get; set; }
    }
}
