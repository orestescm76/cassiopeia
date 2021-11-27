using System;
using System.Collections.Generic;
/*
namespace Cassiopeia.src.Classes
{
    public class VinylAlbum : PhysicalAlbum
    {
        public List<VinylDisc> discList;
        public int TotalSongs => getNumSongs();
        public VinylAlbum(AlbumData a, string s, int ncFront, int ncBack, MediaCondition e, MediaCondition ee, short y, string p) : base(a, s, ee, y, p)
        {
            discList = new List<VinylDisc>(1);
            discList.Add(new VinylDisc(ncFront, ncBack, 'A', ee));
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public string[] ToStringArray()
        {
            String[] d = new string[6];
            Array.Copy(AlbumData.ToStringArray(), d, 5);
            d[5] = Id;
            return d;
        }

        private int getNumSongs()
        {
            int sum = 0;
            foreach (var disc in discList)
            {
                sum += disc.NumberOfSongs;
            }
            return sum;
        }
    }
}
*/