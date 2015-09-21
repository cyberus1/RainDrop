using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace WindowsFormsApplication1
{
    public class CompletedDownload
    {
        #region Public Constants

        public const int HEIGHT = 22;

        public static readonly Color COLOR_NOT_EDITED = Color.Red;
        public static readonly Color COLOR_AUTO_TAGGED = Color.Yellow;
        public static readonly Color COLOR_AUTO_RENAMED = Color.Blue;
        public static readonly Color COLOR_AUTO_TAGGED_AND_RENAMED = Color.Green;
        public static readonly Color COLOR_MANUALLY_EDITED = Color.White;

        public enum EditedStatuses
        {
            NotEdited,
            AutoTagged,
            AutoRenamed,
            AutoTaggedAndAutoRenamed,
            ManuallyEdited,
        };

        #endregion

        #region private variables

        private Panel MainPanel;
        private Button EditButton;
        private PictureBox PlayPausePictureBox;
        private Label FileNameLabel;
        private Label LineLabel;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem PlayPauseToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem autoRenameToolStripMenuItem;
        private ToolStripMenuItem autoTagToolStripMenuItem;
        private ToolStripMenuItem removeOptionsToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem removeAndDeleteToolStripMenuItem;
        private System.ComponentModel.IContainer components = null;

        private string _bucket;
        private string _fileName;
        private form1 _masterform;
        private Point _location;
        private bool _playing = false;
        private EditedStatuses _editedStatus = EditedStatuses.NotEdited;
        

        #endregion

        #region Constuctors

        public CompletedDownload(form1 masterform, string bucket, string fileName, Point location)
        {
            _masterform = masterform;
            _bucket = bucket;
            _fileName = fileName;
            _location = location;

            InitializeComponent();
            EditedStatus = EditedStatuses.NotEdited;
        }

        public CompletedDownload(form1 masterform, string bucket, string fileName, Point location, EditedStatuses editedStatus)
        {
            _masterform = masterform;
            _bucket = bucket;
            _fileName = fileName;
            _location = location;
            

            InitializeComponent();
            EditedStatus = editedStatus;
        }

        #endregion

        #region Get/Set

        public string FullPath
        {
            get { return Path.Combine(_bucket, _fileName); }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Directory
        {
            get { return _bucket; }
        }

        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                MainPanel.Location = value;
            }
        }

        public EditedStatuses EditedStatus
        {
            get { return _editedStatus; }
            protected set
            {
                _editedStatus = value;
                switch (value)
                {
                    case EditedStatuses.NotEdited:
                        ChangeColor(COLOR_NOT_EDITED);
                        break;
                    case EditedStatuses.AutoTagged:
                        ChangeColor(COLOR_AUTO_TAGGED);
                        break;
                    case EditedStatuses.AutoRenamed:
                        ChangeColor(COLOR_AUTO_RENAMED);
                        break;
                    case EditedStatuses.AutoTaggedAndAutoRenamed:
                        ChangeColor(COLOR_AUTO_TAGGED_AND_RENAMED);
                        break;
                    case EditedStatuses.ManuallyEdited:
                        ChangeColor(COLOR_MANUALLY_EDITED);
                        break;
                }
            }
        }

        #endregion

        #region public methods

        public void FileNoLongerExist()
        {

        }

        public void Stop()
        {
            _playing = false;
            PlayPausePictureBox.Image = Properties.Resources.button_play;
        }
        public void Play()
        {
            _playing = true;
            PlayPausePictureBox.Image = Properties.Resources.button_pause;
        }


        public void Dispose()
        {
            MainPanel.Dispose();
        }

        #endregion

        #region Events


        private void PlayPausePictureBox_Click(object sender, EventArgs e)
        {
            PlayPause();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void PlayPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayPause();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void autoRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoRename();
        }

        private void autoTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoTag();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dispose();
        }

        private void removeAndDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        #endregion

        #region Helpers

        private void PlayPause()
        {
            if (!_playing)
            {
                _masterform.Play(this);
            }
            else
            {
                _masterform.Pause(this);
                //Stop();
            }
        }

        private void Edit()
        {

        }

        private void AutoRename()
        {

        }

        private void AutoTag()
        {

        }

        

        private void ChangeColor(Color color)
        {
            EditButton.ForeColor = color;
        }

        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainPanel = new System.Windows.Forms.Panel();
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
            this.MainPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).BeginInit();
            // 
            // panel1
            // 
            this.MainPanel.BackColor = System.Drawing.Color.Black;
            this.MainPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.MainPanel.Controls.Add(this.EditButton);
            this.MainPanel.Controls.Add(this.PlayPausePictureBox);
            this.MainPanel.Controls.Add(this.FileNameLabel);
            this.MainPanel.Controls.Add(this.LineLabel);
            this.MainPanel.ForeColor = System.Drawing.Color.White;
            this.MainPanel.Location = _location;
            this.MainPanel.Name = "panel1";
            this.MainPanel.Size = new System.Drawing.Size(464, 22);
            this.MainPanel.TabIndex = 0;
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
            this.FileNameLabel.Text = _fileName;
            // 
            // LineLabel
            // 
            this.LineLabel.BackColor = System.Drawing.Color.White;
            this.LineLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LineLabel.Location = new System.Drawing.Point(3, 0);
            this.LineLabel.Name = "LineLabel";
            this.LineLabel.Size = new System.Drawing.Size(458, 1);
            this.LineLabel.TabIndex = 4;

            _masterform.Controls.Add(MainPanel);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlayPausePictureBox)).EndInit();

        }
        #endregion

    }
}
