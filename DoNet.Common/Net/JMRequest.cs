/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/7/8 15:42:59
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace DoNet.Common.Net
{
    /// <summary>
    /// 请求
    /// </summary>
    public class JMRequest : WebRequest
    {
        public JMRequest() { }

        /// <summary>
        /// 通过地址实例化http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        public static JMHttpRequest CreateHttp(string url)
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            return new JMHttpRequest(request);
        }

        /// <summary>
        /// 读取http请求的信息
        /// </summary>
        /// <param name="url">访问路径</param>
        /// <returns></returns>
        public static string ReadHttpContent(string url)
        {
            var request = CreateHttp(url);
            var response = request.GetResponse();
            return response.ReadToEnd();
        }

        /// <summary>
        /// 读取http请求的信息
        /// </summary>
        /// <param name="url">访问路径</param>
        /// <param name="encoding">读取编码</param>
        /// <returns></returns>
        public static string ReadHttpContent(string url,Encoding encoding)
        {
            var request = CreateHttp(url);
            var response = request.GetResponse();
            return response.ReadToEnd(encoding);
        }

        
    }

    /// <summary>
    /// http请求
    /// </summary>
    public class JMHttpRequest
    {
        HttpWebRequest request;
        /// <summary>
        /// 当前请求
        /// </summary>
        public HttpWebRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// 实例化一个家猫请求
        /// </summary>
        /// <param name="request">WebRequest</param>
        public JMHttpRequest(HttpWebRequest req)
        {
            request = req;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">需要写的数据</param>
        public void Write(byte[] data)
        {
            if (data == null) return;

            Request.ContentLength = data.Length;

            using (var stream = Request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);

                stream.Close();
            }
        }

        /// <summary>
        /// 写入头信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteHeader(string name,string value)
        {
            Request.Headers.Add(name,value);
        }

        /// <summary>
        /// 添加cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void WriteCookie(Cookie cookie)
        {
            request.CookieContainer.Add(cookie);
        }

        /// <summary>
        /// 获取请求的资源
        /// </summary>
        /// <returns></returns>
        public JMHttpResponse GetResponse()
        {
            var response = request.GetResponse() as HttpWebResponse;
            return new JMHttpResponse(response);
        }
    }
}
