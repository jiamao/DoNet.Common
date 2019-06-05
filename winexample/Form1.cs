using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winexample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            var db3 = DoNet.Common.DbUtility.DbFactory.CreateDbORM("mysql.data.dll", "server=127.0.0.1;port=3306;database=jiamao;user id=root;password=123456;charset=utf8;default command timeout=10");
            var ds = db3.GetDataSet("select * from article");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var context = new DoNet.Common.Net.HttpListenerController("/", @"E:\website\epr_demo");
            context.AddPrefix("http://127.0.0.1:8089/");
            context.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var printer = new DoNet.Common.IO.Printer();
            printer.PrinterItems = new List<DoNet.Common.IO.Printer.PrintModel>();
            var p1 = new DoNet.Common.IO.Printer.PrintModel();
            p1.brush = Brushes.Blue;
            p1.font = new Font(new FontFamily("宋体"), 18);
            p1.location = new Point(20, 20);
            p1.textalgin = "right";
            p1.value = "测试打印";

            printer.PrinterItems.Add(p1);
           // printer.Width = 800;
            printer.print();
        }
    }
}
