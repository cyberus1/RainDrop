//todo: variable comment size if no track number(30 bytes instead of 28

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberus.fileParsing;

namespace Cyberus.ID3Tag
{
    class ID3v1
    {
        #region Constants
        private const string HEADER = "TAG";
        private const int TITLE_ARTIST_ALBUM_LENGTH = 30;
        private const int YEAR_LENGTH = 4;
        private const int COMMENT_LENGTH = 28;
        private const char PADDING_CHAR = ' ';
        private const byte TRACK_NUMBER_EXISTS = 0;
        private const byte TRACK_NUMBER_DOES_NOT_EXIST = (byte)' ';
        private const byte NO_TRACK_NUMBER = 0;
        private const int ID3v1_LENGTH = 128;
        private const int VARIABLES_TO_JOIN = 9;
        private const byte NO_GENRE = 255;

        private const int TITLE_START_INDEX = 3;
        private const int ARTIST_START_INDEX = TITLE_START_INDEX + TITLE_ARTIST_ALBUM_LENGTH;
        private const int ALBUM_START_INDEX = ARTIST_START_INDEX + TITLE_ARTIST_ALBUM_LENGTH;
        private const int YEAR_START_INDEX = ALBUM_START_INDEX + TITLE_ARTIST_ALBUM_LENGTH;
        private const int COMMENT_START_INDEX = YEAR_START_INDEX + YEAR_LENGTH;
        private const int ZERO_BYTE_LOCATION = 125;
        private const int TRACK_BYTE_LOCATION = 126;
        private const int GENRE_BYTE_LOCATION = 127;

        #endregion

        #region Private Variables
        private string _title;
        private string _artist;
        private string _album;
        private string _year;
        private string _comment;
        private byte _zeroByte;
        private byte _trackNum;
        private byte _genreNum;
        #endregion

        #region Get/Set

        public string Title
        {
            get { return _title.TrimEnd(); }
            set
            {
                if (value.Length <= TITLE_ARTIST_ALBUM_LENGTH)
                    _title = value.PadRight(TITLE_ARTIST_ALBUM_LENGTH);
            }
        }
        public string Artist
        {
            get { return _artist.TrimEnd(); }
            set
            {
                if (value.Length <= TITLE_ARTIST_ALBUM_LENGTH)
                    _artist = value.PadRight(TITLE_ARTIST_ALBUM_LENGTH);
            }
        }
        public string Album
        {
            get { return _album.TrimEnd(); }
            set
            {
                if (value.Length <= TITLE_ARTIST_ALBUM_LENGTH)
                    _album = value.PadRight(TITLE_ARTIST_ALBUM_LENGTH);
            }
        }
        public string Year
        {
            get { return _year; }
            set
            {
                if (value.Length == YEAR_LENGTH)
                    _year = value;
            }
        }
        public string Comment
        {
            get { return _comment.TrimEnd(); }
            set
            {
                if (value.Length <= COMMENT_LENGTH)
                    _comment = value.PadRight(COMMENT_LENGTH);
            }
        }
        public int TrackNum
        {
            get 
            {
                if (_zeroByte == 0)
                    return _trackNum;
                else
                    return NO_TRACK_NUMBER;
            }
            set
            {
                if (value == 0)
                {
                    _zeroByte = TRACK_NUMBER_DOES_NOT_EXIST;
                    _trackNum = TRACK_NUMBER_DOES_NOT_EXIST;
                }
                else if (value > 0 && value <= 255)
                {
                    _zeroByte = TRACK_NUMBER_EXISTS;
                    _trackNum = (byte)value;
                }
            }
        }
        
        public bool setTrackNum(string TrackNumber)
        {
            int temp;
            if (Int32.TryParse(TrackNumber, out temp))
            {
                TrackNum = temp;
                return true;
            }
            else
                return false;
        }

        public int GenreNum
        {
            get { return _genreNum; }
            set
            {
                if (value >= 0 && value <= 255)
                    _genreNum = (byte)value;
            }
        }

