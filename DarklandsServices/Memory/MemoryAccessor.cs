using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace DarklandsServices.Memory
{
    internal class MemoryAccessor : IDisposable
    {
        // credits
        //http://www.unknowncheats.me/forum/c/62019-c-non-hooked-external-directx-overlay.html
        //http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory

        private const int MemCommit = 0x00001000;
        private const int PageReadwrite = 0x04;
        private const int PointerLength = 4;
        private readonly string _processName;
        private IntPtr _baseAddress;
        private byte[] _lastValue;
        private IntPtr _processHandle = IntPtr.Zero;
        private int _processId;
        private bool _stopping;

        public MemoryAccessor(string processName)
        {
            _processName = processName;
        }

        public long GetPointer(long offset)
        {
            if (_processHandle == IntPtr.Zero)
            {
                _stopping = false;
                FindProcess();
            }

            var buffer = new byte[PointerLength];
            IntPtr bytesRead;
            var address = offset + (long) _baseAddress;
            NativeMethods.ReadProcessMemory(_processHandle, (IntPtr) address, buffer, (IntPtr) PointerLength, out bytesRead);

            return bytesRead.ToInt32() > 0 ? BitConverter.ToUInt32(buffer, 0) : 0;
        }

        public void StartPolling(long address, int length, int interval, Action<byte[]> dataRead)
        {
            if (_processHandle == IntPtr.Zero)
            {
                _stopping = false;
                FindProcess();
            }

            do
            {
                var buffer = new byte[length];

                IntPtr bytesRead;
                if (
                    !NativeMethods.ReadProcessMemory(_processHandle, (IntPtr) address, buffer, (IntPtr) length,
                        out bytesRead))
                {
                    _stopping = true;
                }
                if (bytesRead.ToInt32() > 0 && (_lastValue == null || !_lastValue.SequenceEqual(buffer)))
                {
                    _lastValue = buffer;
                    dataRead(_lastValue);
                }

                Thread.Sleep(interval);
            } while (!_stopping);
        }

        public bool ReadMemory(long address, byte[] bytes, int length = 0)
        {
            if (_processHandle == IntPtr.Zero)
            {
                _stopping = false;
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

            IntPtr bytesRead;

            return NativeMethods.ReadProcessMemory(_processHandle, (IntPtr) address, bytes, (IntPtr) length,
                out bytesRead);
        }

        public IEnumerable<long> SearchMemory(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException("bytes");
            }

            if (_processHandle == IntPtr.Zero)
            {
                _stopping = false;
                FindProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead);
            }

            // http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory
            var addresses = new List<long>();

            ExternSystemInfo sysInfo;
            NativeMethods.GetSystemInfo(out sysInfo);

            var minAddress = (long) sysInfo.MinimumApplicationAddress;
            var maxAddress = (long) sysInfo.MaximumApplicationAddress;

            var memInfoSize = (IntPtr) 28;
            while (!_stopping && minAddress < maxAddress)
            {
                ExternMemoryBasicInformation memInfo;
                NativeMethods.VirtualQueryEx(_processHandle, new IntPtr(minAddress), out memInfo, memInfoSize);

                if (memInfo.RegionSize == 0)
                {
                    var msg = "Could not read memory info, error code: " + Marshal.GetLastWin32Error()
                              + " handle: 0x" + _processHandle.ToString("x")
                              + " minAddress: 0x" + minAddress.ToString("x");

                    Stop();
                    throw new InvalidOperationException(msg);
                }

                if (minAddress <= 0x086518f0 && minAddress + memInfo.RegionSize <= 0x086518f0)
                {
                }

                if (memInfo.Protect == PageReadwrite && memInfo.State == MemCommit)
                {
                    var buffer = new byte[memInfo.RegionSize];
                    IntPtr bytesRead;
                    if (NativeMethods.ReadProcessMemory(_processHandle, (IntPtr) memInfo.BaseAddress, buffer,
                        (IntPtr) memInfo.RegionSize, out bytesRead))
                    {
                        var index = 0;

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

        private void FindProcess(
            ProcessAccessFlags flags = ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.VirtualMemoryWrite)
        {
            while (_processHandle == IntPtr.Zero && !_stopping)
            {
                var process = Process.GetProcessesByName(_processName).FirstOrDefault();
                if (process == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    _processId = process.Id;
                    _baseAddress = process.MainModule.BaseAddress;
                    _processHandle = NativeMethods.OpenProcess(flags, false, _processId);
                    Console.WriteLine("Found process '{0}' Id: {1:X}h, Handle: {2:X}", _processName, _processId,
                        (int)_processHandle);
                }
            }
        }

        private static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            // http://boncode.blogspot.com/2011/02/net-c-find-pattern-in-byte-array.html

            var found = -1;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) &&
                searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (var i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            var matched = true;
                            for (var y = 1; y <= searchBytes.Length - 1; y++)
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
            _stopping = true;

            if (_processHandle != IntPtr.Zero)
            {
                Console.WriteLine("Closing handle {0:X}", (int)_processHandle);
                NativeMethods.CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
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

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MemoryAccessor()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }

        #endregion
    }
}