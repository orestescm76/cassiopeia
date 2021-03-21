using System;

namespace Cassiopeia.CD
{
    public class PistaCD
    {
        private readonly uint sectorInicio;
        private readonly uint sectorFinal;
        public string ID { get;  }
        public TimeSpan Duracion { get; }
        public PistaCD(uint startSector, uint endSector, string ID)
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
