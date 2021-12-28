using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cassiopeia.src.Classes
{
    public enum SleeveType
    {
        Jewel, Digipack, MiniLP, NoStorage
    }
    public class CompactDisc : IPhysicalAlbum
    {

        public List<Disc> Discos { get; set; }

        public SleeveType SleeveType { get; set; }
        public int TotalSongs => getNumSongs();

        public MediaCondition SleeveCondition { get; set; }
        public short Year { get; set; }
        public string Country { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string[] Anotaciones { get; set; }
        public string Id { get; set; }

        public TimeSpan Length => GetLength();

        public AlbumData Album { get; private set; }

        public CompactDisc() : base()
        {

        }
        /*
         *         public IPhysicalAlbum(AlbumData a, string s, MediaCondition estado, short y = 0, string p = null)
        {
            AlbumData = a;
            EstadoExterior = estado;
            Year = y;
            Country = p;
            Artist = AlbumData.Artist;
            Title = AlbumData.Title;
        }*/

        public CompactDisc(AlbumData album, int nc, MediaCondition e, MediaCondition ee, SleeveType f, short y, string p)
        {
            Album = album;
            Artist = album.Artist;
            Title = album.Title;
            SleeveCondition = ee;
            Year = y;
            Country = p;
            SleeveType = f;
            Discos = new List<Disc>(1);
            Discos.Add(new Disc(nc, e));
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public String[] ToStringArray()
        {
            String[] d = new string[6];
            Array.Copy(Album.ToStringArray(), d, 5);
            d[5] = Id;
            return d;
        }

        private int getNumSongs()
        {
            int sum = 0;
            foreach (var disc in Discos)
            {
                sum += disc.NumberOfSongs;
            }
            return sum;
        }

        public void InstallAlbum()
        {
            Album = Kernel.Collection.GetAlbum(Artist + "/**/" + Title);
        }
        private TimeSpan GetLength()
        {
            TimeSpan time = TimeSpan.Zero;
            int index = 0;
            foreach (var disco in Discos)
            {
                for (int i = 0; i < disco.NumberOfSongs; i++)
                {
                    time += Album.Songs[index++].Length;
                }
            }
            return time;
        }
    }
}
