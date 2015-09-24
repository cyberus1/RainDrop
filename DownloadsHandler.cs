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
        //private Point CompletedDownloadLocation;
        private List<YoutubeDownload> Downloads = new List<YoutubeDownload>(3);
        //private List<string> YoutubeVideoID = new List<string>(5);
        private List<YoutubeDownloadInfo> _queue = new List<YoutubeDownloadInfo>(3);
        //private int _maxRainDrops;
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

        #region New Events
        public delegate void simpleEvent(object sender);
        public event simpleEvent LastDownloadCompletedEvent;
        public delegate void passDownloadEvent(object sender, YoutubeDownload download);
        public event passDownloadEvent YoutubeDownloadStarted;
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
                    _queue.Add(new YoutubeDownloadInfo(_search, downloadDirectory));
                }
            }
        }
        public void YoutubeDownloadClose(YoutubeDownload download)
        {
            if (download != null)
            {
                Downloads.Remove(download);
                download.exit();
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

        private void addDownload(string search, string downloadDirectory)
        {
            YoutubeDownload temp = new YoutubeDownload();
            temp.Location = _downloadLocation;
            temp.Start(downloadDirectory, search);
            if (YoutubeDownloadStarted != null)
                YoutubeDownloadStarted(this, temp);
            _masterform.Controls.Add(temp);
            Downloads.Add(temp);
            _downloadLocation.Y += _VirticalDistanceBetweenDownloadControls;
        }

        

        private void organizeDownloadForms()
        {
            _downloadLocation = _initialDownloadLocation;
            foreach (YoutubeDownload download in Downloads)
            {
                download.Location = _downloadLocation;
                _downloadLocation.Y += _VirticalDistanceBetweenDownloadControls;
            }
        }
        #endregion
    }

    #region YoutubeDownloadInfo
    class YoutubeDownloadInfo
    {
        public string SearchTerm;
        public string DownloadDirectory;
        public YoutubeDownloadInfo(string searchTerm, string downloadDirectory)
        {
            SearchTerm = searchTerm;
            DownloadDirectory = downloadDirectory;
        }
    }
    #endregion

}
