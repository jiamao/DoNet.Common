using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageToBase64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("选择要转换的文件");
                    return;
                }
                if (File.Exists(textBox1.Text))
                {
                    var bs = File.ReadAllBytes(textBox1.Text);
                    var base64 = Convert.ToBase64String(bs, Base64FormattingOptions.None);
                    var ext = Path.GetExtension(textBox1.Text.Trim()).Trim('.');
                    richTextBox1.Text = "data:image/" + ext + ";base64," + base64;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*var mail = new DoNet.Common.Net.MailHelper();

            var unit = new DoNet.Common.Net.MailUnit("273389528@qq.com", "image test", "<div><img src=\"cid:img1\" /></div>");
            unit.LinkedReses = new System.Net.Mail.LinkedResource[] { 
                new System.Net.Mail.LinkedResource("F:\\100646209.png"){ ContentId="img1"}
            };
            unit.Attachments = new System.Net.Mail.Attachment[] { 
                new System.Net.Mail.Attachment("F:\\100646209.png"){  ContentId="img2", Name="中文.png"}
            };
            unit.IsHtml = true;
            unit.MailPwd = "www.jm47.com2";
            unit.Mailserver = "smtp.163.com";
            unit.MailUser = "hh@163.com";
            unit.Sender = "hh@163.com";
            
            mail.QueueSend(unit);

            mail.Startup();*/
        }
    }
}
