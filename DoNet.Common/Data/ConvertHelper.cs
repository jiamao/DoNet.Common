//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 类型转换
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public class ConvertHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct BS2
        {
            //[FieldOffset(0)]   
            public byte b1;
            //   [FieldOffset(8)]   
            public byte b2;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct BS4
        {
            //[FieldOffset(0)]   
            public byte b1;
            //   [FieldOffset(8)]   
            public byte b2;
            //   [FieldOffset(16)]   
            public byte b3;
            //   [FieldOffset(24)]   
            public byte b4;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct BS8
        {
            //[FieldOffset(0)]   
            public byte b1;
            //   [FieldOffset(8)]   
            public byte b2;
            //   [FieldOffset(16)]   
            public byte b3;
            //   [FieldOffset(24)]   
            public byte b4;
            //   [FieldOffset(32)]   
            public byte b5;
            //   [FieldOffset(40)]   
            public byte b6;
            //   [FieldOffset(48)]   
            public byte b7;
            //   [FieldOffset(56)]   
            public byte b8;
        }

        #region 长整型
        [StructLayout(LayoutKind.Explicit)]
        struct UlBs
        {
            [FieldOffset(0)]
            public ulong ulvalue;
            [FieldOffset(0)]
            public BS8 bvalue;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct LBs
        {
            [FieldOffset(0)]
            public long lvalue;
            [FieldOffset(0)]
            public BS8 bvalue;
        }



        /// <summary>
        /// 把无符号长整型转为字节
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static byte[] ULongToBytes(ulong v)
        {
            UlBs ubs = new UlBs();
            ubs.ulvalue = v;
            byte[] bs = new byte[8];
            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { bs[i] = ubs.bvalue.b1; break; }
                    case 1: { bs[i] = ubs.bvalue.b2; break; }
                    case 2: { bs[i] = ubs.bvalue.b3; break; }
                    case 3: { bs[i] = ubs.bvalue.b4; break; }
                    case 4: { bs[i] = ubs.bvalue.b5; break; }
                    case 5: { bs[i] = ubs.bvalue.b6; break; }
                    case 6: { bs[i] = ubs.bvalue.b7; break; }
                    case 7: { bs[i] = ubs.bvalue.b8; break; }
                }
            }

            return bs;
        }
        /// <summary>
        /// 把8字节转为无符号长整型
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static ulong BytesToUlong(byte[] bs)
        {
            UlBs ubs = new UlBs();
            ubs.bvalue = new BS8();

            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { ubs.bvalue.b1 = bs[i]; break; }
                    case 1: { ubs.bvalue.b2 = bs[i]; break; }
                    case 2: { ubs.bvalue.b3 = bs[i]; break; }
                    case 3: { ubs.bvalue.b4 = bs[i]; break; }
                    case 4: { ubs.bvalue.b5 = bs[i]; break; }
                    case 5: { ubs.bvalue.b6 = bs[i]; break; }
                    case 6: { ubs.bvalue.b7 = bs[i]; break; }
                    case 7: { ubs.bvalue.b8 = bs[i]; break; }
                }
            }

            return ubs.ulvalue;
        }

        /// <summary>
        /// 把无符号长整型转为字节
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static byte[] LongToBytes(long v)
        {
            LBs lbs = new LBs();
            lbs.lvalue = v;
            byte[] bs = new byte[8];
            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { bs[i] = lbs.bvalue.b1; break; }
                    case 1: { bs[i] = lbs.bvalue.b2; break; }
                    case 2: { bs[i] = lbs.bvalue.b3; break; }
                    case 3: { bs[i] = lbs.bvalue.b4; break; }
                    case 4: { bs[i] = lbs.bvalue.b5; break; }
                    case 5: { bs[i] = lbs.bvalue.b6; break; }
                    case 6: { bs[i] = lbs.bvalue.b7; break; }
                    case 7: { bs[i] = lbs.bvalue.b8; break; }
                }
            }

            return bs;
        }
        /// <summary>
        /// 把8字节转为无符号长整型
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static long BytesToLong(byte[] bs)
        {
            LBs lbs = new LBs();
            lbs.bvalue = new BS8();

            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { lbs.bvalue.b1 = bs[i]; break; }
                    case 1: { lbs.bvalue.b2 = bs[i]; break; }
                    case 2: { lbs.bvalue.b3 = bs[i]; break; }
                    case 3: { lbs.bvalue.b4 = bs[i]; break; }
                    case 4: { lbs.bvalue.b5 = bs[i]; break; }
                    case 5: { lbs.bvalue.b6 = bs[i]; break; }
                    case 6: { lbs.bvalue.b7 = bs[i]; break; }
                    case 7: { lbs.bvalue.b8 = bs[i]; break; }
                }
            }

            return lbs.lvalue;
        }
        #endregion

        #region 16位整型
        [StructLayout(LayoutKind.Explicit)]
        struct I16Bs
        {
            [FieldOffset(0)]
            public Int16 i16value;
            [FieldOffset(0)]
            public BS2 bvalue;
        }

        /// <summary>
        /// 把16整型转为字节
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static byte[] Int16ToBytes(Int16 v)
        {
            I16Bs lbs = new I16Bs();
            lbs.i16value = v;
            byte[] bs = new byte[2];
            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { bs[i] = lbs.bvalue.b1; break; }
                    case 1: { bs[i] = lbs.bvalue.b2; break; }
                }
            }

            return bs;
        }
        /// <summary>
        /// 把8字节转为无符号长整型
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static Int16 BytesToInt16(byte[] bs)
        {
            I16Bs lbs = new I16Bs();
            lbs.bvalue = new BS2();

            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { lbs.bvalue.b1 = bs[i]; break; }
                    case 1: { lbs.bvalue.b2 = bs[i]; break; }

                }
            }

            return lbs.i16value;
        }
        #endregion

        #region 32位整型
        [StructLayout(LayoutKind.Explicit)]
        struct I32Bs
        {
            [FieldOffset(0)]
            public int i32value;
            [FieldOffset(0)]
            public BS4 bvalue;
        }

        /// <summary>
        /// 把32整型转为字节
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static byte[] Int32ToBytes(int v)
        {
            I32Bs lbs = new I32Bs();
            lbs.i32value = v;
            byte[] bs = new byte[4];
            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { bs[i] = lbs.bvalue.b1; break; }
                    case 1: { bs[i] = lbs.bvalue.b2; break; }
                    case 2: { bs[i] = lbs.bvalue.b3; break; }
                    case 3: { bs[i] = lbs.bvalue.b4; break; }
                }
            }

            return bs;
        }
        /// <summary>
        /// 把4字节转为整型
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static int BytesToInt32(byte[] bs)
        {
            I32Bs lbs = new I32Bs();
            lbs.bvalue = new BS4();

            for (int i = 0; i < bs.Length; i++)
            {
                switch (i)
                {
                    case 0: { lbs.bvalue.b1 = bs[i]; break; }
                    case 1: { lbs.bvalue.b2 = bs[i]; break; }
                    case 2: { lbs.bvalue.b3 = bs[i]; break; }
                    case 3: { lbs.bvalue.b4 = bs[i]; break; }
                }
            }

            return lbs.i32value;
        }
        #endregion

        #region 路径转换

        public const Int16 PathLength = 250;
        /// <summary>
        /// 将路径转成字节
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] PathToBytes(string path)
        {
            byte[] bs = new byte[PathLength];
            Encoding.UTF8.GetBytes(path, 0, path.Length, bs, PathLength - path.Length);
            return bs;
        }
        /// <summary>
        /// 转字节为路径
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static string BytesToPath(byte[] bs)
        {
            string p = Encoding.UTF8.GetString(bs);
            return p.Trim('\0');
        }
        #endregion

        /// <summary>
        /// 泛型转换成DataTable
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">泛型</param>
        /// <returns>返回表</returns>
        public static System.Data.DataTable ConvertToDataTable<T>(IList<T> list)
        {
            //参数为空或无记录
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            //指定表名创建表
            var dt = new System.Data.DataTable(typeof(T).Name);
            System.Data.DataColumn dc;
            System.Data.DataRow dr;
            //获取公共成员或实例成员的公有属性
            System.Reflection.PropertyInfo[] propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                dr = dt.NewRow();
                int j = propertyInfo.Length;
                for (int i = 0; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = propertyInfo[i];
                    string strName = pi.Name;
                    if (dt.Columns[strName] == null)
                    {
                        dc = new System.Data.DataColumn(strName, pi.PropertyType);
                        dt.Columns.Add(dc);
                    }
                    dr[strName] = pi.GetValue(t, null);
                }
                dt.Rows.Add(dt);
            }
            return dt;
        }
        /// <summary>
        /// 转为表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <returns></returns>
        public static System.Data.DataTable ConvertToDataTable<T>(ICollection<T> List)
        {
            List<T> ToList = new List<T>();
            foreach (T t in List)
            {
                ToList.Add(t);
            }
            return ConvertToDataTable<T>(ToList);
        }
    }
}
