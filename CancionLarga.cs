﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    public class CancionLarga : Cancion
    {
        private byte np;
        public List<Cancion> Partes { get; private set; }
        public CancionLarga(string t, ref Album a)
        {
            titulo = t;
            album = a;
            Partes = new List<Cancion>();
            duracion = new TimeSpan();
        }
        //public CancionLarga(ref Cancion[] p)
        //{
        //    Partes = p;
        //    for (int i = 0; i < p.Length; i++)
        //    {
        //        duracion += p[i].duracion;
        //    }
        //}
        public void addParte(ref Cancion p)
        {
            Partes.Add(p);
            duracion += p.duracion;
        }
        /// <summary>
        /// devuelve el número romano del 1 al 99
        /// </summary>
        /// <param name="_x">numero árabe a convertir</param>
        /// <returns></returns>
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