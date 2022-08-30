using System;
using System.Collections.Generic;
using System.IO;


namespace Cassiopeia.src.Classes
{
    public class Playlist
    {
        public List<Song> Songs { get; private set; }
        public TimeSpan Length { get => GetLength(); }
        public string Name { get; set; }

        private TimeSpan GetLength()
        {
            TimeSpan time = new TimeSpan();

            foreach (Song song in Songs)
            {
                time += song.Length;
            }

            return time;
        }

        public Playlist(string name)
        {
            Name = name;
            Songs = new List<Song>();
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void SetSongAt(Song value, int index)
        {
            if (index >= 0 && index < Songs.Count)
                Songs[index] = value;
            else
                Log.Instance.PrintMessage("Trying to set a song in an unexistant position, in playlist " + Name + ", at index " + index, MessageType.Error);
        }

        public Song GetSong(int index) //¡sobre 0!
        {
            if (index >= 0 && index < Songs.Count)
                return Songs[index];
            else
                Log.Instance.PrintMessage("Trying to get an unexistant song from playlist " + Name + ", at index " + index, MessageType.Error);

            return null;
        }

        public void RemoveSong(Song song)
        {
            if (!Songs.Remove(song))
            {
                Log.Instance.PrintMessage("Trying to remove an unexistant song from playlist " + Name, MessageType.Error);
            }
        }

        public void RemoveSong(int index)
        {
            if (index >= 0 && index < Songs.Count)
                Songs.RemoveAt(index);
            else
                Log.Instance.PrintMessage("Trying to remove an unexistant song from playlist " + Name, MessageType.Error);
        }

        public bool IsOutside(int index)
        {
            return index == Songs.Count;
        }

        public bool IsFirstSong(int i)
        {
            return (i == 0 || i == -1);
        }

        public void Shuffle()
        {
            Random rng = new Random();

            int n = Songs.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Song aux = Songs[k];
                Songs[k] = Songs[n];
                Songs[n] = aux;
            }
        }

        public Song this[int index]
        {
            get => GetSong(index);

            set => SetSongAt(value, index);
        }

        public void Save(string name)
        {
            //@ToDo: check if playlist already exists and ask about overwriting
            StreamWriter writer = new StreamWriter(name);
            writer.WriteLine(Name);

            foreach (var song in Songs)
            {
                writer.WriteLine(song.Path);
            }

            writer.Flush();
        }

        public void Load(string path)
        {
            StreamReader reader = new StreamReader(path);

            try
            {
                Name = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    Song song = new Song
                    {
                        Path = reader.ReadLine()
                    };

                    Songs.Add(song);
                }
            }
            catch (Exception)
            {
                Log.Instance.PrintMessage("Error opening file " + path, MessageType.Error);
                throw;
            }
        }
    }

}