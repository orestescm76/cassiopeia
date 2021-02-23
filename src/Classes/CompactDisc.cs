using System;
using Newtonsoft.Json;

namespace Cassiopea
{
    public enum SleeveType
    {
        Jewel, Digipack, MiniLP, NoStorage
    }
    public class CompactDisc : AlbumFisico
    {
        public String Id { get; set; }
        public Disc[] Discos { get; set; }
        public String[] Anotaciones { get; set; }
        public SleeveType SleeveType { get; set; }

        public CompactDisc() : base()
        {

        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType sleeveType) : base(s, ee)
        {
            SleeveType = sleeveType;
            Discos = new Disc[1];
            Discos[0] = new Disc(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());//porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType f, int nCD) : base(s, ee)
        {
            SleeveType = f;
            Discos = new Disc[nCD];
            Discos[0] = new Disc(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, MediaCondition ee, SleeveType f, int nCD) : base(s, ee)
        {
            SleeveType = f;
            Discos = new Disc[nCD];
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public CompactDisc(string s, int nCD) : base(s)
        {
            Discos = new Disc[nCD];
        }

        public CompactDisc(string s, int nc, MediaCondition e, MediaCondition ee, SleeveType f, short y, string p): base(s, ee, y, p)
        {
            SleeveType = f;
            Discos = new Disc[1];
            Discos[0] = new Disc(nc, e);
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

        public void InstallAlbum()
        {
            Album = Program.Collection.GetAlbum(Artist + "_" + Title);
        }
    }
}
