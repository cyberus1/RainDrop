using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YoutubeExtractor;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class YoutubeDownload : UserControl
    {
        #region Constants
        private const string YOUTUBE_VIDEO_URL = "http://www.youtube.com/watch?v=";
        private const string YOUTUBE_SEARCH_URL = "http://www.youtube.com/results?search_query=";
        private const string YOUTUBE_SEARCH_FIRST_STRING = "id=\"results\">";
        private const string YOUTUBE_SEARCH_SECOND_STRING = "id=\"item-section";
        private const string YOUTUBE_SEARCH_THIRD_STRING = "data-context-item-id=\"";
        #endregion

        #region Public Constants
        public enum AdditionAction
        {
            None,
            Close,
            Complete,
            Exception
        }
        #endregion

        #region Private Variables
        private string _searchTerm;
        //private Point _location;
        private Thread downloadThread;
        private bool _isClosing = false;
        #endregion

        #region Get/Set
        public string DownloadDirectory
        {
            get;
            protected set;
        }
        public string FileName
        {
            get;
            protected set;
        }
        #endregion

        public YoutubeDownload()
        {
            InitializeComponent();
        }

        #region DownloadThread
        private void download()
        {
            try
            {
                string url = handleSearch(_searchTerm);

                Notify("Found URL: " + url, AdditionAction.None);

                VideoInfo video = getVideo(url);

                FileName = getFileName(video);

                if (File.Exists(Path.Combine(DownloadDirectory, FileName)))
                {
                    Notify("File Already Exist: " + FileName, AdditionAction.Complete); //switched to complete
                    return;
                }

                //downloadComplete = false;

                var audioDownloader = new AudioDownloader(video, Path.Combine(DownloadDirectory, FileName));

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
                            SetInfoLable("Extracting: " + FileName);
                        }
                        progressBar.Value = (int)(args.ProgressPercentage);
                    }));

                SetInfoLable(FileName);

                audioDownloader.Execute();
                //downloadComplete = true; 
                Notify("Completed Download of: " + FileName, AdditionAction.Complete);
                return;
            }
            catch (Exception ex)
            {
                try
                {
                    Notify("Download Canceled: Exception Thrown: " + ex.ToString(), AdditionAction.Exception);
                }
                catch (Exception) { }
            }
        }
        #endregion

        #region Public Methods

        public void Start(string dowloadDirectory, string searchTerm)
        {
            DownloadDirectory = dowloadDirectory;
            _searchTerm = searchTerm;
            InfoLabel.Text = "Searching for: " + _searchTerm;

            downloadThread = new Thread(new ThreadStart(download));
            downloadThread.Start();
        }

        public void exit()
        {
            //if (File.Exists(_fileName) && !downloadComplete)
            //{
            //    File.Delete(Path.Combine(_bucket, _fileName));
            //}
            Dispose();
        }

        #endregion

        #region New Events
        public delegate void InfoEvent(object sender, string message, AdditionAction action);
        [Browsable(true)]
        public event InfoEvent NotificationEvent;
        public delegate void foundUrlEvent(object sender, string url);
        [Browsable(true)]
        public event foundUrlEvent PlaylistDetected;
        [Browsable(true)]
        public event foundUrlEvent VideoFromPlaylistUrlFound;
        #endregion

        #region Helpers

        #region First Degree Helpers

        private void Notify(string message, AdditionAction action)
        {
            if (_isClosing == true)
                return;
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
                    _isClosing = true;
                    break;
                default:
                    throw new Exception("YoutubeDownload Notify exception");
            }
            if (NotificationEvent != null)
                NotificationEvent(this, message, action);
        }

        private string handleSearch(string search)
        {
            if (search.StartsWith("http://") || search.StartsWith("https://") || search.StartsWith("www.")) //not sure if https:// will work
            {
                if (search.Contains("/playlist?list="))
                {
                    playlistHelper(search);
                    Notify("Playlist Downloading", AdditionAction.Close);
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
                        Notify("Playlist Downloading", AdditionAction.Close);
                    }
                }
            }
            return getYoutubeUrl(search);
        }

        private VideoInfo getVideo(string url)
        {
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
            return video;
        }

        private string getFileName(VideoInfo video)
        {
            string videoTitle = RemoveIllegalPathCharacters(video.Title);
            string fileName = "Error";
            switch (video.AudioType)
            {
                case AudioType.Aac:
                    fileName = videoTitle + ".mp4";
                    break;
                case AudioType.Mp3:
                    fileName = videoTitle + ".mp3";
                    break;
                case AudioType.Vorbis:
                    MessageBox.Show("Warning: .ogg file, will need to be converted");
                    fileName = videoTitle + ".mp3";
                    break;
                case AudioType.Unknown:
                    throw new Exception("YoutubeDownload.cs: AudioType Unknown");
            }
            return fileName;
        }

        delegate void SetInfoLableCallback(string message);
        private void SetInfoLable(string message)
        {
            if (InfoLabel.InvokeRequired)
            {
                SetInfoLableCallback d = new SetInfoLableCallback(SetInfoLable);
                this.InfoLabel.Invoke(d, new object[] { message });
            }
            else
            {
                InfoLabel.Text = message;
            }
        }

        #endregion

        #region Second Degree Helpers
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
            //MessageBox.Show("Found Playlist: " + PlaylistId);
            return PLAYLIST_URL + PlaylistId;
        }


        private void playlistHelper(string PlaylistUrl)
        {
            if (PlaylistDetected != null)
                PlaylistDetected(this, PlaylistUrl);
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
                    if (VideoFromPlaylistUrlFound != null)
                        VideoFromPlaylistUrlFound(this, videoUrl);
                }
            }
        }


        private string getPlaylistName(string html)
        {
            const string PLAYLIST_NAME_SEARCH = "<h1 class=\"pl-header-title\">";
            const string PLAYLIST_NAME_END_SEARCH = "</h1>";

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

        #endregion

        #region Events
        private void CancelPictureBox_Click(object sender, EventArgs e)
        {
            //Notify("Download Cancled by user.", AdditionAction.Close);
        }
        #endregion

    }
}
