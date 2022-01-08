using CSCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;

namespace Cassiopeia.CD
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
            if (handle.IsInvalid)
            {
                Log.Instance.PrintMessage("Couldn't read the CD drive. Handle is invalid", MessageType.Error);
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
