/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/1 16:35:29
// Usage    : 本地缓存
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.Cache
{
    /// <summary>
    /// 缓存
    /// 使用静态变量保存数据，适应于本地EXE程序
    /// </summary>
    public class PoolCach
    {
        static List<CacheItem> _poolfactory = new List<CacheItem>();
        /// <summary>
        /// 所有缓存对象
        /// </summary>
        public static List<CacheItem> Poolfactory
        {
            get { return PoolCach._poolfactory; }
            set { PoolCach._poolfactory = value; }
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetObject(string key)
        {
            var obj = Poolfactory.Find(p => p.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));

            return obj != null ? obj.Value : null;
        }

        /// <summary>
        /// 增加缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void SetObject(string key, object obj)
        {
            UpdateObject(key, obj);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void UpdateObject(string key, object obj)
        {
            ClearObject(key);//先清除

            _poolfactory.Add(new CacheItem()
            {
                Key = key,
                Value = obj
            });
        }

        /// <summary>
        /// 更新对象并带过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="ts"></param>
        public static void UpdateObject(string key, object obj, TimeSpan ts)
        {
            ClearObject(key);//先清除

            _poolfactory.Add(new CacheItem()
            {
                Key = key,
                Value = obj,
                Timeout = ts
            });
        }

        /// <summary>
        /// 清除对象
        /// </summary>
        /// <param name="key"></param>
        public static void ClearObject(string key)
        {
            var v = _poolfactory.Find(p => p.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (v != null) _poolfactory.Remove(v);
        }

        /// <summary>
        /// 移除所有以此开头的项
        /// </summary>
        /// <param name="startStr"></param>
        public static void ClearObjectByStartWith(string startStr)
        {
            try
            {
                var removeKeys = from p in _poolfactory
                                 where p.Key.StartsWith(startStr, StringComparison.CurrentCultureIgnoreCase)
                                 select p.Key;

                foreach (string key in removeKeys)
                {
                    ClearObject(key);
                }
            }
            catch { }
        }

        /// <summary>
        /// 移除所有以此开头的项
        /// </summary>
        /// <param name="startStr"></param>
        public static void ClearObjectByEndWith(string endStr)
        {
            try
            {
                var removeKeys = from p in _poolfactory
                                 where p.Key.EndsWith(endStr, StringComparison.CurrentCultureIgnoreCase)
                                 select p.Key;

                foreach (string key in removeKeys)
                {
                    ClearObject(key);
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 缓存项
    /// </summary>
    [Serializable]
    public class CacheItem
    {
        public CacheItem()
        {
            LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 健
        /// </summary>
        public string Key { get; set; }

        object _obj = null;
        /// <summary>
        /// 对应的值
        /// </summary>
        public object Value
        {
            get
            {
                //表示已过期
                if (IsOld)
                {
                    _obj = null;
                }
                return _obj;
            }
            set { _obj = value; }
        }

        /// <summary>
        /// 有效期
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool IsOld
        {
            get
            {
                return Timeout.TotalMilliseconds > 0 &&
                    LastUpdateTime.AddMilliseconds(Timeout.TotalMilliseconds) < DateTime.Now;
            }
        }

        /// <summary>
        /// 最后一次访问时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}
