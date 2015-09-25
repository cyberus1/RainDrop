using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class DownloadsHandler
    {
        #region Constants
        //private static readonly Point PROGRESS_FORM_LOCATION = new Point(490, 10);
        //private const int Y_DISTANCE_BETWEEN_WINDOWS = 48;
        //private const int MAXIMUM_ALLOWED_RAINDROPS = 10;
        #endregion

        #region Private Variables
        private Form _masterform;
        private Point _downloadLocation;
        private Point _initialDownloadLocation;
        private List<YoutubeDownload> Downloads = new List<YoutubeDownload>(3);
        //private List<string> YoutubeVideoID = new List<string>(5);
        private List<YoutubeDownloadQueueInfo> _queue = new List<YoutubeDownloadQueueInfo>(3);
        private int _NumberOfSimultaneousDownloadsAllowed;
        private int _VirticalDistanceBetweenDownloadControls;
        #endregion

        #region Accessors
        public int NumberOfSimultaneousDownloadsAllowed
        {
            get { return _NumberOfSimultaneousDownloadsAllowed; }
            set
            {
                if (value > 0)
                    _NumberOfSimultaneousDownloadsAllowed = value;
                else throw new Exception("DownloadsHandler: NumberOfSimultaneousDownloadsAllowed must be positive");
            }
        }
        #endregion

        #region Constructor
        public DownloadsHandler(Form masterform, Point downloadLocation, int VirticalDistanceBetweenDownloadControls)
        {
            _masterform = masterform;
            _downloadLocation = downloadLocation;
            _initialDownloadLocation = downloadLocation;
            _VirticalDistanceBetweenDownloadControls = VirticalDistanceBetweenDownloadControls;
        }
        #endregion

        #region Events
        private void YoutubeDownload_VideoFromPlaylistUrlFound(object sender, UrlFoundEventArgs e)
        {
            AddOrQueueDownload(e.URL, e.DownloadDirectory);
        }
        private void YoutubeDownload_NotificationEvent(object sender, NotificationEventArgs e)
        {
            if (e.Message != "")
            {
                if (MessageEvent != null)
                    MessageEvent(this, new MessageEventArgs(e.YoutubeDownloadInfo, e.Message));
            }
        }
        private void YoutubeDownload_DownloadComplete(object sender, DownloadCompleteEventArgs e)
        {
            if (YoutubeDownloadComplete != null)
                YoutubeDownloadComplete(this, new YoutubeDownloadCompleteEventArgs(e.FileName, e.DownloadDirectory));
        }
        private void YoutubeDownload_DownloadClosing(object sender, DownloadClosingEventArgs e)
        {
            Downloads.Remove((YoutubeDownload)sender); //may require invoke

            DownloadClosed();
        }

        #endregion

        #region New Events
        public delegate void simpleEvent(object sender);
        public event simpleEvent LastDownloadCompletedEvent;
        public delegate void YoutubeDownloadStartedEvent(object sender, YoutubeDownloadStartedEventArgs e);
        public event YoutubeDownloadStartedEvent YoutubeDownloadStarted;
        public delegate void messageEvent(object sender, MessageEventArgs e);
        public event messageEvent MessageEvent;
        public delegate void YoutubeDownloadCompleteEvent(object sender, YoutubeDownloadCompleteEventArgs e);
        public event YoutubeDownloadCompleteEvent YoutubeDownloadComplete;
        #endregion

        #region Public Methods
        public void AddOrQueueDownload(string search, string downloadDirectory)
        {
            string _search = search.Trim();
            if (_search != "")
            {
                if (Downloads.Count < NumberOfSimultaneousDownloadsAllowed)
                {
                    addDownload(_search, downloadDirectory);
                }
                else
                {
                    _queue.Add(new YoutubeDownloadQueueInfo(search, downloadDirectory));
                }
            }
        }

        public void CloseAll()
        {
            foreach (YoutubeDownload download in Downloads)
            {
                download.exit();
            }
        }
        #endregion

        #region Helpers

        private void DownloadClosed()
        {
            _downloadLocation.Y -= _VirticalDistanceBetweenDownloadControls;
            if (_queue.Count != 0)
            {
                addDownload(_queue.First().SearchTerm, _queue.First().DownloadDirectory);
                _queue.RemoveAt(0);

            }
            if (Downloads.Count == 0)
            {
                if (LastDownloadCompletedEvent != null)
                    LastDownloadCompletedEvent(this);
            }
            else
                organizeDownloadForms();
        }
        
        private void addDownload(string search, string downloadDirectory)
        {
            YoutubeDownload temp = new YoutubeDownload();
            temp.Location = _downloadLocation;
            temp.VideoFromPlaylistUrlFound += new YoutubeDownload.FoundUrlEvent(YoutubeDownload_VideoFromPlaylistUrlFound);
            temp.Notification += new YoutubeDownload.NotificationEvent(YoutubeDownload_NotificationEvent);
            temp.DownloadClosing += new YoutubeDownload.DownloadClosingEvent(YoutubeDownload_DownloadClosing);
            temp.DownloadComplete += new YoutubeDownload.DownloadCompleteEvent(YoutubeDownload_DownloadComplete);
            temp.Start(downloadDirectory, search);
            if (YoutubeDownloadStarted != null)
                YoutubeDownloadStarted(this, new YoutubeDownloadStartedEventArgs(downloadDirectory, search));
            addControl(temp);
            Downloads.Add(temp);
            _downloadLocation.Y += _VirticalDistanceBetweenDownloadControls;
            
        }

        private delegate void addControlCallBack(YoutubeDownload download);
        private void addControl(YoutubeDownload download)
        {
            if (_masterform.InvokeRequired)
            {
                addControlCallBack d = new addControlCallBack(addControl);
                _masterform.Invoke(d, new object[] { download });
            }
            else
            {
                _masterform.Controls.Add(download);
            }
        }
        
        private void organizeDownloadForms()
        {
            _downloadLocation = _initialDownloadLocation;
            foreach (YoutubeDownload download in Downloads)
            {
                moveDownload(download, _downloadLocation);
                _downloadLocation.Y += _VirticalDistanceBetweenDownloadControls;
            }
        }
        private delegate void moveDownloadDeligate(YoutubeDownload download, Point newLocation);
        private void moveDownload(YoutubeDownload download, Point p)
        {
            if (download.InvokeRequired)
            {
                moveDownloadDeligate d = new moveDownloadDeligate(moveDownload);
                download.Invoke(d, new object[] { download, p });
            }
            else
            {
                download.Location = p;
            }
        }
        #endregion
    }

    #region YoutubeDownloadQueueInfo
    class YoutubeDownloadQueueInfo
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
        public YoutubeDownloadQueueInfo(string searchTerm, string downloadDirectory)
        {
            SearchTerm = searchTerm;
            DownloadDirectory = downloadDirectory;
        }
    }
    #endregion

    #region EventArgs
    class YoutubeDownloadCompleteEventArgs
    {
        public string FileName
        {
            get;
            protected set;
        }
        public string Directory
        {
            get;
            protected set;
        }
        public string ID
        {
            get;
            protected set;
        }
        public YoutubeDownloadCompleteEventArgs(string fileName, string directory, string id = null)
        {
            FileName = fileName;
            Directory = directory;
            if(id != null)
                ID = id;
        }
    }
    class MessageEventArgs
    {
        public YoutubeDownloadInfo SenderInfo //todo
        {
            get;
            protected set;
        }
        public string Message
        {
            get;
            protected set;
        }
        public MessageEventArgs(YoutubeDownloadInfo senderInfo, string message)
        {
            this.SenderInfo = senderInfo;
            this.Message = message;
        }
    }

    class YoutubeDownloadStartedEventArgs
    {
        public string DownloadDirectory
        {
            get;
            protected set;
        }
        public string SearchTerm
        {
            get;
            protected set;
        }
        public YoutubeDownloadStartedEventArgs(string downloadDirectory, string searchTerm)
        {
            this.DownloadDirectory = downloadDirectory;
            this.SearchTerm = searchTerm;
        }
    }
    #endregion
}
