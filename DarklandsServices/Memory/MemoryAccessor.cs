using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DarklandsServices.Memory
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }

    public class MemoryAccessor : IDisposable
    {
        // credits
        //http://www.unknowncheats.me/forum/c/62019-c-non-hooked-external-directx-overlay.html
        //http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory

        public struct ExternSystemInfo
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;  // minimum address
            public IntPtr maximumApplicationAddress;  // maximum address
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        public struct ExternMemoryBasicInformation
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;   // size of the region allocated by the program
            public int State;   // check if allocated (MEM_COMMIT)
            public int Protect; // page protection (must be PAGE_READWRITE)
            public int lType;
        }

        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        
        #region DllImports

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        //[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        //private static extern bool _CloseHandle(IntPtr hObject);

        [DllImportAttribute("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out ExternSystemInfo lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out ExternMemoryBasicInformation lpBuffer, uint dwLength);

        //[DllImport("kernel32.dll")]
        //static extern uint GetLastError();

        #endregion

        private int m_processId;
        private IntPtr m_processHandle = IntPtr.Zero;
        private IntPtr m_baseAddress;
        private string m_processName;
        private bool m_Stopping;

        private byte[] m_lastValue = null;

        public MemoryAccessor(string processName)
        {
            m_processName = processName;
        }

        public long GetPointer(long offset)
        {
            if (m_processHandle == IntPtr.Zero)
            {
                m_Stopping = false;
                FindProcess();
            }

            int length = 4;
            var buffer = new byte[length];
            int bytesRead = 0;
            var address = offset + (long)m_baseAddress;
            ReadProcessMemory(m_processHandle, (IntPtr)address, buffer, length, out bytesRead);

            if (bytesRead > 0)
            {
                return BitConverter.ToUInt32(buffer, 0);
            }

            return 0;
        }

        public void StartPolling(long address, int length, int interval, Action<byte[]> dataRead)
        {
            if (m_processHandle == IntPtr.Zero)
            {
                m_Stopping = false;
                FindProcess();
            }

            do
            {
                var buffer = new byte[length];

                int bytesRead = 0;
                if (!ReadProcessMemory(m_processHandle, (IntPtr)address, buffer, length, out bytesRead))
                {
                    m_Stopping = true;
                }
                if (bytesRead > 0 && (m_lastValue == null || !m_lastValue.SequenceEqual(buffer)))
                {
                    m_lastValue = buffer;
                    dataRead(m_lastValue);
                }

                Thread.Sleep(interval);

            } while (!m_Stopping);
        }

        public bool ReadMemory(long address, byte[] bytes, int length = 0)
        {
            if (m_processHandle == IntPtr.Zero)
            {
                m_Stopping = false;
                FindProcess();
            }

            if (bytes == null || bytes.Length < length)
            {
                bytes = new byte[length];
            }
            else if (length == 0)
            {
                length = bytes.Length;
            }

            int bytesRead = 0;

            return ReadProcessMemory(m_processHandle, (IntPtr)address, bytes, length, out bytesRead);
        }

        public IEnumerable<long> SearchMemory(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException("bytes");
            }

            if (m_processHandle == IntPtr.Zero)
            {
                m_Stopping = false;
                FindProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VMRead);
            }

            // http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory
            var addresses = new List<long>();

            var sysInfo = new ExternSystemInfo();
            GetSystemInfo(out sysInfo);

            long minAddress = (long)sysInfo.minimumApplicationAddress;
            long maxAddress = (long)sysInfo.maximumApplicationAddress;

            var memInfo = new ExternMemoryBasicInformation();
            uint memInfoSize = 28;
            int bytesRead = 0;

            while (!m_Stopping && minAddress < maxAddress)
            {
                VirtualQueryEx(m_processHandle, new IntPtr(minAddress), out memInfo, memInfoSize);

                if (memInfo.RegionSize == 0)
                {
                    var msg = "Could not read memory info, error code: " + Marshal.GetLastWin32Error()
                        + " handle: 0x" + m_processHandle.ToString("x")
                        + " minAddress: 0x" + minAddress.ToString("x");

                    Stop();
                    throw new InvalidOperationException(msg);
                }

                if (minAddress <= 0x086518f0 && minAddress + memInfo.RegionSize <= 0x086518f0)
                {
                }

                if (memInfo.Protect == PAGE_READWRITE && memInfo.State == MEM_COMMIT)
                {
                    var buffer = new byte[memInfo.RegionSize];
                    if (ReadProcessMemory(m_processHandle, (IntPtr)memInfo.BaseAddress, buffer,
                        memInfo.RegionSize, out bytesRead))
                    {
                        int index = 0;

                        do
                        {
                            index = ByteSearch(buffer, bytes, index);

                            if (index >= 0)
                            {
                                addresses.Add(minAddress + index);
                                index += bytes.Length;

                                //System.IO.File.WriteAllBytes("memdumb.bin", buffer);
                            }

                        } while (index > 0);
                    }
                }

                minAddress += memInfo.RegionSize;
            }

             return addresses;
        }

        private void FindProcess(ProcessAccessFlags flags = ProcessAccessFlags.VMRead | ProcessAccessFlags.VMWrite)
        {
            while (m_processHandle == IntPtr.Zero && !m_Stopping)
            {
                var process = Process.GetProcessesByName(m_processName).FirstOrDefault();
                if (process == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    m_processId = process.Id;
                    m_baseAddress = process.MainModule.BaseAddress;
                    m_processHandle = OpenProcess(flags, false, m_processId);
                    Console.WriteLine(string.Format("Found process '{0}' Id: {1:X}h, Handle: {2:X}", m_processName, m_processId, m_processHandle));
                }
            }
        }

        private static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            // http://boncode.blogspot.com/2011/02/net-c-find-pattern-in-byte-array.html

            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            return found;
        }

        public void Stop()
        {
            m_Stopping = true;

            if (m_processHandle != IntPtr.Zero)
            {
                Console.WriteLine(string.Format("Closing handle {0:X}", m_processHandle));
                CloseHandle(m_processHandle);
                m_processHandle = IntPtr.Zero;
            }
        }

        //public static void OpenProcess()
        //{
        //    Process[] procs = Process.GetProcessesByName("dosbox");
        //    if (procs.Length == 0)
        //    {
        //        proccID = 0;
        //    }
        //    else
        //    {
        //        proccID = procs[0].Id;
        //        pHandle = OpenProcess(0x1F0FFF, false, proccID);



        //        ProcessModuleCollection modules = procs[0].Modules;
        //        foreach (ProcessModule module in modules)
        //        {
        //            if (module.ModuleName == "dosbox.exe")
        //            {
        //                base_adress = module.BaseAddress.ToInt32();
        //            }
        //        }

        //    }

        //}
        //public static void setForeground()
        //{
        //    Process p = Process.GetProcessById(proccID);
        //    SetForegroundWindow(p.MainWindowHandle);
        //}
        //public static int readInt(long Address)
        //{
        //    byte[] buffer = new byte[sizeof(int)];
        //    ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
        //    return BitConverter.ToInt32(buffer, 0);
        //}
        //public static uint readUInt(long Address)
        //{
        //    byte[] buffer = new byte[sizeof(int)];
        //    ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
        //    return (uint)BitConverter.ToUInt32(buffer, 0);
        //}
        //public static float readFloat(long Address)
        //{
        //    byte[] buffer = new byte[sizeof(float)];
        //    ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
        //    return BitConverter.ToSingle(buffer, 0);
        //}
        //public static string ReadString(long Address)
        //{
        //    byte[] buffer = new byte[50];

        //    ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)50, IntPtr.Zero);

        //    string ret = Encoding.Unicode.GetString(buffer);

        //    if (ret.IndexOf('\0') != -1)
        //        ret = ret.Remove(ret.IndexOf('\0'));
        //    return ret;
        //}
        //public static byte readByte(long Address)
        //{
        //    byte[] buffer = new byte[1];
        //    ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)1, IntPtr.Zero);
        //    return buffer[0];
        //}
        //public static void WriteFloat(long Address, float value)
        //{
        //    byte[] buffer = BitConverter.GetBytes(value);
        //    WriteProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        //}
        //public static void WriteInt(long Address, int value)
        //{
        //    byte[] buffer = BitConverter.GetBytes(value);
        //    WriteProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        //}
        //public static void WriteUInt(long Address, uint value)
        //{
        //    byte[] buffer = BitConverter.GetBytes(value);
        //    WriteProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        //}


        public void Dispose()
        {
            Stop();
        }
    }
}
