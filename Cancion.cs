using System;

namespace aplicacion_musica
{
    public class Cancion
    {
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
        public Album album {get; protected set;}
        public string titulo { get; set; }
        public TimeSpan duracion { get; set; }
        public String[] toStringArray()
        {
            String[] datos = { titulo, duracion.ToString() };
            return datos;
        }
    }
}
