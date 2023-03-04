#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinverUWP.Helpers
{
    public static unsafe class RegistryHelper
    {
#pragma warning disable CS0649
        private unsafe struct OBJECT_ATTRIBUTES
        {
            public uint Length;

            public Win32Headers.HANDLE RootDirectory;

            public UNICODE_STRING* ObjectName;

            public uint Attributes;

            public void* SecurityDescriptor;

            public void* SecurityQualityOfService;
        }

        private unsafe struct UNICODE_STRING
        {
            public ushort Length;

            public ushort MaximumLength;

            public ushort* Buffer;
        }

        private unsafe struct _KEY_VALUE_PARTIAL_INFORMATION
        {
            public uint TitleIndex;
            public uint Type;
            public uint DataLength;
            public fixed byte Data[1];
        }
#pragma warning restore CS0649

        private static Win32Headers.HMODULE _ntdll;
        private static void* _NtClose;
        private static void* _NtOpenKey;
        private static void* _NtQueryValueKey;
        private static void* _RtlInitUnicodeString;

        private static void Initialize()
        {
            if (_ntdll == Win32Headers.HMODULE.NULL)
            {
                fixed (char* libName = "ntdll.dll")
                {
                    _ntdll = Win32Headers.GetModuleHandleW((ushort*)libName);
                    var test = Marshal.GetLastWin32Error();
                }
            }
            if (_NtClose == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("NtClose"))
                {
                    _NtClose = (void*)Win32Headers.GetProcAddress(_ntdll, (sbyte*)test);
                }
            }
            if (_NtOpenKey == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("NtOpenKey"))
                {
                    _NtOpenKey = (void*)Win32Headers.GetProcAddress(_ntdll, (sbyte*)test);
                }
            }
            if (_NtQueryValueKey == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("NtQueryValueKey"))
                {
                    _NtQueryValueKey = (void*)Win32Headers.GetProcAddress(_ntdll, (sbyte*)test);
                }
            }
            if (_RtlInitUnicodeString == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("RtlInitUnicodeString"))
                {
                    _RtlInitUnicodeString = (void*)Win32Headers.GetProcAddress(_ntdll, (sbyte*)test);
                }
            }
        }

        private static _KEY_VALUE_PARTIAL_INFORMATION* ReadInfo(string valueName)
        {
            Initialize();
            fixed (char* key = @"\Registry\Machine\SOFTWARE\Microsoft\Windows NT\CurrentVersion")
            {
                UNICODE_STRING* uKeyName = (UNICODE_STRING*)Marshal.AllocHGlobal(sizeof(UNICODE_STRING));
                ((delegate* unmanaged[Stdcall]<UNICODE_STRING*, ushort*, void>)_RtlInitUnicodeString)(uKeyName, (ushort*)key);
                OBJECT_ATTRIBUTES objectAttributes = new OBJECT_ATTRIBUTES
                {
                    Length = (uint)sizeof(OBJECT_ATTRIBUTES),
                    ObjectName = uKeyName,
                    Attributes = 0x00000040,
                    SecurityDescriptor = null,
                    SecurityQualityOfService = null,
                    RootDirectory = Win32Headers.HANDLE.NULL
                };
                Win32Headers.HANDLE hKey;
                ((delegate* unmanaged[Stdcall]<Win32Headers.HANDLE*, uint, OBJECT_ATTRIBUTES*, int>)_NtOpenKey)(&hKey, 0x80000000, &objectAttributes);

                fixed (char* pValue = valueName)
                {
                    UNICODE_STRING* uValueName = (UNICODE_STRING*)Marshal.AllocHGlobal(sizeof(UNICODE_STRING));
                    ((delegate* unmanaged[Stdcall]<UNICODE_STRING*, ushort*, void>)_RtlInitUnicodeString)(uValueName, (ushort*)pValue);
                    uint resultLength;
                    int error = ((delegate* unmanaged[Stdcall]<Win32Headers.HANDLE, UNICODE_STRING*, uint, void*, uint, uint*, int>)_NtQueryValueKey)(hKey, uValueName, 2, null, 0, &resultLength);
                    if (error == -1073741772)
                    {
                        ((delegate* unmanaged[Stdcall]<Win32Headers.HANDLE, int>)_NtClose)(hKey);
                        Marshal.FreeHGlobal((IntPtr)uValueName);
                        Marshal.FreeHGlobal((IntPtr)uKeyName);
                        return null;
                    }
                    _KEY_VALUE_PARTIAL_INFORMATION* partialInfo = (_KEY_VALUE_PARTIAL_INFORMATION*)Marshal.AllocHGlobal((int)resultLength);
                    ((delegate* unmanaged[Stdcall]<Win32Headers.HANDLE, UNICODE_STRING*, uint, void*, uint, uint*, int>)_NtQueryValueKey)(hKey, uValueName, 2, partialInfo, resultLength, &resultLength);
                    ((delegate* unmanaged[Stdcall]<Win32Headers.HANDLE, int>)_NtClose)(hKey);
                    Marshal.FreeHGlobal((IntPtr)uValueName);
                    Marshal.FreeHGlobal((IntPtr)uKeyName);
                    return partialInfo;
                }
            }
        }

        public static uint? GetInfoDWord(string valueName)
        {
            _KEY_VALUE_PARTIAL_INFORMATION* buf = ReadInfo(valueName);
            if (buf == null)
                return null;
            uint value;
            Unsafe.CopyBlock(buf->Data, &value, buf->DataLength);
            Marshal.FreeHGlobal((IntPtr)buf);
            return value;
        }

        public static string? GetInfoString(string valueName)
        {
            _KEY_VALUE_PARTIAL_INFORMATION* info = ReadInfo(valueName);
            if (info == null)
                return null;
            byte[] data = new byte[info->DataLength];
            Unsafe.CopyBlock(ref data[0], ref *info->Data, info->DataLength);
            Marshal.FreeHGlobal((IntPtr)info);
            return Encoding.Unicode.GetString(data).Replace("\0", "");
        }
    }
}
