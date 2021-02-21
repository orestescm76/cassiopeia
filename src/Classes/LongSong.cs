using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    public class LongSong : Song
    {
        public List<Song> Parts { get; private set; }

        public override TimeSpan Length { 
            get 
            {
                TimeSpan timeSpan = new TimeSpan();

                foreach (Song song in Parts)
                {
                    timeSpan += song.Length;
                }

                return timeSpan;
            }         
        }

        public LongSong() 
        {
            Parts = new List<Song>();
        }

        public LongSong(string title, AlbumData album)
        {
            Title = title;
            AlbumFrom = album;
            Parts = new List<Song>();
        }

        public void AddPart(Song p)
        {
            Parts.Add(p);
        }
    }
}
