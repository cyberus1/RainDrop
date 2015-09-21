//Error:
//-loops if playlist url is wrong
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
//using System.Threading;
using YoutubeExtractor;
using System.Net;
using System.Threading;

namespace WindowsFormsApplication1
{
    public class DownloadProgress
    {
        #region Constants
        private const string YOUTUBE_VIDEO_URL = "http://www.youtube.com/watch?v=";
        private const string YOUTUBE_SEARCH_URL = "http://www.youtube.com/results?search_query=";
        private const string YOUTUBE_SEARCH_FIRST_STRING = "id=\"results\">";
        private const string YOUTUBE_SEARCH_SECOND_STRING = "id=\"item-section";
        private const string YOUTUBE_SEARCH_THIRD_STRING = "data-context-item-id=\"";
        #endregion

        #region Private Variables
        private Panel MainPanel;
        private Label InfoLabel;
        private PictureBox CancelPictureBox;
        private ProgressBar progressBar;

        private form1 _masterform;
        private string _bucket;
        private string _searchTerm;
        private string _fileName;
        private bool downloadComplete = true;
        private Point _location;
        private Thread downloadThread;
        #endregion

        private bool _isClosing = false;

        public DownloadProgress(form1 masterform, string bucket, string searchTerm, Point location)
        {
            _masterform = masterform;
            _bucket = bucket;
            _searchTerm = searchTerm;
            _location = location;

            InitializeComponent();
            InfoLabel.Text = "Searching for: " + _searchTerm;

            downloadThread = new Thread(new ThreadStart(download));
            downloadThread.Start();
        }

