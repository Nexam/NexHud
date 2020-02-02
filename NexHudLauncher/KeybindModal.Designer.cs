namespace NexHudLauncher
{
    partial class KeybindModal
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
            this.labelMods = new System.Windows.Forms.Label();
            this.labelKey = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.glControl1 = new OpenTK.GLControl();
            this.btnAgain = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelMods
            // 
            this.labelMods.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMods.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labelMods.Location = new System.Drawing.Point(32, 96);
            this.labelMods.Name = "labelMods";
            this.labelMods.Size = new System.Drawing.Size(756, 85);
            this.labelMods.TabIndex = 0;
            this.labelMods.Text = "LCtrl + LShift";
            this.labelMods.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelKey
            // 
            this.labelKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKey.ForeColor = System.Drawing.Color.OliveDrab;
            this.labelKey.Location = new System.Drawing.Point(32, 168);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(756, 85);
            this.labelKey.TabIndex = 1;
            this.labelKey.Text = "Right";
            this.labelKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(12, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(234, 51);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.BackColor = System.Drawing.SystemColors.Control;
            this.btnAccept.Enabled = false;
            this.btnAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccept.Location = new System.Drawing.Point(552, 322);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(236, 51);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = false;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTitle.Location = new System.Drawing.Point(177, 3);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(473, 85);
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "Press key(s) for \'Menu\'";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 34;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(15, 15);
            this.glControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(1, 1);
            this.glControl1.TabIndex = 5;
            this.glControl1.VSync = false;
            // 
            // btnAgain
            // 
            this.btnAgain.Enabled = false;
            this.btnAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgain.Location = new System.Drawing.Point(283, 322);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(236, 51);
            this.btnAgain.TabIndex = 6;
            this.btnAgain.Text = "Go again!";
            this.btnAgain.UseVisualStyleBackColor = true;
            this.btnAgain.Click += new System.EventHandler(this.btnAgain_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 282);
            this.progressBar1.MarqueeAnimationSpeed = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(773, 5);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 7;
            // 
            // KeybindModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 385);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnAgain);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelKey);
            this.Controls.Add(this.labelMods);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "KeybindModal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key binding";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeybindModal_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMods;
        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Timer timer1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button btnAgain;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}