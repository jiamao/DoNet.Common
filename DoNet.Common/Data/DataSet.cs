using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 查询结果集
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataSet
    {
        public DataSet() {
            Tables = new List<DataTable>();
        }

        public DataSet(System.Data.DataSet ds)            
        {
            Tables = new List<DataTable>();
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                Tables.Add(new DataTable(dt));
            }
        }

        /// <summary>
        /// 所有的表
        /// </summary>
        [DataMember]
        public List<DataTable> Tables { get; set; }

        public override string ToString()
        {
            return DoNet.Common.Serialization.FormatterHelper.XMLSerObjectToString(this);
        }

        public string ToJson()
        {
            return DoNet.Common.Serialization.JSon.ModelToJson(this);
        }

        public static DataSet FromString(string source)
        {
            try
            {
                var obj = DoNet.Common.Serialization.FormatterHelper.XMLDerObjectFromString<DataSet>(source);
                return obj;
            }
            catch
            {
                throw new Exception("反序列化失败:" + source);
            }
        }

        public static DataSet FromJson(string json)
        {
            try
            {
            return DoNet.Common.Serialization.JSon.JsonToModel<DataSet>(json);
            }
            catch
            {
                throw new Exception("反序列化失败:" + json);
            }
        }

        public System.Data.DataSet ToDataSet()
        {
            var ds = new System.Data.DataSet();
            foreach (var tb in this.Tables)
            {
                ds.Tables.Add(tb.ToTable());
            }
            return ds;
        }
    }

    /// <summary>
    /// 数据表
    /// </summary>
    [Serializable]
    [DataContract]
    public class DataTable
    {
        public DataTable()
        {
        }

        /// <summary>
        /// 通过表实例化此对象
        /// </summary>
        /// <param name="dt"></param>
        public DataTable(System.Data.DataTable dt)
        {
            FromTable(dt);
        }

        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        [DataMember]
        public DataColumn[] Columns { get; set; }

        /// <summary>
        /// 所有数据行
        /// </summary>
        [DataMember]
        public DataRow[] Rows { get; set; }

        /// <summary>
        /// 从源表转为当前格式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnMappings"></param>
        public void FromTable(System.Data.DataTable dt)
        {
            if (dt != null)
            {
                this.Name = dt.TableName;
                //初始化列信息
                Columns = new DataColumn[dt.Columns.Count];
                for (var i = 0; i < Columns.Length; i++)
                {
                    Columns[i] = new DataColumn(dt.Columns[i]);
                    Columns[i].Index = i;
                }

                this.Rows = new DataRow[dt.Rows.Count];
                for (var i = 0; i < dt.Rows.Count;i++ )
                {
                    this.Rows[i] = new DataRow(dt.Rows[i],Columns);
                }
            }
        }

        public System.Data.DataTable ToTable()
        {
            var tb = new System.Data.DataTable(this.Name);
            foreach (var c in this.Columns)
            {
                tb.Columns.Add(c.ToColumn());
            }

            foreach (var r in this.Rows)
            {
                var row = tb.NewRow();

                foreach (var item in r.ItemArray)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        try
                        {
                            var column = tb.Columns[item.ColumnName];
                            if (column.DataType.FullName.Equals("System.Byte[]", StringComparison.OrdinalIgnoreCase))
                            {
                                row[item.ColumnName] =!string.IsNullOrWhiteSpace(item.Value) ?Convert.FromBase64String(item.Value):null;
                            }
                            else if (column.DataType.FullName.Equals("System.DateTime", StringComparison.OrdinalIgnoreCase))
                            {
                                if (string.IsNullOrWhiteSpace(item.Value) == false)
                                {
                                    var s = item.Value.IndexOf('/');
                                    //如果/在第三个后，则表示为天/月/年
                                    if (s == 2)
                                    {
                                        row[item.ColumnName] = DateTime.ParseExact(item.Value, "dd/MM/yyyy HH:mm:ss", null);
                                    }
                                    else if (s > 2)
                                    {
                                        row[item.ColumnName] = DateTime.ParseExact(item.Value, "yyyy/MM/dd HH:mm:ss", null);
                                    }
                                    else
                                    {
                                        row[item.ColumnName] = DateTime.Parse(item.Value);
                                    }
                                }                                
                            }
                            else
                            {
                                row[item.ColumnName] = item.Value;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }
                tb.Rows.Add(row);
            }

            return tb;
        }
    }

    /// <summary>
    /// 列信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class DataColumn
    {
        public DataColumn() { }
        public DataColumn(System.Data.DataColumn dc)
        {
            FromColumn(dc);
        }

        /// <summary>
        /// 字段名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 显示的名称
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        /// 在当前列中索引
        /// </summary>
        [DataMember]
        public int Index { get; set; }

        /// <summary>
        /// 当前列说明文字
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 转换列
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="ColumnMappings"></param>
        /// <returns></returns>
        private void FromColumn(System.Data.DataColumn dc)
        {
            this.DataType = dc.DataType.FullName;
            this.Name = dc.ColumnName;
            this.DisplayName = this.Name;  
        }

        public System.Data.DataColumn ToColumn()
        {
            var dc = new System.Data.DataColumn();
            dc.ColumnName = this.Name;

            dc.DataType = Type.GetType(this.DataType);

            return dc;
        }
    }

    /// <summary>
    /// 数据项
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataItem
    {
        /// <summary>
        /// 对应的列
        /// </summary>
        [DataMember]
        public string ColumnName { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 表行
    /// </summary>
    [Serializable]
    [DataContract]
    public class DataRow
    {
        public DataRow() { }
        public DataRow(System.Data.DataRow dr, DataColumn[] columns)
        {
            FromRow(dr, columns);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataItem this[int index]
        {
            get { return ItemArray[index]; }
            set { ItemArray[index] = value; }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataItem this[string columnName]
        {
            get {
                foreach (var item in ItemArray)
                {
                    if (columnName.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase))
                        return item;
                }
                return null;
            }
            set {
                var item = this[columnName];
                if (item != null)
                {
                    item.Value = value.Value;
                }
            }
        }


        /// <summary>
        /// 当前行数值
        /// </summary>
        [DataMember]
        public DataItem[] ItemArray { get; set; }

        private void FromRow(System.Data.DataRow dr,DataColumn[] columns)
        {
            ItemArray = new DataItem[dr.ItemArray.Length];
            for (var i = 0; i < ItemArray.Length; i++)
            {
                var obj = dr.ItemArray[i];
                ItemArray[i] = new DataItem() {                    
                    ColumnName = columns[i].Name                    
                };
                if (obj != null)
                {
                    if (obj !=DBNull.Value && columns[i].DataType.Equals("System.Byte[]", StringComparison.OrdinalIgnoreCase))
                    {
                        ItemArray[i].Value = Convert.ToBase64String((byte[])obj);
                    }
                    else
                    {
                        ItemArray[i].Value = obj.ToString();
                    }
                }
            }
        }
    }
}
