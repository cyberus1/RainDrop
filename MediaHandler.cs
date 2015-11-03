using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class MediaHandler
    {
        form1 _masterForm;
        MusicFileControl CurrentPlaying;

        public MediaHandler(form1 masterForm)
        {
            _masterForm = masterForm;
            this.Initialize();
        }

        #region Public Methods
        public void Play(MusicFileControl toPlay)
        {
            PlayHelper(toPlay);
        }
        public void Play()
        {
            if (CurrentPlaying != null)
                PlayHelper(CurrentPlaying);
        }
        public void PlayPause()
        {
            if (_masterForm.mediaPanel1.IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        public void Pause()
        {
            _masterForm.mediaPanel1.Pause();
            if (CurrentPlaying != null)
            {
                CurrentPlaying.Stop(); // error got null reference
            }
        }
        #endregion

        #region Events
        private void mediaPanel1_PlayClicked(object sender)
        {
            _Play();
        }
        private void mediaPanel1_PauseClicked(object sender)
        {
            Pause();
        }
        private void mediaPanel1_NextClicked(object sender)
        {
            playNext();
        }
        private void mediaPanel1_PreviousClicked(object sender)
        {
            playPrevious();
        }
        private void mediaPanel1_MediaEnded(object sender)
        {
            playNext();
        }
        #endregion

        #region Helpers

        private string loopChecking_FirstSkipPath;
        private bool loopChecking_FirstSkipPathSet = false;

        private void PlayHelper(MusicFileControl toPlay)
        {
            if (File.Exists(toPlay.FullPath))
            {
                loopChecking_FirstSkipPathSet = false;
                _Play(toPlay);
            }
            else
            {
                if (loopChecking_FirstSkipPathSet)
                {
                    if (toPlay.FullPath == loopChecking_FirstSkipPath)
                    {
                        loopChecking_FirstSkipPathSet = false;
                        return;
                    }
                }
                else
                {
                    loopChecking_FirstSkipPath = toPlay.FullPath;
                    loopChecking_FirstSkipPathSet = true;
                }
                playNext(toPlay);
            }
        }
        private void playNext(MusicFileControl currentPlaying)
        {
            for (int c = 0; c < _masterForm.Completed.Count; c++) //play Next
            {
                if (_masterForm.Completed.ElementAt(c) == currentPlaying)
                {
                    if (c == _masterForm.Completed.Count - 1)
                    {
                        PlayHelper(_masterForm.Completed.First());
                    }
                    else
                    {
                        PlayHelper(_masterForm.Completed.ElementAt(c + 1));
                    }
                    return;
                }
            }
        }
        private void playNext()
        {
            if (CurrentPlaying != null)
                playNext(CurrentPlaying);
        }

        private void playPrevious(MusicFileControl currentPlaying)
        {
            for (int c = 0; c < _masterForm.Completed.Count; c++)
            {
                if (_masterForm.Completed.ElementAt(c) == currentPlaying)
                {
                    if (c == 0)
                    {
                        PlayPreviousHelper(_masterForm.Completed.Last());
                    }
                    else
                    {
                        PlayPreviousHelper(_masterForm.Completed.ElementAt(c - 1));
                    }
                    return;
                }
            }
        }

        private void playPrevious()
        {
            if (CurrentPlaying != null)
                playPrevious(CurrentPlaying);
        }

        private string loopChecking_PreviousFirstSkipPath;
        private bool loopChecking_PreviousFirstSkipPathSet = false;

        private void PlayPreviousHelper(MusicFileControl toPlay)
        {
            if (File.Exists(toPlay.FullPath))
            {
                _Play(toPlay);
            }
            else
            {
                if (loopChecking_PreviousFirstSkipPathSet)
                {
                    if (toPlay.FullPath == loopChecking_PreviousFirstSkipPath)
                    {
                        loopChecking_PreviousFirstSkipPathSet = false;
                        return;
                    }
                }
                else
                {
                    loopChecking_PreviousFirstSkipPath = toPlay.FullPath;
                    loopChecking_PreviousFirstSkipPathSet = true;
                }
                playPrevious(toPlay);
            }
        }

        private void _Play()
        {
            if (CurrentPlaying != null)
                PlayHelper(CurrentPlaying);
        }
        private void _Play(MusicFileControl toPlay)
        {
            if (CurrentPlaying != toPlay)
            {
                if (CurrentPlaying != null)
                {
                    CurrentPlaying.Stop();
                }
                CurrentPlaying = toPlay;
            }
            _masterForm.mediaPanel1.Play(CurrentPlaying.FullPath);
            CurrentPlaying.Play();
        }
        
        #endregion

        #region Initialize
        private void Initialize()
        {
            _masterForm.mediaPanel1.PlayClicked += new MediaPanel.SimpleEventHandler(mediaPanel1_PlayClicked);
            _masterForm.mediaPanel1.PauseClicked += new MediaPanel.SimpleEventHandler(mediaPanel1_PauseClicked);
            _masterForm.mediaPanel1.NextClicked += new MediaPanel.SimpleEventHandler(mediaPanel1_NextClicked);
            _masterForm.mediaPanel1.PreviousClicked += new MediaPanel.SimpleEventHandler(mediaPanel1_PreviousClicked);
            _masterForm.mediaPanel1.MediaEnded += new MediaPanel.SimpleEventHandler(mediaPanel1_MediaEnded);
                    }
        #endregion
    }
}
