using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aplicacion_ipo
{
    public class Cancion
    {
        public Cancion(string t, TimeSpan d)
        {
            titulo = t;
            duracion = d;
        }

        public string titulo { get; set; }
        public TimeSpan duracion { get; set; }
        public String[] toStringArray()
        {
            String[] datos = { titulo, duracion.ToString() };
            return datos;
        }
    }

}
