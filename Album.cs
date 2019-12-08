using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aplicacion_ipo
{
    class Album
    {
        Album(TimeSpan dur = new TimeSpan(), string n = "", string a = "", short y = 0, short nc = 0)
        {
            duracion = dur;
            nombre = n;
            artista = a;
            year = y;
            numCanciones = nc;
            canciones = new Cancion[nc];
        }
        Album(Album a)
        {
            duracion = a.duracion;
            nombre = a.nombre;
            artista = a.artista;
            year = a.year;
            numCanciones = a.numCanciones;
            canciones = a.canciones;
        }
        void agregarCancion(Cancion c)
        {
            canciones[canciones.Length] = c;
            duracion += c.duracion;
        }
        public TimeSpan duracion { get; set; }
        public String nombre{ get; set; }
        public String artista { get; set; }
        public short year { get; set; }
        public short numCanciones { get; set; }
        public Cancion[] canciones { get; set; }
    }
}
