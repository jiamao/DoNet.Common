using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JM.Update
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
            this.Load += frmUpdate_Load;
        }

        void frmUpdate_Load(object sender, EventArgs e)
        {
            update();
        }

        void update()
        {
            try
            {
                var app = new UpdateApp();
                var rootpath = "";
                if (this.Tag != null)
                {
                    var args = (string[])this.Tag;
                    if (args != null && args.Length > 0)
                    {
                        rootpath = System.IO.Path.GetDirectoryName(args[0]);
                    }
                }

                JM.Update.UpdateApp.LogPath = System.IO.Path.Combine(Application.StartupPath, "update.log");
                System.IO.File.Delete(JM.Update.UpdateApp.LogPath);
                
                app.Run(msgCallback, rootpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void msgCallback(string msg)
        {
            this.BeginInvoke(new messagehandler(showmessage),msg);           
        }
        delegate void messagehandler(string msg);
        void showmessage(string msg)
        {
            if (msg == "complete")
            {
                complete();
                return;
            }

            lstMsg.Items.Add(msg);
            lstMsg.SetSelected(lstMsg.Items.Count - 1, true);
        }

        void complete()
        {
            var args = this.Tag as string[];
            if (args != null && args.Length > 0)
            {
                System.Diagnostics.Process.Start(args[0],"updated");
            }
             Application.Exit();           
        }
    }
}
