/////////////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2011/8/1 16:36:18
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DoNet.Common.Cache
{
    /// <summary>
    /// 本地缓存
    /// </summary>
    public class LocalCache : BaseCache
    {
        /// <summary>
        /// 插入本地缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override void Insert(string key, object value)
        {
            if (DoNet.Common.IO.PathMg.IsWeb)
            {
                HttpContext.Current.Cache.Insert(key, value);
            }
            else
            {
                PoolCach.SetObject(key, value);
            }
        }

        /// <summary>
        /// 插入本地缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tspan"></param>
        public override void Insert(string key, object value, TimeSpan tspan)
        {
            if (DoNet.Common.IO.PathMg.IsWeb)
            {
                HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, tspan);
            }
            else
            {
                PoolCach.UpdateObject(key, value, tspan);
            }
        }

        /// <summary>
        /// 获取本地缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object Get(string key)
        {
            if (DoNet.Common.IO.PathMg.IsWeb)
            {
                return HttpContext.Current.Cache[key];
            }
            else
            {
                return PoolCach.GetObject(key);
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            if (DoNet.Common.IO.PathMg.IsWeb)
            {
                HttpContext.Current.Cache.Remove(key);
            }
            else
            {
                PoolCach.ClearObject(key);
            }
        }

        /// <summary>
        /// 获取缓存管理者
        /// </summary>
        /// <returns></returns>
        public override object GetCacheManager()
        {
            if (DoNet.Common.IO.PathMg.IsWeb)
            {
                return HttpContext.Current.Cache;
            }
            else
            {
                return PoolCach.Poolfactory;
            }
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool LockCache(string key, int timeout)
        {
            return true;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool UnLockCache(string key)
        {
            return true;
        }

        /// <summary>
        /// 检查是对象是否已锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool CheckLock(string key)
        {
            return false;//总为未锁
        }
    }
}
