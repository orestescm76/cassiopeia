﻿using System;

namespace aplicacion_musica
{
    public class Album
    {
        public Album(Genero g, string n = "", string a = "", short y = 0, short nc = 0, string c = "")
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
        public String[] toStringArray()
        {
            String[] datos = { artista, nombre, year.ToString(), duracion.ToString(), genero.traducido };
            return datos;
        }
        private string getID()
        {
            return artista + nombre + year + genero.traducido;
        }
        public bool sonIguales(Album otro)
        {
            if (getID() == otro.getID())
                return true;
            else return false;
        }
        public int buscarCancion(string t)
        {
            int i = 0;
            while (t != canciones[i].titulo)
                i++;
            return i;
        }
        public void RefrescarDuracion()
        {
            duracion = new TimeSpan();
            for (int i = 0; i < canciones.Length; i++)
            {
                duracion += canciones[i].duracion;
            }
        }
        public String nombre{ get; set; }
        public String artista { get; set; }
        public short year { get; set; }
        public short numCanciones { get; set; }
        public TimeSpan duracion { get; set; }
        public Cancion[] canciones { get; set; }
        public String caratula { get; set; }
        public Genero genero { get; set; }
        public Cancion getCancion(int n) { return canciones[n]; }
    }
}