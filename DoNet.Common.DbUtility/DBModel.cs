using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.DbUtility
{
    /// <summary>
    /// DB基础操作model
    /// </summary>
    [Serializable]
    public class DBModel
    {
        const string DBSetting = "default";

        /// <summary>
        /// 获取默认DB连接信息
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static DbUtility.DbORM GetDB(string setting = DBSetting)
        {
            return DbFactory.CreateDbORM(setting);
        }
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int Insert(string setting = DBSetting)
        {
            var db = GetDB();
            return db.Insert(this);
        }

        /// <summary>
        /// 新增并返回自增id
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int InsertWithIdentity(string setting = DBSetting)
        {
            var db = GetDB();
            return db.InsertWithIdentity(this);
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int Update(List<string> properties = null, string setting = DBSetting)
        {
            var db = GetDB();
            return db.Update(this, properties);
        }
    }
}
