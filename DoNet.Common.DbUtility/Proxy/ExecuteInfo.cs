using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DoNet.Common.DbUtility.Proxy
{
    [DataContract]
    [Serializable]
    public class ExecuteInfo
    {
        [DataMember]
        public DBInfo DB { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public List<string> Sqls { get; set; }

        [DataMember]
        public List<string[]> ParaNames{get;set;}

        [DataMember]
        public List<object[]> ParaValues { get; set; }

        public override string ToString()
        {
            return DoNet.Common.Serialization.JSon.ModelToJson(this);
        }
    }
}
