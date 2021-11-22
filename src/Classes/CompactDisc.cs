using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cassiopeia.src.Classes
{
    public enum SleeveType
    {
        Jewel, Digipack, MiniLP, NoStorage
    }
    public class CompactDisc : PhysicalAlbum
    {

        public List<Disc> Discos { get; set; }

        public SleeveType SleeveType { get; set; }
        public int TotalSongs => getNumSongs();

        public CompactDisc() : base()
        {

        }

        public CompactDisc(AlbumData album, string s, int nc, MediaCondition e, MediaCondition ee, SleeveType f, short y, string p): base(album, s, ee, y, p)
        {
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
            Array.Copy(AlbumData.ToStringArray(), d, 5);
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
    }
}
