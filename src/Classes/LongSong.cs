using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    public class LongSong : Song
    {
        public List<Song> Partes { get; private set; }
        public LongSong() 
        {
            Partes = new List<Song>();
        }
        public LongSong(string t, ref AlbumData a)
        {
            Title = t;
            AlbumFrom = a;
            Partes = new List<Song>();
        }
        public void addParte(Song p)
        {
            Partes.Add(p);
            Length += p.Length;
        }
        public String GetNumeroRomano(int _x)
        {
            String num = "";
            int x = _x;
            switch(x/10)
            {
                case 1:
                    num+=("X");
                    x -= 10;
                    break;
                case 2:
                    num += ("XX");
                    x -= 20;
                    break;
                case 3:
                    num += ("XXX");
                    x -= 30;
                    break;
                case 4:
                    num += ("XL");
                    x -= 40;
                    break;
                case 5:
                    num += ("L");
                    x -= 50;
                    break;
                case 6:
                    num += ("LX");
                    x -= 60;
                    break;
                case 7:
                    num += ("LXX");
                    x -= 70;
                    break;
                case 8:
                    num += ("LXXX");
                    x -= 80;
                    break;
                case 9:
                    num += ("XC");
                    x -= 90;
                    break;
                default:
                    break;
            }
            switch(x)
            {
                case 1:
                    num += ("I");
                    x -= 10;
                    break;
                case 2:
                    num += ("II");
                    x -= 20;
                    break;
                case 3:
                    num += ("III");
                    x -= 30;
                    break;
                case 4:
                    num += ("IV");
                    x -= 40;
                    break;
                case 5:
                    num += ("V");
                    x -= 50;
                    break;
                case 6:
                    num += ("VI");
                    x -= 60;
                    break;
                case 7:
                    num += ("VII");
                    x -= 70;
                    break;
                case 8:
                    num += ("VIII");
                    x -= 80;
                    break;
                case 9:
                    num += ("IX");
                    x -= 90;
                    break;
                default:
                    break;
            }
            return num;
        }
    }
}
