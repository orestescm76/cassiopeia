using System;
using Newtonsoft.Json;

namespace aplicacion_musica
{
    /// <summary>
    /// Especifica el tipo de CD
    /// </summary>
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
        /// <summary>
        /// Crea sólo un CD a partir de un álbum de menos de 80 minutos, con todos los datos básicos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nc"></param>
        /// <param name="e"></param>
        /// <param name="ee"></param>
        /// <param name="f"></param>
        public DiscoCompacto(string s, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[1];
            Discos[0] = new Disco(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());//porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        /// <summary>
        /// Crea un CD con varios CD
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nc">número de canciones del primer CD</param>
        /// <param name="e">Estado del primer CD</param>
        /// <param name="ee">Estado del exterior</param>
        /// <param name="f">Formato del CD</param>
        /// <param name="nCD">número de CD</param>
        public DiscoCompacto(string s, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, int nCD) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Discos[0] = new Disco(nc, e);
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        /// <summary>
        /// Crea un CD con varios CD sin discos predefinidos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="ee"></param>
        /// <param name="f"></param>
        /// <param name="nCD"></param>
        public DiscoCompacto(string s, EstadoMedio ee, FormatoCD f, int nCD) : base(s, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //porque puede ser que tenga dos copias del mismo álbum
            Id = Id.Remove(Id.Length - 2);
            Id.Replace('+', 'm');
        }
        /// <summary>
        /// Crea un CD vacío con varios discos vacíos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nCD"></param>
        public DiscoCompacto(string s, int nCD) : base(s)
        {
            Discos = new Disco[nCD];
        }
        public DiscoCompacto(string s, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, short y, string p): base(s, ee, y, p)
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
            Album = Programa.miColeccion.devolverAlbum(Artista + "_" + Nombre);
        }
    }
}
