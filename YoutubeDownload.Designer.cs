namespace WindowsFormsApplication1
{
    partial class YoutubeDownload
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InfoLabel = new System.Windows.Forms.Label();
            this.CancelPictureBox = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.CancelPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.ForeColor = System.Drawing.Color.White;
            this.InfoLabel.Location = new System.Drawing.Point(9, 3);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(86, 12);
            this.InfoLabel.TabIndex = 5;
            this.InfoLabel.Text = "Resolving Url...";
            // 
            // CancelPictureBox
            // 
            this.CancelPictureBox.Image = global::WindowsFormsApplication1.Properties.Resources.fancy_close;
            this.CancelPictureBox.Location = new System.Drawing.Point(377, 10);
            this.CancelPictureBox.Name = "CancelPictureBox";
            this.CancelPictureBox.Size = new System.Drawing.Size(30, 30);
            this.CancelPictureBox.TabIndex = 4;
            this.CancelPictureBox.TabStop = false;
            this.CancelPictureBox.Click += new System.EventHandler(this.CancelPictureBox_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(9, 18);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(362, 18);
            this.progressBar.TabIndex = 3;
            // 
            // YoutubeDownload
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.CancelPictureBox);
            this.Controls.Add(this.progressBar);
            this.MaximumSize = new System.Drawing.Size(417, 43);
            this.MinimumSize = new System.Drawing.Size(417, 43);
            this.Name = "YoutubeDownload";
            this.Size = new System.Drawing.Size(417, 43);
            ((System.ComponentModel.ISupportInitialize)(this.CancelPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.PictureBox CancelPictureBox;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
