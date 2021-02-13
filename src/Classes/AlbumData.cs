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
                if (!song.Bonus)
                    length += song.duracion;
            }

            return length;
        }

        public String[] ToStringArray()
        {
            String[] datos = { Artist, Title, Year.ToString(), Length.ToString(), Genre.Name };
            return datos;
        }

        private string getID()
        {
            return Artist + Title;
        }

        public bool Equals(AlbumData otro)
        {
            if (getID() == otro.getID())
                return true;
            else return false;
        }

        public int buscarCancion(string t)
        {
            int i = 0;
            while (t != Songs[i].titulo)
                i++;
            return i;
        }

        public Song DevolverCancion(string t)
        {
            Song c = null;
            int i = 0;
            c = Songs[i];
            while (t != Songs[i].titulo)
            {
                i++;
                c = Songs[i];
            }

            return c;
        }

        public Song getCancion(int n)
        {
            return Songs[n];
        }

        public Song getCancion(String b)
        {
            for (int i = 0; i < Songs.Count; i++)
            {
                if (b == Songs[i].titulo)
                    return Songs[i];
            }
            return null;
        }
        public override string ToString()
        {
            //artista - nombre (dur) (gen) 
            return Artist + " - " + Title + "(" + Length + ") (" + Genre.Name + ")";
        }

        public void BorrarCancion(int index)
        {
            Songs.RemoveAt(index);
        }

        public void RemoveSong(Song song)
        {
            Songs.Remove(song);
        }

        public void ConfigurarCanciones()
        {
            foreach (Song cancion in Songs)
            {
                cancion.SetAlbum(this);
            }
        }

        public void SetSpotifyID(string id)
        {
            IdSpotify = id;
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