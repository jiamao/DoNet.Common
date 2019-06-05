using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DoNet.Common.DbUtility.Proxy
{
    /// <summary>
    /// 数据库代理接口
    /// </summary>
    [ServiceContract]
    public interface IDBHelper
    {
        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        [OperationContract]
        int ExecuteNonQuery(DBInfo dbinfo, string cmdText);

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        [OperationContract]
        int ExecuteNonQueryWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues);

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="dbparams"></param>
        /// <returns></returns>
        [OperationContract]
        int ExecuteNonQueryWithParams(DBInfo dbinfo, string cmdText, IDictionary<string, object> dbparams);

        /// <summary>
        /// 执行SQL语句返回数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据集</returns>
        [OperationContract]
        string GetDataSet(DBInfo dbinfo, string cmdText);

        /// <summary>
        /// 执行SQL语句返回数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据集</returns>
        [OperationContract]
        string GetDataSetWithParam(DBInfo dbinfo,string cmdText, string[] paraNames, object[] paraValues);

                /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        [OperationContract]
        object ExecuteScalar(DBInfo dbinfo, string cmdText);

                /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        [OperationContract]
        object ExecuteScalarWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues);

                /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        [OperationContract]
        int ExecuteSql(DBInfo dbinfo, ICollection<string> sqls);

                /// <summary>
        /// 在同一事务中
        /// 执行多条带参数的SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        [OperationContract]
        int ExecuteSqlWithParam(DBInfo dbinfo, List<string> sqls, List<string[]> paraNames, List<object[]> paraValues);
    }
}
