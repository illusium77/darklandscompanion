using System;
using System.Runtime.InteropServices;

namespace DarklandsServices.Memory
{
    internal static class NativeMethods
    {
        // http://www.pinvoke.net/

        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess, 
            bool bInheritHandle,
            int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr process,
            IntPtr baseAddress,
            [Out] byte[] buffer,
            IntPtr size,
            out IntPtr bytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(
            IntPtr process,
            IntPtr baseAddress,
            byte[] buffer,
            IntPtr size, 
            out int bytesWritten);

        [DllImport("kernel32.dll")]
        internal static extern Int32 CloseHandle(
            IntPtr process);

        //[DllImportAttribute("User32.dll")]
        //internal static extern bool SetForegroundWindow(
        //    IntPtr wnd);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(
            out ExternSystemInfo systemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualQueryEx(
            IntPtr process,
            IntPtr address,
            out ExternMemoryBasicInformation buffer,
            IntPtr length);

        //[DllImport("kernel32.dll")]
        //internal static extern uint GetLastError();
    }
}
