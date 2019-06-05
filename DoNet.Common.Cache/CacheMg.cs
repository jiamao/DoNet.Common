//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 缓存类
//////////////////////////////////////////////////

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DoNet.Common.Cache
{
    public class CacheMg:IDictionary
    {
        bool _bIsToLowerKey = true;
        string _sSessionID = "";//缓存集合标识
        public CacheMg(string SessionID)
        {
            _sSessionID = SessionID;
            if (!lSessionKeys.Contains(SessionID)) lSessionKeys.Add(SessionID);
            Dictionary<string, object> dic = PoolCach.GetObject(_sSessionID) as Dictionary<string, object>;
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
                PoolCach.UpdateObject(_sSessionID, dic);
            }
        }

        /// <summary>
        /// 此缓存对象
        /// </summary>
        Dictionary<string, object> dic
        {
            get 
            {
                Dictionary<string, object> dic = PoolCach.GetObject(_sSessionID) as Dictionary<string,object>;
                if (dic == null)
                {
                    dic = new Dictionary<string, object>();
                    PoolCach.UpdateObject(_sSessionID, dic);
                }
                return dic;
            }
            set 
            {
                Dictionary<string, object> dic = PoolCach.GetObject(_sSessionID) as Dictionary<string, object>;
                if (dic == null)
                {
                    dic = new Dictionary<string, object>();
                    PoolCach.UpdateObject(_sSessionID, dic);
                }
                dic = value;
            }
        }

        public List<Dictionary<string, object>> AllDic
        {
            get
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                var ide = PoolCach.Poolfactory.GetEnumerator();
                while (ide.MoveNext())
                {
                    Dictionary<string, object> dic = ide.Current.Value as Dictionary<string, object>;
                    if (dic != null)
                    {
                        list.Add(dic);
                    }
                }
                return list;
            }
        }

        static List<string> lSessionKeys = new List<string>();
        public static List<string> SessionKeys
        {
            get
            {
                return lSessionKeys;
            }
        }
        /// <summary>
        /// 移除缓存集合
        /// </summary>
        /// <param name="SessionID"></param>
        public static void RemoveSession(string SessionID)
        {
            lSessionKeys.Remove(SessionID);
            PoolCach.ClearObject(SessionID);
        }
        #region IDictionary 成员
        /// <summary>
        /// 转换健
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string TurnKey(object key)
        {
            if (key == null) return "";
            string skey = key.ToString();
            if (_bIsToLowerKey) skey = skey.ToLower();
            return skey;
        }

        public void Add(object key, object value)
        {
            string skey = TurnKey(key);
            if (dic.ContainsKey(skey))
            {
                dic[skey] = value;
            }
            else
            {
                dic.Add(skey, value);
            }
        }

        public void Clear()
        {
            PoolCach.ClearObject(_sSessionID);
        }

        public bool Contains(object key)
        {
            return dic.ContainsKey(TurnKey(key));
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection Keys
        {
            get { return dic.Keys; }
        }

        public void Remove(object key)
        {
            string skey = TurnKey(key);
            dic.Remove(skey);
        }

        public ICollection Values
        {
            get { return dic.Values; }
        }

        public object this[object key]
        {
            get
            {
                string skey = TurnKey(key);
                if (dic.ContainsKey(skey))
                {
                    return dic[skey];
                }
                else return null;
            }
            set
            {
                Add(TurnKey(key),value);
            }
        }

        public void Set(string key, object value)
        {
            if (Contains(key))
            {
                this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
        public void Remove(List<string> Keys)
        {
            foreach (string key in Keys)
            {
                Remove(key);
            }
        }
        public void ClearStartWith(string StartKey)
        {
            try
            {
                string sStartKey = TurnKey(StartKey);
                List<string> removerkeys = new List<string>();
                foreach (string key in dic.Keys)
                {
                    if (key.StartsWith(sStartKey))
                    {
                        removerkeys.Add(key);
                    }
                }
                Remove(removerkeys);
            }
            catch (Exception ex)
            {
                DoNet.Common.IO.Logger.Write(ex.ToString());
            }
        }

        public void ClearEndWith(string EndKey)
        {
            try
            {
                string sEndKey = TurnKey(EndKey);
                List<string> removerkeys = new List<string>();
                foreach (string key in dic.Keys)
                {
                    if (key.EndsWith(sEndKey))
                    {
                        removerkeys.Add(key);
                    }
                }
                Remove(removerkeys);
            }
            catch (Exception ex)
            {
                DoNet.Common.IO.Logger.Write(ex.ToString());
            }
        }
        #endregion

        #region ICollection 成员

        public void CopyTo(Array array, int index)
        {
            return;
            
        }

        public int Count
        {
            get { return dic.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        #endregion
    }
}
