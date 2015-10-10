namespace WindowsFormsApplication1
{
    partial class MusicFileControl
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PlayPauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAndDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditButton = new System.Windows.Forms.Button();
            this.PlayPausePictureBox = new System.Windows.Forms.PictureBox();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.LineLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).BeginInit();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlayPauseToolStripMenuItem,
            this.editToolStripMenuItem,
            this.autoRenameToolStripMenuItem,
            this.autoTagToolStripMenuItem,
            this.removeOptionsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 136);
            // 
            // PlayPauseToolStripMenuItem
            // 
            this.PlayPauseToolStripMenuItem.Image = Properties.Resources.play_pause;
            this.PlayPauseToolStripMenuItem.Name = "PlayPauseToolStripMenuItem";
            this.PlayPauseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.PlayPauseToolStripMenuItem.Text = "Play/Pause";
            this.PlayPauseToolStripMenuItem.Click += new System.EventHandler(this.PlayPauseToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = Properties.Resources.editIcon;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // autoRenameToolStripMenuItem
            // 
            this.autoRenameToolStripMenuItem.Image = Properties.Resources.rename_icon;
            this.autoRenameToolStripMenuItem.Name = "autoRenameToolStripMenuItem";
            this.autoRenameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.autoRenameToolStripMenuItem.Text = "Auto Rename";
            this.autoRenameToolStripMenuItem.Click += new System.EventHandler(this.autoRenameToolStripMenuItem_Click);
            // 
            // autoTagToolStripMenuItem
            // 
            this.autoTagToolStripMenuItem.Image = Properties.Resources.tagIcon;
            this.autoTagToolStripMenuItem.Name = "autoTagToolStripMenuItem";
            this.autoTagToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.autoTagToolStripMenuItem.Text = "Auto Tag";
            this.autoTagToolStripMenuItem.Click += new System.EventHandler(this.autoTagToolStripMenuItem_Click);
            // 
            // removeOptionsToolStripMenuItem
            // 
            this.removeOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.removeAndDeleteToolStripMenuItem});
            this.removeOptionsToolStripMenuItem.Image = Properties.Resources.remove_icon;
            this.removeOptionsToolStripMenuItem.Name = "removeOptionsToolStripMenuItem";
            this.removeOptionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeOptionsToolStripMenuItem.Text = "Remove";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = Properties.Resources.remove_icon;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // removeAndDeleteToolStripMenuItem
            // 
            this.removeAndDeleteToolStripMenuItem.Image = Properties.Resources.deleteIcon;
            this.removeAndDeleteToolStripMenuItem.Name = "removeAndDeleteToolStripMenuItem";
            this.removeAndDeleteToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.removeAndDeleteToolStripMenuItem.Text = "Remove and Delete";
            this.removeAndDeleteToolStripMenuItem.Click += new System.EventHandler(this.removeAndDeleteToolStripMenuItem_Click);
            // 
            // EditButton
            // 
            this.EditButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditButton.ForeColor = System.Drawing.Color.Yellow;
            this.EditButton.Location = new System.Drawing.Point(402, 1);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(59, 21);
            this.EditButton.TabIndex = 7;
            this.EditButton.Tag = "";
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            // 
            // PlayPausePictureBox
            // 
            this.PlayPausePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayPausePictureBox.Image = Properties.Resources.button_play;
            this.PlayPausePictureBox.Location = new System.Drawing.Point(5, 1);
            this.PlayPausePictureBox.Name = "PlayPausePictureBox";
            this.PlayPausePictureBox.Size = new System.Drawing.Size(20, 20);
            this.PlayPausePictureBox.TabIndex = 6;
            this.PlayPausePictureBox.TabStop = false;
            this.PlayPausePictureBox.Click += new System.EventHandler(this.PlayPausePictureBox_Click);
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(35, 5);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(57, 13);
            this.FileNameLabel.TabIndex = 5;
            // 
            // LineLabel
            // 
            this.LineLabel.BackColor = System.Drawing.Color.White;
            this.LineLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LineLabel.Location = new System.Drawing.Point(3, 0);
            this.LineLabel.Name = "LineLabel";
            this.LineLabel.Size = new System.Drawing.Size(458, 1);
            this.LineLabel.TabIndex = 4;

            this.BackColor = System.Drawing.Color.Black;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.PlayPausePictureBox);
            this.Controls.Add(this.FileNameLabel);
            this.Controls.Add(this.LineLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximumSize = new System.Drawing.Size(464, 22);
            this.MinimumSize = new System.Drawing.Size(464, 22);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).EndInit();

        }

        #endregion

        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.PictureBox PlayPausePictureBox;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.Label LineLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem PlayPauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoRenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAndDeleteToolStripMenuItem;
    }
}
