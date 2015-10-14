// todo: modify so it can close itself;
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
        public enum Status
        {
            Searching,
            VideoUrlObtained,
            VideoDownloading,
            VideoDownloadComplete,
            PlaylistUrlObtained,
            PlaylistHandlingComplete,
            None
        }
        public enum ReasonForClose
        {
            User,
            Complete,
            PlaylistParsingFinished,
            Exception
        }
        #endregion

        #region Private Variables
        private bool _isClosing = false;
        private WebClient webClient = new WebClient();

        /// <summary>
        /// passes the mtml document recieved by the webclient between threads
        /// </summary>
        private volatile string _htmlDoc;

        private enum DownloadReason
        {
            getYoutubeUrl,
            getPlaylistUrl,
            playlistHelper
        }
        
        private Thread SearchThread;
        private Thread FinishGetYoutubeUrlThread;
        private Thread FinishGetPlaylistUrlThread;
        private Thread FinishPlaylistHelperThread;
        private Thread downloadThread;
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
        public string SearchTerm
        {
            get;
            protected set;
        }
        public Status CurrentStatus
        {
            get;
            protected set;
        }
        public string URL
        {
            get;
            protected set;
        }
        #endregion

        #region Constructor
        public YoutubeDownload()
        {
            InitializeComponent();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            SearchThread = new Thread(new ThreadStart(SearchFunction));
            FinishGetYoutubeUrlThread = new Thread(new ThreadStart(FinishGetYoutubeUrlFunction));
            FinishGetPlaylistUrlThread = new Thread(new ThreadStart(FinishGetPlaylistUrlFunction));
            FinishPlaylistHelperThread = new Thread(new ThreadStart(FinishPlaylistHelperFunction));
            downloadThread = new Thread(new ThreadStart(download));
            CurrentStatus = Status.None;
        }
        #endregion

        #region ThreadFunctions

        #region DownloadThread
        private void download()
        {
            try
            {

                Notify("Found URL: " + URL);

                VideoInfo video = getVideo(URL);

                FileName = getFileName(video);

                CurrentStatus = Status.VideoDownloading;

                if (File.Exists(Path.Combine(DownloadDirectory, FileName)))
                {
                    Notify("File Already Exist: " + FileName); //switched to complete
                    ThrowComplete();
                    return;
                }

                var audioDownloader = new AudioDownloader(video, Path.Combine(DownloadDirectory, FileName));

                audioDownloader.DownloadProgressChanged += (sender, args) =>
                    progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        //if (_isClosing)
                        //    return;
                        progressBar.Value = (int)(args.ProgressPercentage);
                    }));

                audioDownloader.AudioExtractionProgressChanged += (sender, args) =>
                    progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        if (args.ProgressPercentage == 0)
                        {
                            //if (_isClosing)
                            //    return;
                            SetInfoLable("Extracting: " + FileName);
                        }
                        progressBar.Value = (int)(args.ProgressPercentage);
                    }));

                SetInfoLable(FileName);

                

                audioDownloader.Execute();

                CurrentStatus = Status.VideoDownloadComplete;

                Notify("Completed Download of: " + FileName);
                ThrowComplete();
                return;
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.GetType() == typeof(YoutubeParseException))
                    {
                        Notify("Download Canceled: YoutubeParseException");
                        ThrowClose(ReasonForClose.Exception);
                    }
                    else
                    {
                        if (!_isClosing)
                        {
                            Notify("Download Canceled: Exception Thrown: " + ex.ToString());
                            ThrowClose(ReasonForClose.Exception);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
        #endregion

        #region Other Threads
        private void SearchFunction()
        {
            handleSearch(SearchTerm);
        }

        private void FinishGetYoutubeUrlFunction()
        {
            URL = continueGetYoutubeUrl(_htmlDoc);
            CurrentStatus = Status.VideoUrlObtained;
            downloadThread.Start();
        }
        private void FinishGetPlaylistUrlFunction()
        {
            URL = continueGetPlaylistUrl(_htmlDoc);
            CurrentStatus = Status.PlaylistUrlObtained;
            playlistHelper(URL);
        }
        private void FinishPlaylistHelperFunction()
        {
            continuePlaylistHelper(_htmlDoc);
        }
        #endregion

        #endregion

        #region Public Methods

        public void Start(string dowloadDirectory, string searchTerm)
        {
            if (Started != null)
                Started(this);
            DownloadDirectory = dowloadDirectory;
            SearchTerm = searchTerm;
            InfoLabel.Text = "Searching for: " + SearchTerm;
            SearchThread.Start();
            CurrentStatus = Status.Searching;
        }

        public void exit()
        {
            try { webClient.CancelAsync(); }
            catch { }
            try { downloadThread.Abort(); }
            catch { }
            try { FinishPlaylistHelperThread.Abort(); }
            catch { }
            try { FinishGetPlaylistUrlThread.Abort(); }
            catch { }
            try { FinishGetYoutubeUrlThread.Abort(); }
            catch { }
            try { SearchThread.Abort(); }
            catch { }
            Dispose();
        }

        #endregion

        #region Events
        private void CancelPictureBox_Click(object sender, EventArgs e)
        {
            Notify("Download Cancled by user.");
            this.ThrowClose(ReasonForClose.User);
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                _htmlDoc = e.Result;
                switch ((DownloadReason)e.UserState)
                {
                    case DownloadReason.getPlaylistUrl:
                        FinishGetPlaylistUrlThread.Start();
                        break;
                    case DownloadReason.getYoutubeUrl:
                        FinishGetYoutubeUrlThread.Start();
                        break;
                    case DownloadReason.playlistHelper:
                        FinishPlaylistHelperThread.Start();
                        break;
                }
            }
            catch (Exception)
            {
                Notify("Web Client Exception thrown");
                this.ThrowClose(ReasonForClose.Exception);
            }
        }
        #endregion

        #region New Events
        public delegate void NotificationEvent(object sender, NotificationEventArgs e);
        [Browsable(true)]
        public event NotificationEvent Notification;
        public delegate void FoundUrlEvent(object sender, UrlFoundEventArgs e);
        [Browsable(true)]
        public event FoundUrlEvent PlaylistDetected;
        [Browsable(true)]
        public event FoundUrlEvent VideoFromPlaylistUrlFound;
        public delegate void simpleEvent(object sender);
        [Browsable(true)]
        public event simpleEvent Started;
        public delegate void DownloadCompleteEvent(object sender, DownloadCompleteEventArgs e);
        [Browsable(true)]
        public event DownloadCompleteEvent DownloadComplete;
        public delegate void DownloadClosingEvent(object sender, DownloadClosingEventArgs e);
        [Browsable(true)]
        public event DownloadClosingEvent DownloadClosing;
        #endregion

        #region Helpers

        #region First Degree Helpers

        private void Notify(string message)
        {
            if (Notification != null)
                Notification(this, new NotificationEventArgs(new YoutubeDownloadInfo(this), message));
        }
        private void ThrowComplete()
        {
            if (!_isClosing)
            {
                if (DownloadComplete != null)
                    DownloadComplete(this, new DownloadCompleteEventArgs(new YoutubeDownloadInfo(this)));
                ThrowClose(ReasonForClose.Complete);
            }
            
        }
        private void ThrowClose(ReasonForClose reason)
        {
            if (!_isClosing)
            {
                _isClosing = true;
                this.DisposeCallBack();
                if (DownloadClosing != null)
                    DownloadClosing(this, new DownloadClosingEventArgs(new YoutubeDownloadInfo(this), reason));
            }

        }

        private delegate void voidCallback();
        private void DisposeCallBack()
        {
            if (this.InvokeRequired)
            {
                voidCallback d = new voidCallback(DisposeCallBack);
                this.Invoke(d);
            }
            else
            {
                webClient.CancelAsync();
                this.Dispose();
            }
        }

        private void handleSearch(string search)
        {
            if (search.StartsWith("http://") || search.StartsWith("https://") || search.StartsWith("www.")) //not sure if https:// will work
            {
                if (search.StartsWith("www."))
                {
                    search = "http://" + search;
                }
                if (search.Contains("/playlist?list="))
                {
                    URL = search;
                    CurrentStatus = Status.PlaylistUrlObtained;
                    playlistHelper(search);
                    //Notify("Playlist Downloading", AdditionAction.Close);
                    return;
                }
                else
                {
                    URL = search;
                    downloadThread.Start();
                    return;
                }
            }
            else
            {
                if (search.Contains("playlist"))
                {
                    DialogResult dialogResult = MessageBox.Show("Would you like to download the playlist?", "Download Playlist", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        getPlaylistUrl(search);
                        //Notify("Playlist Downloading", AdditionAction.Close);
                        return;
                    }
                }
                getYoutubeUrl(search);
            }
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

        private void getYoutubeUrl(string search)
        {
            downloadString(YOUTUBE_SEARCH_URL + replaceSpaceWithPlus(search), DownloadReason.getYoutubeUrl);
        }

        private void getPlaylistUrl(string search)
        {
            downloadString(YOUTUBE_SEARCH_URL + replaceSpaceWithPlus(search), DownloadReason.getPlaylistUrl);
        }


        private void playlistHelper(string PlaylistUrl)
        {
            if (PlaylistDetected != null)
                PlaylistDetected(this, new UrlFoundEventArgs(new YoutubeDownloadInfo(this)));
            downloadString(PlaylistUrl, DownloadReason.playlistHelper);
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


        private void downloadString(string url, DownloadReason downloadReason)
        {
            webClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri(url);
            //update for proxies
            IWebProxy wp = WebRequest.DefaultWebProxy;
            wp.Credentials = CredentialCache.DefaultCredentials;
            webClient.Proxy = wp;
            //
            webClient.DownloadStringAsync(uri, downloadReason);
        }
        private string replaceSpaceWithPlus(string search)
        {
            return search.Trim().Replace(' ', '+');
        }
        #endregion

        #region Continue Functions
        private string continueGetPlaylistUrl(string htmlDoc)
        {
            const string PLAYLIST_URL = "http://www.youtube.com/playlist?list=";
            const string FIRST_SEARCH_STRING = "<div class=\"yt-lockup yt-lockup-tile";
            const string SECOND_SEARCH_STRING = "<h3 class=\"yt-lockup-title \">";
            const string THIRD_SEARCH_STRING = "?list=";
            int startOfPlaylistId = htmlDoc.IndexOf(THIRD_SEARCH_STRING, htmlDoc.IndexOf(SECOND_SEARCH_STRING, htmlDoc.IndexOf(FIRST_SEARCH_STRING))) + THIRD_SEARCH_STRING.Length;
            int endOfPlaylistId = htmlDoc.IndexOf('\"', startOfPlaylistId);
            string PlaylistId = htmlDoc.Substring(startOfPlaylistId, endOfPlaylistId - startOfPlaylistId);
            return PLAYLIST_URL + PlaylistId;
        }
        private string continueGetYoutubeUrl(string htmlDoc)
        {
            int startOfVideoId = htmlDoc.IndexOf(YOUTUBE_SEARCH_THIRD_STRING, htmlDoc.IndexOf(YOUTUBE_SEARCH_SECOND_STRING, htmlDoc.IndexOf(YOUTUBE_SEARCH_FIRST_STRING))) + YOUTUBE_SEARCH_THIRD_STRING.Length;
            int endOfVideoId = htmlDoc.IndexOf('\"', startOfVideoId);
            string videoId = htmlDoc.Substring(startOfVideoId, endOfVideoId - startOfVideoId);
            return YOUTUBE_VIDEO_URL + videoId;
        }
        private void continuePlaylistHelper(string htmlDoc)
        {
            List<string> videoIDs = getVideoIDsFromPlaylist(htmlDoc);
            DialogResult dialogResult =
                MessageBox.Show("Would you like to download all " + videoIDs.Count.ToString() + " videos found in playlist: \"" + getPlaylistName(htmlDoc) + "\"?",
                "Download Playlist", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Notify("Playlist Downloading");
                foreach (string id in videoIDs)
                {
                    string videoURL = YOUTUBE_VIDEO_URL + id;
                    if (VideoFromPlaylistUrlFound != null)
                        VideoFromPlaylistUrlFound(this, new UrlFoundEventArgs(new YoutubeDownloadInfo(this), videoURL));
                }
                CurrentStatus = Status.PlaylistHandlingComplete;
                ThrowClose(ReasonForClose.PlaylistParsingFinished);
                return;
            }
            Notify("Playlist Download Canceled");
            ThrowClose(ReasonForClose.User);
        }
        #endregion

        #endregion

    }

    #region YoutubeDownloadInfo
    public class YoutubeDownloadInfo
    {
        public string SearchTerm
        {
            get;
            protected set;
        }
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
        public string URL
        {
            get;
            protected set;
        }
        public YoutubeDownload.Status Status
        {
            get;
            protected set;
        }
        public YoutubeDownloadInfo(YoutubeDownload download)
        {
            Status = download.CurrentStatus;
            if (download.CurrentStatus == YoutubeDownload.Status.None)
                return;
            else
            {
                SearchTerm = download.SearchTerm;
                DownloadDirectory = download.DownloadDirectory;
            }
            if (download.CurrentStatus == YoutubeDownload.Status.Searching)
            {
                return;
            }
            else
            {
                URL = download.URL;
            }
            if (download.CurrentStatus == YoutubeDownload.Status.VideoDownloadComplete || download.CurrentStatus == YoutubeDownload.Status.VideoDownloading)
            {
                FileName = download.FileName;
            }
        }
    }
    #endregion

    #region EventArgs

    #region YoutubeDownloadInfoEventArgs
    public class YoutubeDownloadInfoEventArgs
    {
        public YoutubeDownloadInfo YoutubeDownloadInfo
        {
            get;
            protected set;
        }
        public YoutubeDownloadInfoEventArgs(YoutubeDownloadInfo info)
        {
            this.YoutubeDownloadInfo = info;
        }

    }
    #endregion

    #region NotificationEventArgs
    public class NotificationEventArgs : YoutubeDownloadInfoEventArgs
    {
        public string Message
        {
            get;
            protected set;
        }
        private NotificationEventArgs(YoutubeDownloadInfo info)
            : base(info)
        {
        }
        public NotificationEventArgs(YoutubeDownloadInfo info, string message)
            : this(info)
        {
            Message = message;
        }
    }
    #endregion

    #region UrlFoundEventArgs
    public class UrlFoundEventArgs : YoutubeDownloadInfoEventArgs
    {
        public string URL
        {
            get;
            protected set;
        }
        public string DownloadDirectory
        {
            get { return this.YoutubeDownloadInfo.DownloadDirectory; }
        }
        public UrlFoundEventArgs(YoutubeDownloadInfo info, string url = null)
            : base(info)
        {
            if (url == null)
            {
                URL = info.URL;
            }
            else
            {
                URL = url;
            }
            this.YoutubeDownloadInfo = info;
            
        }
    }
    #endregion

    #region DownloadCompleteEventArgs
    public class DownloadCompleteEventArgs : YoutubeDownloadInfoEventArgs
    {
        public string FileName
        {
            get { return this.YoutubeDownloadInfo.FileName; }
        }
        public string DownloadDirectory
        {
            get { return this.YoutubeDownloadInfo.DownloadDirectory; }
        }
        public DownloadCompleteEventArgs(YoutubeDownloadInfo info)
            : base(info)
        {
        }
    }
    #endregion

    #region DownloadClosingEventArgs
    public class DownloadClosingEventArgs : YoutubeDownloadInfoEventArgs
    {
        public YoutubeDownload.ReasonForClose ReasonForClose
        {
            get;
            protected set;
        }
        private DownloadClosingEventArgs(YoutubeDownloadInfo info)
            : base(info)
        { }
        public DownloadClosingEventArgs(YoutubeDownloadInfo info, YoutubeDownload.ReasonForClose closeReason)
            :this(info)
        {
            ReasonForClose = closeReason;
        }
        
    }
    #endregion

    #endregion
}
