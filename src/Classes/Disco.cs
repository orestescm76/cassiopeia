using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    public class Disco
    {
        public EstadoMedio EstadoDisco { get; set; }
        public short NumCanciones { get; set; }
        public Disco()
        {

        }
        public Disco(short nc, EstadoMedio e)
        {
            NumCanciones = nc;
            EstadoDisco = e;
        }
    }
}
