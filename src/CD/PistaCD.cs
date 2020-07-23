using System;

namespace aplicacion_musica.CD
{
    public class PistaCD
    {
        private readonly uint sectorInicio;
        private readonly uint sectorFinal;
        public TimeSpan Duracion { get; private set; }
        public PistaCD(uint startSector, uint endSector)
        {
            this.sectorInicio = startSector;
            this.sectorFinal = endSector;
            uint numSectores = endSector - startSector;
            Duracion = TimeSpan.FromSeconds(numSectores / 75.0);
        }

        internal uint StartSector
        {
            get
            {
                return this.sectorInicio;
            }
        }

        internal uint EndSector
        {
            get
            {
                return this.sectorFinal;
            }
        }
    }
}
