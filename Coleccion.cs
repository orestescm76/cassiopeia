using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aplicacion_ipo
{
    class Coleccion
    {
        public List<Album> albumes { get; }
        public Coleccion()
        {
            albumes = new List<Album>();
        }
        public void cargarAlbumes(string fichero)
        {
            using(StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while(!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    if (linea == "") linea = lector.ReadLine(); //si la linea es vacia sigue leyendo, es un separador para facilitar lectura
                    if (linea == null) continue; //si no hay nada tu sigue, que hemos llegado al final del fichero, después del nulo porque siempre al terminar un disco pongo línea nueva.
                    string[] datos = linea.Split(';');
                    short nC = Convert.ToInt16(datos[3]);
                    if (datos[5] == null) datos[5] = "";
                    Album a = new Album(datos[0], datos[1], Convert.ToInt16(datos[2]), Convert.ToInt16(datos[3]), datos[4], datos[5]);
                    for (int i = 0; i < nC; i++)
                    {
                        linea = lector.ReadLine();
                        string[] datosCancion = linea.Split(';');
                        Cancion c = new Cancion(datosCancion[0], new TimeSpan(0,Convert.ToInt32(datosCancion[1]), Convert.ToInt32(datosCancion[2])));
                        a.agregarCancion(c, i);
                    }
                    Programa.miColeccion.agregarAlbum(ref a);
                }
            }
        }
        public void agregarAlbum(ref Album a) { albumes.Add(a); }

    }
}
