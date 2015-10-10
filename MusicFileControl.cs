using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class MusicFileControl : UserControl
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

        #region Accessors
        [Browsable(true)]
        public string FullPath
        {
            get { return Path.Combine(Directory, FileName); }
            set
            {
                if(System.IO.File.Exists(value))
                {
                    Directory = System.IO.Path.GetDirectoryName(value);
                    FileName = System.IO.Path.GetFileName(value);
                }
                else
                    throw new MusicFileControlException("Set FullPath exception: File does not exist");
            }
        }

        private string __Filename;
        public string FileName
        {
            get
            {
                return __Filename;
            }
            protected set
            {
                __Filename = value;
                FileNameLabel.Text = value;
            }
        }

        private string __Directory;
        public string Directory
        {
            get
            {
                return __Directory;
            }
            protected set
            {
                if (System.IO.Directory.Exists(value))
                {
                    __Directory = value;
                }
                else
                {
                    throw new MusicFileControlException("Derectory set error: Directory does not exist");
                }
            }
        }

        [Browsable(true)]
        public string Comment
        {
            get;
            set;
        }

        #endregion

        #region Private Variables
        private bool _playing = false;
        private EditedStatuses _editedStatus = EditedStatuses.NotEdited;
        #endregion

        public MusicFileControl()
        {
            InitializeComponent();
        }

        #region Public Methods
        public void FileNoLongerExist()
        {

        }

        public void SetFullPath(string directory, string filename)
        {
            FullPath = System.IO.Path.Combine(directory, filename);
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

        #region New Events

        #region Event Definitons
        public delegate void PlayPauseClickedEvent(object sender);
        public event PlayPauseClickedEvent PlayClicked;
        public event PlayPauseClickedEvent PauseClicked;
        #endregion

        #endregion

        #region Helpers

        private void PlayPause()
        {
            if (!_playing)
            {
                if(PlayClicked != null)
                    PlayClicked(this);
            }
            else
            {
                if(PauseClicked!= null)
                    PauseClicked(this);
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
    }

    #region MusicFileControlException
    public class MusicFileControlException : Exception
    {
        public MusicFileControlException()
            : base()
        { }
        public MusicFileControlException(string message)
            : base(message)
        { }
        public MusicFileControlException(string message, Exception inner)
            : base(message, inner)
        { }
    }
    #endregion

}

