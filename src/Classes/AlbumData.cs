using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace aplicacion_musica
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
            Genre = Programa.genres.Last();
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

        private TimeSpan GetLength()
        {
            TimeSpan length = new TimeSpan();

            foreach (Song song in Songs)
            {
                if (!song.IsBonus)
                    length += song.duracion;
            }

            return length;
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
                if (s.titulo.Equals(title))
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
                if (Songs[i].titulo.Equals(title))
                {
                    songPos = i;
                    break;
                }
            }

            return songPos;
        }

        //---DATA---
        public override string ToString()
        {
            //Artist - Title (Length) (Genre) 
            return Artist + " - " + Title + "(" + Length + ") (" + Genre.Name + ")";
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

        public string GetPortapapeles()
        {
            string val = Config.Portapapeles.Replace("%artist%", Artist); //Es seguro.
            try
            {
                val = val.Replace("%title%", Title);
                val = val.Replace("%year%", Year.ToString());
                val = val.Replace("%genre%", Genre.Name);
                val = val.Replace("%length%", Length.ToString());
                val = val.Replace("%length_seconds%", ((int)Length.TotalSeconds).ToString());
                return val;
            }
            catch (NullReferenceException)
            {
                return val;
            }
        }
    }
}