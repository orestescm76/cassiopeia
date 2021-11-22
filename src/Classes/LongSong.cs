using System;
using System.Collections.Generic;

namespace Cassiopeia.src.Classes
{
    public class LongSong : Song
    {
        public List<Song> Parts { get; private set; }

        public void AddPart(Song p)
        {
            Parts.Add(p);
        }

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
    }

}