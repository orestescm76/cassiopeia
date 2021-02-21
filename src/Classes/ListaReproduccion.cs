using System;
using System.Collections.Generic;
using System.IO;


namespace aplicacion_musica
{
    public class ListaReproduccion
    {
        public List<Song> Canciones { get; private set; }
        public TimeSpan Duration { get => GetDuration(); }

        private TimeSpan GetDuration()
        {
            TimeSpan time = new TimeSpan();
            foreach (Song song in Canciones)
            {
                time += song.duracion;
            }
            return time;
        }

        public ListaReproduccion(String n)
        {
            Nombre = n;
            Canciones = new List<Song>();
        }
        public String Nombre { get; set; }
        public void AgregarCancion(Song c)
        {
            Canciones.Add(c);
        }
        public Song GetCancion(int cual) //¡sobre 0!
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
        private void Shuffle(List<Song> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Song valor = list[k];
                list[k] = list[n];
                list[n] = valor;
            }
        }
        public void Mezclar()
        {
            Shuffle(Canciones);
        }

        public Song this[int key]
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
                Writer.WriteLine(cancion.Path);
            }
            Writer.Flush();
        }
        public void Cargar(string path)
        {
            StreamReader reader = new StreamReader(path);
            try
            {
                Nombre = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    Song c = new Song();
                    c.PATH = reader.ReadLine();
                    Canciones.Add(c);
                }
            }
            catch (Exception)
            {
                Log.Instance.ImprimirMensaje("Error abriendo el fichero " + path, TipoMensaje.Error);
                throw;
            }
        }
    }
}
