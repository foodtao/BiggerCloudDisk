namespace BCD.WinApp
{
    partial class Main
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSetUserSina = new System.Windows.Forms.Button();
            this.tbPwdSina = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUserNameSina = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSetUserBaidu = new System.Windows.Forms.Button();
            this.tbPwdBaidu = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUserNameBaidu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSetUserKing = new System.Windows.Forms.Button();
            this.tbPwdKing = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbUserNameKing = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSetDiskPosition = new System.Windows.Forms.Button();
            this.tbDiskPostion = new System.Windows.Forms.TextBox();
            this.fbdDiskPosition = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 167);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "账户设置";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(4, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(416, 144);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSetUserSina);
            this.tabPage1.Controls.Add(this.tbPwdSina);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbUserNameSina);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(408, 118);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "新浪";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSetUserSina
            // 
            this.btnSetUserSina.Location = new System.Drawing.Point(65, 70);
            this.btnSetUserSina.Name = "btnSetUserSina";
            this.btnSetUserSina.Size = new System.Drawing.Size(130, 35);
            this.btnSetUserSina.TabIndex = 4;
            this.btnSetUserSina.Text = "设置";
            this.btnSetUserSina.UseVisualStyleBackColor = true;
            this.btnSetUserSina.Click += new System.EventHandler(this.btnSetUserSina_Click);
            // 
            // tbPwdSina
            // 
            this.tbPwdSina.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPwdSina.Location = new System.Drawing.Point(66, 40);
            this.tbPwdSina.Name = "tbPwdSina";
            this.tbPwdSina.PasswordChar = '*';
            this.tbPwdSina.Size = new System.Drawing.Size(207, 21);
            this.tbPwdSina.TabIndex = 3;
            this.tbPwdSina.Text = "**************";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密  码：";
            // 
            // tbUserNameSina
            // 
            this.tbUserNameSina.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUserNameSina.Location = new System.Drawing.Point(66, 8);
            this.tbUserNameSina.Name = "tbUserNameSina";
            this.tbUserNameSina.Size = new System.Drawing.Size(207, 21);
            this.tbUserNameSina.TabIndex = 1;
            this.tbUserNameSina.Text = "deathcoil@gmail.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSetUserBaidu);
            this.tabPage2.Controls.Add(this.tbPwdBaidu);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.tbUserNameBaidu);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(408, 118);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "百度";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSetUserBaidu
            // 
            this.btnSetUserBaidu.Location = new System.Drawing.Point(65, 70);
            this.btnSetUserBaidu.Name = "btnSetUserBaidu";
            this.btnSetUserBaidu.Size = new System.Drawing.Size(130, 35);
            this.btnSetUserBaidu.TabIndex = 9;
            this.btnSetUserBaidu.Text = "设置";
            this.btnSetUserBaidu.UseVisualStyleBackColor = true;
            this.btnSetUserBaidu.Click += new System.EventHandler(this.btnSetUserBaidu_Click);
            // 
            // tbPwdBaidu
            // 
            this.tbPwdBaidu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPwdBaidu.Location = new System.Drawing.Point(66, 40);
            this.tbPwdBaidu.Name = "tbPwdBaidu";
            this.tbPwdBaidu.PasswordChar = '*';
            this.tbPwdBaidu.Size = new System.Drawing.Size(207, 21);
            this.tbPwdBaidu.TabIndex = 8;
            this.tbPwdBaidu.Text = "********";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "密  码：";
            // 
            // tbUserNameBaidu
            // 
            this.tbUserNameBaidu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUserNameBaidu.Location = new System.Drawing.Point(66, 8);
            this.tbUserNameBaidu.Name = "tbUserNameBaidu";
            this.tbUserNameBaidu.Size = new System.Drawing.Size(207, 21);
            this.tbUserNameBaidu.TabIndex = 6;
            this.tbUserNameBaidu.Text = "deathcoil@gmail.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "用户名：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSetUserKing);
            this.tabPage3.Controls.Add(this.tbPwdKing);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.tbUserNameKing);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(408, 118);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "金山";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSetUserKing
            // 
            this.btnSetUserKing.Location = new System.Drawing.Point(65, 70);
            this.btnSetUserKing.Name = "btnSetUserKing";
            this.btnSetUserKing.Size = new System.Drawing.Size(130, 35);
            this.btnSetUserKing.TabIndex = 14;
            this.btnSetUserKing.Text = "设置";
            this.btnSetUserKing.UseVisualStyleBackColor = true;
            this.btnSetUserKing.Click += new System.EventHandler(this.btnSetUserKing_Click);
            // 
            // tbPwdKing
            // 
            this.tbPwdKing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPwdKing.Location = new System.Drawing.Point(66, 40);
            this.tbPwdKing.Name = "tbPwdKing";
            this.tbPwdKing.PasswordChar = '*';
            this.tbPwdKing.Size = new System.Drawing.Size(207, 21);
            this.tbPwdKing.TabIndex = 13;
            this.tbPwdKing.Text = "***********";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "密  码：";
            // 
            // tbUserNameKing
            // 
            this.tbUserNameKing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUserNameKing.Location = new System.Drawing.Point(66, 8);
            this.tbUserNameKing.Name = "tbUserNameKing";
            this.tbUserNameKing.Size = new System.Drawing.Size(207, 21);
            this.tbUserNameKing.TabIndex = 11;
            this.tbUserNameKing.Text = "cuishengbin0212@163.com";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "用户名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnSetDiskPosition);
            this.groupBox2.Controls.Add(this.tbDiskPostion);
            this.groupBox2.Location = new System.Drawing.Point(7, 178);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 97);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "同步位置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "本地文件夹位置：";
            // 
            // btnSetDiskPosition
            // 
            this.btnSetDiskPosition.Location = new System.Drawing.Point(328, 27);
            this.btnSetDiskPosition.Name = "btnSetDiskPosition";
            this.btnSetDiskPosition.Size = new System.Drawing.Size(76, 21);
            this.btnSetDiskPosition.TabIndex = 1;
            this.btnSetDiskPosition.Text = "设置";
            this.btnSetDiskPosition.UseVisualStyleBackColor = true;
            this.btnSetDiskPosition.Click += new System.EventHandler(this.btnSetDiskPosition_Click);
            // 
            // tbDiskPostion
            // 
            this.tbDiskPostion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDiskPostion.Location = new System.Drawing.Point(113, 27);
            this.tbDiskPostion.Name = "tbDiskPostion";
            this.tbDiskPostion.ReadOnly = true;
            this.tbDiskPostion.Size = new System.Drawing.Size(204, 21);
            this.tbDiskPostion.TabIndex = 0;
            this.tbDiskPostion.Click += new System.EventHandler(this.tbDiskPostion_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 282);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "超云盘设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUserNameSina;
        private System.Windows.Forms.TextBox tbPwdSina;
        private System.Windows.Forms.Button btnSetUserSina;
        private System.Windows.Forms.Button btnSetUserBaidu;
        private System.Windows.Forms.TextBox tbPwdBaidu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUserNameBaidu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetUserKing;
        private System.Windows.Forms.TextBox tbPwdKing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbUserNameKing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSetDiskPosition;
        private System.Windows.Forms.TextBox tbDiskPostion;
        private System.Windows.Forms.FolderBrowserDialog fbdDiskPosition;
        private System.Windows.Forms.Label label7;

    }
}