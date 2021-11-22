﻿using System;

namespace Cassiopeia.src.Classes
{
    public enum MediaCondition
    {
        M,
        NMint,
        VGPlus,
        VG,
        GPlus,
        G,
        F,
        P
    }

    public class PhysicalAlbum
    {
        //no quiero guardarlo 2 veces, ni cargarlo 
        [Newtonsoft.Json.JsonIgnore] public AlbumData AlbumData { get; protected set; }
        public MediaCondition EstadoExterior { get; set; }
        public short Year {get;set;}
        public string Country { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string[] Anotaciones { get; set; }
        public string Id { get; set; }

        public PhysicalAlbum()
        {

        }

        public PhysicalAlbum(AlbumData a, string s, MediaCondition estado, short y = 0, string p = null)
        {
            AlbumData = a;
            EstadoExterior = estado;
            Year = y;
            Country = p;
            Artist = AlbumData.Artist;
            Title = AlbumData.Title;
        }

        public PhysicalAlbum(string s)
        {
            AlbumData = Kernel.Collection.GetAlbum(s);
        }
        public void InstallAlbum()
        {
            AlbumData = Kernel.Collection.GetAlbum(Artist + "/**/" + Title);
        }
    }
}
