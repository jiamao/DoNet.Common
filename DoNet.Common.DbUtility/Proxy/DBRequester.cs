using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.DbUtility.Proxy
{
    public class DBRequester:IDBHelper
    {
        private string Request(string methodname, DBInfo dbinfo, List<string> sqls, List<string[]> paraNames = null, List<object[]> paraValues = null)
        {
            DoNet.Common.IO.Logger.Debug("Request :", methodname, dbinfo);

            var url = dbinfo.ProxyUrl??System.Configuration.ConfigurationManager.AppSettings["DBProxyUrl"];
            var request = DoNet.Common.Net.JMRequest.CreateHttp(url);
            request.Request.Method = "post";
            //request.Request.TransferEncoding = System.Text.Encoding.UTF8;

            var db = new ExecuteInfo() { 
             DB=dbinfo,
             MethodName = methodname,
               ParaNames=paraNames,
                ParaValues=paraValues,
                 Sqls=sqls
            };
            var data = System.Text.Encoding.UTF8.GetBytes(db.ToString());
            request.Write(data);

            var result = "";
            using (var response = request.GetResponse())
            {
                response.ResponseEncoding = System.Text.Encoding.UTF8;
                result = response.ReadToEnd();
            }

            if (!string.IsNullOrWhiteSpace(result))
            {
                DoNet.Common.IO.Logger.Debug(result);
            }
            return result;
        }

        public int ExecuteNonQuery(DBInfo dbinfo, string cmdText)
        {
            var r = Request("ExecuteNonQuery", dbinfo, new List<string>() { cmdText });
            return int.Parse(r);
        }

        public int ExecuteNonQueryWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            var r = Request("ExecuteNonQueryWithParam", dbinfo, new List<string>() { cmdText }, new List<string[]>() { paraNames }, new List<object[]>() { paraValues });
            return int.Parse(r);
        }

        public int ExecuteNonQueryWithParams(DBInfo dbinfo, string cmdText, IDictionary<string, object> dbparams)
        {
            var parnames = dbparams.Keys.ToArray<string>();
            var parvalues = dbparams.Values.ToArray<object>();

            var r = Request("ExecuteNonQueryWithParams", dbinfo, new List<string>() { cmdText }, new List<string[]>() { parnames }, new List<object[]>() { parvalues });
            return int.Parse(r);
        }

        public string GetDataSet(DBInfo dbinfo, string cmdText)
        {
            var r = Request("GetDataSet", dbinfo, new List<string>() { cmdText });
           
            var newds = Data.DataSet.FromString(r);
            
            return newds.ToString();
        }


        public string GetDataSetWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            var r = Request("GetDataSetWithParam", dbinfo, new List<string>() { cmdText }, new List<string[]>() { paraNames }, new List<object[]>() { paraValues });

            var newds = Data.DataSet.FromString(r);

            return newds.ToString();
        }

        public object ExecuteScalar(DBInfo dbinfo, string cmdText)
        {
            var r = Request("ExecuteScalar", dbinfo, new List<string>() { cmdText });
            return r;
        }

        public object ExecuteScalarWithParam(DBInfo dbinfo, string cmdText, string[] paraNames, object[] paraValues)
        {
            var r = Request("ExecuteScalarWithParam", dbinfo, new List<string>() { cmdText }, new List<string[]>() { paraNames }, new List<object[]>() { paraValues });

            return r;
        }

        public int ExecuteSql(DBInfo dbinfo, ICollection<string> sqls)
        {
            var r = Request("ExecuteSql", dbinfo, new List<string>(sqls));

            return int.Parse(r);
        }

        public int ExecuteSqlWithParam(DBInfo dbinfo, List<string> sqls, List<string[]> paraNames, List<object[]> paraValues)
        {
            var r = Request("ExecuteSqlWithParam", dbinfo, sqls, paraNames, paraValues);

            return int.Parse(r);
        }
    }
}
