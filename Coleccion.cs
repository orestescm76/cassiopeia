using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    while (linea == "") linea = lector.ReadLine();
                    if (linea == null) continue; //si no hay nada tu sigue, que hemos llegado al final del fichero, después del nulo porque siempre al terminar un disco pongo línea nueva.
                    string[] datos = linea.Split(';');
                    short nC = Convert.ToInt16(datos[3]);
                    int gen = Programa.findGenero(datos[4]);
                    Genero g = Programa.generos[gen];
                    if (datos[5] == null) datos[5] = "";
                    Album a = new Album(g, datos[0], datos[1], Convert.ToInt16(datos[2]), Convert.ToInt16(datos[3]), datos[5]);
                    bool exito = false;
                    for (int i = 0; i < nC; i++)
                    {
                        exito = false;
                        linea = lector.ReadLine();
                        if(linea == null || linea == "")
                        {
                            System.Windows.Forms.MessageBox.Show("mensajeError"+Environment.NewLine
                                + a.nombre + " - " + a.nombre + Environment.NewLine
                                + "saltarAlSiguiente", "error", System.Windows.Forms.MessageBoxButtons.OK);
                            break; //no sigue cargando el álbum
                        }
                        else
                        {
                            exito = true;
                            string[] datosCancion = linea.Split(';');
                            Cancion c = new Cancion(datosCancion[0], new TimeSpan(0, Convert.ToInt32(datosCancion[1]), Convert.ToInt32(datosCancion[2])));
                            a.agregarCancion(c, i);
                        }
                    }
                    if (estaEnColeccion(a))
                    {
                        exito = false; //pues ya está repetido.
                        Debug.WriteLine("Repetido");
                    }
                    if(exito)
                        Programa.miColeccion.agregarAlbum(ref a);
                }
            }
        }
        public void agregarAlbum(ref Album a) { albumes.Add(a); }
        public void quitarAlbum(ref Album a) { albumes.Remove(a); }
        public Album[] buscarAlbum(string titulo)
        {
            Album[] encontrados = new Album[0];
            foreach(Album a in albumes)
            {
                if (a.nombre == titulo)
                    encontrados[encontrados.Length] = a;
            }
            return encontrados;
        }
        public bool estaEnColeccion(Album a)
        {
            foreach(Album album in albumes)
            {
                if (album.sonIguales(a))
                    return true;
            }
            return false;
        }
        public Album devolverAlbum(string s)
        {
            String[] busqueda = s.Split(',');
            foreach (Album album in albumes)
            {
                if (album.artista == busqueda[0] && album.nombre == busqueda[1])
                    return album;
            }
            return null;
        }
    }
}
