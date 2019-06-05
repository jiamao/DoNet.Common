using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JM.UpdateServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btndir_Click(object sender, EventArgs e)
        {
            var dirf = new FolderBrowserDialog();
            if (dirf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtdir.Text = dirf.SelectedPath;
            }
        }

        private void btntarget_Click(object sender, EventArgs e)
        {
            var dirf = new FolderBrowserDialog();
            if (dirf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txttarget.Text = dirf.SelectedPath;
            }
        }

        private void btncreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtdir.Text))
                {
                    MessageBox.Show("请指定软件路径");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txttarget.Text))
                {
                    MessageBox.Show("请指定更新包保存路径");
                    return;
                }

                JM.Update.UpdateApp.CreateUpdatePack(txtdir.Text, txttarget.Text);

                MessageBox.Show("更新文件生成完成");

                System.Diagnostics.Process.Start("Explorer.exe", txttarget.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
