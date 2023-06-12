using System;
using System.Drawing;
using System.IO;
using File = TagLib.File;

namespace Cassiopeia.src.Classes
{
    public class LocalSong : Song
    {
        private readonly File songFile;

        public string Artist { get; private set; }
        public int TrackNumber { get; private set; }
        public string Album { get; private set; }
        public int Year { get; private set; }
        public Image AlbumCover { get; set; }
        public LocalSong(string filePath)
        {
            try
            {
                //This new library doesn't care about the file.
                songFile = File.Create(filePath);
                Path = filePath;
            }
            catch (Exception ex)
            {
                Log.Instance.PrintMessage("Couldn't open the file. " + ex.Message, MessageType.Warning);
                return;
            }
            Artist = songFile.Tag.FirstPerformer;
            Title = songFile.Tag.Title;
            Album = songFile.Tag.Album;
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
                        AlbumCover = Image.FromStream(memoryStream);
                    }
                }
                catch (Exception)
                {
                    Log.Instance.PrintMessage("Cannot extract the internal cover.", MessageType.Warning);
                    AlbumCover = null;
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
        public override string ToString()
        {
            return Artist + " - " + Title;
        }
    }
}