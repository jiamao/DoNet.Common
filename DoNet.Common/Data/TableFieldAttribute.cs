using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 映射到表字段的属性
    /// </summary>
    [Serializable]
    public class TableFieldAttribute:Attribute
    {
        public TableFieldAttribute()
        {
            
        }

        /// <summary>
        /// 映射到表字段的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否为主健
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}
