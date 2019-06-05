using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoNet.Common.DbUtilityProxy
{
    /// <summary>
    /// DBProxy 的摘要说明
    /// </summary>
    public class DBProxy : IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Request.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.HeaderEncoding = context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "text/plain";
            object result = "";

            try
            {
                var bs = new byte[context.Request.InputStream.Length];
                
                var l = context.Request.InputStream.Read(bs, 0, bs.Length);
                var par = System.Text.Encoding.UTF8.GetString(bs);
                DoNet.Common.IO.Logger.Debug(par);

                var info = DoNet.Common.Serialization.JSon.JsonToModel<DbUtility.Proxy.ExecuteInfo>(par);

                if (info.Sqls != null && info.Sqls.Count > 0)
                {
                    var db = new DbUtility.Proxy.DBHelper();

                    switch (info.MethodName)
                    {
                        case "ExecuteNonQuery":
                            {
                                result = db.ExecuteNonQuery(info.DB, info.Sqls[0]);
                                break;
                            }
                        case "ExecuteNonQueryWithParams":
                        case "ExecuteNonQueryWithParam":
                            {
                                result = db.ExecuteNonQueryWithParam(info.DB, info.Sqls[0], info.ParaNames[0], info.ParaValues[0]);
                                break;
                            }
                        case "GetDataSet":
                            {
                                result = db.GetDataSet(info.DB, info.Sqls[0]);
                                break;
                            }
                        case "GetDataSetWithParam":
                            {
                                result = db.GetDataSetWithParam(info.DB, info.Sqls[0], info.ParaNames[0], info.ParaValues[0]);
                                break;
                            }
                        case "ExecuteScalar":
                            {
                                result = db.ExecuteScalar(info.DB, info.Sqls[0]);
                                break;
                            }
                        case "ExecuteScalarWithParam":
                            {
                                result = db.ExecuteScalarWithParam(info.DB, info.Sqls[0], info.ParaNames[0], info.ParaValues[0]);
                                break;
                            }
                        case "ExecuteSql":
                            {
                                result = db.ExecuteSql(info.DB, info.Sqls);
                                break;
                            }
                        case "ExecuteSqlWithParam":
                            {
                                result = db.ExecuteSqlWithParam(info.DB, info.Sqls, info.ParaNames, info.ParaValues);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex;
                DoNet.Common.IO.Logger.Write(ex);
            }
            context.Response.Write(result==null?"":result.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}