using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cassiopeia
{
    public enum SleeveType
    {
        Jewel, Digipack, MiniLP, NoStorage
    }
    public class CompactDisc : PhysicalAlbum
    {
        public String Id { get; set; }
        public List<Disc> Discos { get; set; }
        public String[] Anotaciones { get; set; }
        public SleeveType SleeveType { get; set; }
        public int TotalSongs { get => getNumSongs(); }
        public CompactDisc() : base()
        {

        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType sleeveType) : base(s, ee)
        {
            SleeveType = sleeveType;
            Discos = new List<Disc>(1);
            Discos[0] = new Disc(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());//porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType f, int nCD) : base(s, ee)
        {
            SleeveType = f;
            Discos = new List<Disc>(nCD);
            Discos[0] = new Disc(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, MediaCondition ee, SleeveType f, int nCD) : base(s, ee)
        {
            SleeveType = f;
            Discos = new List<Disc>(nCD);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, int nCD) : base(s)
        {
            Discos = new List<Disc>(nCD);
        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType f, short y, string p): base(s, ee, y, p)
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

        public void InstallAlbum()
        {
            AlbumData = Kernel.Collection.GetAlbum(Artist + "_" + Title);
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
