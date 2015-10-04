namespace WindowsFormsApplication1
{
    partial class form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ResultRTB = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BucketPathTB = new System.Windows.Forms.TextBox();
            this.PlaceBucketButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MaxRainDropBox = new System.Windows.Forms.ComboBox();
            this.HelpAboutButton = new System.Windows.Forms.Button();
            this.RainButton = new Cyberus.FormComponents.NoSelectButton();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BucketFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.mediaPanel1 = new WindowsFormsApplication1.MediaPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.ResultRTB);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.BucketPathTB);
            this.panel1.Controls.Add(this.PlaceBucketButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.MaxRainDropBox);
            this.panel1.Controls.Add(this.HelpAboutButton);
            this.panel1.Controls.Add(this.RainButton);
            this.panel1.Controls.Add(this.InputTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel1.Location = new System.Drawing.Point(8, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 360);
            this.panel1.TabIndex = 0;
            // 
            // ResultRTB
            // 
            this.ResultRTB.BackColor = System.Drawing.Color.Black;
            this.ResultRTB.ForeColor = System.Drawing.Color.Lime;
            this.ResultRTB.Location = new System.Drawing.Point(3, 187);
            this.ResultRTB.Name = "ResultRTB";
            this.ResultRTB.ReadOnly = true;
            this.ResultRTB.Size = new System.Drawing.Size(457, 144);
            this.ResultRTB.TabIndex = 8;
            this.ResultRTB.Text = "";
            this.ResultRTB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ResultRTB_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Location = new System.Drawing.Point(3, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Results:";
            // 
            // BucketPathTB
            // 
            this.BucketPathTB.BackColor = System.Drawing.Color.Black;
            this.BucketPathTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BucketPathTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BucketPathTB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BucketPathTB.Location = new System.Drawing.Point(95, 142);
            this.BucketPathTB.Name = "BucketPathTB";
            this.BucketPathTB.ReadOnly = true;
            this.BucketPathTB.Size = new System.Drawing.Size(365, 26);
            this.BucketPathTB.TabIndex = 6;
            this.BucketPathTB.DoubleClick += new System.EventHandler(this.BucketPathTB_DoubleClick);
            // 
            // PlaceBucketButton
            // 
            this.PlaceBucketButton.BackColor = System.Drawing.Color.Black;
            this.PlaceBucketButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlaceBucketButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PlaceBucketButton.Location = new System.Drawing.Point(3, 142);
            this.PlaceBucketButton.Name = "PlaceBucketButton";
            this.PlaceBucketButton.Size = new System.Drawing.Size(86, 25);
            this.PlaceBucketButton.TabIndex = 5;
            this.PlaceBucketButton.Text = "Place Bucket";
            this.PlaceBucketButton.UseVisualStyleBackColor = false;
            this.PlaceBucketButton.Click += new System.EventHandler(this.PlaceBucketButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(219, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maximum Simultaneous Raindrops";
            // 
            // MaxRainDropBox
            // 
            this.MaxRainDropBox.BackColor = System.Drawing.Color.Black;
            this.MaxRainDropBox.DisplayMember = "1";
            this.MaxRainDropBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaxRainDropBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.MaxRainDropBox.FormattingEnabled = true;
            this.MaxRainDropBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.MaxRainDropBox.Location = new System.Drawing.Point(423, 335);
            this.MaxRainDropBox.Name = "MaxRainDropBox";
            this.MaxRainDropBox.Size = new System.Drawing.Size(37, 21);
            this.MaxRainDropBox.TabIndex = 3;
            this.MaxRainDropBox.Text = "2";
            this.MaxRainDropBox.ValueMember = "int";
            this.MaxRainDropBox.SelectedIndexChanged += new System.EventHandler(this.MaxRainDropBox_SelectedIndexChanged);
            // 
            // HelpAboutButton
            // 
            this.HelpAboutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HelpAboutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.HelpAboutButton.Location = new System.Drawing.Point(257, 111);
            this.HelpAboutButton.Name = "HelpAboutButton";
            this.HelpAboutButton.Size = new System.Drawing.Size(203, 25);
            this.HelpAboutButton.TabIndex = 2;
            this.HelpAboutButton.Text = "Help / About";
            this.HelpAboutButton.UseVisualStyleBackColor = true;
            this.HelpAboutButton.Click += new System.EventHandler(this.HelpAboutButton_Click);
            // 
            // RainButton
            // 
            this.RainButton.BackColor = System.Drawing.Color.Black;
            this.RainButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RainButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RainButton.Location = new System.Drawing.Point(3, 111);
            this.RainButton.Name = "RainButton";
            this.RainButton.Size = new System.Drawing.Size(203, 25);
            this.RainButton.TabIndex = 1;
            this.RainButton.Text = "Make It Rain";
            this.RainButton.UseVisualStyleBackColor = false;
            this.RainButton.Click += new System.EventHandler(this.RainButton_Click);
            // 
            // InputTextBox
            // 
            this.InputTextBox.BackColor = System.Drawing.Color.Black;
            this.InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.InputTextBox.Location = new System.Drawing.Point(3, 16);
            this.InputTextBox.Multiline = true;
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputTextBox.Size = new System.Drawing.Size(457, 89);
            this.InputTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RainDrops - One Link Per Line:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.label4);
            this.panel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Location = new System.Drawing.Point(8, 432);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(464, 16);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Completed Downloads:";
            // 
            // mediaPanel1
            // 
            this.mediaPanel1.BackColor = System.Drawing.Color.Black;
            this.mediaPanel1.Location = new System.Drawing.Point(8, 0);
            this.mediaPanel1.MaximumSize = new System.Drawing.Size(0, 58);
            this.mediaPanel1.MinimumSize = new System.Drawing.Size(464, 58);
            this.mediaPanel1.Name = "mediaPanel1";
            this.mediaPanel1.Size = new System.Drawing.Size(464, 58);
            this.mediaPanel1.StoreSettings = false;
            this.mediaPanel1.TabIndex = 4;
            // 
            // form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(983, 450);
            this.Controls.Add(this.mediaPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "form1";
            this.Text = "RainDrop - Condensing the Cloud";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox MaxRainDropBox;
        private System.Windows.Forms.Button HelpAboutButton;
        private Cyberus.FormComponents.NoSelectButton RainButton;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button PlaceBucketButton;
        private System.Windows.Forms.TextBox BucketPathTB;
        private System.Windows.Forms.RichTextBox ResultRTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog BucketFolderBrowser;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        public MediaPanel mediaPanel1;
    }
}

