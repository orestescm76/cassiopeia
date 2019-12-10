using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aplicacion_ipo
{
    public class Album
    {
        public Album(string n = "", string a = "", short y = 0, short nc = 0, string g = "", string c = "")
        {
            duracion = new TimeSpan();
            nombre = n;
            artista = a;
            year = y;
            numCanciones = nc;
            canciones = new Cancion[nc];
            caratula = c;
            genero = g;
        }
        public Album(Album a)
        {
            duracion = a.duracion;
            nombre = a.nombre;
            artista = a.artista;
            year = a.year;
            numCanciones = a.numCanciones;
            canciones = a.canciones;
            caratula = a.caratula;
        }
        public void agregarCancion(Cancion c, int cual)
        {
            canciones[cual] = c;
            duracion += c.duracion;
        }
        public TimeSpan duracion { get; set; }
        public String nombre{ get; set; }
        public String artista { get; set; }
        public short year { get; set; }
        public short numCanciones { get; set; }
        public Cancion[] canciones { get; set; }
        public String caratula { get; set; }
        public String genero { get; set; }
    }
}
