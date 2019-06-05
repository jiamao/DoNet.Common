//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 数据库操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
//using System.Data.SqlClient;
//using System.Data.OracleClient;
using System.Configuration;

namespace DoNet.Common.DbUtility
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType
    {
        Unknown = 0,
        MySql,
        Sqlite,
        MSSqlServer,
        Oracle,
        Npgsql
    }

    /// <summary>
    /// 数据库操作类
    /// </summary>
    public abstract class BaseDB:IDisposable
    {
        //数据连接接口
        private IDbConnection connection = null;        

        //数据提供程序
        private DbProviderFactory dbFactory = null;

        public delegate void DelegateExecuteSql(string sql);
        public event DelegateExecuteSql ExecuteSqlCallback;

        Proxy.DBInfo dbinfo = new Proxy.DBInfo();
        /// <summary>
        /// 当前数据库信息
        /// </summary>
        public Proxy.DBInfo DBInfo
        {
            get {
                return dbinfo;
            }
        }

        //锁
        static object synLock = new object();

        //sqlite驱动因为分32位和64位版，首先加载正常dll，，如果不行再尝试加载64位
        static string sqliteDllName = "System.Data.SQLite.dll";

        //数据库类型
        private string strDBType;
        /// <summary>
        /// 数据库类型标识
        /// </summary>
        public string StrDBType
        {
            get { return strDBType; }
            set { strDBType = value; }
        }

        //数据库连接字符串
        private string connectionString;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        /// 所有驱动的路径集合
        /// 不同的数据库驱动不同
        /// </summary>
        static Dictionary<string, string> providerDllPaths = new Dictionary<string, string>();

        /// <summary>
        /// 数据库驱动路径
        /// </summary>
        public string DbProviderDllPath
        {
            get
            {
                //通过数据库类型获取驱动路径
                if (!string.IsNullOrWhiteSpace(StrDBType))
                {
                    if (providerDllPaths.ContainsKey(StrDBType))
                    {
                        return providerDllPaths[StrDBType];
                    }
                }
                return string.Empty;
            }
            set
            {
                //数据库类型
                if (!string.IsNullOrWhiteSpace(StrDBType))
                {
                    if (providerDllPaths.ContainsKey(StrDBType))
                    {
                        providerDllPaths[StrDBType] = value;
                    }
                    else
                    {
                        lock (synLock)
                        {
                            if (!providerDllPaths.ContainsKey(StrDBType))
                            {
                                providerDllPaths.Add(StrDBType, value);
                            }
                            else
                            {
                                providerDllPaths[StrDBType] = value;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("请在设置驱动路径前指定数据库类型");
                }
            }
        }

        /// <summary>
        /// 设置数据库驱动路径
        /// </summary>
        /// <param name="strDbType">驱动类型</param>
        /// <param name="path">驱动路径</param>
        public static void SetDBProviderPath(string strDbType, string path)
        {
            lock (synLock)
            {
                if (!providerDllPaths.ContainsKey(strDbType))
                {
                    providerDllPaths.Add(strDbType, path);
                }
                else
                {
                    providerDllPaths[strDbType] = path;
                }
            }
        }

        /// <summary>
        /// 参数标识字符
        /// </summary>
        static Dictionary<DbType, char> paramMarkChar = new Dictionary<DbType, char>();

        /// <summary>
        /// 参数标识字符
        /// </summary>
        public static Dictionary<DbType, char> ParamMarkChar
        {
            get
            {
                if (paramMarkChar == null || paramMarkChar.Count == 0)
                {
                    lock (synLock)
                    {
                        if (paramMarkChar == null || paramMarkChar.Count == 0)
                        {
                            paramMarkChar.Add(DbType.MSSqlServer, '@');
                            paramMarkChar.Add(DbType.Sqlite, '@');
                            paramMarkChar.Add(DbType.Oracle, ':');
                            paramMarkChar.Add(DbType.Npgsql, ':');
                            paramMarkChar.Add(DbType.MySql, '?');
                        }
                    }
                }
                return paramMarkChar;
            }
            set { BaseDB.paramMarkChar = value; }
        }

        /// <summary>
        /// 当前DB关健字区分标识开始字符
        /// </summary>
        public char DBSPStartChar
        {
            get 
            {
                switch (this.GetDbType())
                {
                    case DbType.MSSqlServer: return '[';
                    case DbType.MySql:
                    case DbType.Sqlite:
                        return '`';
                    //case DbType.Oracle:
                     //   return '"';
                    default: return ' ';
                }
            }
        }

        /// <summary>
        /// 当前DB关健字区分标识结束字符
        /// </summary>
        public char DBSPEndChar
        {
            get
            {
                switch (this.GetDbType())
                {
                    case DbType.MSSqlServer: return ']';
                    case DbType.MySql:
                    case DbType.Sqlite:
                        return '`';
                    //case DbType.Oracle:
                    //    return '"';
                    default: return ' ';
                }
            }
        }

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// 从配置文件中读取数据库类型、连接字符串
        /// </summary>
        public BaseDB()
        {           
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDBType">数据库类型:"System.Data.SqlClient" or "System.Data.OracleClient" or "System.Data.Odbc" or "System.Data.OleDb"</param>
        /// <param name="strConn">连接字符串</param>
        public BaseDB(string strDBType_)
        {
            strDBType = strDBType_;            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDBType">数据库类型:"System.Data.SqlClient" or "System.Data.OracleClient" or "System.Data.Odbc" or "System.Data.OleDb"</param>
        /// <param name="strConn">连接字符串</param>
        public BaseDB(string strDBType,string strConn)
        {
            //获取连接
            SetConnection(strDBType, strConn);
        }

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="strDBType">数据库类型:"System.Data.SqlClient" or "System.Data.OracleClient" or "System.Data.Odbc" or "System.Data.OleDb,MySql.Data.MySqlClient"</param>
        /// <param name="strConn">连接字符串</param>
        public void SetConnection(string strdbtype = null, string strConn = null)
        {
            if(!string.IsNullOrWhiteSpace(strdbtype))StrDBType = strdbtype.ToUpper();
            if(!string.IsNullOrWhiteSpace(strConn))ConnectionString = strConn;

            dbinfo.DBType = GetDbType();//数据库类别
            dbinfo.ConnectionString = ConnectionString;
            if (connection == null)
            {
                if (dbinfo.DBType == DbType.MySql)
                {
                    LoadProviderFactoery("MySql.Data.dll", "MySql.Data", "MySql.Data.MySqlClient.MySqlClientFactory");   
                    //dbFactory = MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                }
                else if (dbinfo.DBType == DbType.Sqlite)
                {
                     LoadProviderFactoery(sqliteDllName, "System.Data.SQLite", "System.Data.SQLite.SQLiteFactory");                    
                    
                    //dbFactory = DbProviderFactories.GetFactory("System.Data.SQLite.SQLiteFactory");
                    //dbFactory = System.Data.SQLite.SQLiteFactory.Instance;
                }
                else if (dbinfo.DBType == DbType.Oracle)
                {                    
                    //dbFactory = System.Data.OracleClient.OracleClientFactory.Instance;
                    //LoadProviderFactoery("System.Data.OracleClient.dll", "System.Data.OracleClient", "System.Data.OracleClient.OracleClientFactory");  
                    var oracle = Reflection.ClassHelper.GetClassType("System.Data.OracleClient.dll", "System.Data.OracleClient.OracleClientFactory", "System.Data.OracleClient");
                    dbFactory = (DbProviderFactory)Reflection.ClassHelper.GetStaticFieldValue(oracle, "Instance");
                }
                else if (dbinfo.DBType == DbType.Npgsql)
                {
                    //dbFactory = Npgsql.NpgsqlFactory.Instance;
                    var npgsql = Reflection.ClassHelper.GetClassType("Npgsql.dll", "Npgsql.NpgsqlFactory", "Npgsql");
                    dbFactory = (DbProviderFactory)Reflection.ClassHelper.GetStaticFieldValue(npgsql, "Instance");
                }
                else if (dbinfo.DBType == DbType.MSSqlServer)
                {
                    dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                }
                else if (!string.IsNullOrEmpty(strdbtype))
                {
                    //获取指定提供程序名称的 DbProviderFactory 的一个实例
                    dbFactory = DbProviderFactories.GetFactory(StrDBType);
                }
                
            }
        }

        /// <summary>
        /// 生成数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            if (!string.IsNullOrEmpty(StrDBType) && dbFactory != null)
            {
                //创建连接
                connection = dbFactory.CreateConnection();

                //连接字符串
                if (!string.IsNullOrEmpty(ConnectionString)) connection.ConnectionString = ConnectionString;
            }
            return connection;
        }

        /// <summary>
        /// 设置数据库连接信息
        /// </summary>
        /// <param name="strdbtype"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="dbname"></param>
        /// <param name="charset"></param>
        public void SetConnection(string strdbtype, string server, int port, string username, string pwd, string dbname,string otherAttr="")
        {
            var connstring = "";
            strDBType = strdbtype.ToUpper();
            var dbType = GetDbType();//数据库类别
            switch (dbType)
            {
                case DbType.MSSqlServer:
                    {
                        connstring = string.Format("Data Source={0};Database={1};user id={2};password={3};" + otherAttr, server + (port > 0 ? "," + port : ""),
                            dbname,username,pwd);
                        break;
                    }
                case DbType.Sqlite:
                    {
                        connstring = string.Format("Data Source={0};" + otherAttr, dbname);
                        break;
                    }
                case DbType.Npgsql:
                    {
                        connstring = string.Format("server={0};database={1};user id={2};password={3};", server,
                            dbname, username, pwd) + (port > 0 ? "port=" + port + ";" : "") + otherAttr;
                        break;
                    }
                case DbType.MySql:
                    {
                        connstring = string.Format("server={0};database={1};user id={2};password={3};", server,
                            dbname, username, pwd) + (port>0?"port=" + port + ";":"") + otherAttr;
                        break;
                    }
                
                case DbType.Oracle:
                    {
                        connstring = string.Format("Data Source={0};user id={1};password={2};" + otherAttr, server,
                            username, pwd);
                        break;
                    }               
                default: {
                    throw new Exception("不支持的数据库类型!");
                }
            }
            SetConnection(strdbtype, connstring);
        }

        /// <summary>
        /// 加载数据库连接驱动
        /// </summary>
        /// <param name="dll">DLL文件名</param>
        /// <param name="nameSpace">驱动的命名空间</param>
        /// <param name="className">驱动类全名</param>
        void LoadProviderFactoery(string dll,string nameSpace, string className)
        {
            //web的放在bin目录下
            if (IO.PathMg.IsWeb)
            {
                dll = System.IO.Path.Combine("bin", dll);
            }
            DbProviderDllPath = string.IsNullOrEmpty(DbProviderDllPath) ? IO.PathMg.CheckPath(dll) : DbProviderDllPath;
            var t = Reflection.ClassHelper.GetClassType(DbProviderDllPath,className,nameSpace);
            if (t == null)
            {
                throw new Exception("无法加载类型：" + className);
            }
            dbFactory = (DbProviderFactory)Activator.CreateInstance(t);           
        }
        #endregion

        #region 获取数据库连接接口
        /// <summary>
        /// 获取数据库连接接口
        /// </summary>
        public IDbConnection IConn
        {
            get
            {
                return connection;
            }
        }
        #endregion

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        public DbType GetDbType()
        {
            switch (strDBType)
            {

                case "MYSQL.DATA.MYSQLCLIENT":
                    return DbType.MySql ;
                case "SYSTEM.DATA.SQLITE":
                    return DbType.Sqlite;
                case "SYSTEM.DATA.SQLCLIENT":
                    return DbType.MSSqlServer ;
                case "SYSTEM.DATA.ORACLECLIENT":
                    return DbType.Oracle;
                case "NPGSQL.DATA.NPGSQLCLIENT":
                    return DbType.Npgsql;         
                    }
            if (strDBType.Contains("MYSQL"))
            {
                return DbType.MySql;
            }
            else if (strDBType.Contains("SQLITE"))
            {
                return DbType.Sqlite;
            }
            else if (strDBType.Contains("SQLSERVER"))
            {
                return DbType.MSSqlServer;
            }
            else if (strDBType.Contains("ORACLE"))
            {
                return DbType.Oracle;
            }
            else if (strDBType.Contains("NPGSQL"))
            {
                return DbType.Npgsql;
            }
            return DbType.Unknown;
        }

        #region 获取事务
        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction GetTransaction()
        {
            return connection.BeginTransaction();
        }
        #endregion

        #region 打开连接

        /// <summary>
        /// 打开连接
        /// </summary>
        public IDbConnection Open()
        {
            var con = CreateConnection();
            if (con.State == ConnectionState.Closed)
            {
                //关闭状态，打开连接
                con.Open();
            }
            else if (con.State == ConnectionState.Broken)
            {
                //中断状态，先关闭后打开连接
                con.Close();
                con.Open();
            }
            return con;
        }

        #endregion

        #region 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (connection == null)
                {
                    return;
                }
                else if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 获取数据库命令接口
        /// <summary>
        /// 获取数据库命令接口
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="pars"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        protected IDbCommand GetCommand(string cmdText, IDbDataParameter[] pars, CommandType cmdType = CommandType.Text)
        {
            //创建数据库命令
            IDbCommand cmd = dbFactory.CreateCommand();
            cmd.CommandType = cmdType;
            //设置数据源运行的文本命令
            cmd.CommandText = cmdText;
            //设置连接
            cmd.Connection = connection;
           
            if (pars != null)
            {
                cmd.Parameters.Clear();
                foreach (var p in pars)
                {
                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        /// <summary>
        /// 获取数据库命令接口
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据库命令接口</returns>
        protected IDbCommand GetCommand(string cmdText,IDictionary<string,object> pars, CommandType cmdType= CommandType.Text)
        {
            //创建数据库命令
            IDbCommand cmd = dbFactory.CreateCommand();
            cmd.CommandType = cmdType;
            //设置数据源运行的文本命令
            cmd.CommandText = cmdText;            
            //设置连接
            cmd.Connection = connection;
          
            if (pars != null)
            {
                cmd.Parameters.Clear();
                foreach (var p in pars)
                {
                    var v = p.Value;
                    if (v == null) v = DBNull.Value;
                    cmd.Parameters.Add(CreateParam(cmd,p.Key,v));
                }
            }

            if (ExecuteSqlCallback != null) ExecuteSqlCallback.BeginInvoke(cmdText, null, pars);

            return cmd;
        }

        /// <summary>
        /// 获取数据库命令接口
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        protected IDbCommand GetCommand(string cmdText, string[] paramNames,object[] paramValues, CommandType cmdType = CommandType.Text)
        {
            var pars = new Dictionary<string, object>();
            if (paramNames != null && paramValues != null)
            {
                for (var i = 0; i < paramNames.Length; i++)
                {
                    pars.Add(paramNames[i], paramValues[i]);
                }
            }
            return GetCommand(cmdText, pars, cmdType);
        }
        /// <summary>
        /// 获取数据库命令接口
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        protected IDbCommand GetCommand(string cmdText, CommandType cmdType = CommandType.Text)
        {
            //创建数据库命令
            IDbCommand cmd = dbFactory.CreateCommand();
           
            cmd.CommandType = cmdType;
            //设置数据源运行的文本命令
            cmd.CommandText = cmdText;
            //设置连接
            cmd.Connection = connection;

            if (ExecuteSqlCallback != null) ExecuteSqlCallback.BeginInvoke(cmdText, null, cmdType);

            return cmd;
        }

        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTableName()
        {
            var TableNames = new List<string>();

            OleDbConnection oledb = IConn as OleDbConnection;
            oledb.Open();
            DataTable dt = oledb.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            foreach (DataRow dr in dt.Rows)
            {
                TableNames.Add(dr[2].ToString());
            }

            return TableNames;
        }  
    
        #endregion

        #region 获取数据读取器

        /// <summary>
        /// 获取数据读取器
        /// </summary>
        /// <param name="IDbCommand">dbcommand</param>
        /// <returns>返回数据读取器接口</returns>
        protected IDataReader GetDataReader(IDbCommand cmd)
        {
            cmd.Connection = Open();
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// 获取数据读取器
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public IDataReader GetDataReader(string sql)
        {
            var cmd = GetCommand(sql);
            return GetDataReader(cmd);
        }

        /// <summary>
        /// 获取当前语句查得的数据条数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetDataCount(string sql)
        {
            int count = 0;
            using (var reader = this.GetDataReader(sql))
            {                
                while (reader.Read())
                {
                    count++;
                }
            }
            return count;
        }

        #endregion

        #region 执行SQL语句返回受影响的行数

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        protected int ExecuteNonQuery(IDbCommand cmd)
        {
            int iRows = 0;
            //打开并执行命令
            using (var con = Open())
            {
                cmd.Connection = con;
                iRows = cmd.ExecuteNonQuery();
            }
            return iRows;
        }

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        public int ExecuteNonQuery(string cmdText)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteNonQuery(DBInfo, cmdText);
            }
            else
            {
                var cmd = GetCommand(cmdText);
                return ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        public int ExecuteNonQuery(string cmdText,params IDbDataParameter[] ps)
        {
             var cmd = GetCommand(cmdText,ps);
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回受影响的行数(-1:执行失败)</returns>
        public int ExecuteNonQuery(string cmdText, string[] paraNames, object[] paraValues)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteNonQueryWithParam(DBInfo, cmdText,paraNames,paraValues);
            }
            else
            {
                var cmd = GetCommand(cmdText, paraNames, paraValues);

                return ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="dbparams"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, IDictionary<string,object> dbparams)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteNonQueryWithParams(DBInfo, cmdText,dbparams);
            }
            else
            {
                var cmd = GetCommand(cmdText, dbparams);

                return ExecuteNonQuery(cmd);
            }
        }  
        #endregion

        #region 执行SQL语句返回数据集

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="pars">参数</param>       
        /// <returns>返回数据集</returns>
        protected DataSet GetDataSet(IDbCommand cmd)
        {
            DataSet ds = new DataSet();

            //DoNet.Common.IO.Logger.Write("DBLog", "Start-Query:" + cmd.CommandText);
            using (var con = Open())
            {
                IDbDataAdapter da = dbFactory.CreateDataAdapter();
                cmd.Connection = con;
                da.SelectCommand = cmd;               
                da.Fill(ds);
            }
            //DoNet.Common.IO.Logger.Write("DBLog", "End-Query:" + cmd.CommandText);
            return ds;
        }

        /// <summary>
        /// 执行SQL语句返回数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSet(string cmdText)
        {
            if (DBInfo.IsProxy)
            {
                var dsstring = Proxy.Access.CreateProxy().GetDataSet(DBInfo, cmdText);
                var ds = Data.DataSet.FromString(dsstring);
                var nds = ds.ToDataSet();
                return nds;
            }
            else
            {
                var cmd = GetCommand(cmdText);
                return GetDataSet(cmd);
            }
        }

        /// <summary>
        /// 执行SQL语句返回数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSet(string cmdText, string[] paraNames, object[] paraValues)
        {
            if (DBInfo.IsProxy)
            {
                var dsstring = Proxy.Access.CreateProxy().GetDataSetWithParam(DBInfo, cmdText, paraNames, paraValues);
                var ds = Data.DataSet.FromString(dsstring);
                var nds = ds.ToDataSet();
                return nds;
            }
            else
            {
                var cmd = GetCommand(cmdText,paraNames,paraValues);
                return GetDataSet(cmd);
            }
        }

        /// <summary>
        /// 执行SQL语句返回数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSet(string cmdText, IDictionary<string,object> pars)
        {
            if (DBInfo.IsProxy)
            {
                var keys = new string[pars.Count];
                var values = new object[pars.Count];
                pars.Keys.CopyTo(keys, 0);
                pars.Values.CopyTo(values, 0);
                var dsstring = Proxy.Access.CreateProxy().GetDataSetWithParam(DBInfo, cmdText, keys, values);
                var ds = Data.DataSet.FromString(dsstring);
                var nds = ds.ToDataSet();
                return nds;
            }
            else
            {
                var cmd = GetCommand(cmdText, pars);
                return GetDataSet(cmd);
            }
        }

        /// <summary>
        /// 执行带参数SQL语句返回数据集
        /// </summary>
        /// <param name="strProcudureName">查询语句</param>
        /// <param name="param">SQL语句参数</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSet(string strSql, params IDbDataParameter[] pars)
        {
            var cmd = GetCommand(strSql,pars);
            return GetDataSet(cmd);
        }

        /// <summary>
        /// 执行存储过程返回数据集
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="strProcudureName">存储过程名称</param>
        /// <param name="param">存储过程参数</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSetByStore(string strProcudureName, params IDbDataParameter[] pars)
        {
            var cmd = GetCommand(strProcudureName,pars, CommandType.StoredProcedure);            
            return GetDataSet(cmd);
        }

        #endregion

        #region 执行SQL语句返回单值对象

        /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        protected object ExecuteScalar(IDbCommand cmd)
        {
            using (var con = Open())
            {
                cmd.Connection = con;
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        public object ExecuteScalar(string cmdText)
        {
            if (DBInfo.IsProxy)
            {
               return Proxy.Access.CreateProxy().ExecuteScalar(DBInfo, cmdText);
            }
            else
            {
                var cmd = GetCommand(cmdText);
                return ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        public object ExecuteScalar(string cmdText, string[] paraNames, object[] paraValues)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteScalarWithParam(DBInfo, cmdText, paraNames, paraValues);
            }
            else
            {
                var cmd = GetCommand(cmdText, paraNames, paraValues);

                return ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// 执行SQL语句返回单值对象
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <returns>返回结果集中第一行的第一列数据</returns>
        public object ExecuteScalar(string cmdText, IDictionary<string,object> pars)
        {
            if (DBInfo.IsProxy)
            {
                var keys=new string[pars.Count];
                var values=new object[pars.Count];
                pars.Keys.CopyTo(keys,0);
                pars.Values.CopyTo(values,0);

                return Proxy.Access.CreateProxy().ExecuteScalarWithParam(DBInfo, cmdText, keys, values);
            }
            else
            {
                var cmd = GetCommand(cmdText, pars);

                return ExecuteScalar(cmd);
            }
        }

        #endregion

        #region 执行多条SQL语句 
        /// <summary>
        /// 在同一事务中
        /// 执行多个命令
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        protected int ExecuteCommands(ICollection<IDbCommand> cmds)
        {
            var count = 0;

            using (var con = Open())
            {
                using(var tran = GetTransaction())
                {
                    foreach (var cmd in cmds)
                    {
                        cmd.Connection = con;
                        cmd.Transaction = tran;
                        count += cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
            return count;
        }

        /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteSql(ICollection<string> sqls)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteSql(DBInfo, sqls);
            }
            else
            {
                var cmds = new List<IDbCommand>();
                foreach (var sql in sqls)
                {
                    var cmd = GetCommand(sql);
                    cmds.Add(cmd);
                }
                return ExecuteCommands(cmds);
            }
        }

        /// <summary>
        /// 在同一事务中
        /// 执行多条带参数的SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteSql(List<string> sqls, List<string[]> paraNames, List<object[]> paraValues)
        {
            if (DBInfo.IsProxy)
            {
                return Proxy.Access.CreateProxy().ExecuteSqlWithParam(DBInfo, sqls, paraNames, paraValues);
            }
            else
            {
                var cmds = new List<IDbCommand>();
                for (int i = 0; i < sqls.Count; i++)
                {
                    var cmd = GetCommand(sqls[i], paraNames[i], paraValues[i]);
                    cmds.Add(cmd);
                }
                return ExecuteCommands(cmds);
            }
        }

        /// <summary>
        /// 在同一事务中
        /// 执行多条带参数的SQL语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int ExecuteSql(List<string> sqls, List<Dictionary<string,object>> pars)
        {
            if (DBInfo.IsProxy)
            {
                var parnames=new List<string[]>();
                var parvalues=new List<object[]>();
                if (pars != null)
                {
                    foreach (var p in pars)
                    {
                        var ns = new string[p.Keys.Count];
                        p.Keys.CopyTo(ns, 0);
                        parnames.Add(ns);
                        var vs = new object[p.Values.Count];
                        p.Values.CopyTo(vs, 0);
                        parvalues.Add(vs);
                    }
                }
                return Proxy.Access.CreateProxy().ExecuteSqlWithParam(DBInfo, sqls, parnames, parvalues);
            }
            else
            {
                var cmds = new List<IDbCommand>();
                for (var i = 0; i < sqls.Count; i++)
                {
                    var cmd = GetCommand(sqls[i], pars[i]);
                    cmds.Add(cmd);
                }
                return ExecuteCommands(cmds);
            }
        }

        #endregion

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paraName"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        IDbDataParameter CreateParam(IDbCommand cmd, string paraName, object paraValue)
        {
            var p = cmd.CreateParameter();
            var paramchar = GetParamChar();//参数标识字符
            paraName = paraName.Trim();

            if (paraName[0] != paramchar) paraName = paramchar + paraName;//组合参数

            p.ParameterName = paraName;
            p.Value = paraValue;
            return p;
        }

        /// <summary>
        /// 通过数据库类型获取参数标识
        /// </summary>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        public static char GetParamChar(DbType dbtype)
        {
            if (ParamMarkChar.ContainsKey(dbtype))
            {
                return ParamMarkChar[dbtype];
            }
            return '@';
        }

        /// <summary>
        /// 获取参数字符
        /// </summary>
        /// <returns></returns>
        public char GetParamChar()
        {
            var dbtype = GetDbType();
            return GetParamChar(dbtype);
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        /// <returns></returns>
        public string GetParamChar(string parName)
        {
            var dbtype = GetDbType();
            return GetParamChar(dbtype) + parName;
        }

        /// <summary>
        /// 去除关健的字符，以免被注入
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SplitSqlChar(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            return source.Replace("'", "").Replace("=", "").Replace("delete from", "").Replace(" or ", "").Replace("%", "");
        }

        /// <summary>
        /// 生成分页查询SQL语句
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="tableName"></param>
        /// <param name="strFields"></param>
        /// <param name="strWhere"></param>
        /// <param name="order"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string CreatePagerSql(DbType dbType, string tableName,
            string strFields,
            string strWhere,
            string order, int index, int count)
        {
            var sql = "";
            //组合查询语句
            switch (dbType)
            {
                case DbType.Sqlite:
                case DbType.MySql:
                    {
                        sql = string.Format("select {0} from {1} {2} {3} limit {4},{5}",
                                                     strFields, tableName,
                                                     string.IsNullOrWhiteSpace(strWhere) ? "" : "where " + strWhere,
                                                    string.IsNullOrWhiteSpace(order) ? "" : "order by " + order
                                                    , index, count);
                        break;
                    }
                case DbType.MSSqlServer:
                    {
                        sql =
                            string.Format(
                                "select t__1.* from (select {0}, ROW_NUMBER() OVER(ORDER BY (SELECT 0)) AS _RowNum from {2} {5} {1}) t__1 where t__1._RowNum between {3} and {4}",
                                string.IsNullOrWhiteSpace(order) ? strFields : (" top(100000000) " + strFields),//如果有orderby 则子查询必须带top关健词
                                string.IsNullOrWhiteSpace(order) ? "" : order,
                                tableName, index + 1, index + count,
                                string.IsNullOrWhiteSpace(strWhere) ? "" : "where " + strWhere);//因为sqlserver的行号是从一开始的
                        break;
                    }
                case DbType.Npgsql:
                    {
                        sql = string.Format("select {0} from {1} {2} {3} limit {4} offset {5}",
                                                     strFields, tableName,
                                                     string.IsNullOrWhiteSpace(strWhere) ? "" : "where " + strWhere,
                                                    string.IsNullOrWhiteSpace(order) ? "" : "order by " + order
                                                    , count, index);
                        break;
                    }
                case DbType.Oracle:
                    {
                        sql =
                            string.Format(
                                "select t__1.* from (select {0}, ROWNUM row___number from {1} {4} {5}) t__1 where t__1.row___number between {2} and {3}",
                                strFields, tableName, index + 1, index + count,
                                string.IsNullOrWhiteSpace(strWhere) ? "" : "where " + strWhere,
                                string.IsNullOrWhiteSpace(order) ? "" : "order by " + order);//因为行号是从一开始的
                        break;
                    }
            }
            return sql;
        }

        /// <summary>
        /// 处理sqlserver子查询中有orderby的情况，如果有必须加上top关分健词才能正常执行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string DerSubOrderSql(string sql)
        {
            //只有sqlserver才如此处理
            if (this.DBInfo.DBType == DbType.MSSqlServer)
            {
                //只有在存在order by的情况下才处理这个逻辑
                var orderreg = new System.Text.RegularExpressions.Regex("\\W*order\\s+by\\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (orderreg.IsMatch(sql))
                {
                    var selectreg = new System.Text.RegularExpressions.Regex("(?<sel>(^|[\\W]?)select\\s+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var seltopreg = new System.Text.RegularExpressions.Regex("^[\\s\\(]+top[\\s\\(]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var ms = selectreg.Matches(sql);

                    for (var i = ms.Count - 1; i >= 0; i--)
                    {
                        var m = ms[i];
                        var sel = m.Groups["sel"].ToString();
                        var selend = m.Index + sel.Length;
                        //如果后面跟随了top则直接略过
                        var tmp = sql.Substring(selend - 1);
                        if (seltopreg.IsMatch(tmp)) continue;
                        sql = sql.Insert(selend, " top(10000000) ");
                    }
                }
            }
            return sql;
        }
        #region IDisposable 成员

        public void Dispose()
        {
            dispose(true);
        }

        private void dispose(bool disposing)
        {
            if (disposing)
            {
                Close();

                GC.SuppressFinalize(true);
            }
            
        }
        #endregion
    }

}
