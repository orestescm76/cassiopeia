using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace aplicacion_musica
{
    public class Album
    {
        public String Title { get; set; }
        public String Artist { get; set; }
        public short Year { get; set; }

        [JsonIgnore] public int NumberOfSongs { 
            get
            {
                return Songs.Count;
            }
        }

        [JsonIgnore] public TimeSpan Lenght { get; set; }

        public List<Cancion> Songs { get; set; }
        public String caratula { get; set; }
        public Genre genero { get; set; }
        public String IdSpotify { get; set; }
        public String DirectorioSonido { get; set; }
        [JsonIgnore]
        public bool PuedeBorrarse { get; private set; }

        public Album()
        {
            Songs = new List<Cancion>();
            genero = Programa.genres.Last();
        }

        public Album(Genre g, string n = "", string a = "", short y = 0, string c = "")
        {
            Lenght = new TimeSpan();
            Title = n;
            Artist = a;
            Year = y;
            Songs = new List<Cancion>();
            caratula = c;
            genero = g;
            PuedeBorrarse = true;
        }

        public Album(string n = "", string a = "", short y = 0, string c = "")
        {
            Lenght = new TimeSpan();
            Title = n;
            Artist = a;
            Year = y;
            caratula = c;
            genero = new Genre("");
            PuedeBorrarse = true;
        }

        public Album(Album a)
        {
            Lenght = a.Lenght;
            Title = a.Title;
            Artist = a.Artist;
            Year = a.Year;
            Songs = a.Songs;
            caratula = a.caratula;
            PuedeBorrarse = true;
        }

        public void agregarCancion(Cancion c)
        {
            Songs.Add(c);

            if (!c.Bonus)
                Lenght += c.duracion;
        }

        public void agregarCancion(Cancion c, int cual)
        {
            Songs.Insert(cual, c);

            if (!c.Bonus)
                Lenght += c.duracion;
        }

        public String[] ToStringArray()
        {
            String[] datos = { Artist, Title, Year.ToString(), Lenght.ToString(), genero.Name };
            return datos;
        }

        public String[] SongsToStringArray()
        {
            String[] datos = new string[NumberOfSongs];
            for (int i = 0; i < Songs.Count; i++)
            {
                datos[i] = Songs[i].titulo;
            }
            return datos;
        }

        private string getID()
        {
            return Artist + Title;
        }

        public bool sonIguales(Album otro)
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

        public Cancion DevolverCancion(string t)
        {
            Cancion c = null;
            int i = 0;
            c = Songs[i];
            while (t != Songs[i].titulo)
            {
                i++;
                c = Songs[i];
            }

            return c;
        }
        public void RefrescarDuracion()
        {
            Lenght = new TimeSpan();
            for (int i = 0; i < Songs.Count; i++)
            {
                if (!Songs[i].Bonus)
                    Lenght += Songs[i].duracion;
            }
        }
        public Cancion getCancion(int n)
        {
            return Songs[n];
        }

        public Cancion getCancion(String b)
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
            return Artist + " - " + Title + "(" + Lenght + ") (" + genero.Name + ")";
        }

        public void BorrarCancion(int cual)
        {
            if (!Songs[cual].Bonus)
                Lenght -= Songs[cual].duracion;

            Songs.RemoveAt(cual);
        }

        public void BorrarCancion(Cancion cancion)
        {
            if (!cancion.Bonus)
                Lenght -= cancion.duracion;

            Songs.Remove(cancion);
        }

        public void ConfigurarCanciones()
        {
            foreach (Cancion cancion in Songs)
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
        public void ProtegerBorrado()
        {
            PuedeBorrarse = false;
        }
        public void LevantarBorrado()
        {
            PuedeBorrarse = true;
        }
        public void QuitarCancion(Cancion c)
        {
            Songs.Remove(c);
            Lenght -= c.duracion;
        }
        public string GetPortapapeles()
        {
            string val = Config.Portapapeles.Replace("%artist%", Artist); //Es seguro.
            try
            {
                val = val.Replace("%title%", Title);
                val = val.Replace("%year%", Year.ToString());
                val = val.Replace("%genre%", genero.Name);
                val = val.Replace("%length%", Lenght.ToString());
                val = val.Replace("%length_seconds%", ((int)Lenght.TotalSeconds).ToString());
                return val;
            }
            catch (NullReferenceException)
            {
                return val;
            }
        }
    }
}