using System;
using Newtonsoft.Json;

namespace aplicacion_musica
{
    public class Cancion
    {
        [JsonIgnore]
        public Album album { get; protected set; }
        public string titulo { get; set; }
        [JsonConverter(typeof(TiempoConverter))]
        public TimeSpan duracion { get; set; }
        public bool Bonus { get; set; }
        [JsonIgnore]
        public String PATH { get; set; }
        public string[] Lyrics { get; set; }
        public int Num
        {
            get
            {
                return album.canciones.IndexOf(this)+1;
            }
            set
            {

            }
        }

        public Cancion()
        {
        }
        public Cancion(String titulo, int ms, bool Bonus)
        {
            this.titulo = titulo;
            duracion = new TimeSpan(0, 0, 0, 0, ms);
            this.Bonus = Bonus;
        }
        public Cancion(Cancion c)
        {
            titulo = c.titulo;
            album = c.album;
            duracion = c.duracion;
            Bonus = c.Bonus;
        }
        public Cancion(string t, TimeSpan d, ref Album a)
        {
            titulo = t;
            duracion = d;
            album = a;
        }
        public Cancion(string t, TimeSpan d, ref Album a, bool b)
        {
            titulo = t;
            duracion = d;
            album = a;
            Bonus = b;
        }
        public Cancion(string path) //Crea una canción fantasma con sólo un PATH
        {
            this.PATH = path;
        }
        public override string ToString()
        {
            if (album != null)
                return album.artista + " - " + titulo + " (" + album.nombre + ")";
            else
                return titulo;
        }
        public String[] ToStringArray()
        {
            String[] datos;
            if (duracion.TotalMinutes>=60)
                datos = new string[] { titulo, duracion.ToString(@"h\:mm\:ss") };
            else
                datos = new string[] { titulo, duracion.ToString(@"mm\:ss") };
            return datos;
        }
        public int GetMilisegundos()
        {
            return Convert.ToInt32(duracion.TotalMilliseconds);
        }
        public void SetAlbum(Album a)
        {
            album = a;
        }
        //Tame Impala;The Less I Know The Better;Currents
        public String GuardarPATH()
        {
            return album.artista+";"+titulo+";"+album.nombre + Environment.NewLine+PATH + Environment.NewLine;
        }
    }
}
