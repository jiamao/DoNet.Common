using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace JM.Common.Examples.web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“test2”。
    public class test2 : Itest2
    {
        public string DoWork()
        {
            return "adfsadf";
        }
    }
}
