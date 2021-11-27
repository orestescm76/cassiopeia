using System;

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

    public interface IPhysicalAlbum
    {
        //no quiero guardarlo 2 veces, ni cargarlo 
        [Newtonsoft.Json.JsonIgnore]
        AlbumData Album { get; }
        MediaCondition EstadoExterior { get; set; }
        short Year {get;set;}
        string Country { get; set; }
        string Artist { get; set; }
        string Title { get; set; }
        string[] Anotaciones { get; set; }
        string Id { get; set; }
        TimeSpan Length { get; }

        public void InstallAlbum();
    }
}
