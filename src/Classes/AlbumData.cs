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
        public List<Song> Songs { get; set; }
        public String Cover { get; set; }
        public Genre Genre { get; set; }
        public String IdSpotify { get; set; }
        public String SoundFilesPath { get; set; }

        [JsonIgnore] public int NumberOfSongs { get { return Songs.Count; } }
        [JsonIgnore] public TimeSpan Length { get => GetLength(); }
        [JsonIgnore] public bool CanBeRemoved { get; set; }

        public AlbumData()
        {
            Songs = new List<Song>();
            Genre = Programa.genres.Last();
        }

        public AlbumData(Genre g, string n = "", string a = "", short y = 0, string c = "")
        {
            Title = n;
            Artist = a;
            Year = y;
            Songs = new List<Song>();
            Cover = c;
            Genre = g;
            CanBeRemoved = true;
        }

        public AlbumData(string n = "", string a = "", short y = 0, string c = "")
        {
            Title = n;
            Artist = a;
            Year = y;
            Cover = c;
            Genre = new Genre("");
            CanBeRemoved = true;
        }

        public AlbumData(AlbumData a)
        {
            Title = a.Title;
            Artist = a.Artist;
            Year = a.Year;
            Songs = a.Songs;
            Cover = a.Cover;
            CanBeRemoved = true;
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void AddSong(Song song, int index)
        {
            Songs.Insert(index, song);
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

        public String[] ToStringArray()
        {
            String[] datos = { Artist, Title, Year.ToString(), Length.ToString(), Genre.Name };
            return datos;
        }

        private string GetID()
        {
            return Artist + Title;
        }

        public override bool Equals(Object other)
        {
            AlbumData albumData = other as AlbumData;
            return GetID() == albumData.GetID();
        }

        public static bool operator ==(AlbumData leftAlbumData, AlbumData rightAlbumData)
        {
            return leftAlbumData.GetID() == rightAlbumData.GetID();
        }

        public static bool operator !=(AlbumData leftAlbumData, AlbumData rightAlbumData)
        {
            return !(leftAlbumData == rightAlbumData);
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

        public Song getCancion(int n)
        {
            return Songs[n];
        }

        public override string ToString()
        {
            //artista - nombre (dur) (gen) 
            return Artist + " - " + Title + "(" + Length + ") (" + Genre.Name + ")";
        }

        public void RemoveSong(string title)
        {
            Song song = GetSong(title);
            Songs.Remove(song);
        }

        public void ConfigurarCanciones()
        {
            foreach (Song cancion in Songs)
            {
                cancion.SetAlbum(this);
            }
        }

        public String GetTerminoBusqueda()
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