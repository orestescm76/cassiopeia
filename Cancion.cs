using System;

namespace aplicacion_musica
{
    public class Cancion
    {
        public Album album { get; protected set; }
        public string titulo { get; set; }
        public TimeSpan duracion { get; set; }
        public bool Bonus { get; set; }
        public Cancion()
        {
            titulo = "";
            duracion = new TimeSpan(0, 0, 0);
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
    }
}
