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
        public override string ToString()
        {
            if (album != null)
                return album.artista + " - " + titulo + " (" + album.nombre + ")";
            else
                return titulo;
        }
        public String[] ToStringArray()
        {
            String[] datos = { titulo, duracion.ToString(@"mm\:ss") };
            return datos;
        }
        public int GetMilisegundos()
        {
            return Convert.ToInt32(duracion.TotalMilliseconds);
        }
        public int GetBonus()
        {
            return Bonus ? 1 : 0;
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
