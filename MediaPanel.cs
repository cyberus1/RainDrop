using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace WindowsFormsApplication1
{
    public partial class MediaPanel : UserControl
    {
        private WindowsMediaPlayer mplayer = new WindowsMediaPlayer();

        #region Accessors

        public bool IsPlaying
        {
            get
            {
                if (mplayer.playState == WMPPlayState.wmppsPlaying)
                    return true;
                return false;
            }
        }

        [DefaultValue(75),
        Browsable(true)]
        public int Volume
        {
            get
            {
                return VolumeSlider.Value;
            }
            private set
            {
                if (value >= 0 && value <= 100)
                {
                    VolumeSlider.Value = value;
                }
                else
                {
                    throw new MediaPanelException("Volume out of range");
                }
            }
        }

        [Description("If true the volume will be saved as a setting and loaded at runtime"),
        DefaultValue(true),
        Browsable(true)]
        public bool StoreSettings
        {
            get { return _StoreSettings; }
            set { _StoreSettings = value; }
        }
        private bool _StoreSettings = true;
        #endregion

        public MediaPanel()
        {
            InitializeComponent();
            mplayer.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wplayer_PlayStateChange);
            PositionTimer.Start();
            if(StoreSettings)
                LoadSettings();
        }

        #region Settings
        
        private void LoadSettings()
        {
            try
            {
                Volume = Properties.Settings.Default.Volume;
            }
            catch (MediaPanelException)
            {
                //Volume = DEFAULT_VOLUME;
            }
        }
        private void SaveSettings()
        {
            Properties.Settings.Default.Volume = Volume;
            Properties.Settings.Default.Save();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (StoreSettings)
                SaveSettings();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Events
        private void MediaPanel_Load(object sender, EventArgs e)
        {
            PositionTimer.Start();
        }

        private void PlayPausePictureBox_Click(object sender, EventArgs e)
        {
            if (PlayPauseClicked != null)
                PlayPauseClicked(this);
            if (mplayer.playState == WMPPlayState.wmppsPlaying)
            {
                if (PauseClicked != null)
                    PauseClicked(this);
            }
            else
            {
                if (PlayClicked != null)
                    PlayClicked(this);
            }
        }

        private void PreviousPictureBox_Click(object sender, EventArgs e)
        {
            if (PreviousClicked != null)
                PreviousClicked(this);
        }

        private void NextPictureBox_Click(object sender, EventArgs e)
        {
            if (NextClicked != null)
                NextClicked(this);
        }

        private void VolumeSlider_Scroll(object sender, ScrollEventArgs e)
        {
            mplayer.settings.volume = VolumeSlider.Value;
            if (VolumeChanged != null)
                VolumeChanged(this, mplayer.settings.volume);
        }

        private void LocationSlider_Scroll(object sender, ScrollEventArgs e)
        {
            if (mplayer.currentMedia != null)
            {
                mplayer.controls.currentPosition = LocationSlider.Value * mplayer.currentMedia.duration / LocationSlider.Maximum;
                if (MediaPositionChanged != null)
                    MediaPositionChanged(this, mplayer.controls.currentPosition);
            }
        }
        private void PositionTimer_Tick(object sender, EventArgs e)
        {
            if (mplayer.currentMedia != null)
            {
                int temp = (int)(mplayer.controls.currentPosition / mplayer.currentMedia.duration * LocationSlider.Maximum);
                if (temp > LocationSlider.Maximum)
                    LocationSlider.Value = LocationSlider.Maximum;
                else if (temp < 0)
                    LocationSlider.Value = 0;
                else
                    LocationSlider.Value = temp;
            }
        }
        private void wplayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                WMP_PlayNextTimer.Start();
            }
        }

        /// <summary>
        /// The next song cannot be played from the PlayStateChange event
        /// </summary>
        private void WMP_PlayNextTimer_Tick(object sender, EventArgs e)
        {
            WMP_PlayNextTimer.Stop();
            if (MediaEnded != null)
                MediaEnded(this);
        }
        #endregion

        #region Public Methods

        public void Play(string path)
        {
            if(path != mplayer.URL)
                mplayer.URL = path;
            mplayer.controls.play();
            PlayPausePictureBox.Image = Properties.Resources.pause_image;
            this.InfoLabel.Text = Path.GetFileName(path);
        }

        public void Pause()
        {
            if (mplayer.playState == WMPPlayState.wmppsPlaying)
            {
                mplayer.controls.pause();
                PlayPausePictureBox.Image = Properties.Resources.play_image;
            }
        }


        #endregion

        #region New Events
        public delegate void SimpleEventHandler(object sender);
        [Browsable(true)]
        public event SimpleEventHandler NextClicked;
        [Browsable(true)]
        public event SimpleEventHandler PreviousClicked;
        [Browsable(true)]
        public event SimpleEventHandler PlayPauseClicked;
        [Browsable(true)]
        public event SimpleEventHandler PlayClicked;
        [Browsable(true)]
        public event SimpleEventHandler PauseClicked;
        [Browsable(true)]
        public event SimpleEventHandler MediaEnded;
        public delegate void IntEventHandler(object sender, int value);
        [Browsable(true)]
        public event IntEventHandler VolumeChanged;
        public delegate void DoubleEventHandler(object sender, double value);
        [Browsable(true)]
        public event DoubleEventHandler MediaPositionChanged;
        #endregion
    }

    #region MediaPanelException
    public class MediaPanelException : Exception
    {
        public MediaPanelException()
            : base()
        {
        }

        public MediaPanelException(string message)
            : base(message)
        {
        }

        public MediaPanelException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion
}
