using System;

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
        public String Id { get; private set; }
        public Disco[] Discos { get; set; }
        public FormatoCD FormatoCD { get; set; }
        /// <summary>
        /// Crea sólo un CD a partir de un álbum de menos de 80 minutos, con todos los datos básicos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nc"></param>
        /// <param name="e"></param>
        /// <param name="ee"></param>
        /// <param name="f"></param>
        public DiscoCompacto(ref Album a, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f) : base(ref a, ee)
        {
            FormatoCD = f;
            Discos = new Disco[1];
            Discos[0] = new Disco(nc, e);
            Id = Guid.NewGuid().ToString(); //porque puede ser que tenga dos copias del mismo álbum
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
        public DiscoCompacto(ref Album a, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, int nCD) : base(ref a, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Discos[0] = new Disco(nc, e);
            Id = Guid.NewGuid().ToString(); //porque puede ser que tenga dos copias del mismo álbum
        }
        /// <summary>
        /// Crea un CD con varios CD sin discos predefinidos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="ee"></param>
        /// <param name="f"></param>
        /// <param name="nCD"></param>
        public DiscoCompacto(ref Album a, EstadoMedio ee, FormatoCD f, int nCD) : base(ref a, ee)
        {
            FormatoCD = f;
            Discos = new Disco[nCD];
            Id = Guid.NewGuid().ToString(); //porque puede ser que tenga dos copias del mismo álbum
        }
        /// <summary>
        /// Crea un CD vacío con varios discos vacíos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nCD"></param>
        public DiscoCompacto(ref Album a, int nCD) : base(ref a)
        {
            Discos = new Disco[nCD];
        }
        public String[] toStringArray()
        {
            String[] d = new string[6];
            Array.Copy(Album.toStringArray(), d, 5);
            d[5] = Id;
            return d;
        }
        public void InstallID(string id)
        {
            Id = id;
        }

        //public DiscoCompacto(ref Album a, short nc, EstadoMedio e, EstadoMedio ee, FormatoCD f, RegionInfo p, short y) : base(ref a, nc, e, ee, y, p)
        //{
        //    FormatoCD = f;
        //}
    }
}
