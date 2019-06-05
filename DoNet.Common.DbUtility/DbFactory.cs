using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DoNet.Common.DbUtility
{
    /// <summary>
    /// DB工厂
    /// </summary>
    public abstract class DbFactory
    {
        /// <summary>
        /// 通过配置创建
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static DbORM CreateDbORM()
        {
            return new DbORM();
        }
 
        /// <summary>
        /// 通过配置名创建数据库操作类
        /// </summary>
        /// <param name="settingName">连接名称</param>
        /// <returns></returns>
        public static DbORM CreateDbORM(string settingName)
        {
            return CreateDbORM(ConfigurationManager.ConnectionStrings[settingName]);
        }

        /// <summary>
        /// 创建数据库操作类
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbORM CreateDbORM(string providerName, string connectionString)
        {
            return new DbORM(providerName, connectionString);
        }

        /// <summary>
        /// 通过配置创建
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static DbORM CreateDbORM(ConnectionStringSettings setting)
        {
            return new DbORM(setting.ProviderName, setting.ConnectionString);
        }
/*
        /// <summary>
        /// 生成带缓存的DB基类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static JmDB<T> CreateJMDB<T>()
        {
            return new JmDB<T>();
        }

        /// <summary>
        ///通过数据库连接配置名实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static JmDB<T> CreateJMDB<T>(string connConfigName)
        {
            return new JmDB<T>(connConfigName);
        }

        /// <summary>
        /// 通过配置路径和数据库连接配置初始化类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static JmDB<T> CreateJMDB<T>(string configPath, string connConfigName)
        {
            return new JmDB<T>(configPath, connConfigName);
        }*/
    }
}
