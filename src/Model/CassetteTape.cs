using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassiopeia.src.Classes
{
    public class CassetteTape : IPhysicalAlbum
    {
        public CassetteTape()
        {

        }
        public CassetteTape(AlbumData a, int ncFront, int ncBack, MediaCondition e, MediaCondition ee, short y, string p) : base()
        {
            Album = a;
            Title = a.Title;
            Artist = a.Artist;
            NumSongsBack = ncBack;
            NumSongsFront = ncFront;
            SleeveCondition = ee;
            MediaCondition = e;
            Year = y;
            Country = p;
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        public int NumSongsFront { get; set; }
        public int NumSongsBack { get; set; }
        public int TotalSongs => getNumSongs();
            private int getNumSongs()
        {
            return NumSongsBack + NumSongsFront;
        }
        public AlbumData Album { get; private set; }

        public MediaCondition SleeveCondition { get; set; }
        public MediaCondition MediaCondition { get; set; }
        public short Year { get; set; }
        public string Country { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string[] Anotaciones { get; set; }
        public string Id { get; set; }

        public TimeSpan Length => GetLength();

        private TimeSpan GetLength()
        {
            TimeSpan time = TimeSpan.Zero;
            int index = 0;
            for (int i = 0; i < NumSongsBack + NumSongsFront; i++)
            {
                time += Album.Songs[index++].Length;
            }
            return time;
        }

        public void InstallAlbum()
        {
            Album = null;//Kernel.Collection.GetAlbum(Artist + Kernel.SearchSeparator + Title);
        }
        public String[] ToStringArray()
        {
            String[] d = new string[5];
            Array.Copy(Album.ToStringArray(), d, 5);
            return d;
        }
        
    }
}
