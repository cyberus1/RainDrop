﻿// ****************************************************************************
//
// FLV Extract
// Copyright (C) 2013-2014 Dennis Daume (daume.dennis@gmail.com)
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// ****************************************************************************

using System;
using System.IO;
using System.Net;
using NReco.VideoConverter;

namespace YoutubeExtractor
{
    /// <summary>
    /// Provides a method to download a video and extract its audio track.
    /// </summary>
    public class AudioDownloader : Downloader
    {
        private bool isCanceled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDownloader"/> class.
        /// </summary>
        /// <param name="video">The video to convert.</param>
        /// <param name="savePath">The path to save the audio.</param>
        /// /// <param name="bytesToDownload">An optional value to limit the number of bytes to download.</param>
        /// <exception cref="ArgumentNullException"><paramref name="video"/> or <paramref name="savePath"/> is <c>null</c>.</exception>
        public AudioDownloader(VideoInfo video, string savePath, int? bytesToDownload = null)
            : base(video, savePath, bytesToDownload)
        { }

        /// <summary>
        /// Occurs when the progress of the audio extraction has changed.
        /// </summary>
        public event EventHandler<ProgressEventArgs> AudioExtractionProgressChanged;

        /// <summary>
        /// Occurs when the download progress of the video file has changed.
        /// </summary>
        public event EventHandler<ProgressEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Downloads the video from YouTube and then extracts the audio track out if it.
        /// </summary>
        /// <exception cref="IOException">
        /// The temporary video file could not be created.
        /// - or -
        /// The audio file could not be created.
        /// </exception>
        /// <exception cref="WebException">An error occured while downloading the video.</exception>
        public override void Execute()
        {
            string tempPath = Path.GetTempFileName();

            this.DownloadVideo(tempPath);

            if (!this.isCanceled)
            {
                this.ExtractAudio(tempPath);
            }

            this.OnDownloadFinished(EventArgs.Empty);
        }

        private void DownloadVideo(string path)
        {
            var videoDownloader = new VideoDownloader(this.Video, path, this.BytesToDownload);

            videoDownloader.DownloadProgressChanged += (sender, args) =>
            {
                if (this.DownloadProgressChanged != null)
                {
                    this.DownloadProgressChanged(this, args);

                    this.isCanceled = args.Cancel;
                }
            };

            videoDownloader.Execute();
        }

        private void ExtractAudio(string path)
        {
            FFMpegConverter converter = new FFMpegConverter();
            converter.ConvertProgress += (sender, args) =>
            {
                if (this.AudioExtractionProgressChanged != null)
                {
                   
                    double progressPercent = args.Processed.TotalSeconds / args.TotalDuration.TotalSeconds * 100;
                    this.AudioExtractionProgressChanged(this, new ProgressEventArgs(progressPercent));
                }
            };
            //string filename = System.IO.Path.GetFileName(path); //testing
            //string dir = System.IO.Path.GetDirectoryName(this.SavePath); //testing
            //System.IO.File.Copy(path, System.IO.Path.Combine(dir, filename)); //testing
            string inputType;
            switch(Video.VideoType) 
            {
                case VideoType.Mp4:
                    inputType = "mp4";
                    break;
                case VideoType.Flash:
                    inputType = "flv";
                    break;
                case VideoType.WebM:
                    inputType = "webm";
                    break;
                case VideoType.Mobile:
                    inputType = "3gp";
                    break;
                default:
                    throw new Exception("AudioDownloader.cs: VideoType unsupported");

            }
            NReco.VideoConverter.ConvertSettings cs = new ConvertSettings();
            switch(Video.AudioType)
            {
                case AudioType.Aac:
                    cs.CustomOutputArgs = "-vn -acodec copy ";
                    converter.ConvertMedia(path, inputType, this.SavePath, "mp4", cs);
                    break;
                case AudioType.Mp3:
                    cs.CustomOutputArgs = "-vn -acodec copy ";
                    converter.ConvertMedia(path, inputType, this.SavePath, "mp3", cs);
                    break;
                case AudioType.Vorbis:
                    converter.ConvertMedia(path, inputType, this.SavePath, "mp3", cs);
                    break;
                case AudioType.Unknown:
                    throw new Exception("AudioDownloader.cs: AudioType Unknown");
            }
            

            

            //converter.ConvertMedia(path, this.SavePath, "mp3");
        }
    }
}