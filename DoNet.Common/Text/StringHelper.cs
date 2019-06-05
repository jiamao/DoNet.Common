//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : string操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DoNet.Common.Text
{
    public static class StringHelper
    {
        /// <summary>
        /// 获取字符的字节长度
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int VarCharLength(this string source)
        {
            if (string.IsNullOrEmpty(source)) return 0;
            //如果是ascii码，如字母、数值等，长度为1
            //否则为2

            Encoding en = Encoding.UTF8;
            int length = 0;
            foreach (char c in source)
            {
                byte[] bs = en.GetBytes(c.ToString());
                if (bs.Length == 1 && bs[0] <= 127)
                {
                    length++;
                }
                else
                {
                    length += 2;
                }
            }
            return length;
        }

        /// <summary>
        /// 用字符串分割字串串，
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="splitstr">用来分割的字符串</param>
        /// <returns></returns>
        public static string[] Split(this string source, string splitstr)
        {
            List<string> re = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                int start = 0;
                int end = source.IndexOf(splitstr);
                while (end > 0)
                {
                    re.Add(source.Substring(start, end - start));
                    start = end + splitstr.Length;
                    end = source.IndexOf(splitstr, start);
                }
                //start = end + splitchar.Length;
                re.Add(source.Substring(start));
            }
            return re.ToArray();
        }

        /// <summary>
        /// html格式化text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHtml(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return text.Replace("<", "&lt;").Replace(">", "&gt;").Replace(" ", "&nbsp;").Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// 字符串转为url编码
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string UrlEncode(this string source)
        {
            if (null == source) return source;
            var re = System.Web.HttpUtility.UrlEncode(source);
            re = re.ToUpper();
            re = re.Replace("+","").Replace("*","").Replace("%7E","~").Replace("%0A","").Replace("%0D","");
            return re;
        }

        /// <summary>
        /// 获取字符串共同的起始字符
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为true</param>
        /// <returns></returns>
        public static string GetStartSameString(string str1, string str2, bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2)) return string.Empty;

            if (str1.Equals(str2) || (ignoreCase && str1.Equals(str2, StringComparison.OrdinalIgnoreCase))) return str1;

            for (var i = 0; i < str1.Length; i++)
            {
                if (i >= str2.Length) return str2;//如果已配匹到字符串2的末尾则返回第二个字符串
                if (str1[i].Equals(str2[i]) || (ignoreCase && char.ToLower(str1[i]) == char.ToLower(str2[i])))
                {
                    continue;
                }
                else
                {
                    if (i == 0) return string.Empty;//如果第一个就不相同
                    return str1.Substring(0, i);//截取到第i-1个字符
                }
            }

            return str1;
        }

        /// <summary>
        /// 获取二个路径前面相同的部分
        /// </summary>
        /// <param name="str1">路径1</param>
        /// <param name="str2">路径2</param>
        /// <param name="ignoreCase">是否铁略大小写</param>
        /// <returns></returns>
        public static string GetStartSamePath(string str1, string str2, bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2)) return string.Empty;

            if (str1.Equals(str2) || (ignoreCase && str1.Equals(str2, StringComparison.OrdinalIgnoreCase))) return str1;

            str1 = str1.Replace('\\', '/');
            str2 = str2.Replace('\\', '/');
            //先获取相同的部分
            var smpath = GetStartSameString(str1, str2, ignoreCase);

            if (!string.IsNullOrWhiteSpace(smpath) && smpath.Length > 3)
            {
                //如果相同的前缀刚好到/，则为正确的前置路径
                if (smpath.EndsWith("/"))
                    return smpath;


                //如果前缀对二个路径中的任一个都不是刚好到/
                //则再往上退一层目录
                if (((str1.Length > smpath.Length && str1[smpath.Length] != '/') ||
                    (str2.Length > smpath.Length && str2[smpath.Length] != '/')) ||
                    (str1.Length == smpath.Length && str2.Length > smpath.Length && str2[smpath.Length] != '/') ||
                    (str2.Length == smpath.Length && str1.Length > smpath.Length && str1[smpath.Length] != '/'))
                {
                    var index = smpath.LastIndexOf('/');
                    if (index > 0)
                    {
                        return smpath.Substring(0, index);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

            }

            return smpath;
        }

        /// <summary>
        /// 处理字符集转换
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TurnEncodingString(string source, string sourceEncoding, string toEncoding)
        {
            var en = System.Text.Encoding.GetEncoding(sourceEncoding);
            var bs = en.GetBytes(source);
            bs = Encoding.Convert(en, System.Text.Encoding.GetEncoding(toEncoding), bs);
            var a = System.Text.Encoding.GetEncoding(toEncoding).GetString(bs);
            return a;
        }

        /// <summary>
        /// 转换拉打字符为目标编码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toEncoding"></param>
        /// <returns></returns>
        public static string TurnEncodingFromLatin1(string source, string toEncoding)
        {
            var result =System.Text.Encoding.GetEncoding(toEncoding).GetString( System.Text.Encoding.GetEncoding("latin1").GetBytes(source));
            return result;
        }

        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断一个word是否为Big5编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsBig5Code(string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))     //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
