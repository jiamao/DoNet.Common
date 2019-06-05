/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/10 11:20:18
// Usage    :分页数据
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.Data
{
    [Serializable]
    [System.Runtime.Serialization.DataContract]
    public class BasePageData
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Data.DataSet Source
        {
            get;
            set;
        }       

        /// <summary>
        /// 当前第几页
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int Index
        { get; set; }

        /// <summary>
        /// 总共查得多少页
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// 总共有多少符合条件的记录
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int DataCount { get; set; }
    }

    /// <summary>
    /// 分页数据
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract]
    public class PageData<T> : BasePageData where T : class
    {
        /// <summary>
        /// 当前查得的数据
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public List<T> Data
        {
            get;
            set;
        }

        /// <summary>
        /// 当前记录数
        /// </summary>       
        public int Count
        {
            get
            {
                if (Data == null) return 0;
                return Data.Count;
            }
        }
    }
}
