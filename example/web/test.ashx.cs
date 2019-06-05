using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JM.Common.Examples.web
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var bs = new byte[context.Request.InputStream.Length];
            var l = context.Request.InputStream.Read(bs, 0, bs.Length);
            var par = System.Text.Encoding.UTF8.GetString(bs);
            var result = "{\"respons\": {\"state\": 1, \"result\": \"hahahah\"}}";

            context.Response.Write(result);
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