//Errors: 
//all threads not stopped
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using WMPLib;

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
        private const int Y_DISTANCE_BETWEEN_DOWNLOAD_CONTROLS = 48;
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
        public List<CompletedDownload> Completed = new List<CompletedDownload>(3);
        #endregion

        #region Constructor
        public form1()
        {
            InitializeComponent();
            
            CompletedDownloadLocation = COMPLETED_FILE_LOCATION;
            Size = DEFAULT_SIZE;
            
            mediaHandler = new MediaHandler(this);
            downloadsHandler = new DownloadsHandler(this, YOUTUBE_DOWNLOAD_CONTROL_LOCATION, Y_DISTANCE_BETWEEN_DOWNLOAD_CONTROLS);
            downloadsHandler.YoutubeDownloadStarted += new DownloadsHandler.passDownloadEvent(downloadsHandler_YoutubeDownloadStarted);
            downloadsHandler.LastDownloadCompletedEvent += new DownloadsHandler.simpleEvent(downloadsHandler_LastDownloadCompletedEvent);
            downloadsHandler.NumberOfSimultaneousDownloadsAllowed = 3;

            loadSettings();
        }
        #endregion

        #region Events

        #region DownloadsHandler Events
        private void downloadsHandler_YoutubeDownloadStarted(object sender, YoutubeDownload download)
        {
            download.VideoFromPlaylistUrlFound += new YoutubeDownload.foundUrlEvent(YoutubeDownload_VideoFromPlaylistUrlFound);
            download.NotificationEvent += new YoutubeDownload.InfoEvent(YoutubeDownload_NotificationEvent);
        }

        private void downloadsHandler_LastDownloadCompletedEvent(object sender)
        {
            srinkForm();
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

        #region YoutubeDownload
        private void YoutubeDownload_NotificationEvent(object sender, string message, YoutubeDownload.AdditionAction action)
        {
            YoutubeDownload_NotificationEvent_Sync((YoutubeDownload)sender, message, action);
        }

        private void YoutubeDownload_VideoFromPlaylistUrlFound(object sender, string url)
        {
            YoutubeDownload_VideoFromPlaylistUrlFound_Sync(sender, url);
        }
        #endregion

        #region YoutubeDownload Callbacks
        delegate void NotificationCallback(YoutubeDownload sender, string message, YoutubeDownload.AdditionAction action);
        private void YoutubeDownload_NotificationEvent_Sync(YoutubeDownload sender, string message, YoutubeDownload.AdditionAction action)
        {
            if (this.InvokeRequired)
            {
                NotificationCallback d = new NotificationCallback(YoutubeDownload_NotificationEvent_Sync);
                this.Invoke(d, new object[] { sender, message, action });
            }
            else
            {
                if (message != "") 
                    displayMessage(message);
                switch (action)
                {
                    case YoutubeDownload.AdditionAction.Close:
                        downloadsHandler.YoutubeDownloadClose(sender);
                        break;
                    case YoutubeDownload.AdditionAction.Complete:
                        DownloadComplete(sender);
                        break;
                    case YoutubeDownload.AdditionAction.Exception:
                        downloadsHandler.YoutubeDownloadClose(sender);
                        break;
                    case YoutubeDownload.AdditionAction.None:
                        break;
                    default:
                        throw new Exception("DownloadProgress_NotificationEvent action not recognized");
                }
            }
        }

        delegate void Callback(object sender, string url);
        private void YoutubeDownload_VideoFromPlaylistUrlFound_Sync(object sender, string url)
        {
            if (this.InvokeRequired)
            {
                Callback d
                    = new Callback(YoutubeDownload_VideoFromPlaylistUrlFound_Sync);
                this.Invoke(d, new object[] { sender, url });
            }
            else
            {
                downloadsHandler.AddOrQueueDownload(url, BucketPathTB.Text);
            }
        }
        #endregion

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

        public void DownloadComplete(YoutubeDownload downloadProgressPanel)
        {
            downloadsHandler.YoutubeDownloadClose(downloadProgressPanel);
            
            ExpandFormHeight();

            Completed.Add(new CompletedDownload(this, downloadProgressPanel.DownloadDirectory, downloadProgressPanel.FileName, CompletedDownloadLocation));
            CompletedDownloadLocation.Y += CompletedDownload.HEIGHT;
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
            formHeight += CompletedDownload.HEIGHT;
            _size.Width = Size.Width;
            _size.Height = formHeight;
            Size = _size;
        }
        #endregion

        #region Media Player
        public void Play(CompletedDownload toPlay)
        {
            mediaHandler.Play(toPlay);
        }
        public void Pause(CompletedDownload toPlay)
        {
            mediaHandler.Pause();
        }
        

        #endregion

    }
}
