//Errors: 
//
//
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using WMPLib;
//using Cyberus.FormComponents;

namespace WindowsFormsApplication1
{
    public partial class form1 : Form
    {
        #region Constants
        private static readonly Point YOUTUBE_DOWNLOAD_CONTROL_LOCATION = new Point(490, 10);
        private const int EXPANDED_WIDTH = 950;
        private const int DEFAULT_WIDTH = 500;
        private const int DEFAULT_HEIGHT = 470;
        private static readonly Point COMPLETED_FILE_LOCATION = new Point(8, 448);
        private const int TAG_FORM_HEIGHT = 136;
        private static readonly Size DEFAULT_SIZE = new Size(500, 470);
        private const int COMPLETED_HEADER_HEIGHT = 30;
        private const int Y_DISTANCE_BETWEEN_DOWNLOAD_CONTROLS = 5;
        private const int MAXIMUM_ALLOWED_RAINDROPS = 10;

        //private Size EXPANDED_SIZE = new Size(950, 438);
        #endregion

        #region Private Variables
        private int formHeight = DEFAULT_HEIGHT;
        private Point CompletedDownloadLocation;
        private List<string> YoutubeVideoID = new List<string>(5);
        private Size _size;
        private MediaHandler mediaHandler;
        private DownloadsHandler downloadsHandler;
        #endregion

        #region Public Variables
        public List<MusicFileControl> Completed = new List<MusicFileControl>(3);
        #endregion

        #region Constructor
        public form1()
        {
            InitializeComponent();
            KeyPreview = true;
            
            CompletedDownloadLocation = COMPLETED_FILE_LOCATION;
            Size = DEFAULT_SIZE;
            
            mediaHandler = new MediaHandler(this);
            downloadsHandler = new DownloadsHandler(this, YOUTUBE_DOWNLOAD_CONTROL_LOCATION, Y_DISTANCE_BETWEEN_DOWNLOAD_CONTROLS);
            downloadsHandler.LastDownloadCompletedEvent += new DownloadsHandler.simpleEvent(downloadsHandler_LastDownloadCompletedEvent);
            downloadsHandler.MessageEvent += new DownloadsHandler.messageEvent(downloadsHandler_MessageEvent);
            downloadsHandler.YoutubeDownloadComplete += new DownloadsHandler.YoutubeDownloadCompleteEvent(downloadsHandler_DownloadComplete);
            downloadsHandler.NumberOfSimultaneousDownloadsAllowed = 3;

            loadSettings();


        }
        #endregion

        #region Events

        #region DownloadsHandler Events
        private void downloadsHandler_DownloadComplete(object sender, YoutubeDownloadCompleteEventArgs e)
        {
            downloadsHandler_DownloadCompleteCallBack(e);
        }
        private void downloadsHandler_MessageEvent(object sender, MessageEventArgs e)
        {
            downloadsHandler_MessageEventCallBack(e); //I feel like i can remove somehow
        }

        private void downloadsHandler_LastDownloadCompletedEvent(object sender)
        {
            downloadsHandler_LastDownloadCompletedEventCallBack();
        }
        #endregion

        #region DonwloadsHandler Event Callbacks