        public bool setGenreNum(string GenreNumber)
        {
            int temp;
            if (Int32.TryParse(GenreNumber, out temp))
            {
                GenreNum = temp;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a Blank ID3v1 Class
        /// </summary>
        public ID3v1()
        {
            setToBlank();
        }
        public ID3v1(byte[] mp3file)
        {
            if (tagExist(mp3file))
                _fillFromFileBytes(mp3file);
            else
                setToBlank();

        }
        public ID3v1(string fileName)
        {
            setToBlank();
            string newFileName = removeTextWithinBrackets(fileName.Remove(fileName.LastIndexOf(".mp3")));
            int DashIndex = newFileName.IndexOf('-');
            if (DashIndex >= 0)
            {
                Artist = newFileName.Remove(DashIndex).Trim();
                Title = newFileName.Remove(0, DashIndex + 1).Trim();
            }
        }
        #endregion

        #region Static Functions

        public static int Length
        {
            get { return 128; }
        }

        public static bool tagExist(byte[] fileBytes)
        {
            try
            {
                if (File_Parsing.Equals(File_Parsing.getBytes(fileBytes, fileBytes.Length - ID3v1_LENGTH, HEADER.Length), File_Parsing.getBytes(HEADER)))
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }
        #endregion

        #region Public Functions
        public void setToBlank()
        {
            Title = "";
            Artist = "";
            Album = "";
            Year = "    ";
            Comment = "";
            TrackNum = 0;
            GenreNum = NO_GENRE;
        }

        public void fillFromFileBytes(byte[] fileBytes)
        {
            if (tagExist(fileBytes))
                _fillFromFileBytes(fileBytes);
        }

        public byte[] toByteArray()
        {
            List<byte[]> toJoin = new List<byte[]>(VARIABLES_TO_JOIN);
            toJoin.Add(File_Parsing.getBytes(HEADER));
            toJoin.Add(File_Parsing.getBytes(_title));
            toJoin.Add(File_Parsing.getBytes(_artist));
            toJoin.Add(File_Parsing.getBytes(_album));
            toJoin.Add(File_Parsing.getBytes(_year));
            toJoin.Add(File_Parsing.getBytes(_comment));
            byte[] temp = new byte[1];
            temp[0] = _zeroByte;
            toJoin.Add(temp);
            temp[0] = _trackNum;
            toJoin.Add(temp);
            temp[0] = _genreNum;
            toJoin.Add(temp);

            return File_Parsing.joinBytes(toJoin);
        }

        #endregion

        #region Helpers



        private void _fillFromFileBytes(byte[] fileBytes)
        {
            byte[] tag = File_Parsing.getBytes(fileBytes, fileBytes.Length - ID3v1_LENGTH);
            Title = File_Parsing.getString(File_Parsing.getBytes(tag, TITLE_START_INDEX, TITLE_ARTIST_ALBUM_LENGTH));
            Artist = File_Parsing.getString(File_Parsing.getBytes(tag, ARTIST_START_INDEX, TITLE_ARTIST_ALBUM_LENGTH));
            Album = File_Parsing.getString(File_Parsing.getBytes(tag, ALBUM_START_INDEX, TITLE_ARTIST_ALBUM_LENGTH));
            Year = File_Parsing.getString(File_Parsing.getBytes(tag, YEAR_START_INDEX, YEAR_LENGTH));
            Comment = File_Parsing.getString(File_Parsing.getBytes(tag, COMMENT_START_INDEX, COMMENT_LENGTH));
            if (tag[ZERO_BYTE_LOCATION] == TRACK_NUMBER_EXISTS)
                TrackNum = tag[TRACK_BYTE_LOCATION];
            else
                TrackNum = 0;
            GenreNum = tag[GENRE_BYTE_LOCATION];
        }
        private string removeTextWithinBrackets(string input)
        {
            int i;
            int end;
            bool doAgain;
            string toReturn = input;
            do
            {
                i = toReturn.IndexOf('(');
                end = toReturn.IndexOf(')');
                if (i >= 0 && end > i)
                {
                    doAgain = true;
                    if (toReturn[i-1] == ' ')
                        i--;
                    toReturn = toReturn.Remove(i, end - i + 1);
                }
                else
                    doAgain = false;
            } while (doAgain);
            do
            {
                i = toReturn.IndexOf('[');
                end = toReturn.IndexOf(']');
                if (i >= 0 && end > i)
                {
                    doAgain = true;
                    if (toReturn[i-1] == ' ')
                        i--;
                    toReturn = toReturn.Remove(i, end - i + 1);
                }
                else
                    doAgain = false;
            } while (doAgain);
            return toReturn;
        }



        //private string addPadding(string to_pad, int size)
        //{
        //    return to_pad.PadRight(size);
        //}

        #endregion

    }

}
