using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace WindowsFormsApplication1
{
    class EditPanel : Panel
    {
        #region Constants
        private const int WIDTH = 464;
        private const int HEIGHT = 136;
        #endregion

        #region Private Variables
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Label Tag_detected_label;
        private System.Windows.Forms.Button AddTagButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.MaskedTextBox YearMTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox GenreNumMTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox TrackNumMTB;
        private System.Windows.Forms.TextBox AlbumTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ArtistTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox ClosePictureBox;

        private form1 _masterform;
        private CompletedDownload _completedDownload;
        #endregion

        #region Constructors
        public EditPanel()
        {
            InitializeComponent();
            _masterform = (form1)this.Parent;
        }

        public EditPanel(form1 masterform, CompletedDownload completedDownload, Point Location)
        {
            _completedDownload = completedDownload;
            _masterform = masterform;
            this.Location = Location;
            InitializeComponent();
        }
        #endregion

        #region get/set

        private CompletedDownload CompletedDownload
        {
            get { return _completedDownload; }
            set 
            {
                if (File.Exists(value.FullPath))
                {
                    _completedDownload = value;
                    this.Fill();
                }
                else
                {
                    value.FileNoLongerExist();
                }
            }
        }


        #endregion

        #region Public Methods

        
        #endregion

        #region Private Methods
        private void Fill()
        {

        }

        #endregion

        #region Events
        private void AddTagButton_Click(object sender, EventArgs e)
        {

        }

        private void ClosePictureBox_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.Tag_detected_label = new System.Windows.Forms.Label();
            this.AddTagButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.YearMTB = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GenreNumMTB = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TrackNumMTB = new System.Windows.Forms.MaskedTextBox();
            this.AlbumTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ArtistTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ClosePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(71, 8);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(358, 20);
            this.FileNameTextBox.TabIndex = 0;
            // 
            // Tag_detected_label
            // 
            this.Tag_detected_label.AutoSize = true;
            this.Tag_detected_label.Location = new System.Drawing.Point(300, 112);
            this.Tag_detected_label.Name = "Tag_detected_label";
            this.Tag_detected_label.Size = new System.Drawing.Size(58, 13);
            this.Tag_detected_label.TabIndex = 35;
            this.Tag_detected_label.Text = "tag_detect";
            // 
            // AddTagButton
            // 
            this.AddTagButton.Enabled = false;
            this.AddTagButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddTagButton.ForeColor = System.Drawing.Color.Red;
            this.AddTagButton.Location = new System.Drawing.Point(377, 36);
            this.AddTagButton.Name = "AddTagButton";
            this.AddTagButton.Size = new System.Drawing.Size(76, 70);
            this.AddTagButton.TabIndex = 34;
            this.AddTagButton.Text = "Add Tag";
            this.AddTagButton.UseVisualStyleBackColor = true;
            this.AddTagButton.Click += new System.EventHandler(this.AddTagButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Comment";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(65, 109);
            this.CommentTextBox.MaxLength = 28;
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(204, 20);
            this.CommentTextBox.TabIndex = 32;
            // 
            // YearMTB
            // 
            this.YearMTB.Location = new System.Drawing.Point(323, 84);
            this.YearMTB.Mask = "0000";
            this.YearMTB.Name = "YearMTB";
            this.YearMTB.Size = new System.Drawing.Size(35, 20);
            this.YearMTB.TabIndex = 31;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(279, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Year";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(279, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Genre #";
            // 
            // GenreNumMTB
            // 
            this.GenreNumMTB.Location = new System.Drawing.Point(330, 59);
            this.GenreNumMTB.Mask = "000";
            this.GenreNumMTB.Name = "GenreNumMTB";
            this.GenreNumMTB.ResetOnSpace = false;
            this.GenreNumMTB.Size = new System.Drawing.Size(28, 20);
            this.GenreNumMTB.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(279, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Track #";
            // 
            // TrackNumMTB
            // 
            this.TrackNumMTB.Location = new System.Drawing.Point(330, 34);
            this.TrackNumMTB.Mask = "000";
            this.TrackNumMTB.Name = "TrackNumMTB";
            this.TrackNumMTB.Size = new System.Drawing.Size(28, 20);
            this.TrackNumMTB.TabIndex = 26;
            // 
            // AlbumTextBox
            // 
            this.AlbumTextBox.Location = new System.Drawing.Point(53, 84);
            this.AlbumTextBox.MaxLength = 30;
            this.AlbumTextBox.Name = "AlbumTextBox";
            this.AlbumTextBox.Size = new System.Drawing.Size(216, 20);
            this.AlbumTextBox.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Album";
            // 
            // ArtistTextBox
            // 
            this.ArtistTextBox.Location = new System.Drawing.Point(53, 59);
            this.ArtistTextBox.MaxLength = 30;
            this.ArtistTextBox.Name = "ArtistTextBox";
            this.ArtistTextBox.Size = new System.Drawing.Size(216, 20);
            this.ArtistTextBox.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Artist";
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(53, 34);
            this.TitleTextBox.MaxLength = 30;
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(216, 20);
            this.TitleTextBox.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Title";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "File Name";
            // 
            // ClosePictureBox
            // 
            this.ClosePictureBox.Image = Properties.Resources.fancy_close;
            this.ClosePictureBox.Location = new System.Drawing.Point(434, 0);
            this.ClosePictureBox.Name = "pictureBox1";
            this.ClosePictureBox.Size = new System.Drawing.Size(30, 30);
            this.ClosePictureBox.TabIndex = 37;
            this.ClosePictureBox.TabStop = false;
            this.ClosePictureBox.Click += new System.EventHandler(this.ClosePictureBox_Click);
            // 
            // TagForm
            // 
            this.MinimumSize = new System.Drawing.Size(WIDTH, HEIGHT);
            this.MaximumSize = new System.Drawing.Size(WIDTH, HEIGHT);
            this.BackColor = System.Drawing.Color.Black;
            this.Size = new System.Drawing.Size(WIDTH, HEIGHT);
            this.Controls.Add(this.ClosePictureBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Tag_detected_label);
            this.Controls.Add(this.AddTagButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.YearMTB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GenreNumMTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TrackNumMTB);
            this.Controls.Add(this.AlbumTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ArtistTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameTextBox);
            this.ForeColor = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.ClosePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
