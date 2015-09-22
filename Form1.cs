﻿//Errors: 
//when downloading playlists and max raidrops is set to 3, 4 will download - seems to be fixed
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
        private static readonly Point PROGRESS_FORM_LOCATION = new Point(490, 10);
        private const int EXPANDED_WIDTH = 950;
        private const int DEFAULT_WIDTH = 500;
        private const int DEFAULT_HEIGHT = 470;
        private static readonly Point COMPLETED_FILE_LOCATION = new Point(8, 448);
        private const int TAG_FORM_HEIGHT = 136;
        private static readonly Size DEFAULT_SIZE = new Size(500, 470);
        private const int COMPLETED_HEADER_HEIGHT = 30;
        //private Size EXPANDED_SIZE = new Size(950, 438);
        private const int Y_DISTANCE_BETWEEN_WINDOWS = 48;
        private const int MAXIMUM_ALLOWED_RAINDROPS = 10;
        #endregion

        #region Private Variables
        private Point ProgressFormLocation;
        private int formHeight = DEFAULT_HEIGHT;
        private Point CompletedDownloadLocation;
        private List<YoutubeDownload> Downloads = new List<YoutubeDownload>(3);
        private List<string> YoutubeVideoID = new List<string>(5);
        private List<string> _queue = new List<string>(3);
        private int _maxRainDrops;
        private Size _size;
        private MediaHandler mediaHandler;
        #endregion

        #region Public Variables
        public List<CompletedDownload> Completed = new List<CompletedDownload>(3);
        #endregion

        public form1()
        {
            InitializeComponent();
            _maxRainDrops = Int32.Parse(MaxRainDropBox.Text);
            ProgressFormLocation = PROGRESS_FORM_LOCATION;
            CompletedDownloadLocation = COMPLETED_FILE_LOCATION;
            Size = DEFAULT_SIZE;
            loadSettings();
            mediaHandler = new MediaHandler(this);
        }
        
        #region Events

        //private void Notification(object sender, string message, YoutubeDownload.AdditionAction action);

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
                AddOrQueueRaindrop(line);
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
            _maxRainDrops = MaxRainDropBox.SelectedIndex + 1;
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
            foreach (YoutubeDownload downloadProgressPanel in Downloads)
            {
                downloadProgressPanel.exit();
            }
            saveSettings();
            
        }

        private void DownloadProgress_NotificationEvent(object sender, string message, YoutubeDownload.AdditionAction action)
        {
            DownloadProgress_NotificationEvent_Sync((YoutubeDownload)sender, message, action);
        }

        delegate void NotificationCallback(YoutubeDownload sender, string message, YoutubeDownload.AdditionAction action);

        private void DownloadProgress_NotificationEvent_Sync(YoutubeDownload sender, string message, YoutubeDownload.AdditionAction action)
        {
            if (this.InvokeRequired)
            {
                NotificationCallback d = new NotificationCallback(DownloadProgress_NotificationEvent_Sync);
                this.Invoke(d, new object[] { sender, message, action });
            }
            else
            {
                displayMessage(message);
                switch (action)
                {
                    case YoutubeDownload.AdditionAction.Close:
                        YoutubeDowloadClose(sender);
                        break;
                    case YoutubeDownload.AdditionAction.Complete:
                        DownloadComplete(sender);
                        break;
                    case YoutubeDownload.AdditionAction.Exception:
                        YoutubeDowloadClose(sender);
                        break;
                    case YoutubeDownload.AdditionAction.None:
                        break;
                    default:
                        throw new Exception("DownloadProgress_NotificationEvent action not recognized");
                }
            }
        }

        private void DownloadProgress_VideoFromPlaylistUrlFound(object sender, string url)
        {
            DownloadProgress_VideoFromPlaylistUrlFound_Sync(url);
        }

        delegate void Callback(string url);
        private void DownloadProgress_VideoFromPlaylistUrlFound_Sync(string url)
        {
            if (this.InvokeRequired)
            {
                Callback d
                    = new Callback(DownloadProgress_VideoFromPlaylistUrlFound_Sync);
                this.Invoke(d, new object[] { url });
            }
            else
            {
                AddOrQueueRaindrop(url);
            }
        }


        private void form1_Resize(object sender, EventArgs e)
        {
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
                _maxRainDrops = Properties.Settings.Default.MaxRainDrops;
                try
                {
                    MaxRainDropBox.SelectedIndex = _maxRainDrops - 1;
                }
                catch (Exception) { }
            }

        }
        private void saveSettings()
        {
            Properties.Settings.Default.Bucket = BucketPathTB.Text;
            Properties.Settings.Default.MaxRainDrops = _maxRainDrops;
            Properties.Settings.Default.Save(); 
        }

        #endregion

        #region For Progress Panels

        public void AddOrQueueRaindrop(string search)
        {
            string _search = search.Trim();
            if (_search != "")
            {
                if (Downloads.Count < _maxRainDrops)
                {
                    addRainDrop(_search);
                }
                else
                {
                    _queue.Add(_search);
                }
            }
        }

        private void addRainDrop(string url)
        {
            YoutubeDownload temp = new YoutubeDownload();
            temp.Location = ProgressFormLocation;
            temp.Start(BucketPathTB.Text, url);
            temp.NotificationEvent +=new YoutubeDownload.InfoEvent(DownloadProgress_NotificationEvent);
            temp.VideoFromPlaylistUrlFound += new YoutubeDownload.foundUrlEvent(DownloadProgress_VideoFromPlaylistUrlFound);
            this.Controls.Add(temp); 
            Downloads.Add(temp);
            
            ProgressFormLocation.Y += Y_DISTANCE_BETWEEN_WINDOWS;
        }

        public void displayMessage(string message)
        {
            ResultRTB.Text += message + "\r\n";
        }

        public void YoutubeDowloadClose(YoutubeDownload downloadProgressPanel)
        {
            if (downloadProgressPanel != null)
            {
                Downloads.Remove(downloadProgressPanel);
                downloadProgressPanel.exit();
                ProgressFormLocation.Y -= Y_DISTANCE_BETWEEN_WINDOWS;
                if (_queue.Count != 0)
                {
                    addRainDrop(_queue.First());
                    _queue.RemoveAt(0);

                }
                if (Downloads.Count == 0)
                    srinkForm();
                else
                    organizeDownloadForms();
            }
        }

        public void DownloadComplete(YoutubeDownload downloadProgressPanel)
        {
            YoutubeDowloadClose(downloadProgressPanel);
            if (formHeight == DEFAULT_HEIGHT)
            {
                formHeight += COMPLETED_HEADER_HEIGHT;
            }
            formHeight += CompletedDownload.HEIGHT;
            ExpandFormHeight();

            Completed.Add(new CompletedDownload(this, downloadProgressPanel.DownloadDirectory, downloadProgressPanel.FileName, CompletedDownloadLocation));
            CompletedDownloadLocation.Y += CompletedDownload.HEIGHT;
        }

        private void organizeDownloadForms()
        {
            ProgressFormLocation = PROGRESS_FORM_LOCATION;
            foreach (YoutubeDownload downloadProgressPanel in Downloads)
            {
                downloadProgressPanel.Location = ProgressFormLocation;
                ProgressFormLocation.Y += Y_DISTANCE_BETWEEN_WINDOWS;
            }
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