        #region Get/Set
        public string Bucket
        {
            get { return _bucket; }
        }
        public string FileName
        {
            get { return _fileName; }
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
        public bool isClosing
        {
            get { return _isClosing; }
        }
        #endregion

        #region public methods

        private void download()
        {
            try
            {
                if (handleSearch(_searchTerm))
                {
                    //_masterform.BeginInvoke(
                    //    new Action(() =>
                    //    {
                    //        _isClosing = true;
                    //        _masterform.displayMessage("Playlist Downloading");
                    //        _masterform.DownloadPanelClose(this);
                    //    }));
                    Notify("Playlist Downloading", AdditionAction.Close);
                }

                string url;
                if (_searchTerm.StartsWith("http"))
                {
                    url = _searchTerm;
                }
                else
                {
                    url = getYoutubeUrl(_searchTerm);
                }
                //_masterform.BeginInvoke(
                //   new Action(() =>
                //   {
                //       _masterform.displayMessage("Found URL: " + url);
                //   }));
                Notify("Found URL: " + url, AdditionAction.None);

                IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(url, false)
                    .Where(info => info.AudioBitrate != 0)
                    .OrderByDescending(info => info.AudioBitrate);
                int bestBitRate = videos.First().AudioBitrate;
                videos = videos
                    .Where(info => info.AudioBitrate == bestBitRate);
                
                VideoInfo video = videos.First();
                foreach (VideoInfo info in videos) // this sellects .mp3 files first and then .aac files and finaly .ogg
                {
                    if (info.AudioType == AudioType.Mp3)
                    {
                        video = info;
                    }
                    else if (info.AudioType == AudioType.Aac && (video.AudioType != AudioType.Mp3))
                    {
                        video = info;
                    }
                }
                if (video.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(video);
                }
                string videoTitle = RemoveIllegalPathCharacters(video.Title);

                ////////////////////////////////////
                switch (video.AudioType)
                {
                    case AudioType.Aac:
                        _fileName = videoTitle + ".mp4";
                        break;
                    case AudioType.Mp3:
                        _fileName = videoTitle + ".mp3";
                        break;
                    case AudioType.Vorbis:
                        MessageBox.Show("Warning: .ogg file, will need to be converted");
                        _fileName = videoTitle + ".mp3";
                        break;
                    case AudioType.Unknown:
                        throw new Exception("DownloadProgress.cs: AudioType Unknown");
                }
                //MessageBox.Show(video.VideoType.ToString());
                //MessageBox.Show(video.AudioType.ToString());
                //MessageBox.Show(video.AudioBitrate.ToString());

                if (File.Exists(Path.Combine(_bucket, _fileName)))
                {
                    //_masterform.BeginInvoke(
                    //new Action(() =>
                    //{
                    //    _isClosing = true;
                    //    _masterform.displayMessage("File Already Exist: " + _fileName);
                    //    _masterform.DownloadPanelClose(this);
                    //}));
                    Notify("File Already Exist: " + _fileName, AdditionAction.Complete); //switched to complete
                    return;
                }
                downloadComplete = false;
                var audioDownloader = new AudioDownloader(video,
                    Path.Combine(_bucket, _fileName));
                audioDownloader.DownloadProgressChanged += (sender, args) =>
                    progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        progressBar.Value = (int)(args.ProgressPercentage);
                    }));
                audioDownloader.AudioExtractionProgressChanged += (sender, args) =>
                    progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        if (args.ProgressPercentage == 0)
                        {
                            InfoLabel.BeginInvoke(
                                new Action(() =>
                                    {
                                        InfoLabel.Text = "Extracting: " + _fileName; ;
                                    }));
                        }
                        progressBar.Value = (int)(args.ProgressPercentage);
                    }));
                InfoLabel.BeginInvoke(
                    new Action(() =>
                    {
                        InfoLabel.Text = _fileName;
                    }));

                audioDownloader.Execute();
                downloadComplete = true; 

                //_masterform.BeginInvoke(
                //    new Action(() =>
                //    {
                //        _isClosing = true;
                //        _masterform.displayMessage("Completed Download of: " + _fileName);
                //        _masterform.DownloadComplete(this);
                //        return;
                //    }));
                Notify("Completed Download of: " + _fileName, AdditionAction.Complete);
            }
            catch (Exception ex)
            {
                try
                {
                    //_masterform.BeginInvoke(
                    //    new Action(() =>
                    //    {
                    //        if (!_isClosing)
                    //        {
                    //            _isClosing = true;
                    //            _masterform.displayMessage("Download Canceled: Exception Thrown: " + ex.ToString());
                    //            _masterform.DownloadPanelClose(this);
                    //        }
                    //    }));
                    Notify("Download Canceled: Exception Thrown: " + ex.ToString(), AdditionAction.Exception);
                }
                catch (Exception) { }
            }
        }

        public void exit()
        {
            try
            {
                downloadThread.Abort();
            }
            catch (Exception) { }
            if (File.Exists(_fileName) && !downloadComplete)
            {
                File.Delete(Path.Combine(_bucket, _fileName));
            }
            Dispose();
        }

        public void Dispose()
        {
            MainPanel.Dispose();
        }
        #endregion

        #region Events
        private void CancelPictureBox_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            _masterform.DownloadPanelClose(this);
        }
        #endregion

        #region Helpers

        public enum AdditionAction
        {
            None,
            Close,
            Complete,
            Exception
        }

        private void Notify(string message, AdditionAction action)
        {
            switch (action)
            {
                case AdditionAction.None:
                    break;
                case AdditionAction.Complete:
                    _isClosing = true;
                    break;
                case AdditionAction.Close:
                    _isClosing = true;
                    break;
                case AdditionAction.Exception:
                    if (_isClosing)
                    {
                        return;
                    }
                    _isClosing = true;
                    break;
                default:
                    throw new Exception("DownloadProgress Notify exception");
            }
            if (NotificationEvent != null) 
                NotificationEvent(this, message, action);
        }

        //private void DisplayMessage(string message, AdditionAction action)
        //{
        //    if (_masterform.InvokeRequired)
        //    {
        //        DisplayMessageCallback d = new DisplayMessageCallback(message, action);
        //        this.Invoke(d, new object[] { message });
        //    }
        //    switch (action)
        //    {
        //        case AdditionAction.None:
        //            break;
        //        default:
        //            break;
        //    }        
        // }

        public delegate void InfoEvent(object sender, string message, AdditionAction action);
        [Browsable(true)]
        public event InfoEvent NotificationEvent;
        
        

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private string getYoutubeUrl(string search)
        {
            string html = downloadString(YOUTUBE_SEARCH_URL + replaceSpaceWithPlus(search));
            int startOfVideoId = html.IndexOf(YOUTUBE_SEARCH_THIRD_STRING, html.IndexOf(YOUTUBE_SEARCH_SECOND_STRING, html.IndexOf(YOUTUBE_SEARCH_FIRST_STRING))) + YOUTUBE_SEARCH_THIRD_STRING.Length;
            int endOfVideoId = html.IndexOf('\"', startOfVideoId);
            string videoId = html.Substring(startOfVideoId, endOfVideoId - startOfVideoId);

            return YOUTUBE_VIDEO_URL + videoId;
        }


        private string getPlaylistUrl(string search)
        {
            const string PLAYLIST_URL = "http://www.youtube.com/playlist?list=";
            const string FIRST_SEARCH_STRING = "<div class=\"yt-lockup yt-lockup-tile";
            const string SECOND_SEARCH_STRING = "<h3 class=\"yt-lockup-title \">";
            const string THIRD_SEARCH_STRING = "?list=";
            string html = downloadString(YOUTUBE_SEARCH_URL + replaceSpaceWithPlus(search));
            int startOfPlaylistId = html.IndexOf(THIRD_SEARCH_STRING, html.IndexOf(SECOND_SEARCH_STRING, html.IndexOf(FIRST_SEARCH_STRING))) + THIRD_SEARCH_STRING.Length;
            int endOfPlaylistId = html.IndexOf('\"', startOfPlaylistId);
            string PlaylistId = html.Substring(startOfPlaylistId, endOfPlaylistId - startOfPlaylistId);
            MessageBox.Show("Found Playlist: " + PlaylistId);
            return PLAYLIST_URL + PlaylistId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Returns True if playlist handled</returns>
        private bool handleSearch(string search)
        {
            if (search.StartsWith("http://") || search.StartsWith("https://") || search.StartsWith("www.")) //not sure if https:// will work
            {
                if (search.Contains("/playlist?list="))
                {
                    playlistHelper(search);
                    return true;
                }
            }
            else
            {
                if (search.Contains("playlist"))
                {
                    DialogResult dialogResult = MessageBox.Show("Would you like to download the playlist?", "Download Playlist", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        playlistHelper(getPlaylistUrl(search));
                        return true;
                    }
                }
            }
            return false;
        }

        private void playlistHelper(string PlaylistUrl)
        {
            string html = downloadString(PlaylistUrl);
            List<string> videoIDs = getVideoIDsFromPlaylist(downloadString(PlaylistUrl));
            DialogResult dialogResult =
                MessageBox.Show("Would you like to download all " + videoIDs.Count.ToString() + " videos found in playlist: \"" + getPlaylistName(html) + "\"?",
                "Download Playlist", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (string id in videoIDs)
                {
                    string videoUrl = YOUTUBE_VIDEO_URL + id;
                    if (_masterform.InvokeRequired)
                    {
                        _masterform.BeginInvoke(
                            new Action(() =>
                            {
                                _masterform.AddOrQueueRaindrop(videoUrl);
                            }));
                    }
                    else
                    {
                        _masterform.AddOrQueueRaindrop(videoUrl);
                    }
                }
            }
        }


        private string getPlaylistName(string html)
        {
            const string PLAYLIST_NAME_SEARCH = "<h1 class=\"pl-header-title\">";
            const string PLAYLIST_NAME_END_SEARCH =  "</h1>";

            int startIndex = html.IndexOf(PLAYLIST_NAME_SEARCH) + PLAYLIST_NAME_SEARCH.Length;
            int endIndex = html.IndexOf(PLAYLIST_NAME_END_SEARCH, startIndex);
            return html.Substring(startIndex, endIndex - startIndex).Trim();
        }

        private List<string> getVideoIDsFromPlaylist(string html)
        {
            const string PLAYLIST_TABLE_SEARCH_STRING = "<table id=\"pl-video-table\"";
            const string PLAYLIST_VIDEO_SEARCH_STRING = "data-video-id=\"";

            List<string> VideoIDs = new List<string>(10);
            int startIndex;
            int highestIndex = 0;

            int index = html.IndexOf(PLAYLIST_TABLE_SEARCH_STRING) + PLAYLIST_TABLE_SEARCH_STRING.Length;

            for (; index < html.Length; /*index++*/)
            {
                try
                {
                    index = html.IndexOf(PLAYLIST_VIDEO_SEARCH_STRING, index) + PLAYLIST_VIDEO_SEARCH_STRING.Length;
                    startIndex = index;
                    index = html.IndexOf('\"', index);
                    if (index < highestIndex)
                        break;
                    highestIndex = index;
                    string videoId = html.Substring(startIndex, index - startIndex);
                    VideoIDs.Add(videoId);
                }
                catch (Exception)
                {
                    break;
                }
            }
            return VideoIDs;
        }

        private string downloadString(string url)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return client.DownloadString(url);
            }
        }
        private string replaceSpaceWithPlus(string search)
        {
            return search.Trim().Replace(' ', '+');
        }
        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.CancelPictureBox = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CancelPictureBox)).BeginInit();
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.Black;
            this.MainPanel.Controls.Add(this.InfoLabel);
            this.MainPanel.Controls.Add(this.CancelPictureBox);
            this.MainPanel.Controls.Add(this.progressBar);
            this.MainPanel.Location = _location;
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(417, 43);
            this.MainPanel.TabIndex = 2;
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
            this.CancelPictureBox.Image = Properties.Resources.fancy_close;
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

            _masterform.BeginInvoke(
                new Action(() =>
                {
                    _masterform.Controls.Add(this.MainPanel);
                }));
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CancelPictureBox)).EndInit();
        }
        #endregion

    }
}
