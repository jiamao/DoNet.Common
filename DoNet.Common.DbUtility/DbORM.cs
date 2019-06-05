using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNet.Common.DbUtility
{
    /// <summary>
    /// 数据库对象转换类
    /// </summary>
    public class DbORM:BaseDB
    {        
        /// <summary>
        /// 字段映射的分隔符
        /// </summary>
        public const char MappingFieldSplit = ',';

        /// <summary>
        /// 属性与字段的分隔符
        /// </summary>
        public const char MappingSplitChar = '|';

        /// <summary>
        /// 数据库操作类
        /// </summary>
        public BaseDB DbManager
        {
            get {
                return this;
            }
        }

        /// <summary>
        /// 用基础类实例化
        /// </summary>
        /// <param name="db"></param>
        public DbORM(BaseDB db)
        {
            SetConnection(db.StrDBType, db.ConnectionString);
        }

        /// <summary>
        /// 构造函数
        /// 从配置文件中读取数据库类型、连接字符串
        /// </summary>
        public DbORM()
        {           
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDBType">数据库类型:"System.Data.SqlClient" or "System.Data.OracleClient" or "System.Data.Odbc" or "System.Data.OleDb"</param>
        /// <param name="strConn">连接字符串</param>
        public DbORM(string strDBType_)
            :base(strDBType_)
        {            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDBType">数据库类型:"System.Data.SqlClient" or "System.Data.OracleClient" or "System.Data.Odbc" or "System.Data.OleDb"</param>
        /// <param name="strConn">连接字符串</param>
        public DbORM(string strDBType, string strConn)
            :base(strDBType,strConn)
        {
        }       

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="strFields">表字段，用逗号分隔</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pageIndex">查询第几页，页码从1开始</param>
        /// <param name="countInPage">每页显示多少页记录,当此参数为0时，将会查出所有符合条件的记录</param>
        /// <param name="order">排序字段,比如:ondate desc</param>
        /// <param name="fieldMapping">字段映射</param>
        /// <returns>当前页数据对象</returns>
        public DoNet.Common.Data.PageData<T> SearchDataPage<T>(string tableName,
            string strFields,
            string strWhere,            
            int pageIndex,
            int countInPage,
            string order,
            string fieldMapping = "", IDictionary<string, object> pars = null) where T : class
        {
            var mapping = DoNet.Common.Data.DataConvert.CreateMapping(fieldMapping,MappingFieldSplit,MappingSplitChar);            
            var data = SearchDataPage<T>(tableName, strFields, strWhere, pageIndex,countInPage, order, mapping, pars);
            return data;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="strFields"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="countInPage"></param>
        /// <param name="order"></param>
        /// <param name="fieldMapping">字段映射 key=model属性，value=数据库字段名</param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public DoNet.Common.Data.PageData<T> SearchDataPage<T>(string tableName,
            string strFields,
            string strWhere,
            int pageIndex,
            int countInPage,
            string order,
            IDictionary<string, string> fieldMapping = null, IDictionary<string, object> pars = null) where T : class
        {
            //总页数
            var pagecount = 1;
            //符合条件的数据条数
            var datacount = 0;
            var data = new DoNet.Common.Data.PageData<T>();
            data.Data = SearchDataPage<T>(tableName, strFields, strWhere, order,
                ref pageIndex, ref pagecount, ref datacount, countInPage,
                fieldMapping, pars);

            //获取结果值
            data.Index = pageIndex;
            data.PageCount = pagecount;
            data.DataCount = datacount;

            return data;
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="strFields">字段列表，用逗号分隔</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pageIndex">第几页,从1开始</param>
        /// <param name="pageCount">返回的页数</param>
        /// <param name="dataCount">返回符合条件的总条数</param>
        /// <param name="countInPage">每页显示多少条记录,当此参数为0时，将会查出所有符合条件的记录</param>
        /// <param name="order">排序字段,比如:ondate desc</param>
        /// <param name="fieldMapping">字段映射，可选</param>
        /// <returns></returns>
        public List<T> SearchDataPage<T>(string tableName, 
            string strFields, 
            string strWhere,                             
            ref int pageIndex, 
            ref int pageCount, 
            ref int dataCount, 
            int countInPage,
            string order,                                           
            string fieldMapping = "",
            IDictionary<string, object> pars = null) where T : class
        {
            return SearchDataPage<T>(tableName, strFields, strWhere, order,
                    ref pageIndex, ref pageCount, ref dataCount, countInPage, fieldMapping,pars);
        }

        /// <summary>
        /// 按页查询数据
        /// </summary>
        /// <typeparam name="T">生成的对象</typeparam>
        /// <param name="orderType">排序方式：比如asc,desc</param>
        /// <param name="pageIndex">当前第几页,起始页为1</param>
        /// <param name="pageCount">总查得页数</param>
        /// <param name="countInPage">每页显示多少条,当此参数为0时，将会查出所有符合条件的记录</param>
        /// <param name="tableName">表名</param>
        /// <param name="strFields">需要查询的字段</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="order">排序字段,比如:ondate desc</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public List<T> SearchDataPage<T>(string tableName, 
            string strFields, 
            string strWhere,                                                               
            string order,                                                               
            ref int pageIndex, 
            ref int pageCount, 
            ref int dataCount, 
            int countInPage,
            string fieldMapping = "", IDictionary<string, object> pars = null) where T : class
        {
            return SearchDataPage<T>(tableName, strFields, strWhere, order,
                ref pageIndex, ref pageCount, ref dataCount, countInPage,
                DoNet.Common.Data.DataConvert.CreateMapping(fieldMapping,MappingFieldSplit,MappingSplitChar),pars);
        }
        

        /// <summary>
        /// 按页查询数据
        /// </summary>
        /// <typeparam name="T">生成的对象</typeparam>
        /// <param name="orderType">排序方式：比如asc,desc</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageCount">总查得页数</param>
        /// <param name="countInPage">每页显示多少条,当此参数为0时，将会查出所有符合条件的记录</param>
        /// <param name="tableName">表名</param>
        /// <param name="strFields">需要查询的字段</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="order">排序字段,比如:ondate desc</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public List<T> SearchDataPage<T>(string tableName, 
            string strFields, 
            string strWhere,
            string order,
            ref int pageIndex,
            ref int pageCount, 
            ref int dataCount, 
            int countInPage,
            IDictionary<string, string> fieldMapping,
            IDictionary<string,object> pars=null) where T : class
        {
            if (string.IsNullOrWhiteSpace(strWhere)) strWhere = "1=1";
            if (countInPage > 0)
            {
                var countsql=string.Format("select count(0) from {0} where {1}", tableName, strWhere);
                //总查得数据条数dataCount
                object obj =pars!= null && pars.Count > 0?
                    this.ExecuteScalar(countsql,pars.Keys.ToArray<string>(),pars.Values.ToArray<object>()):
                    this.ExecuteScalar(countsql);

                //查询符合条件的记录有多少条
                if (obj != null && obj != DBNull.Value)
                {
                    dataCount = int.Parse(obj.ToString()); //获得符合条件的数据数
                }

                pageCount = dataCount / countInPage; //记算查得多少页
                if (dataCount % countInPage != 0)
                {
                    pageCount++; //如果没有除尽则应该再加一页
                }

                //如果页面索引小于等于0则为第一页
                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }

                //如果页面索引大于总页数则索引等于最后一页
                if (pageIndex > pageCount && pageCount >= 0)
                {
                    pageIndex = pageCount;
                }

                int startIndex = (pageIndex - 1) * countInPage; //计算数据的起始位置
                startIndex = startIndex < 0 ? 0 : startIndex;
                string strSelectSql = CreatePagerSql(this.GetDbType(),tableName,strFields,strWhere,order,startIndex,countInPage);

                ////组合查询语句
                //switch (this.GetDbType())
                //{
                //    case DbType.Sqlite:
                //    case DbType.MySql:
                //        {
                //            strSelectSql = string.Format("select {0} from {1} where {2} order by {3} limit {4},{5}",
                //                                         strFields, tableName, strWhere, order, startIndex,
                //                                         countInPage);
                //            break;
                //        }
                //    case DbType.MSSqlServer:
                //        {
                //            strSelectSql =
                //                string.Format(
                //                    "select * from (select {0}, ROW_NUMBER() OVER(ORDER BY {1}) AS _RowNum from {2} where {5}) _t1 where _t1._RowNum between {3} and {4}",
                //                    strFields, order, tableName, startIndex + 1, startIndex + countInPage, strWhere);//因为sqlserver的行号是从一开始的
                //            break;
                //        }
                //    case DbType.Npgsql:
                //    case DbType.Oracle:
                //        {
                //            strSelectSql =
                //                string.Format(
                //                    "select * from (select {0}, ROWNUM rn from {1} where {4} order by {5}) _t1 where _t1.rn between {2} and {3}",
                //                    strFields, tableName, startIndex + 1, startIndex + countInPage, strWhere,order);//因为行号是从一开始的
                //            break;
                //        }
                //}

                return GetDataCollectionBySql<T>(strSelectSql, fieldMapping, pars); //查询对象集合
            }
            else//如果每页显示的数据小于或等于0,则全查出
            {
                return GetDataCollectionBySql<T>(string.Format("select {0} from {1} where {2} {3}",
                    strFields, tableName, strWhere, string.IsNullOrWhiteSpace(order) ? "" : "order by " + order), fieldMapping, pars); //查询对象集合
            }
        }

        /// <summary>
        /// 分页查询返回dataset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="strFields"></param>
        /// <param name="strWhere"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="dataCount"></param>
        /// <param name="countInPage"></param>
        /// <param name="fieldMapping"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Data.BasePageData SearchDataPage(string tableName,
           string strFields,
           string strWhere,
           string order,
           int pageIndex,
           int countInPage,
           IDictionary<string, object> pars = null)
        {
            var dataCount = 0;
            if (string.IsNullOrWhiteSpace(strWhere)) strWhere = "1=1";
            if (countInPage > 0)
            {
                var countsql = string.Format("select count(0) from {0} where {1}", tableName, strWhere);
                //总查得数据条数dataCount
                object obj = pars != null && pars.Count > 0 ?
                    this.ExecuteScalar(countsql, pars.Keys.ToArray<string>(), pars.Values.ToArray<object>()) :
                    this.ExecuteScalar(countsql);

                //查询符合条件的记录有多少条
                if (obj != null && obj != DBNull.Value)
                {
                    dataCount = int.Parse(obj.ToString()); //获得符合条件的数据数
                }

                var pageCount = dataCount / countInPage; //记算查得多少页
                if (dataCount % countInPage != 0)
                {
                    pageCount++; //如果没有除尽则应该再加一页
                }

                //如果页面索引小于等于0则为第一页
                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }

                //如果页面索引大于总页数则索引等于最后一页
                if (pageIndex > pageCount && pageCount >= 0)
                {
                    pageIndex = pageCount;
                }

                int startIndex = (pageIndex - 1) * countInPage; //计算数据的起始位置
                startIndex = startIndex < 0 ? 0 : startIndex;
                string strSelectSql = CreatePagerSql(this.GetDbType(), tableName, strFields, strWhere, order, startIndex, countInPage);

                var ds = GetDataSet(strSelectSql, pars); //查询对象集合
                var result = new Data.BasePageData();
                result.DataCount = dataCount;
                result.Index = pageIndex;
                result.PageCount = pageCount;
                result.Source = new Data.DataSet(ds);
                return result;
            }
            else//如果每页显示的数据小于或等于0,则全查出
            {
                var ds = GetDataSet(string.Format("select {0} from {1} where {2} {3}",
                    strFields, tableName, strWhere, string.IsNullOrWhiteSpace(order) ? "" : "order by " + order), pars); //查询对象集合
                var result = new Data.BasePageData();
                result.Source = new Data.DataSet(ds);
                return result;
            }
        }

        /// <summary>
        /// 通过SQL语句获取数据对象
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public List<T> GetDataCollectionBySql<T>(string strSql, string fieldMapping = "") where T : class
        {
            return GetDataCollectionBySql<T>(strSql, Common.Data.DataConvert.CreateMapping(fieldMapping, MappingFieldSplit, MappingSplitChar));
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="fieldMapping"></param>
        /// <returns></returns>
        public List<T> GetDataCollectionBySql<T>(string strSql, IDictionary<string, string> fieldMapping) where T : class
        {
            return GetDataCollectionBySql<T>(strSql, fieldMapping, null);
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="strSql">查询语句</param>
        /// <param name="fieldMapping">字段映射</param>
        /// <param name="dbparams">参数，可为空</param>
        /// <returns></returns>
        public List<T> GetDataCollectionBySql<T>(string strSql, string fieldMapping, IDictionary<string, object> dbparams) where T : class
        {
            return GetDataCollectionBySql<T>(strSql, Common.Data.DataConvert.CreateMapping(fieldMapping, MappingFieldSplit, MappingSplitChar), dbparams);
        }

        /// <summary>
        /// 通过SQL语句获取数据对象
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="fieldMapping">字段映射,key为属性名，value为字段名</param>
        /// <returns></returns>
        public List<T> GetDataCollectionBySql<T>(string strSql, IDictionary<string, string> fieldMapping, IDictionary<string, object> dbparams) where T : class
        {
            var dataCollection = new List<T>();

            System.Data.DataSet ds = null;
            if (dbparams == null)
            {
                ds = this.GetDataSet(strSql);
            }
            else
            {
                ds = this.GetDataSet(strSql, dbparams.Keys.ToArray<string>(), dbparams.Values.ToArray<object>());
            }


            var t = typeof(T);
            //循环获取对象
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                var obj = (T)Common.Data.DataConvert.ConvertDataToObject(dr, fieldMapping, t);
                dataCollection.Add(obj);
            }

            return dataCollection;
        }

        /// <summary>
        /// 通过SQL语句获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="fieldMapping"></param>
        /// <returns></returns>
        public T GetDataBySql<T>(string strSql, string fieldMapping = "") where T : class
        {
            return GetDataBySql<T>(strSql, Common.Data.DataConvert.CreateMapping(fieldMapping, MappingFieldSplit, MappingSplitChar));
        }

        /// <summary>
        /// 通过SQL语句获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="fieldMapping"></param>
        /// <returns></returns>
        public T GetDataBySql<T>(string strSql, IDictionary<string, string> fieldMapping) where T : class
        {
            return GetDataBySql<T>(strSql, fieldMapping, null);           
        }

        /// <summary>
        /// 通过语句获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="strSql">查询语句</param>
        /// <param name="fieldMapping">映射</param>
        /// <param name="dbparams">参数</param>
        /// <returns></returns>
        public T GetDataBySql<T>(string strSql, IDictionary<string, string> fieldMapping, IDictionary<string, object> dbparams) where T : class
        {
            var cmd = base.GetCommand(strSql, dbparams);
            System.Data.DataSet ds;
            if (dbparams == null)
            {
                ds = this.GetDataSet(strSql);
            }
            else
            {
                ds = this.GetDataSet(strSql, dbparams.Keys.ToArray<string>(), dbparams.Values.ToArray<object>());
            }

            T obj = default(T);

            if (ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    var t = typeof(T);
                    //获取对象

                    obj = Common.Data.DataConvert.ConvertDataToObject<T>(dr, fieldMapping);
                    break;
                }
            }
            return obj;
        }

        #region 通过表达式操作数据库

        /// <summary>
        /// 通过表达式获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public T GetData<T>(System.Linq.Expressions.Expression<Func<T, bool>> fun) where T:class
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get<T>();
            Dictionary<string, object> pars = null;
            string strwhere = "1=1";
            if (fun != null)
            {
                pars = new Dictionary<string, object>();
                strwhere = DoNet.Common.LinqExpression.Helper.DserExpressionToWhere<T>(fun, pars, this.GetParamChar(), this.DBSPStartChar, this.DBSPEndChar);
            }
            var sql = string.Format("select {0} from {1} where {2}", 
                DBSPStartChar + string.Join(DBSPEndChar + "," + DBSPStartChar, dbmappinginfo.Fields.Keys) + DBSPEndChar, 
                dbmappinginfo.TableName, strwhere);

            return this.GetDataBySql<T>(sql, dbmappinginfo.Mapping, pars);
        }

        /// <summary>
        /// 通过表达式获取对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public List<T> GetDataCollection<T>(System.Linq.Expressions.Expression<Func<T, bool>> fun = null) where T : class
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get<T>();
            Dictionary<string, object> pars = null;
            string strwhere = "1=1";
            if (fun != null)
            {
                pars = new Dictionary<string, object>();
                strwhere = DoNet.Common.LinqExpression.Helper.DserExpressionToWhere<T>(fun, pars, this.GetParamChar(),this.DBSPStartChar,this.DBSPEndChar);
            }
            var sql = string.Format("select {0} from {1} where {2}", 
                DBSPStartChar + string.Join(DBSPEndChar + "," + DBSPStartChar, dbmappinginfo.Fields.Keys) + DBSPEndChar, dbmappinginfo.TableName, strwhere);

            return this.GetDataCollectionBySql<T>(sql, dbmappinginfo.Mapping, pars);
        }

        /// <summary>
        /// 通过表达式分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public DoNet.Common.Data.PageData<T> Search<T>(int page, int count, System.Linq.Expressions.Expression<Func<T, bool>> fun = null) where T:class
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get<T>();
            Dictionary<string, object> pars = null;
            string strwhere = "1=1";
            if (fun != null)
            {
                pars = new Dictionary<string, object>();
                strwhere = DoNet.Common.LinqExpression.Helper.DserExpressionToWhere<T>(fun, pars, this.GetParamChar(), this.DBSPStartChar, this.DBSPEndChar);
            }
            var fields = DBSPStartChar + string.Join(DBSPEndChar + "," + DBSPStartChar, dbmappinginfo.Fields.Keys) + DBSPEndChar;
            return this.SearchDataPage<T>(dbmappinginfo.TableName, fields, strwhere, page, count, "", dbmappinginfo.Mapping, pars);
        }

        /// <summary>
        /// 添加新model到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(object model)
        {
            //如果是集合，则逐个处理
            if (model is System.Collections.IEnumerable)
            {
                var sqls = new List<string>();
                var pars = new List<Dictionary<string, object>>();
                foreach (object m in (System.Collections.IEnumerable)model)
                {
                    var par = new Dictionary<string, object>();
                    sqls.Add(CreateInsertSql(m, par).ToString());
                    pars.Add(par);
                }
                return this.ExecuteSql(sqls, pars);
            }
            else
            {
                var pars = new Dictionary<string, object>();
                var sql = CreateInsertSql(model, pars);
                return this.ExecuteNonQuery(sql.ToString(), pars);
            }
        }

        /// <summary>
        /// 添加新model到数据库并返回自增长ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertWithIdentity(object model)
        {

            var pars = new Dictionary<string, object>();
            var sql = CreateInsertSql(model, pars);
            var ds = this.GetDataSet(sql.ToString() + ";select @@identity;", pars);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }
        
        /// <summary>
        /// 生成插入sql和参数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public string CreateInsertSql(object model, Dictionary<string, object> pars, List<string> properties = null)
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get(model.GetType());
           
            var sql = new StringBuilder("insert into " + dbmappinginfo.TableName + "(");
            var sqlvs = new StringBuilder(" values(");
            var index = 0;
            foreach (var m in dbmappinginfo.Fields)
            {
                //不在新增属性内，则不拼入
                if (properties != null && !properties.Contains(m.Key)) continue;
                sql.Append(DBSPStartChar + m.Key + DBSPEndChar);
                var parkey = this.GetParamChar() + m.Key;
                sqlvs.Append(parkey);

                pars.Add(parkey, DoNet.Common.Reflection.ClassHelper.GetPropertyValue(model, m.Value));
                //如果不是最后一个则加逗号
                if (index < dbmappinginfo.Fields.Count - 1)
                {
                    sql.Append(',');
                    sqlvs.Append(',');
                }
                index++;
            }
          
            sql.Append(')');
            sqlvs.Append(')');
            sql.Append(sqlvs);
            return sql.ToString();
        }

        /// <summary>
        /// 更新某个model对象，需要指定主健
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="proterties">需要修改的字段属性</param>
        /// <returns></returns>
        public int Update(object model, List<string> proterties = null)
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get(model.GetType());
            if (dbmappinginfo.Primaries.Count == 0)
            {
                throw new Exception("没有指定当前Model的主健，无法使用此方法更新，请指定主健或手动执行SQL。");
            }
            var pars = new Dictionary<string, object>();
            var sql = new StringBuilder("update " + dbmappinginfo.TableName + " set ");
            var index = 0;
            foreach (var m in dbmappinginfo.Fields)
            {
                //在修改的指定字段中
                if (!dbmappinginfo.Primaries.Contains(m.Key) && (proterties == null || proterties.Contains(m.Value)))
                {
                    sql.Append(m.Key);
                    sql.Append("=");
                    var key = this.GetParamChar() + m.Key;
                    sql.Append(key);
                    pars.Add(key, DoNet.Common.Reflection.ClassHelper.GetPropertyValue(model, m.Value));
                    //如果不是最后一个则加逗号
                    if (index < dbmappinginfo.Fields.Count - 1)
                    {
                        sql.Append(',');
                    }
                }
                index++;
            }

            sql.Replace(',',' ',sql.Length-1,1);
            
            sql.Append(" where ");
            foreach (var p in dbmappinginfo.Primaries)
            {
                sql.Append(p);
                sql.Append("=");
                var key = this.GetParamChar() + p;
                sql.Append(key);
                if (!pars.ContainsKey(key)) pars.Add(key, DoNet.Common.Reflection.ClassHelper.GetPropertyValue(model, dbmappinginfo.Fields[p]));
                //如果不是最后一个则加and
                if (dbmappinginfo.Primaries.IndexOf(p) < dbmappinginfo.Primaries.Count - 1)
                {
                    sql.Append(" and ");
                }
            }

            return this.ExecuteNonQuery(sql.ToString(), pars);
        }

        /// <summary>
        /// 利用表达式移除元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> fun) where T : class
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get<T>();
            Dictionary<string, object> pars = null;
            string strwhere = "1=0";
            pars = new Dictionary<string, object>();
            strwhere = DoNet.Common.LinqExpression.Helper.DserExpressionToWhere<T>(fun, pars, this.GetParamChar(), this.DBSPStartChar, this.DBSPEndChar);

            var sql = "delete from " + dbmappinginfo.TableName + " where " + strwhere;
            return this.ExecuteNonQuery(sql,pars);
        }

        /// <summary>
        /// 移除某个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int Delete(object model)
        {
            var dbmappinginfo = DoNet.Common.Data.DBMappingInfo.Get(model.GetType());
            if (dbmappinginfo.Primaries.Count == 0)
            {
                throw new Exception("没有指定当前Model的主健，无法使用此方法更新，请指定主健或手动执行SQL。");
            }
            Dictionary<string, object> pars = null;
            var sql = new StringBuilder("delete from " + dbmappinginfo.TableName + " where ");
            pars = new Dictionary<string, object>();
            foreach (var p in dbmappinginfo.Primaries)
            {
                sql.Append(p);
                sql.Append("=");
                var key = this.GetParamChar() + p;
                sql.Append(key);
                pars.Add(key, DoNet.Common.Reflection.ClassHelper.GetPropertyValue(model, p));
                //如果不是最后一个则加and
                if (dbmappinginfo.Primaries.IndexOf(p) < dbmappinginfo.Primaries.Count - 1)
                {
                    sql.Append(" and ");
                }
            }
                      
            return this.ExecuteNonQuery(sql.ToString(), pars);
        }
        #endregion
    }
}
