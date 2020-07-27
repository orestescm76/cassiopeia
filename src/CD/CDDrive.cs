using Microsoft.Win32.SafeHandles;
using CSCore;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace aplicacion_musica.CD
{
    public sealed class CDDrive : IDisposable
    {
        private readonly SafeFileHandle safeHandle;
        private CDDrive(SafeFileHandle sf)
        {
            safeHandle = sf;
        }
        public SafeFileHandle GetHandle()
        {
            return safeHandle;
        }
        public void Dispose()
        {
            if (safeHandle != null)
                safeHandle.Dispose();
        }
        public static CDDrive Open(char letra)
        {
            string ruta = @"\\.\" + letra + ":";
            var handle = WinAPI.CreateFile(ruta,
                FileAccess.Read,
                FileShare.ReadWrite,
                IntPtr.Zero,
                FileMode.Open,
                0,
                IntPtr.Zero);
            if(handle.IsInvalid)
            {
                Console.WriteLine("No se puede abrir el CD");
                return null;
            }
            return new CDDrive(handle);
        }
        public PistaCD[] LeerPistas()
        {
            using(DriveLock.Lock(safeHandle))
            {
                WinAPI.CDROM_TOC toc;
                uint bytesRead = 0;
                if (WinAPI.DeviceIoControl(
                    this.safeHandle,
                    (uint)WinAPI.IOControlCode.IOCTL_CDROM_READ_TOC,
                    IntPtr.Zero,
                    0,
                    out toc,
                    (uint)Marshal.SizeOf(typeof(WinAPI.CDROM_TOC)),
                    ref bytesRead,
                    IntPtr.Zero) == 0)
                {
                    throw new Exception();
                }

                var tracks = new PistaCD[toc.LastTrack];
                for (var i = toc.FirstTrack - 1; i < toc.LastTrack; i++)
                {
                    tracks[i] = new PistaCD(
                        this.AddressToSector(toc.TrackData[i].Address),
                        this.AddressToSector(toc.TrackData[i + 1].Address), "");
                }

                return tracks;
            }
        }
        private uint AddressToSector(byte[] address)
        {
            return address[1] * 4500u + address[2] * 75u + address[3];
        }

        public IWaveSource ReadTrack(PistaCD track)
        {
            return new CdWaveProvider(this.safeHandle, track.StartSector, track.EndSector);
        }
    }
}
