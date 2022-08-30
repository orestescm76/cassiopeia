﻿using System;
using System.Drawing;
using System.IO;
using File = TagLib.File;

namespace Cassiopeia.src.Classes
{
    public class MetadataSong
    {
        private readonly File songFile;

        public string Artist { get; private set; }
        public string Title { get; private set; }
        public int TrackNumber { get; private set; }
        public string AlbumFrom { get; private set; }
        public int Year { get; private set; }
        public Image Cover { get; set; }
        public TimeSpan Length { get; private set; }

        public MetadataSong(string filePath)
        {
            try
            {
                //This new library doesn't care about the file.
                songFile = File.Create(filePath);

            }
            catch (Exception ex)
            {
                Log.Instance.PrintMessage("Couldn't open the file. " + ex.Message, MessageType.Warning);
                return;
            }
            Artist = songFile.Tag.FirstPerformer;
            Title = songFile.Tag.Title;
            AlbumFrom = songFile.Tag.Album;
            TrackNumber = (int)songFile.Tag.Track;
            Length = songFile.Properties.Duration;
            Year = (int)songFile.Tag.Year;

            if (songFile.Tag.Pictures.Length > 0)
            {
                try
                {
                    byte[] imageData = songFile.Tag.Pictures[0].Data.Data;

                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        Cover = Image.FromStream(memoryStream);
                    }
                }
                catch (Exception)
                {
                    Log.Instance.PrintMessage("Cannot extract the internal cover.", MessageType.Warning);
                    Cover = null;
                }
            }

            Log.Instance.PrintMessage(filePath + " successfully read!", MessageType.Correct);
        }

        public bool Evaluable()
        {
            return !(string.IsNullOrEmpty(Artist)) && !(string.IsNullOrEmpty(Title));
        }

        public string GetSongID()
        {
            return Artist + " - " + Title;
        }

        public void Dispose()
        {
            songFile.Dispose();
        }
    }
}