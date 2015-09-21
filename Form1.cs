//Errors: 
//when downloading playlists and max raidrops is set to 3, 4 will download
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

        Point ProgressFormLocation;

        private int formHeight = DEFAULT_HEIGHT;
        private Point CompletedDownloadLocation;
        private List<DownloadProgress> Downloads = new List<DownloadProgress>(3);
        private List<string> YoutubeVideoID = new List<string>(5);
        private List<string> _queue = new List<string>(3);
        private int _maxRainDrops;
        private Size _size;
        private MediaHandler mediaHandler;

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
            foreach (DownloadProgress downloadProgressPanel in Downloads)
            {
                downloadProgressPanel.exit();
            }
            saveSettings();
            
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
            Downloads.Add(new DownloadProgress(this, BucketPathTB.Text, url, ProgressFormLocation));
            ProgressFormLocation.Y += Y_DISTANCE_BETWEEN_WINDOWS;
        }

        public void displayMessage(string message)
        {
            ResultRTB.Text += message + "\r\n";
        }

        public void DownloadPanelClose(DownloadProgress downloadProgressPanel)
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

        public void DownloadComplete(DownloadProgress downloadProgressPanel)
        {
            DownloadPanelClose(downloadProgressPanel);
            if (formHeight == DEFAULT_HEIGHT)
            {
                formHeight += COMPLETED_HEADER_HEIGHT;
            }
            formHeight += CompletedDownload.HEIGHT;
            ExpandFormHeight();

            Completed.Add(new CompletedDownload(this, downloadProgressPanel.Bucket, downloadProgressPanel.FileName, CompletedDownloadLocation));
            CompletedDownloadLocation.Y += CompletedDownload.HEIGHT;
        }

        private void organizeDownloadForms()
        {
            ProgressFormLocation = PROGRESS_FORM_LOCATION;
            foreach (DownloadProgress downloadProgressPanel in Downloads)
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
            //mediaPanel1.Play(toPlay);
            mediaHandler.Play(toPlay);
        }
        public void Pause(CompletedDownload toPlay)
        {
            //mediaPanel1.Pause(toPlay);
            mediaHandler.Pause();
        }
        

        #endregion

    }
}
