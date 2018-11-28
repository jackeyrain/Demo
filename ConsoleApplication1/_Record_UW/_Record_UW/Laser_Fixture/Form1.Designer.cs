namespace Laser_Fixture
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPLC = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblNG = new System.Windows.Forms.Label();
            this.lblOK = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbFix = new System.Windows.Forms.Label();
            this.lbBC = new System.Windows.Forms.Label();
            this.timerPLC = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.lblPLC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(459, 44);
            this.panel1.TabIndex = 0;
            // 
            // lblPLC
            // 
            this.lblPLC.AutoSize = true;
            this.lblPLC.Location = new System.Drawing.Point(12, 9);
            this.lblPLC.Name = "lblPLC";
            this.lblPLC.Size = new System.Drawing.Size(46, 17);
            this.lblPLC.TabIndex = 0;
            this.lblPLC.Text = "label1";
            this.lblPLC.Click += new System.EventHandler(this.lblPLC_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.lblNG);
            this.panel2.Controls.Add(this.lblOK);
            this.panel2.Controls.Add(this.lblCode);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.lbFix);
            this.panel2.Controls.Add(this.lbBC);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(459, 351);
            this.panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 140);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(435, 208);
            this.textBox1.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(394, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 49;
            this.label1.Text = "label1";
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(15, 86);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(425, 51);
            this.label19.TabIndex = 48;
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNG
            // 
            this.lblNG.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblNG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNG.Location = new System.Drawing.Point(166, 46);
            this.lblNG.Name = "lblNG";
            this.lblNG.Size = new System.Drawing.Size(89, 24);
            this.lblNG.TabIndex = 47;
            this.lblNG.Text = "NG";
            this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOK
            // 
            this.lblOK.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOK.Location = new System.Drawing.Point(16, 46);
            this.lblOK.Name = "lblOK";
            this.lblOK.Size = new System.Drawing.Size(129, 24);
            this.lblOK.TabIndex = 46;
            this.lblOK.Text = "OK";
            this.lblOK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.Location = new System.Drawing.Point(296, 46);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(144, 28);
            this.lblCode.TabIndex = 44;
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCode.Click += new System.EventHandler(this.lblCode_Click);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(119, 230);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(296, 28);
            this.label15.TabIndex = 28;
            this.label15.Text = "STV1 - #10  pink ";
            // 
            // lbFix
            // 
            this.lbFix.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbFix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFix.Location = new System.Drawing.Point(296, 5);
            this.lbFix.Name = "lbFix";
            this.lbFix.Size = new System.Drawing.Size(144, 28);
            this.lbFix.TabIndex = 17;
            this.lbFix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbFix.Click += new System.EventHandler(this.lbFix_Click);
            // 
            // lbBC
            // 
            this.lbBC.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbBC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBC.Location = new System.Drawing.Point(16, 5);
            this.lbBC.Name = "lbBC";
            this.lbBC.Size = new System.Drawing.Size(239, 28);
            this.lbBC.TabIndex = 5;
            this.lbBC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbBC.Click += new System.EventHandler(this.Sensor1_Click);
            this.lbBC.DoubleClick += new System.EventHandler(this.Sensor1_DoubleClick);
            // 
            // timerPLC
            // 
            this.timerPLC.Tick += new System.EventHandler(this.timerPLC_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(358, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 395);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "数据采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPLC;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timerPLC;
        private System.Windows.Forms.Label lbBC;
        private System.Windows.Forms.Label lbFix;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblNG;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblOK;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

