using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            //var public_key = "<RSAKeyValue><Modulus>3WCQr1IQqCh34WKOCTzpiC0wfAP8cEHojP9LBqklaNheITFFrP1Ec4bXp042YW6bYxHnMDyDTg//4nUcC/RR3qIEzExNmf2V0FMtzdEcKovXxM7yzFJkaM35DM1MsKhJdyNmgbh/ja1EJDpVt+zn8qsFBUtqkVO0ElS7nk9ZBZM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //var private_key = "<RSAKeyValue><Modulus>3WCQr1IQqCh34WKOCTzpiC0wfAP8cEHojP9LBqklaNheITFFrP1Ec4bXp042YW6bYxHnMDyDTg//4nUcC/RR3qIEzExNmf2V0FMtzdEcKovXxM7yzFJkaM35DM1MsKhJdyNmgbh/ja1EJDpVt+zn8qsFBUtqkVO0ElS7nk9ZBZM=</Modulus><Exponent>AQAB</Exponent><P>75ViW3Zcn8p11X23Xj5xBFwyhEC7a9xIVv28Ob4lajlig89zBuZ8Zi9JqC3sRxWmO5J3xRtk3GEwnld6lVFdtQ==</P><Q>7IvRUvXoyhRKiPU3tMQVt/z8mjGjQoSYPRhu4T/TnzTdMEcDNLdpHaqB8ELAog7utd7C4TEAye4tUCY+ZhQjJw==</Q><DP>m1whlfHhCnV9h92oBOM04oDu+TgI0V7dQhvz7PXSyVlA+vyROM5JqPHNL9PnvgjZ7RODuzuSYh5cKrHLefxzaQ==</DP><DQ>zMtwe0b0OKDAtzq29AYgV57shAMdueVaeOrCdLnx2hDGv5l7qRRyKYEJ5p2kcapD+anXR2hJqopPKOkzdOVSWQ==</DQ><InverseQ>fMBAzUlLGvcMNfvlPKc/vLSEzO5WVQz6SDvBHZhHJXFzXZbq7Yfvfr5qw7bN5f49ITIPedlz5MiO/G8U6ciX6w==</InverseQ><D>a5zveGpaMoRJkkSIazEzDMF62i5N3nwLgc7wN7KtvsO/Lj93cVpEliwsVOYORVqxKn2fdrFT2vSoHPt0wNLpoHqJ4ryMtsJ/sL/0vyaF7D2HeVitdZ7PIL+S7MfywAv24IG51XvFbiuDyMYaQzp3E+6MS2vsuKq3U4KIABzJOfE=</D></RSAKeyValue>";


            //var source = "abcdefg";
            //var prikey = "";
            //var pubkey = "";
            //DoNet.Common.Text.Security.RSACreateKey(ref prikey, ref pubkey);
            //var s = DoNet.Common.Text.Security.RSAEncrypt(source, pubkey);


            //var s2 = DoNet.Common.Text.Security.Des3Encrypt("111111111111112222222233", "for 语句有三个参数。第一个参数初始化变量，第二个参数保存条件，第三个参数包含执行循环所需的增");

            //var s = DoNet.Common.Text.Security.Des3Decrypt("111111111111112222222233", s2);

            DateTime dt;
            DateTime.TryParse("d3", out dt);
           
            //Console.WriteLine(s);

            var ths = new System.Threading.Thread[100];
            Console.WriteLine(DateTime.Now.ToString());
            for (var i = 0; i < 1;i++ )
            {
                ths[i] = new System.Threading.Thread(new System.Threading.ThreadStart(Log));
                ths[i].Start();
            }

            Console.Read();
        }

        static void Log()
        {
            var index=0;
            while (index < 1000)
            {
                DoNet.Common.IO.Logger.Write("logger:" + index);
                //DoNet.Common.IO.Logger.Debug("debug-logger:" + index);
                DoNet.Common.IO.Logger.Debug("person:",new person(),2,0.5);
                index++;
            }

            Console.WriteLine("log-end:" +DateTime.Now.ToString());
        }
    }
    public class person
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
