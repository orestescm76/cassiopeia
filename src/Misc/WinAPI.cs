using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cassiopea
{
    //Clase para tener funciones de la API de Windows
    internal class WinAPI
    {
        public enum CreationDisposition : uint
        {
            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXISTING = 5,
        }

        public enum IOControlCode : uint
        {
            IOCTL_CDROM_READ_TOC = 0x00024000,
            IOCTL_STORAGE_CHECK_VERIFY = 0x002D4800,
            IOCTL_CDROM_RAW_READ = 0x0002403E,
            IOCTL_STORAGE_MEDIA_REMOVAL = 0x002D4804,
            IOCTL_STORAGE_EJECT_MEDIA = 0x002D4808,
            IOCTL_STORAGE_LOAD_MEDIA = 0x002D480C
        }

        public enum TRACK_MODE_TYPE
        {
            YellowMode2,
            XAForm2,
            CDDA
        }

        public const int MAXIMUM_NUMBER_TRACKS = 100;

        [StructLayout(LayoutKind.Sequential)]
        public struct PREVENT_MEDIA_REMOVAL
        {
            public byte PreventMediaRemoval;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRACK_DATA
        {
            public byte Reserved;
            public byte ControlAdr;
            public byte TrackNumber;
            public byte Reserved1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Address;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CDROM_TOC
        {
            public ushort Length;
            public byte FirstTrack;
            public byte LastTrack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXIMUM_NUMBER_TRACKS)]
            public TRACK_DATA[] TrackData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAW_READ_INFO
        {
            public long DiskOffset;
            public uint SectorCount;
            public TRACK_MODE_TYPE TrackModeType;
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static SafeFileHandle CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flags,
            IntPtr hTemplateFile);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public extern static int DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            [In] ref PREVENT_MEDIA_REMOVAL lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            [In] ref RAW_READ_INFO lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            ref int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public extern static int DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            [Out] out CDROM_TOC lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped);
    }
}
