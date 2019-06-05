using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace DoNet.Common.Web
{
    /// <summary>
    /// 缓存处理
    /// </summary>
    public class Cache
    {
        /// <summary>
        ///  插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            Cache.Insert(key, value);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tspan"></param>
        public static void Insert(string key, object value, TimeSpan tspan)
        {
            Cache.Insert(key, value, tspan);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return Cache.Get(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        public static bool LockCache(string key, int timeout = 5)
        {
            return Cache.LockCache(key, timeout);
        }

        public static bool UnLockCache(string key)
        {
            return Cache.UnLockCache(key);
        }

        /// <summary>
        /// 检查锁状态:true=已锁,false=未锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckLock(string key)
        {
            return Cache.CheckLock(key);
        }

        public static object GetCacheManager()
        {
            return Cache.GetCacheManager();
        }
    }
}
