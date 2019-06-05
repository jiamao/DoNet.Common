/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/7/8 15:43:13
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.Globalization;
using System.Runtime.Serialization;

namespace DoNet.Common.Net
{
    /// <summary>
    /// 请求的资源
    /// </summary>
    public class JMHttpResponse :IDisposable
    {
        HttpWebResponse _response;
        /// <summary>
        /// 当前请求资源
        /// </summary>
        public HttpWebResponse Response
        {
            get { return _response; }
        }

        public JMHttpResponse(HttpWebResponse response)
        {
            _response = response;
        }

        Encoding _encoding;
        /// <summary>
        /// 资 源编码
        /// </summary>
        public Encoding ResponseEncoding
        {
            get {
                if (_encoding == null)
                {
                    var str = string.IsNullOrWhiteSpace(_response.ContentEncoding) ? _response.CharacterSet : _response.ContentEncoding;
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        _encoding = Encoding.UTF8;
                    }
                    else
                    {
                        _encoding = Encoding.GetEncoding(str);
                    }
                }
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }

        /// <summary>
        /// 读取所有字节
        /// </summary>
        /// <returns></returns>
        public List<byte> ReadAllBytes()
        {
            var bs = new List<byte>();
            var stream = _response.GetResponseStream();
            if (stream.CanSeek)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }
            var b = stream.ReadByte();
            while (b != -1)
            {
                bs.Add((byte)b);
                b = stream.ReadByte();
            }

            return bs;
        }

        /// <summary>
        /// 读取所有资源字符
        /// </summary>
        /// <returns></returns>
        public string ReadToEnd()
        {
            return ReadToEnd(ResponseEncoding);
        }

        /// <summary>
        /// 用指定的编码读取字符
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadToEnd(Encoding encoding)
        {
            var data = ReadAllBytes();//读取所有字节

            //如果有utf8的bom头。则移除前三个字节
            if (data.Count > 2 && data[0] == 239 && data[1] == 187 && data[2] == 191)
            {
                data.RemoveRange(0, 3);
            }
            if (data != null && data.Count > 0)
            {
                return encoding.GetString(data.ToArray());
            }
            else 
            {
                return string.Empty;
            }
        }

        public void Dispose()
        {
            if (Response != null)
            {
                Response.Close();
            }
        }
    }
}
