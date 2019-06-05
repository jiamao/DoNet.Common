using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.DbUtility.Proxy
{
    /// <summary>
    /// 数据库访问的远程代理
    /// </summary>
    public class DBHelper:IDBHelper
    {
        /// <summary>
        /// 生成本地访问实例
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private DbUtility.DbORM CreateDB(DBInfo db)
        {
            DoNet.Common.IO.Logger.Debug(db);

            var dbHelper = DbFactory.CreateDbORM(db.DBType.ToString(), db.ConnectionString);
            return dbHelper;
        }

        public int ExecuteNonQuery(DBInfo dbinfo, string cmdText)
        {
            return CreateDB(dbinfo).ExecuteNonQuery(cmdText);
        }

        public int ExecuteNonQueryWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            return CreateDB(dbinfo).ExecuteNonQuery(cmdText, paraNames, paraValues);
        }

        public int ExecuteNonQueryWithParams(DBInfo dbinfo, string cmdText, IDictionary<string, object> dbparams)
        {
            return CreateDB(dbinfo).ExecuteNonQuery(cmdText, dbparams);
        }

        public string GetDataSet(DBInfo dbinfo, string cmdText)
        {

            var ds = CreateDB(dbinfo).GetDataSet(cmdText);
            var newds = new Data.DataSet(ds);
            
            return newds.ToString();
        }


        public string GetDataSetWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            var ds = CreateDB(dbinfo).GetDataSet(cmdText,paraNames,paraValues);
            var newds = new Data.DataSet(ds);
            
            return newds.ToString();
        }

        public object ExecuteScalar(DBInfo dbinfo, string cmdText)
        {
            return CreateDB(dbinfo).ExecuteScalar(cmdText);
        }

        public object ExecuteScalarWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            return CreateDB(dbinfo).ExecuteScalar(cmdText, paraNames,paraValues);
        }

        public int ExecuteSql(DBInfo dbinfo, ICollection<string> sqls)
        {
            return CreateDB(dbinfo).ExecuteSql(sqls);
        }

        public int ExecuteSqlWithParam(DBInfo dbinfo, List<string> sqls, List<string[]> paraNames, List<object[]> paraValues)
        {
            return CreateDB(dbinfo).ExecuteSql(sqls, paraNames, paraValues);
        }


    }
}
