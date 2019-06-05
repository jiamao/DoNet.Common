using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JM.Common.Examples.web
{
    /// <summary>
    /// socket 的摘要说明
    /// </summary>
    public class socket : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("hello");

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