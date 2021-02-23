using Microsoft.Win32.SafeHandles;
using CSCore;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cassiopea.CD
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
        public IWaveSource ReadTrack(PistaCD track)
        {
            return new CdWaveProvider(this.safeHandle, track.StartSector, track.EndSector);
        }
    }
}
