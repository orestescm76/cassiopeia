using CSCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cassiopeia.CD
{
    internal class CdWaveProvider : IWaveSource
    {
        private const int SectorsToRead = 32;
        private const int SectorSize = 2048;
        private const int SectorAudioSize = 2352;

        private static readonly WaveFormat CDWaveFormat = new WaveFormat();

        private readonly SafeFileHandle handle;
        private uint startSector;
        private uint currentSector;
        private readonly uint endSector;

        private readonly byte[] currentBlock = new byte[SectorsToRead * SectorAudioSize];
        private int currentBlockLength;
        private int currentBlockIndex;
        public CdWaveProvider(SafeFileHandle handle, uint startSector, uint endSector)
        {
            this.handle = handle;
            this.currentSector = startSector;
            this.endSector = endSector;
            this.startSector = startSector;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            if (this.currentBlockLength > 0)
            {
                var bytesCopied = this.ReadFromCurrentBlock(buffer, offset, count);
                count -= bytesCopied;
                offset += bytesCopied;
                totalBytesRead += bytesCopied;
            }

            while (count > SectorsToRead * SectorSize && this.currentSector != this.endSector)
            {
                var bytesRead = this.ReadSector(buffer, offset, count);
                count -= bytesRead;
                offset += bytesRead;
                totalBytesRead += bytesRead;
            }

            if (count > 0 && this.currentSector != this.endSector)
            {
                this.currentBlockLength = this.ReadSector(this.currentBlock, 0, this.currentBlock.Length);
                this.currentBlockIndex = 0;

                var bytesCopied = this.ReadFromCurrentBlock(buffer, offset, count);
                count -= bytesCopied;
                offset += bytesCopied;
                totalBytesRead += bytesCopied;
            }

            Debug.Assert(count == 0);
            return totalBytesRead;
        }

        private int ReadFromCurrentBlock(byte[] buffer, int offset, int count)
        {
            var bytesCopied = Math.Min(count, this.currentBlockLength - this.currentBlockIndex);
            Array.Copy(this.currentBlock, this.currentBlockIndex, buffer, offset, bytesCopied);
            this.currentBlockIndex += bytesCopied;

            if (this.currentBlockIndex == this.currentBlockLength)
            {
                this.currentBlockLength = 0;
                this.currentBlockIndex = -1;
            }
            return bytesCopied;
        }

        private unsafe int ReadSector(byte[] buffer, int offset, int count)
        {
            var sectorCount = Math.Min(SectorsToRead, this.currentSector - this.endSector);
            Debug.Assert(count >= sectorCount);

            var info = new WinAPI.RAW_READ_INFO
            {
                DiskOffset = this.currentSector * SectorSize,
                SectorCount = sectorCount,
                TrackModeType = WinAPI.TRACK_MODE_TYPE.CDDA
            };
            int bytesRead = 0;

            fixed (byte* bufferPointer = &buffer[offset])
            {
                if (WinAPI.DeviceIoControl(
                    this.handle,
                    (uint)WinAPI.IOControlCode.IOCTL_CDROM_RAW_READ,
                    ref info,
                    Marshal.SizeOf(typeof(WinAPI.RAW_READ_INFO)),
                    (IntPtr)bufferPointer,
                    count,
                    ref bytesRead,
                    IntPtr.Zero) == 0)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }

            this.currentSector += sectorCount;
            return bytesRead;
        }

        public void Dispose()
        {
            handle.Dispose();
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return CDWaveFormat;
            }
        }

        public bool CanSeek
        {
            get
            {
                return true;
            }
        }
        //duracion en sectores
        public long Length
        {
            get
            {
                return endSector - startSector;
            }
        }
        //implementado por sectores
        public long Position
        {
            get
            {
                return currentSector - startSector;
            }
            set
            {
                if (value > (endSector - startSector) && value < 0)
                {
                    Log.Instance.PrintMessage("Value exceeds the total number of sectors", MessageType.Error);
                    throw new ArgumentOutOfRangeException();
                }

                else
                {
                    currentSector = (uint)(startSector + value);
                }
            }
        }
    }
}
