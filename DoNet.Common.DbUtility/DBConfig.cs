using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DoNet.Common.DbUtility
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig
    {
        public DBConfig() {
            Init();//初始化配置
        }

        //连接在XML中的路径
        const string ConSettingParentNodeName = "DBConnection";
        const string ConSettingNodeName = "connectiongstring";
        //对象配置在XML中的路径
        const string ModelSettingParentNodeName = "Items";
        const string ModelSettingNodeName = "Model";
        //读取节点路径组合
        const string NodelValuePathMark = "{0}/value";
        /// <summary>
        /// 查询配置
        /// </summary>
        const string SelectNodeName = "Select";
        /// <summary>
        /// 新增配置
        /// </summary>
        const string InsertNodeName = "Insert";
        /// <summary>
        /// 修改配置
        /// </summary>
        const string UpdateNodeName = "Update";

        //数据库配置路径
        private string _DBConfigPath = "Config/JMDB.config";

        /// <summary>
        /// 数据库配置路径
        /// 默认为:Config/JMDB.config
        /// </summary>
        public string DBConfigPath
        {
            get { return _DBConfigPath; }
            set { _DBConfigPath = value; }
        }

        /// <summary>
        /// 所有连接
        /// </summary>
        public List<ConnSetting> ConSettings { get; set; }

        /// <summary>
        /// 获取数据连接配置
        /// </summary>
        /// <param name="conName"></param>
        /// <returns></returns>
        public ConnSetting GetConSettingByName(string conName) {
            if (ConSettings != null)
            {
                if (string.IsNullOrWhiteSpace(conName) && ConSettings.Count > 0) return ConSettings[0];//默认返回第一个

                foreach (var con in ConSettings)
                {
                    if (con.Name.Equals(conName, StringComparison.CurrentCultureIgnoreCase)) return con;
                }
            }
            return null;
        }

        /// <summary>
        /// 所有对象配置
        /// </summary>
        public List<ModelSetting> ModelSettings { get; set; }

        /// <summary>
        /// 通过MODEL命名空间获取配置
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public ModelSetting GetModelSettingByContract(string contract)
        {
            if (ModelSettings != null)
            {
                foreach (var ms in ModelSettings)
                {
                    if (ms.Contract.Equals(contract, StringComparison.CurrentCultureIgnoreCase)) return ms;
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public void Init()
        {
            var configpath = IO.PathMg.CheckPath(DBConfigPath);//组合路径
            var xmlwr = new IO.XmlHelper(configpath);//XML读取器

            InitConnectionSetting(xmlwr);//初始化连接

            InitModelSetting(xmlwr);//初始化对象配置
        }

        /// <summary>
        /// 初始化连接字符串
        /// </summary>
        /// <param name="xmlwr"></param>
        private void InitConnectionSetting(IO.XmlHelper xmlwr)
        {
            var connodes = xmlwr.GetNodeListByDelegate(delegate(XmlNode xn)
            {
                return xn.Name.Equals(ConSettingNodeName, StringComparison.CurrentCultureIgnoreCase) &&
                    xn.ParentNode != null && xn.ParentNode.Name.Equals(ConSettingParentNodeName, StringComparison.CurrentCultureIgnoreCase);
            });

            if (connodes != null)
            {
                ConSettings = new List<ConnSetting>();
                foreach (var xn in connodes)
                {
                    var con = new ConnSetting()
                    {
                        ConnectionString = IO.XmlHelper.ReadAttributeValue(xn, "connectionString"),//读取连接字符串
                        Name = IO.XmlHelper.ReadAttributeValue(xn, "name"),//读取配置名
                        ProviderName = IO.XmlHelper.ReadAttributeValue(xn, "providerName")//读取驱动
                    };
                    ConSettings.Add(con);
                }
            }
        }

        /// <summary>
        /// 初始化对象配置
        /// </summary>
        /// <param name="xmlwr"></param>
        private void InitModelSetting(IO.XmlHelper xmlwr)
        {
            var modelnodes = xmlwr.GetNodeListByDelegate(delegate(XmlNode xn)
            {
                return xn.Name.Equals(ModelSettingNodeName, StringComparison.CurrentCultureIgnoreCase) &&
                    xn.ParentNode != null && xn.ParentNode.Name.Equals(ModelSettingParentNodeName, StringComparison.CurrentCultureIgnoreCase);
            });

            if (modelnodes != null)
            {
                ModelSettings = new List<ModelSetting>();
                foreach (var xn in modelnodes)
                {
                    var model = new ModelSetting();
                    model.Contract = IO.XmlHelper.ReadAttributeValue(xn, "contract");//读取类型

                    model.TableName = IO.XmlHelper.ReadAttributeValue(xn, "TableName/value");//表名
                    model.PrimaryKey = IO.XmlHelper.ReadAttributeValue(xn, "PrimaryKey/value");//表的主健
                    //model.Fields = IO.XmlWR.ReadAttributeValue(xn, "Model/Fields/value");//字段
                    model.Mapping = IO.XmlHelper.ReadAttributeValue(xn, "Mapping/value");//获取映射

                    foreach (XmlNode cn in xn.ChildNodes)
                    {     
                        if (cn.Name.Equals(SelectNodeName, StringComparison.OrdinalIgnoreCase))//查询字段
                        {
                            model.Selects = GetCommandFromNode(cn);                           
                        }
                        else if (cn.Name.Equals(InsertNodeName, StringComparison.OrdinalIgnoreCase))//新增需要插入的字段
                        {
                            model.InsertCommands = GetCommandFromNode(cn);                    
                        }
                        else if (cn.Name.Equals(UpdateNodeName, StringComparison.OrdinalIgnoreCase))//修改需要插入的字段
                        {
                            model.UpdateCommands = GetCommandFromNode(cn);                            
                        }                        
                    }
             
                    ModelSettings.Add(model);
                }
            }
        }

        /// <summary>
        /// 读取操作配置，
        /// 新增，修改，删除等操作配置
        /// </summary>
        /// <param name="xn">配置xml</param>
        /// <returns></returns>
        private List<CommandSetting> GetCommandFromNode(XmlNode xn)
        {
            var result = new List<CommandSetting>();
            const string FieldKey = "{0}/Fields";
            const string WhereKey = "{0}/Where";
            if (xn.ChildNodes != null)
            {
                foreach (XmlNode uxn in xn.ChildNodes)
                {
                    if (uxn.NodeType != XmlNodeType.Element) continue;
                    var command = new CommandSetting();
                    command.Key = uxn.Name;
                    command.Fields = IO.XmlHelper.ReadAttributeValue(uxn, string.Format(FieldKey, uxn.Name));//读取字段
                    command.Where = IO.XmlHelper.ReadAttributeValue(uxn, string.Format(WhereKey, uxn.Name));//读取条件
                    result.Add(command);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// model与表对应的配置
    /// </summary>
    [Serializable]
    public class ModelSetting
    {
        /// <summary>
        /// 对应的MODEL类型
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        /// 对应的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主健
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 主健数组
        /// </summary>
        public string[] PrimaryKeys { 
            get {
                if (!string.IsNullOrWhiteSpace(PrimaryKey))
                {
                    var keys = PrimaryKey.Split(DbORM.MappingFieldSplit);
                    return keys.Where<string>(p => !string.IsNullOrWhiteSpace(p)).ToArray<string>();
                }
                return null;
            }
        }

        string _fields;
        /// <summary>
        /// 字段
        /// </summary>
        public string Fields {
            get {
                if (string.IsNullOrWhiteSpace(_fields))
                {
                    _fields=  string.Join(DbORM.MappingFieldSplit.ToString(),
                        MappingCollection.Values.ToArray<string>());
                }
                return _fields;
            } 
        }

        /// <summary>
        /// 字段映射
        /// </summary>
        public string Mapping { get; set; }

        Dictionary<string, string> _mapping;
        /// <summary>
        /// 字段与属性映射
        /// </summary>
        public Dictionary<string, string> MappingCollection
        {
            get
            {
                if (_mapping == null) _mapping = Common.Data.DataConvert.CreateMapping(Mapping,DbORM.MappingFieldSplit,DbORM.MappingSplitChar);//生成映射
                return _mapping;
            }
        }

        /// <summary>
        /// 通过属性获取对应的字段名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetFieldNameByProperty(string propertyName)
        {
            var fieldname = "";
            propertyName = propertyName.ToLower();
            if (MappingCollection.TryGetValue(propertyName, out fieldname))
            {
                return fieldname;
            }
            return propertyName;
        }

        /// <summary>
        /// 通过字段名获取对应的属性
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetPropertyNameByField(string fieldName)
        {
            foreach (var f in MappingCollection)
            {
                if (f.Value.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
                    return f.Key;
            }

            return fieldName;
        }

        /// <summary>
        /// 查询配置
        /// </summary>
        public List<CommandSetting> Selects { get; set; }

        /// <summary>
        /// 获取查询属性配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CommandSetting GetSelectSetting(string key)
        {
            return GetCommandValue(Selects, key);
        }

        /// <summary>
        /// 插入相关的属性
        /// </summary>
        public List<CommandSetting> InsertCommands { get; set; }

        /// <summary>
        /// 获取插入需要更新的属性
        /// </summary>
        /// <param name="key">配置节点名</param>
        /// <returns></returns>
        public CommandSetting GetInsertCommand(string key)
        {
            return GetCommandValue(InsertCommands, key);
        }

        /// <summary>
        /// 更新相关的属性
        /// </summary>
        public List<CommandSetting> UpdateCommands { get; set; }

        /// <summary>
        /// 获取更新的属性
        /// </summary>
        /// <param name="key">配置节点名</param>
        /// <returns></returns>
        public CommandSetting GetUpdateCommand(string key)
        {
            return GetCommandValue(UpdateCommands, key);
        }

        /// <summary>
        /// 删除操作配置集合
        /// </summary>
        public List<CommandSetting> DeleteCommands { get; set; }
        /// <summary>
        /// 获取删除操作配置
        /// </summary>
        /// <param name="key">配置节点名</param>
        /// <returns></returns>
        public CommandSetting GetDeleteCommand(string key)
        {
            return GetCommandValue(DeleteCommands, key);
        }

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSelectSql()
        {
            return string.Format("select {0} from {1} ", Fields, TableName);
        }

        /// <summary>
        /// 提取值
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private CommandSetting GetCommandValue(List<CommandSetting> dic, string key)
        {
            if (dic != null && dic.Count > 0)
            {
                CommandSetting command=null;
                //默认取第一个
                if (string.IsNullOrWhiteSpace(key))
                {
                    command = dic[0];
                }
                else
                {
                    foreach (var cs in dic)
                    {
                        if (cs.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                        {
                            command = cs;
                            break;
                        }
                    }
                }
                //处理操作的字段，把属性转为数据库字段
                if (command != null)
                {
                    //如果没有初始化
                    //初始化字段
                    if (command.FieldCollection == null)
                    {
                        command.FieldCollection = TurnFieldToDataField(command.Fields);
                    }
                    //初始化条件字段
                    if (command.WhereCollection == null)
                    {
                        command.WhereCollection = TurnFieldToDataField(command.Where);
                    }
                }
                return command;
            }
            return null;
        }

        /// <summary>
        /// 把用逗号分隔的字段串转为数据库中的字段集合
        /// </summary>
        /// <param name="fields">用逗号分隔的字段串</param>
        /// <returns>数据库中的字段集合</returns>
        private List<string> TurnFieldToDataField(string fields)
        {
            var result = new List<string>();
            //如果字段不为空
            if (!string.IsNullOrWhiteSpace(fields))
            {
                //拆分字段
                var fs = fields.Split(',');
                foreach (var f in fs)
                {
                    if (string.IsNullOrWhiteSpace(f)) continue;
                    var r = GetFieldNameByProperty(f);//获取数居库中对应的字段名
                    if (!string.IsNullOrWhiteSpace(r))
                    {
                        result.Add(r);
                    }
                }
            }
            return result;
        }

    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    [Serializable]
    public class ConnSetting
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 指定的驱动
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// 查询，插入，更新，删除操作
    /// </summary>
    [Serializable]
    public class CommandSetting
    {
        /// <summary>
        /// 当前配置的唯一标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 需要处理的字段
        /// </summary>
        public string Fields
        {
            get;
            set;
        }

        /// <summary>
        /// 处理字段的集合
        /// 转为数据库表中字段的集合
        /// </summary>
        public List<string> FieldCollection
        {
            get;
            set;
        }

        /// <summary>
        /// 处理对象的条件
        /// </summary>
        public string Where
        {
            set;
            get;
        }

        /// <summary>
        /// 处理条件字段的集合
        /// </summary>
        public List<string> WhereCollection
        {
            get;
            set;
        }
    }
}
