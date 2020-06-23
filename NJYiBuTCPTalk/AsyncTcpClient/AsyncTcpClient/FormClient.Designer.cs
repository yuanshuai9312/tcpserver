namespace AsyncTcpClient
{
    partial class FormClient
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_Send = new System.Windows.Forms.Button();
            this.textBox_Send = new System.Windows.Forms.TextBox();
            this.textBox_Receive = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_Stop_Listen = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_Status = new System.Windows.Forms.ListBox();
            this.textBox_ServerIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_Send);
            this.groupBox3.Controls.Add(this.textBox_Send);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 327);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(548, 135);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "发送的信息";
            // 
            // button_Send
            // 
            this.button_Send.Location = new System.Drawing.Point(427, 105);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(69, 23);
            this.button_Send.TabIndex = 1;
            this.button_Send.Text = "发送";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // textBox_Send
            // 
            this.textBox_Send.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_Send.Location = new System.Drawing.Point(3, 17);
            this.textBox_Send.Multiline = true;
            this.textBox_Send.Name = "textBox_Send";
            this.textBox_Send.Size = new System.Drawing.Size(542, 78);
            this.textBox_Send.TabIndex = 0;
            // 
            // textBox_Receive
            // 
            this.textBox_Receive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Receive.Location = new System.Drawing.Point(3, 17);
            this.textBox_Receive.Multiline = true;
            this.textBox_Receive.Name = "textBox_Receive";
            this.textBox_Receive.Size = new System.Drawing.Size(519, 112);
            this.textBox_Receive.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_Receive);
            this.groupBox2.Location = new System.Drawing.Point(2, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 132);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "接受的信息";
            // 
            // button_Stop_Listen
            // 
            this.button_Stop_Listen.Location = new System.Drawing.Point(436, 119);
            this.button_Stop_Listen.Name = "button_Stop_Listen";
            this.button_Stop_Listen.Size = new System.Drawing.Size(69, 23);
            this.button_Stop_Listen.TabIndex = 1;
            this.button_Stop_Listen.Text = "连接";
            this.button_Stop_Listen.UseVisualStyleBackColor = true;
            this.button_Stop_Listen.Click += new System.EventHandler(this.button_Stop_Listen_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_ServerIp);
            this.groupBox1.Controls.Add(this.button_Stop_Listen);
            this.groupBox1.Controls.Add(this.listBox_Status);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 156);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态信息";
            // 
            // listBox_Status
            // 
            this.listBox_Status.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox_Status.FormattingEnabled = true;
            this.listBox_Status.ItemHeight = 12;
            this.listBox_Status.Location = new System.Drawing.Point(3, 17);
            this.listBox_Status.Name = "listBox_Status";
            this.listBox_Status.Size = new System.Drawing.Size(542, 88);
            this.listBox_Status.TabIndex = 0;
            // 
            // textBox_ServerIp
            // 
            this.textBox_ServerIp.Location = new System.Drawing.Point(113, 120);
            this.textBox_ServerIp.Name = "textBox_ServerIp";
            this.textBox_ServerIp.Size = new System.Drawing.Size(172, 21);
            this.textBox_ServerIp.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "服务器地址";
            // 
            // FormClient
            // 
            this.AcceptButton = this.button_Send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 462);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormClient";
            this.Text = "异步TCP聊天客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClient_FormClosing);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.TextBox textBox_Send;
        private System.Windows.Forms.TextBox textBox_Receive;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Stop_Listen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_Status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ServerIp;
    }
}

