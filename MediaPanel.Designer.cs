namespace WindowsFormsApplication1
{
    partial class MediaPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.NextPictureBox = new System.Windows.Forms.PictureBox();
            this.PreviousPictureBox = new System.Windows.Forms.PictureBox();
            this.PlayPausePictureBox = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.VolumeSlider = new MB.Controls.ColorSlider();
            this.WMP_PlayNextTimer = new System.Windows.Forms.Timer(this.components);
            this.PositionTimer = new System.Windows.Forms.Timer(this.components);
            this.LocationSlider = new MB.Controls.ColorSlider();
            ((System.ComponentModel.ISupportInitialize)(this.NextPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.InfoLabel.Location = new System.Drawing.Point(12, 26);
            this.InfoLabel.MaximumSize = new System.Drawing.Size(150, 13);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 39;
            // 
            // NextPictureBox
            // 
            this.NextPictureBox.Image = global::WindowsFormsApplication1.Properties.Resources.next_image;
            this.NextPictureBox.Location = new System.Drawing.Point(262, 16);
            this.NextPictureBox.Name = "NextPictureBox";
            this.NextPictureBox.Size = new System.Drawing.Size(32, 32);
            this.NextPictureBox.TabIndex = 43;
            this.NextPictureBox.TabStop = false;
            this.NextPictureBox.Click += new System.EventHandler(this.NextPictureBox_Click);
            // 
            // PreviousPictureBox
            // 
            this.PreviousPictureBox.Image = global::WindowsFormsApplication1.Properties.Resources.previous_image;
            this.PreviousPictureBox.Location = new System.Drawing.Point(170, 16);
            this.PreviousPictureBox.Name = "PreviousPictureBox";
            this.PreviousPictureBox.Size = new System.Drawing.Size(32, 32);
            this.PreviousPictureBox.TabIndex = 42;
            this.PreviousPictureBox.TabStop = false;
            this.PreviousPictureBox.Click += new System.EventHandler(this.PreviousPictureBox_Click);
            // 
            // PlayPausePictureBox
            // 
            this.PlayPausePictureBox.Image = global::WindowsFormsApplication1.Properties.Resources.play_image;
            this.PlayPausePictureBox.Location = new System.Drawing.Point(208, 10);
            this.PlayPausePictureBox.Name = "PlayPausePictureBox";
            this.PlayPausePictureBox.Size = new System.Drawing.Size(48, 48);
            this.PlayPausePictureBox.TabIndex = 41;
            this.PlayPausePictureBox.TabStop = false;
            this.PlayPausePictureBox.Click += new System.EventHandler(this.PlayPausePictureBox_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            // 
            // VolumeSlider
            // 
            this.VolumeSlider.BackColor = System.Drawing.Color.Transparent;
            this.VolumeSlider.BarInnerColor = System.Drawing.Color.Transparent;
            this.VolumeSlider.BarOuterColor = System.Drawing.Color.LightSlateGray;
            this.VolumeSlider.BarPenColor = System.Drawing.Color.Transparent;
            this.VolumeSlider.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.VolumeSlider.ElapsedInnerColor = System.Drawing.Color.DarkSlateBlue;
            this.VolumeSlider.ElapsedOuterColor = System.Drawing.Color.SkyBlue;
            this.VolumeSlider.LargeChange = ((uint)(0u));
            this.VolumeSlider.Location = new System.Drawing.Point(314, 29);
            this.VolumeSlider.MouseEffects = false;
            this.VolumeSlider.MouseWheelBarPartitions = 100;
            this.VolumeSlider.Name = "VolumeSlider";
            this.VolumeSlider.Size = new System.Drawing.Size(131, 10);
            this.VolumeSlider.SmallChange = ((uint)(1u));
            this.VolumeSlider.TabIndex = 0;
            this.VolumeSlider.Text = "colorSlider3";
            this.VolumeSlider.ThumbInnerColor = System.Drawing.Color.Blue;
            this.VolumeSlider.ThumbOuterColor = System.Drawing.Color.Azure;
            this.VolumeSlider.ThumbPenColor = System.Drawing.Color.Transparent;
            this.VolumeSlider.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            this.VolumeSlider.ThumbSize = 10;
            this.toolTip1.SetToolTip(this.VolumeSlider, "Volume");
            this.VolumeSlider.Value = 75;
            this.VolumeSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VolumeSlider_Scroll);
            // 
            // WMP_PlayNextTimer
            // 
            this.WMP_PlayNextTimer.Tick += new System.EventHandler(this.WMP_PlayNextTimer_Tick);
            // 
            // PositionTimer
            // 
            this.PositionTimer.Interval = 1000;
            this.PositionTimer.Tick += new System.EventHandler(this.PositionTimer_Tick);
            // 
            // LocationSlider
            // 
            this.LocationSlider.BackColor = System.Drawing.Color.Transparent;
            this.LocationSlider.BarInnerColor = System.Drawing.Color.LightSteelBlue;
            this.LocationSlider.BarOuterColor = System.Drawing.Color.Transparent;
            this.LocationSlider.BarPenColor = System.Drawing.Color.Transparent;
            this.LocationSlider.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.LocationSlider.Dock = System.Windows.Forms.DockStyle.Top;
            this.LocationSlider.ElapsedInnerColor = System.Drawing.Color.DarkSlateBlue;
            this.LocationSlider.ElapsedOuterColor = System.Drawing.Color.SkyBlue;
            this.LocationSlider.LargeChange = ((uint)(0u));
            this.LocationSlider.Location = new System.Drawing.Point(0, 0);
            this.LocationSlider.Maximum = 400;
            this.LocationSlider.MouseEffects = false;
            this.LocationSlider.MouseWheelBarPartitions = 200;
            this.LocationSlider.Name = "LocationSlider";
            this.LocationSlider.Size = new System.Drawing.Size(464, 10);
            this.LocationSlider.SmallChange = ((uint)(0u));
            this.LocationSlider.TabIndex = 38;
            this.LocationSlider.Text = "colorSlider2";
            this.LocationSlider.ThumbInnerColor = System.Drawing.Color.Blue;
            this.LocationSlider.ThumbPenColor = System.Drawing.Color.Transparent;
            this.LocationSlider.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            this.LocationSlider.ThumbSize = 8;
            this.LocationSlider.Value = 0;
            this.LocationSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.LocationSlider_Scroll);
            // 
            // MediaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.NextPictureBox);
            this.Controls.Add(this.PreviousPictureBox);
            this.Controls.Add(this.PlayPausePictureBox);
            this.Controls.Add(this.VolumeSlider);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.LocationSlider);
            this.MaximumSize = new System.Drawing.Size(0, 58);
            this.MinimumSize = new System.Drawing.Size(464, 58);
            this.Name = "MediaPanel";
            this.Size = new System.Drawing.Size(464, 58);
            this.Load += new System.EventHandler(this.MediaPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NextPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox NextPictureBox;
        private System.Windows.Forms.PictureBox PreviousPictureBox;
        private System.Windows.Forms.PictureBox PlayPausePictureBox;
        private MB.Controls.ColorSlider VolumeSlider;
        private System.Windows.Forms.Label InfoLabel;
        private MB.Controls.ColorSlider LocationSlider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer WMP_PlayNextTimer;
        private System.Windows.Forms.Timer PositionTimer;
    }
}
