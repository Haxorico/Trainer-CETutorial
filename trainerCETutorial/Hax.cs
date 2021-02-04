using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices; //USED TO CALL THE DLL IMPORTS


static class Hax
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern Int32 CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

    [Flags]
    private enum ProcessAccessType
    {
        PROCESS_TERMINATE = (0x0001),
        PROCESS_CREATE_THREAD = (0x0002),
        PROCESS_SET_SESSIONID = (0x0004),
        PROCESS_VM_OPERATION = (0x0008),
        PROCESS_VM_READ = (0x0010),
        PROCESS_VM_WRITE = (0x0020),
        PROCESS_DUP_HANDLE = (0x0040),
        PROCESS_CREATE_PROCESS = (0x0080),
        PROCESS_SET_QUOTA = (0x0100),
        PROCESS_SET_INFORMATION = (0x0200),
        PROCESS_QUERY_INFORMATION = (0x0400)
    }

    private static IntPtr open(Process p)
    {
        ProcessAccessType access = ProcessAccessType.PROCESS_VM_READ
        | ProcessAccessType.PROCESS_VM_WRITE
        | ProcessAccessType.PROCESS_VM_OPERATION;
        return OpenProcess((uint)access, 1, (uint)p.Id);
    }
    private static void close(IntPtr hProcess)
    {
        int iRetValue;
        iRetValue = CloseHandle(hProcess);
        if (iRetValue == 0)
        {
            throw new Exception("CloseHandle Failed");
        }
    }
    private static byte[] read(IntPtr MemoryAddress, uint bytesToRead, IntPtr hProcess)
    {
        byte[] buffer = new byte[bytesToRead];
        IntPtr ptrBytesRead;
        ReadProcessMemory(hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);
        return buffer;
    }
    private static byte[] write(IntPtr MemoryAddress, byte[] bytesToWrite, IntPtr hProcess)
    {
        IntPtr ptrBytesWritten;
        WriteProcessMemory(hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);
        if (bytesToWrite.Length>4)
            return BitConverter.GetBytes(ptrBytesWritten.ToInt64());
        return BitConverter.GetBytes(ptrBytesWritten.ToInt32());
    }
    
    public static string Bytes2Hex(byte[] bytes)
    {
        string ret = "";
        foreach (byte b in bytes)
        {
            ret += b.ToString("X") + " ";
        }
        return ret.Substring(0, ret.Length - 1);
    }
    public static string Dec2Hex(int dec)
    {
        return dec.ToString("X");
    }
    public static int Hex2Dec(string hex)
    {
        return int.Parse(hex, NumberStyles.HexNumber);
    }

    public static Process GetProcessByName(string processName)
    {
        Process[] p = Process.GetProcessesByName(processName);
        if (p.Length != 0)
            return p[0];
        return null;
    }
    public static int GetModuleAddress(Process targetProcess, string moduleName)
    {
        foreach (ProcessModule m in targetProcess.Modules)
        {
            if (m.ModuleName == moduleName)
            {
                return (int)m.BaseAddress;
            }
        }
        return -1;
    }
    public static byte[] GetBytesFromAddress(Process targetProcess, int address, uint byteSize = 4)
    {
        IntPtr hProcess = open(targetProcess);
        IntPtr mem = new IntPtr(address);
        byte[] outputVal = read(mem, byteSize, hProcess);
        close(hProcess);
        return outputVal;
    }
    public static int GetIntFromAddress(Process targetProcess, int address)
    {
        return BitConverter.ToInt32(GetBytesFromAddress(targetProcess, address),0);
    }
    public static float GetFloatFromAddress(Process targetProcess, int address)
    {
        return BitConverter.ToSingle(GetBytesFromAddress(targetProcess, address), 0);
    }
    public static double GetDoubleFromAddress(Process targetProcess, int address)
    {
        return BitConverter.ToDouble(GetBytesFromAddress(targetProcess, address,8), 0);
    }
    public static byte[] WriteBytesToAddress(Process targetProcess, int address, byte[] value)
    {
        IntPtr hProcess = open(targetProcess);
        IntPtr mem = new IntPtr(address);
        return write(mem, value, hProcess);
    }
    public static int WriteIntToAddress(Process targetProcess, int address, int value)
    {
        return BitConverter.ToInt32(WriteBytesToAddress(targetProcess, address, BitConverter.GetBytes(value)),0);
    }
    public static float WriteFloatToAddress(Process targetProcess, int address, float value)
    {
        return BitConverter.ToSingle(WriteBytesToAddress(targetProcess, address, BitConverter.GetBytes(value)), 0);
    }
    public static double WriteDoubleToAddress(Process targetProcess, int address, double value)
    {
        byte[] ret = WriteBytesToAddress(targetProcess, address, BitConverter.GetBytes(value));
        return BitConverter.ToDouble(ret, 0);
    }
    public static byte[] NopOutAddress(Process targetProcess, int address, int amountOfNops)
    {
        byte[] value = new byte[amountOfNops];
        for (int i = 0; i < amountOfNops; i++)
        {
            value[i] = 0x90;
        }
        return WriteBytesToAddress(targetProcess, address, value);
    }
    public static int GetResolvedAddress(Process targetProcess, int MODULE_ADDRESS, int BASE_ADDRESS, int[] offsets)
    {
        int a = MODULE_ADDRESS + BASE_ADDRESS;
        for (int i = 0; i < offsets.Length - 1; i++)
        {
            a = GetIntFromAddress(targetProcess, a) + offsets[i];
        }
        return GetIntFromAddress(targetProcess, a) + offsets[offsets.Length - 1];
    }
}

