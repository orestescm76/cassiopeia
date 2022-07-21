using CSCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cassiopeia.CD
{
    public sealed class CDDrive : IDisposable
    {
        public readonly SafeFileHandle safeHandle;
        private bool tocValid = false;
        private WinAPI.CDROM_TOC toc;
        public string CDID { get; set; }
        private CDDrive(SafeFileHandle sf)
        {
            safeHandle = sf;
            toc = new();
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
        public bool readTOC()
        {
            if (!safeHandle.IsInvalid)
            {
                uint bytesread = 0;
                tocValid = WinAPI.DeviceIoControl(safeHandle, (uint)WinAPI.IOControlCode.IOCTL_CDROM_READ_TOC, IntPtr.Zero, 0, out toc, (uint)Marshal.SizeOf(toc), ref bytesread, IntPtr.Zero) != 0;
            }
            else tocValid = false;
            return tocValid;
        }
        public IWaveSource ReadTrack(PistaCD track)
        {
            return new CdWaveProvider(this.safeHandle, (uint)track.StartSector, (uint)track.EndSector);
        }
        private int sumofdigits(int n)
        {
            int sum = 0;
            while (n > 0)
            {
                sum += (n % 10);
                n /= 10;
            }
            return sum;
        }
        public int GetNumTracks()
        {
            if (tocValid)
            {
                return toc.LastTrack - toc.FirstTrack + 1;
            }
            else return -1;
        }
        public string calcDiscID()
        {
            int numTracks = GetNumTracks();
            if (numTracks == -1)
                throw new Exception("Unable to retrieve the number of tracks, Cannot calculate DiskID.");

            int XX = 0;
            for (int i = 0; i < numTracks; i++)
            {
                //mins*60+secs
                XX += sumofdigits((toc.TrackData[i].Address[1] * 60) + toc.TrackData[i].Address[2]);
            }
            //Calc YYYY
            WinAPI.TRACK_DATA lastTrack = toc.TrackData[numTracks];
            WinAPI.TRACK_DATA firstTrack = toc.TrackData[0];
            int YYYY = (lastTrack.Address[1] * 60) + lastTrack.Address[2] - ((firstTrack.Address[1] * 60) + firstTrack.Address[2]);
            //int numSecs = toc.TrackData[i].Address[1] * 60 + toc.TrackData[i].Address[2];

            ulong lDiscId = (((uint)XX % 0xff) << 24 | (uint)YYYY << 8 | (uint)numTracks);

            string sDiscId = String.Format("{0:x8}", lDiscId);
            CDID = sDiscId;
            return sDiscId;
        }
        public void GetSectorTrack(int track, out int startSec, out int endSec)
        {
            startSec = 0;
            endSec = 0;
            if(track >= toc.FirstTrack && track <= toc.LastTrack)
            {
                WinAPI.TRACK_DATA trackData = toc.TrackData[track-1];
                WinAPI.TRACK_DATA nextTrackData = toc.TrackData[track];
                startSec = (trackData.Address[1] * 60 * 75) + (trackData.Address[2] * 75) + trackData.Address[3] - 150;
                endSec = (nextTrackData.Address[1] * 60 * 75) + (nextTrackData.Address[2] * 75) + nextTrackData.Address[3] - 151;
            }
        }
    }
}
