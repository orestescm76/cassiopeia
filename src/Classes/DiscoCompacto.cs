using System;
using Newtonsoft.Json;

namespace aplicacion_musica
{
    public enum FormatoCD
    {
        Jewel, Digipack, MiniLP, NoStorage
    }
    public class DiscoCompacto : AlbumFisico
    {
        public String Id { get; set; }
        public Disco[] Discos { get; set; }
        public String[] Anotaciones { get; set; }
        public FormatoCD FormatoCD { get; set; }
        public DiscoCompacto() : base()
        {

        }
        public DiscoCompacto(string s, int nc, EstadoMedio e, EstadoMedio ee, FormatoCD f) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[1];
            Discos[0] = new Disco(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());//porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        public DiscoCompacto(string s, int nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, int nCD) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Discos[0] = new Disco(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        public DiscoCompacto(string s, EstadoMedio ee, FormatoCD f, int nCD) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        public DiscoCompacto(string s, int nCD) : base(s)
        {
            Discos = new Disco[nCD];
        }
        public DiscoCompacto(string s, int nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, short y, string p): base(s, ee, y, p)
        {
            FormatoCD = f;
            Discos = new Disco[1];
            Discos[0] = new Disco(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }

        public String[] toStringArray()
        {
            String[] d = new string[6];
            Array.Copy(Album.ToStringArray(), d, 5);
            d[5] = Id;
            return d;
        }

        public void InstallAlbum()
        {
            Album = Program.Collection.GetAlbum(Artist + "_" + Title);
        }
    }
}
