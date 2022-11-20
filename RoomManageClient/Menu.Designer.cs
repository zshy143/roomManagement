namespace hotleManagement
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fenfang = new System.Windows.Forms.Button();
            this.diaofang = new System.Windows.Forms.Button();
            this.roomInfo = new System.Windows.Forms.Button();
            this.tuifang = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fenfang
            // 
            this.fenfang.Location = new System.Drawing.Point(182, 111);
            this.fenfang.Name = "fenfang";
            this.fenfang.Size = new System.Drawing.Size(151, 54);
            this.fenfang.TabIndex = 0;
            this.fenfang.Text = "分房申请";
            this.fenfang.UseVisualStyleBackColor = false;
            this.fenfang.Click += new System.EventHandler(this.fenfang_Click);
            // 
            // diaofang
            // 
            this.diaofang.Location = new System.Drawing.Point(182, 298);
            this.diaofang.Name = "diaofang";
            this.diaofang.Size = new System.Drawing.Size(151, 54);
            this.diaofang.TabIndex = 1;
            this.diaofang.Text = "调房申请";
            this.diaofang.UseVisualStyleBackColor = true;
            this.diaofang.Click += new System.EventHandler(this.diaofang_Click);
            // 
            // roomInfo
            // 
            this.roomInfo.Location = new System.Drawing.Point(182, 386);
            this.roomInfo.Name = "roomInfo";
            this.roomInfo.Size = new System.Drawing.Size(151, 54);
            this.roomInfo.TabIndex = 2;
            this.roomInfo.Text = "房间查询";
            this.roomInfo.UseVisualStyleBackColor = true;
            this.roomInfo.Click += new System.EventHandler(this.roomInfo_Click);
            // 
            // tuifang
            // 
            this.tuifang.Location = new System.Drawing.Point(182, 206);
            this.tuifang.Name = "tuifang";
            this.tuifang.Size = new System.Drawing.Size(151, 54);
            this.tuifang.TabIndex = 3;
            this.tuifang.Text = "退房";
            this.tuifang.UseVisualStyleBackColor = true;
            this.tuifang.Click += new System.EventHandler(this.tuifang_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 598);
            this.Controls.Add(this.tuifang);
            this.Controls.Add(this.roomInfo);
            this.Controls.Add(this.diaofang);
            this.Controls.Add(this.fenfang);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button fenfang;
        private System.Windows.Forms.Button diaofang;
        private System.Windows.Forms.Button roomInfo;
        private System.Windows.Forms.Button tuifang;
    }
}