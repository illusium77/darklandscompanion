using System;

namespace DarklandsServices.Memory
{
    internal struct ExternSystemInfo
    {
        // disable reshaper warnings - stuct used in native code must remain as they are
#pragma warning disable 649
#pragma warning disable 169
        public ushort ProcessorArchitecture;
        ushort _reserved;
        public uint PageSize;
        public IntPtr MinimumApplicationAddress;  // minimum address
        public IntPtr MaximumApplicationAddress;  // maximum address
        public IntPtr ActiveProcessorMask;
        public uint NumberOfProcessors;
        public uint ProcessorType;
        public uint AllocationGranularity;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;
#pragma warning restore 169
#pragma warning restore 649
    }
}
