namespace JM.UpdateServer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtdir = new System.Windows.Forms.TextBox();
            this.btndir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btntarget = new System.Windows.Forms.Button();
            this.txttarget = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btncreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtdir
            // 
            this.txtdir.Location = new System.Drawing.Point(14, 30);
            this.txtdir.Name = "txtdir";
            this.txtdir.Size = new System.Drawing.Size(438, 21);
            this.txtdir.TabIndex = 0;
            // 
            // btndir
            // 
            this.btndir.Location = new System.Drawing.Point(471, 28);
            this.btndir.Name = "btndir";
            this.btndir.Size = new System.Drawing.Size(36, 23);
            this.btndir.TabIndex = 1;
            this.btndir.Text = "...";
            this.btndir.UseVisualStyleBackColor = true;
            this.btndir.Click += new System.EventHandler(this.btndir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "需要生成更新包的路径";
            // 
            // btntarget
            // 
            this.btntarget.Location = new System.Drawing.Point(471, 72);
            this.btntarget.Name = "btntarget";
            this.btntarget.Size = new System.Drawing.Size(36, 23);
            this.btntarget.TabIndex = 4;
            this.btntarget.Text = "...";
            this.btntarget.UseVisualStyleBackColor = true;
            this.btntarget.Click += new System.EventHandler(this.btntarget_Click);
            // 
            // txttarget
            // 
            this.txttarget.Location = new System.Drawing.Point(14, 74);
            this.txttarget.Name = "txttarget";
            this.txttarget.Size = new System.Drawing.Size(438, 21);
            this.txttarget.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "生成更新包保存路径";
            // 
            // btncreate
            // 
            this.btncreate.Location = new System.Drawing.Point(219, 123);
            this.btncreate.Name = "btncreate";
            this.btncreate.Size = new System.Drawing.Size(75, 23);
            this.btncreate.TabIndex = 6;
            this.btncreate.Text = "生成";
            this.btncreate.UseVisualStyleBackColor = true;
            this.btncreate.Click += new System.EventHandler(this.btncreate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 185);
            this.Controls.Add(this.btncreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btntarget);
            this.Controls.Add(this.txttarget);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btndir);
            this.Controls.Add(this.txtdir);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtdir;
        private System.Windows.Forms.Button btndir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btntarget;
        private System.Windows.Forms.TextBox txttarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btncreate;
    }
}

