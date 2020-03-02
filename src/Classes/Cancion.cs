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
        public String[] ToStringArray()
        {
            String[] datos = { titulo, duracion.ToString() };
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
    }
}