        private delegate void MessageEventCallBack(MessageEventArgs e);
        private void downloadsHandler_MessageEventCallBack(MessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                MessageEventCallBack d = new MessageEventCallBack(downloadsHandler_MessageEventCallBack);
                this.Invoke(d, new object[] { e }); //error found object form1 disposed
            }
            else
            {
                displayMessage(e.Message);
            }
        }
        private delegate void DownloadCompleteCallBack(YoutubeDownloadCompleteEventArgs e);
        private void downloadsHandler_DownloadCompleteCallBack(YoutubeDownloadCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                DownloadCompleteCallBack d = new DownloadCompleteCallBack(downloadsHandler_DownloadCompleteCallBack);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                DownloadComplete(e.Directory, e.FileName);
            }
        }
        private delegate void LastDownloadCompletedEventCallBack();
        private void downloadsHandler_LastDownloadCompletedEventCallBack()
        {
            if (this.InvokeRequired)
            {
                LastDownloadCompletedEventCallBack d = new LastDownloadCompletedEventCallBack(downloadsHandler_LastDownloadCompletedEventCallBack);
                this.Invoke(d, new object[] { });
            }
            else
            {
                srinkForm();
            }
        }
        
        #endregion

        #region MainForm Events
        private void RainButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(BucketPathTB.Text))
            {
                MessageBox.Show("Please place a RainBucket", "RainBucket Required");
                return;
            }
            if (InputTextBox.Text.Trim() == "")
            {
                MessageBox.Show("No valid RainDrops found...", "Paste Links First Noob :D");
                return;
            }
            expandForm();

            foreach (string line in InputTextBox.Lines)
            {
                downloadsHandler.AddOrQueueDownload(line, BucketPathTB.Text);
            }
        }
        
        private void PlaceBucketButton_Click(object sender, EventArgs e)
        {
            BucketFolderBrowser.ShowDialog();
            if (Directory.Exists(BucketFolderBrowser.SelectedPath))
            {
                BucketPathTB.Text = BucketFolderBrowser.SelectedPath;
            }
        }

        private void HelpAboutButton_Click(object sender, EventArgs e)
        {
            HelpAboutForm HelpForm = new HelpAboutForm();
            HelpForm.Show();
        }

        private void MaxRainDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            downloadsHandler.NumberOfSimultaneousDownloadsAllowed = MaxRainDropBox.SelectedIndex + 1;
        }

        private void BucketPathTB_DoubleClick(object sender, EventArgs e)
        {
            if (Directory.Exists(BucketPathTB.Text))
                Process.Start(BucketPathTB.Text);
        }

        private void ResultRTB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            downloadsHandler.CloseAll();
            saveSettings();
        }
        #endregion

        private void form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (this.InputTextBox.ContainsFocus == false)
                {
                    mediaHandler.PlayPause();
                }
            }
        }
        
        #endregion

        #region Settings

        private void loadSettings()
        {
            if (Directory.Exists(Properties.Settings.Default.Bucket))
            {
                BucketPathTB.Text = Properties.Settings.Default.Bucket;
            }
            if (Properties.Settings.Default.MaxRainDrops > 0 && Properties.Settings.Default.MaxRainDrops <= MAXIMUM_ALLOWED_RAINDROPS)
            {
                downloadsHandler.NumberOfSimultaneousDownloadsAllowed = Properties.Settings.Default.MaxRainDrops;
                try
                {
                    MaxRainDropBox.SelectedIndex = downloadsHandler.NumberOfSimultaneousDownloadsAllowed - 1;
                }
                catch (Exception) { }
            }

        }
        private void saveSettings()
        {
            Properties.Settings.Default.Bucket = BucketPathTB.Text;
            Properties.Settings.Default.MaxRainDrops = downloadsHandler.NumberOfSimultaneousDownloadsAllowed;
            Properties.Settings.Default.Save(); 
        }

        #endregion

        #region Downloads

        public void displayMessage(string message)
        {
            ResultRTB.Text += message + "\r\n";
        }

        public void DownloadComplete(string downloadDirectory, string fileName)
        {
            ExpandFormHeight();
            AddMusicFileControl(downloadDirectory, fileName);
        }

        private void AddMusicFileControl(string downloadDirectory, string filename)
        {
            MusicFileControl mf = new MusicFileControl();
            Completed.Add(mf);
            mf.setInfo(downloadDirectory, filename, CompletedDownloadLocation, MusicFileControl.EditedStatuses.NotEdited);
            this.Controls.Add(mf);
            mediaHandler.addEvents(mf);
            mf.Show();
            CompletedDownloadLocation.Y += mf.Height;
        }


        #endregion

        #region Expand/Srink Form
        private void expandForm()
        {
            _size.Height = Size.Height;
            _size.Width = EXPANDED_WIDTH;
            Size = _size;
        }
        private void srinkForm()
        {
            _size.Height = Size.Height;
            _size.Width = DEFAULT_WIDTH;
            Size = _size;
        }
        private void ExpandFormHeight()
        {
            if (formHeight == DEFAULT_HEIGHT)
            {
                formHeight += COMPLETED_HEADER_HEIGHT;
            }
            formHeight +=  MusicFileControl.HEIGHT;
            _size.Width = Size.Width;
            _size.Height = formHeight;
            Size = _size;
        }
        #endregion

        #region Media Player
        public void Play(MusicFileControl toPlay)
        {
            mediaHandler.Play(toPlay);
        }
        public void Pause(MusicFileControl toPlay)
        {
            mediaHandler.Pause();
        }
        #endregion

    }
}
