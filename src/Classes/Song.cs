using System;
using Newtonsoft.Json;

namespace Cassiopea
{
    public class Song
    {
        [JsonIgnore] public AlbumData AlbumFrom { get; protected set; }
        public string Title { get; set; }
        [JsonConverter(typeof(TiempoConverter))] public virtual TimeSpan Length { get; set; }
        public bool IsBonus { get; set; }

        [JsonIgnore] public string Path { get; set; }
        public string[] Lyrics { get; set; }

        public int IndexInAlbum { get => AlbumFrom.Songs.IndexOf(this) + 1; set { } }

        public Song()
        {
        }

        public Song(String titulo, int ms, bool Bonus)
        {
            this.Title = titulo;
            Length = new TimeSpan(0, 0, 0, 0, ms);
            this.IsBonus = Bonus;
        }

        public Song(Song c)
        {
            Title = c.Title;
            AlbumFrom = c.AlbumFrom;
            Length = c.Length;
            IsBonus = c.IsBonus;
        }

        public Song(string t, TimeSpan d, ref AlbumData a)
        {
            Title = t;
            Length = d;
            AlbumFrom = a;
        }

        public Song(string t, TimeSpan d, ref AlbumData a, bool b)
        {
            Title = t;
            Length = d;
            AlbumFrom = a;
            IsBonus = b;
        }

        public override string ToString()
        {
            if (!ReferenceEquals(AlbumFrom, null))
                return AlbumFrom.Artist + " - " + Title + " (" + AlbumFrom.Title + ")";
            else
                return Title;
        }

        public String[] ToStringArray()
        {
            String[] datos;

            string length = Length.TotalMinutes >= 60 ? @"h\:mm\:ss" : @"mm\:ss";

            datos = new string[] { Title, Length.ToString(length) };

            return datos;
        }

        public int LengthMiliseconds()
        {
            return Convert.ToInt32(Length.TotalMilliseconds);
        }

        public void SetAlbum(AlbumData a)
        {
            AlbumFrom = a;
        }

        //Tame Impala;The Less I Know The Better;Currents
        public String GuardarPATH()
        {
            return AlbumFrom.Artist+";"+Title+";"+AlbumFrom.Title + Environment.NewLine+Path + Environment.NewLine;
        }
    }
}
