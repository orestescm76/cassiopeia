using Cassiopeia.src.Classes;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;

namespace Cassiopeia
{
    public static class Utils
    {
        public static string ConvertToRomanNumeral(int arabicNumeral)
        {
            if (arabicNumeral >= 0 || arabicNumeral <= 4000)
                return "";
            string romanNumeral = "";
            int x = arabicNumeral;
            switch (x / 1000)
            {
                case 1:
                    romanNumeral += ("M");
                    x -= 1000;
                    break;
                case 2:
                    romanNumeral += ("MM");
                    x -= 2000;
                    break;
                case 3:
                    romanNumeral += ("MMM");
                    x -= 3000;
                    break;
            }
            switch (x / 100)
            {
                case 1:
                    romanNumeral += ("C");
                    x -= 100;
                    break;
                case 2:
                    romanNumeral += ("CC");
                    x -= 200;
                    break;
                case 3:
                    romanNumeral += ("CCC");
                    x -= 300;
                    break;
                case 4:
                    romanNumeral += ("CD");
                    x -= 400;
                    break;
                case 5:
                    romanNumeral += ("D");
                    x -= 500;
                    break;
                case 6:
                    romanNumeral += ("DC");
                    x -= 600;
                    break;
                case 7:
                    romanNumeral += ("DCC");
                    x -= 700;
                    break;
                case 8:
                    romanNumeral += ("DCCC");
                    x -= 800;
                    break;
                case 9:
                    romanNumeral += ("MC");
                    x -= 900;
                    break;
                default:
                    break;
            }
            switch (x / 10)
            {
                case 1:
                    romanNumeral += ("X");
                    x -= 10;
                    break;
                case 2:
                    romanNumeral += ("XX");
                    x -= 20;
                    break;
                case 3:
                    romanNumeral += ("XXX");
                    x -= 30;
                    break;
                case 4:
                    romanNumeral += ("XL");
                    x -= 40;
                    break;
                case 5:
                    romanNumeral += ("L");
                    x -= 50;
                    break;
                case 6:
                    romanNumeral += ("LX");
                    x -= 60;
                    break;
                case 7:
                    romanNumeral += ("LXX");
                    x -= 70;
                    break;
                case 8:
                    romanNumeral += ("LXXX");
                    x -= 80;
                    break;
                case 9:
                    romanNumeral += ("XC");
                    x -= 90;
                    break;
                default:
                    break;
            }

            switch (x)
            {
                case 1:
                    romanNumeral += ("I");
                    x -= 10;
                    break;
                case 2:
                    romanNumeral += ("II");
                    x -= 20;
                    break;
                case 3:
                    romanNumeral += ("III");
                    x -= 30;
                    break;
                case 4:
                    romanNumeral += ("IV");
                    x -= 40;
                    break;
                case 5:
                    romanNumeral += ("V");
                    x -= 50;
                    break;
                case 6:
                    romanNumeral += ("VI");
                    x -= 60;
                    break;
                case 7:
                    romanNumeral += ("VII");
                    x -= 70;
                    break;
                case 8:
                    romanNumeral += ("VIII");
                    x -= 80;
                    break;
                case 9:
                    romanNumeral += ("IX");
                    x -= 90;
                    break;
                default:
                    break;
            }

            return romanNumeral;
        }
        public static string GetClipboardString(AlbumData album)
        {
            string result = Config.Clipboard;
            try
            {
                result = result.Replace("%artist%", album.Artist);
                result = result.Replace("%title%", album.Title);
                result = result.Replace("%year%", album.Year.ToString());
                result = result.Replace("%genre%", album.Genre.Name);
                result = result.Replace("%length%", album.Length.ToString());
                result = result.Replace("%length_seconds%", ((int)album.Length.TotalSeconds).ToString());
                result = result.Replace("%length_min%", album.Length.TotalMinutes.ToString());
                result = result.Replace("%totaltracks%", album.NumberOfSongs.ToString());
                result = result.Replace("%path%", album.SoundFilesPath);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
        public static string GetHistoryString(Song s, uint songnum)
        {
            string result = Config.Clipboard;
            try
            {
                result = result.Replace("%track_num%", songnum.ToString());
                result = result.Replace("%artist%", s.AlbumFrom.Artist);
                result = result.Replace("%title%", s.Title);
                result = result.Replace("%length%", s.Length.ToString(@"mm\:ss"));
                result = result.Replace("%date%", DateTime.Now.Date.ToString("d"));
                result = result.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                result = result.Replace("%year%", s.AlbumFrom.Year.ToString());
            }
            catch (NullReferenceException)
            {
                return result;
            }
            return result;
        }
        public static string GetHistoryString(FullTrack s, uint songnum)
        {
            string result = Config.History;
            try
            {
                result = result.Replace("%track_num%", songnum.ToString());
                result = result.Replace("%artist%", s.Artists[0].Name);
                result = result.Replace("%title%", s.Name);
                result = result.Replace("%length%", TimeSpan.FromMilliseconds(s.DurationMs).ToString(@"mm\:ss"));
                result = result.Replace("%date%", DateTime.Now.Date.ToString("d"));
                result = result.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                result = result.Replace("%year%", s.Album.ReleaseDate);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
        public static string GetStreamString(Song s, uint songnum, TimeSpan pos)
        {
            string result = Config.StreamString;
            try
            {
                result = result.Replace("%track_num%", songnum.ToString());
                result = result.Replace("%artist%", s.AlbumFrom.Artist);
                result = result.Replace("%title%", s.Title);
                result = result.Replace("%length%", s.Length.ToString(@"mm\:ss"));
                result = result.Replace("%pos%", pos.ToString(@"mm\:ss"));
                result = result.Replace("%date%", DateTime.Now.Date.ToString("d"));
                result = result.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                result = result.Replace("%year%", s.AlbumFrom.Year.ToString());
                result = result.Replace("%album%", s.AlbumFrom.Title);
                result = result.Replace("\\n", Environment.NewLine);
            }
            catch (NullReferenceException)
            {
                return result;
            }
            return result;
        }
        public static string GetStreamString(FullTrack s, uint songnum, TimeSpan pos)
        {
            string result = Config.StreamString;
            try
            {
                result = result.Replace("%track_num%", songnum.ToString());
                result = result.Replace("%artist%", s.Artists[0].Name);
                result = result.Replace("%title%", s.Name);
                result = result.Replace("%length%", TimeSpan.FromMilliseconds(s.DurationMs).ToString(@"mm\:ss"));
                result = result.Replace("%pos%", pos.ToString(@"mm\:ss"));
                result = result.Replace("%date%", DateTime.Now.Date.ToString("d"));
                result = result.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                result = result.Replace("%year%", s.Album.ReleaseDate);
                result = result.Replace("%album%", s.Album.Name);
                result = result.Replace("\\n", Environment.NewLine);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
        public static AlbumData GetRandomAlbum()
        {
            if (Kernel.Collection.Albums.Count == 0)
                return null;
            else
            {
                //Select a random album from the collection.
                Random random = new Random();
                AlbumData randomAlbum = Kernel.Collection.GetAlbum(random.Next(Kernel.Collection.Albums.Count), false);
                return randomAlbum;
            }
        }
        public static Song GetRandomSong()
        {
            return null;
        }
        public static Song GetRandomSong(AlbumData from)
        {
            Random random = new Random();
            int index = random.Next(from.Songs.Count);
            return from.Songs[index];
        }
        public static HashSet<AlbumData> GetAlbumsWithSongTitle(string song)
        {
            HashSet<AlbumData> list = new();
            for (int i = 0; i < Kernel.Collection.Albums.Count; i++)
            {
                for (int j = 0; j < Kernel.Collection.Albums[i].Songs.Count; j++)
                {
                    Song s = Kernel.Collection.Albums[i].Songs[j];
                    if (s.Title.Contains(song, StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(s.AlbumFrom);
                        break;
                    }
                }
            }
            return list;
        }
    }
}
