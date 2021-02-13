﻿using System;
using System.Collections.Generic;
using System.IO;


namespace aplicacion_musica
{
    public class ListaReproduccion
    {
        public List<Cancion> Canciones { get; private set; }
        public ListaReproduccion(String n)
        {
            Nombre = n;
            Canciones = new List<Cancion>();
        }
        public String Nombre { get; set; }
        public void AgregarCancion(Cancion c)
        {
            Canciones.Add(c);
        }
        public Cancion GetCancion(int cual) //¡sobre 0!
        {
            return Canciones[cual];
        }
        public bool Final(int i)
        {
            if (Canciones.Count == (i-1))
                return true;
            else return false;
        }
        public bool Inicio(int i)
        {
            if (i == 0)
                return true;
            else if (i == -1)
                return true;
            else return false;
        }
        private void Shuffle(List<Cancion> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Cancion valor = list[k];
                list[k] = list[n];
                list[n] = valor;
            }
        }
        public void Mezclar()
        {
            Shuffle(Canciones);
        }

        public Cancion this[int key]
        {
            get => Canciones[key];
            set => Canciones[key] = value;
        }
        public void Guardar(string name)
        {
            StreamWriter Writer = new StreamWriter(name);
            Writer.WriteLine(Nombre);
            foreach (var cancion in Canciones)
            {
                Writer.WriteLine(cancion.PATH);
            }
        }
        public void Cargar(string path)
        {
            StreamReader reader = new StreamReader(path);
            while(!reader.EndOfStream)
            {
                Cancion c = new Cancion();
                c.PATH = reader.ReadLine();
                Canciones.Add(c);
            }
        }
    }
}
