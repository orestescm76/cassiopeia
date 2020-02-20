using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    public class Disco
    {
        /// <summary>
        /// Estado del medio según el estado Goldmine
        /// </summary>
        public EstadoMedio EstadoDisco { get; set; }
        /// <summary>
        /// número de canciones del disco
        /// </summary>
        public short NumCanciones { get; set; }
        public Disco(short nc, EstadoMedio e)
        {
            NumCanciones = nc;
            EstadoDisco = e;
        }
    }
}
