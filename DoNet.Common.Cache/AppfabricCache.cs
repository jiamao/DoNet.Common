/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/1 16:42:03
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace DoNet.Common.Cache
{
    /// <summary>
    /// Appfabric分布式缓存
    /// </summary>
    public class AppfabricCache : BaseCache
    {
        /// <summary>
        /// 缓存池名称
        /// </summary>
        public string CacheName { get; set; }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">关健词</param>
        /// <param name="value">缓存对象</param>
        public override void Insert(string key, object value)
        {
            GetAppfabricCache();
            _memCache.Put(key, value);
        }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">关健词</param>
        /// <param name="value">缓存对象</param>
        /// <param name="tspan">超时值</param>
        public override void Insert(string key, object value, TimeSpan tspan)
        {
            GetAppfabricCache();
            _memCache.Put(key, value, tspan);
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关健词</param>
        /// <returns></returns>
        public override object Get(string key)
        {
            GetAppfabricCache();
            return _memCache.Get(key);
        }

        /// <summary>
        /// 获取缓存管理者
        /// </summary>
        /// <returns></returns>
        public override object GetCacheManager()
        {
            GetAppfabricCache();
            return _memCache;
        }

        //所有锁
        static Dictionary<string, DataCacheLockHandle> _lockHandles = new Dictionary<string, DataCacheLockHandle>();

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool LockCache(string key, int timeout)
        {
            var cachekey = key;
            var lockobject = Get(cachekey);
            if (lockobject == null)
            {
                Insert(cachekey, false);
            }

            DataCacheLockHandle lockHandle = null;

            try
            {
                GetAppfabricCache();
                Insert(cachekey, true);
                lockobject = _memCache.GetAndLock(cachekey, TimeSpan.FromMinutes(timeout), out lockHandle);
            }
            catch//异常为此缓存已被加锁
            {
                return false;
            }

            if (_lockHandles.ContainsKey(key)) _lockHandles.Remove(key);
            _lockHandles.Add(key, lockHandle);
            return true;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool UnLockCache(string key)
        {
            var cachekey = key;

            var handler = _lockHandles.ContainsKey(key) ? _lockHandles[key] : null;//获取锁
            if (handler != null)
            {
                GetAppfabricCache();
                try
                {
                    _memCache.PutAndUnlock(cachekey, false, handler);//解锁
                }
                catch
                {

                }
                _lockHandles.Remove(key);
            }

            return true;
        }

        /// <summary>
        /// 检查锁状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool CheckLock(string key)
        {
            if (LockCache(key, 5))
            {
                UnLockCache(key);
                return false;
            }
            return true;
        }

        private static DataCacheFactory _factory = null;
        private DataCache _memCache = null;

        /// <summary>
        /// 生成缓存访问对象
        /// </summary>
        /// <returns></returns>
        public DataCache GetAppfabricCache()
        {
            if (_memCache != null)
                return _memCache;

            //Create cache configuration
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();
            //Set default properties for local cache (local cache disabled)
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties();

            //Disable tracing to avoid informational/verbose messages on the web page
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Info);

            //Pass configuration settings to cacheFactory constructor
            if (_factory == null)
            {
                _factory = new DataCacheFactory(configuration);
            }

            if (string.IsNullOrWhiteSpace(CacheName))
            {
                CacheName = System.Configuration.ConfigurationManager.AppSettings["CacheName"];
                if (string.IsNullOrWhiteSpace(CacheName)) CacheName = "default";
            }

            //Get reference to named cache called "default"
            _memCache = _factory.GetCache(CacheName);

            return _memCache;
        }

        /// <summary>
        /// /移除缓存
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            GetAppfabricCache();
            _memCache.Remove(key);
        }
    }
}
