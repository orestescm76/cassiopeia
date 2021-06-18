using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cassiopeia
{
    public class AlbumData
    {
        public String Title { get; set; }
        public String Artist { get; set; }
        public short Year { get; set; }
        public Genre Genre { get; set; }

        public List<Song> Songs { get; set; }

        public String ID { get => Title+Artist; }
        public String IdSpotify { get; set; }

        public String CoverPath { get; set; }
        public String SoundFilesPath { get; set; }

        [JsonIgnore] public int NumberOfSongs { get { return Songs.Count; } }
        [JsonIgnore] public TimeSpan Length { get => GetLength(); }
        [JsonIgnore] public bool CanBeRemoved { get; set; }

        public AlbumData()
        {
            Songs = new List<Song>();
            Genre = Kernel.Genres.Last();
        }

        public AlbumData(Genre genre, string title = "", string artist = "", short year = 0, string coverPath = "")
        {
            Title = title;
            Artist = artist;
            Year = year;
            Songs = new List<Song>();
            CoverPath = coverPath;
            Genre = genre;
            CanBeRemoved = true;
        }

        public AlbumData(string title = "", string artist = "", short year = 0, string coverPath = "")
        {
            Title = title;
            Artist = artist;
            Year = year;
            CoverPath = coverPath;
            Genre = new Genre("");
            Songs = new List<Song>();
            CanBeRemoved = true;
        }

        public AlbumData(AlbumData other)
        {
            Title = other.Title;
            Artist = other.Artist;
            Year = other.Year;
            Songs = other.Songs;
            CoverPath = other.CoverPath;
            CanBeRemoved = true;
        }
        //---COMPARISON---
        public override bool Equals(Object other)
        {
            AlbumData albumData = other as AlbumData;
            return ID == albumData.ID;
        }

        public static bool operator ==(AlbumData leftAlbumData, AlbumData rightAlbumData)
        {
            return leftAlbumData.ID == rightAlbumData.ID;
        }

        public static bool operator !=(AlbumData leftAlbumData, AlbumData rightAlbumData)
        {
            return !(leftAlbumData == rightAlbumData);
        }

        //---SONGS MANAGEMENT---
        public void AddSong(Song song)
        {
            Songs.Add(song);
            song.SetAlbum(this);
        }

        public void AddSong(Song song, int index)
        {
            Songs.Insert(index, song);
            song.SetAlbum(this);
        }

        public void RemoveSong(string title)
        {
            Song song = GetSong(title);
            Songs.Remove(song);
        }

        public Song GetSong(string title)
        {
            Song song = null;

            foreach (Song s in Songs)
            {
                if (s.Title.Equals(title))
                {
                    song = s;
                    break;
                }
            }

            return song;
        }

        public Song GetSong(int n)
        {
            return Songs[n];
        }

        public int GetSongPosition(string title)
        {
            int songPos = -1;

            for (int i = 0; i < Songs.Count; i++)
            {
                if (Songs[i].Title.Equals(title))
                {
                    songPos = i;
                    break;
                }
            }

            return songPos;
        }

        //---DATA---
        private TimeSpan GetLength()
        {
            TimeSpan length = new TimeSpan();

            foreach (Song song in Songs)
            {
                if (!song.IsBonus)
                    length += song.Length;
            }

            return length;
        }

        public override string ToString()
        {
            //Returns whatever the clipboard string is.
            return ToClipboard();
        }

        public String[] ToStringArray()
        {
            String[] datos = { Artist, Title, Year.ToString(), Length.ToString(), Genre.Name };
            return datos;
        }

        public String GetSpotifySearchLabel()
        {
            return Artist + " " + Title;
        }

        public string ToClipboard()
        {
            string clipboardText = Config.Clipboard.Replace("%artist%", Artist); //Es seguro.

            try
            {
                clipboardText = clipboardText.Replace("%title%", Title);
                clipboardText = clipboardText.Replace("%year%", Year.ToString());
                clipboardText = clipboardText.Replace("%genre%", Genre.Name);
                clipboardText = clipboardText.Replace("%length%", Length.ToString());
                clipboardText = clipboardText.Replace("%length_seconds%", ((int)Length.TotalSeconds).ToString());
                return clipboardText;
            }
            catch (NullReferenceException)
            {
                return clipboardText;
            }
        }

        public bool NeedsMetadata()
        {
            if (string.IsNullOrEmpty(Artist) || string.IsNullOrEmpty(Title))
                return true;
            else return false;
        }
    }
}