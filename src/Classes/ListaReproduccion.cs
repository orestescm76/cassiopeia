using System;
using System.Collections.Generic;


namespace aplicacion_musica
{
    public class ListaReproduccion
    {
        public List<Cancion> Canciones { get; }
        public ListaReproduccion()
        {
            Nombre = "";
            Canciones = new List<Cancion>();
        }
        public String Nombre { get; set; }
        public void AgregarCancion(Cancion c)
        {
            Canciones.Add(c);
        }
        public Cancion GetCancion(uint cual) //¡sobre 0!
        {
            return Canciones[(int)cual];
        }
    }
}
