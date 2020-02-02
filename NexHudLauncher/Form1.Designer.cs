namespace NexHudLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.autoLaunch = new System.Windows.Forms.CheckBox();
            this.engineMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EliteCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSCmenu = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSCback = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSCselect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSCup = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSCdown = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSCleft = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSCright = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.menuMode = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "NexHud is running";
            this.notifyIcon.BalloonTipTitle = "NexHud Launcher";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "NexHud";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.e_MouseDoubleClick);
            // 
            // autoLaunch
            // 
            this.autoLaunch.AutoSize = true;
            this.autoLaunch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.autoLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoLaunch.Location = new System.Drawing.Point(73, 131);
            this.autoLaunch.Name = "autoLaunch";
            this.autoLaunch.Size = new System.Drawing.Size(324, 35);
            this.autoLaunch.TabIndex = 0;
            this.autoLaunch.Text = "Auto-launch with Elite";
            this.autoLaunch.UseVisualStyleBackColor = true;
            this.autoLaunch.CheckedChanged += new System.EventHandler(this.autoLaunch_CheckedChanged);
            this.autoLaunch.Click += new System.EventHandler(this.autoLaunch_Click);
            // 
            // engineMode
            // 
            this.engineMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.engineMode.DropDownWidth = 200;
            this.engineMode.FormattingEnabled = true;
            this.engineMode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.engineMode.Items.AddRange(new object[] {
            "Auto (default)",
            "VR",
            "Classic"});
            this.engineMode.Location = new System.Drawing.Point(272, 87);
            this.engineMode.Name = "engineMode";
            this.engineMode.Size = new System.Drawing.Size(200, 33);
            this.engineMode.TabIndex = 1;
            this.engineMode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(67, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "NexHud Mode:";
            // 
            // EliteCheckTimer
            // 
            this.EliteCheckTimer.Interval = 5000;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSCmenu, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSCback, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSCselect, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSCup, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSCdown, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnSCleft, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnSCright, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(578, 74);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(518, 356);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Menu :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCmenu
            // 
            this.btnSCmenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCmenu.Location = new System.Drawing.Point(212, 6);
            this.btnSCmenu.Name = "btnSCmenu";
            this.btnSCmenu.Size = new System.Drawing.Size(300, 40);
            this.btnSCmenu.TabIndex = 1;
            this.btnSCmenu.Text = "( click to assign )";
            this.btnSCmenu.UseVisualStyleBackColor = true;
            this.btnSCmenu.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 40);
            this.label3.TabIndex = 2;
            this.label3.Text = "Back :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCback
            // 
            this.btnSCback.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCback.Location = new System.Drawing.Point(212, 56);
            this.btnSCback.Name = "btnSCback";
            this.btnSCback.Size = new System.Drawing.Size(300, 40);
            this.btnSCback.TabIndex = 3;
            this.btnSCback.Text = "( click to assign )";
            this.btnSCback.UseVisualStyleBackColor = true;
            this.btnSCback.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 40);
            this.label4.TabIndex = 4;
            this.label4.Text = "Select :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCselect
            // 
            this.btnSCselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCselect.Location = new System.Drawing.Point(212, 106);
            this.btnSCselect.Name = "btnSCselect";
            this.btnSCselect.Size = new System.Drawing.Size(300, 40);
            this.btnSCselect.TabIndex = 5;
            this.btnSCselect.Text = "( click to assign )";
            this.btnSCselect.UseVisualStyleBackColor = true;
            this.btnSCselect.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(200, 40);
            this.label5.TabIndex = 6;
            this.label5.Text = "Up :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnSCup
            // 
            this.btnSCup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCup.Location = new System.Drawing.Point(212, 156);
            this.btnSCup.Name = "btnSCup";
            this.btnSCup.Size = new System.Drawing.Size(300, 40);
            this.btnSCup.TabIndex = 7;
            this.btnSCup.Text = "( click to assign )";
            this.btnSCup.UseVisualStyleBackColor = true;
            this.btnSCup.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 40);
            this.label6.TabIndex = 8;
            this.label6.Text = "Down :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCdown
            // 
            this.btnSCdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCdown.Location = new System.Drawing.Point(212, 206);
            this.btnSCdown.Name = "btnSCdown";
            this.btnSCdown.Size = new System.Drawing.Size(300, 40);
            this.btnSCdown.TabIndex = 9;
            this.btnSCdown.Text = "( click to assign )";
            this.btnSCdown.UseVisualStyleBackColor = true;
            this.btnSCdown.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(200, 40);
            this.label7.TabIndex = 10;
            this.label7.Text = "Left :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCleft
            // 
            this.btnSCleft.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCleft.Location = new System.Drawing.Point(212, 256);
            this.btnSCleft.Name = "btnSCleft";
            this.btnSCleft.Size = new System.Drawing.Size(300, 40);
            this.btnSCleft.TabIndex = 11;
            this.btnSCleft.Text = "( click to assign )";
            this.btnSCleft.UseVisualStyleBackColor = true;
            this.btnSCleft.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 303);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(200, 40);
            this.label8.TabIndex = 12;
            this.label8.Text = "Right :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSCright
            // 
            this.btnSCright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCright.Location = new System.Drawing.Point(212, 306);
            this.btnSCright.Name = "btnSCright";
            this.btnSCright.Size = new System.Drawing.Size(300, 40);
            this.btnSCright.TabIndex = 13;
            this.btnSCright.Text = "( click to assign )";
            this.btnSCright.UseVisualStyleBackColor = true;
            this.btnSCright.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(584, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(181, 31);
            this.label10.TabIndex = 4;
            this.label10.Text = "Keybindings ";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(610, 451);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(170, 31);
            this.label11.TabIndex = 6;
            this.label11.Text = "Menu mode :";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // menuMode
            // 
            this.menuMode.DisplayMember = "Press";
            this.menuMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuMode.DropDownWidth = 200;
            this.menuMode.FormattingEnabled = true;
            this.menuMode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.menuMode.Items.AddRange(new object[] {
            "Hold",
            "Press"});
            this.menuMode.Location = new System.Drawing.Point(790, 451);
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(200, 33);
            this.menuMode.TabIndex = 5;
            this.menuMode.ValueMember = "Press";
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(73, 218);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(399, 50);
            this.button9.TabIndex = 7;
            this.button9.Text = "Open logs folder";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(73, 274);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(399, 50);
            this.button10.TabIndex = 8;
            this.button10.Text = "Open configs folder";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(73, 402);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(399, 50);
            this.button11.TabIndex = 9;
            this.button11.Text = "Force launch now";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1136, 519);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.menuMode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.engineMode);
            this.Controls.Add(this.autoLaunch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NexHud Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.CheckBox autoLaunch;
        private System.Windows.Forms.ComboBox engineMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer EliteCheckTimer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSCmenu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSCback;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSCselect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSCup;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSCdown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSCleft;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSCright;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox menuMode;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
    }
}

