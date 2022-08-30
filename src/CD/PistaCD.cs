using System;

namespace Cassiopeia.CD
{
    public class PistaCD
    {
        private int sectorInicio;
        private int sectorFinal;
        public int startTime { get; private set; }
        public int endTime { get; private set; }
        public string ID { get; }
        public TimeSpan Duracion { get; }
        public PistaCD(int startSector, int endSector)
        {
            this.sectorInicio = startSector;
            this.sectorFinal = endSector;
            int numSectores = endSector - startSector;
            this.startTime = sectorInicio / 75;
            this.endTime = sectorFinal / 75;
            Duracion = TimeSpan.FromSeconds(numSectores / 75.0);
        }

        internal int StartSector
        {
            get
            {
                return this.sectorInicio;
            }
        }

        internal int EndSector
        {
            get
            {
                return this.sectorFinal;
            }
        }
        public void fixSector()
        {
            sectorFinal++;
        }
    }
}
