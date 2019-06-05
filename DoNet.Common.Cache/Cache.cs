using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace DoNet.Common.Cache
{
    /// <summary>
    /// 缓存处理
    /// </summary>
    public class Cache
    {
        static BaseCache _cache = null;

        /// <summary>
        /// 处理缓存对象
        /// </summary> 
        public static void GetCache()
        {
            if (_cache == null)
            {
                var cachetype = ConfigurationManager.AppSettings["CacheType"];

                if (string.IsNullOrWhiteSpace(cachetype))
                {
                    cachetype = "LocalCache";
                }
                var ct = Assembly.GetExecutingAssembly().GetType("DoNet.Common.Cache." + cachetype, false, true);
                _cache = Activator.CreateInstance(ct) as BaseCache;
            }
        }

        /// <summary>
        ///  插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            GetCache();
            _cache.Insert(key, value);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tspan"></param>
        public static void Insert(string key, object value, TimeSpan tspan)
        {
            GetCache();
            _cache.Insert(key, value, tspan);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            GetCache();
            return _cache.Get(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            GetCache();
            _cache.Remove(key);
        }

        public static bool LockCache(string key, int timeout = 5)
        {
            GetCache();
            return _cache.LockCache(key,timeout);
        }

        public static bool UnLockCache(string key)
        {
            GetCache();
            return _cache.UnLockCache(key);
        }

        /// <summary>
        /// 检查锁状态:true=已锁,false=未锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckLock(string key)
        {
            GetCache();
            return _cache.CheckLock(key);
        }

        public static object GetCacheManager()
        {
            GetCache();
            return _cache.GetCacheManager();
        }
    }

    /// <summary>
    /// 缓存抽象
    /// </summary>
    public abstract class BaseCache
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">值</param>
        /// <param name="tspan">过期时间</param>
        /// <returns></returns>
        public abstract void Insert(string key, object value);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">值</param>
        /// <param name="tspan">过期时间</param>
        /// <returns></returns>
        public abstract void Insert(string key, object value, TimeSpan tspan);

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关健词</param>
        /// <returns></returns>
        public abstract object Get(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public abstract void Remove(string key);

        /// <summary>
        /// 锁缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool LockCache(string key,int timeout);

        /// <summary>
        /// 解锁缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool UnLockCache(string key);

        /// <summary>
        /// 检查锁状态:true=已锁,false=未锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool CheckLock(string key);

        /// <summary>
        /// 获取缓存管理者
        /// </summary>
        /// <returns></returns>
        public abstract object GetCacheManager();
    }

    
    
}
