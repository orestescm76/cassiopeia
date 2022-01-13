using System;
using System.Collections.Generic;

namespace Cassiopeia.src.Classes
{
    public class VinylAlbum : IPhysicalAlbum
    {
        public List<VinylDisc> DiscList { get; set; }
        public int TotalSongs => getNumSongs();
        public AlbumData Album { get; private set; }

        public MediaCondition SleeveCondition { get; set; }
        public short Year { get; set; }
        public string Country { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string[] Anotaciones { get; set; }
        public string Id { get; set; }

        public TimeSpan Length => GetLength();
        public VinylAlbum() : base()
        {

        }
        public VinylAlbum(AlbumData a, int ncFront, int ncBack, MediaCondition e, MediaCondition ee, short y, string p) : base()
        {
            Album = a;
            Artist = a.Artist;
            Title = a.Title;
            SleeveCondition = ee;
            Year = y;
            Country = p;
            DiscList = new();
            DiscList.Add(new VinylDisc(ncFront, ncBack, 'A', e));
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        public void AddDisc(VinylDisc disc)
        {
            DiscList.Add(disc);
        }
        private int getNumSongs()
        {
            int sum = 0;
            foreach (var disc in DiscList)
            {
                sum += disc.NumberOfSongs;
            }
            return sum;
        }
        public String[] ToStringArray()
        {
            String[] d = new string[5];
            Array.Copy(Album.ToStringArray(), d, 5);
            return d;
        }
        public void InstallAlbum()
        {
            Album = Kernel.Collection.GetAlbum(Artist + "/**/" + Title);
        }
        private TimeSpan GetLength()
        {
            TimeSpan time = TimeSpan.Zero;
            int index = 0;
            foreach (var disco in DiscList)
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
