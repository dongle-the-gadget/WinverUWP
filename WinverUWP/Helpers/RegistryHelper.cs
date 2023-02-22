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

        private delegate int NtClose(Win32Headers.HANDLE Handle);
        private delegate int NtOpenKey(
            Win32Headers.HANDLE* KeyHandle,
            uint DesiredAccess,
            OBJECT_ATTRIBUTES* ObjectAttributes);
        private delegate int NtQueryValueKey(
            Win32Headers.HANDLE KeyHandle,
            UNICODE_STRING* ValueName,
            uint KeyValueInformationClass,
            void* KeyValueInformation,
            uint Length,
            uint* ResultLength);

        private unsafe struct _KEY_VALUE_PARTIAL_INFORMATION
        {
            public uint TitleIndex;
            public uint Type;
            public uint DataLength;
            public fixed byte Data[1];
        }

        private delegate void RtlInitUnicodeString(UNICODE_STRING* DestinationString, ushort* SourceString);

        private static Win32Headers.HMODULE _ntdll;
        private static NtClose? _NtClose;
        private static NtOpenKey? _NtOpenKey;
        private static NtQueryValueKey? _NtQueryValueKey;
        private static RtlInitUnicodeString? _RtlInitUnicodeString;

        [DllImport("kernel32", ExactSpelling = true)]
        private static extern Win32Headers.HANDLE GetProcessHeap();

        [DllImport("kernel32", ExactSpelling = true)]
        private static extern void* HeapAlloc(Win32Headers.HANDLE hHeap, uint dwFlags, nuint dwBytes);

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
                    _NtClose = Marshal.GetDelegateForFunctionPointer<NtClose>(Win32Headers.GetProcAddress(_ntdll, (sbyte*)test));
                }
            }
            if (_NtOpenKey == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("NtOpenKey"))
                {
                    _NtOpenKey = Marshal.GetDelegateForFunctionPointer<NtOpenKey>(Win32Headers.GetProcAddress(_ntdll, (sbyte*)test));
                }
            }
            if (_NtQueryValueKey == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("NtQueryValueKey"))
                {
                    _NtQueryValueKey = Marshal.GetDelegateForFunctionPointer<NtQueryValueKey>(Win32Headers.GetProcAddress(_ntdll, (sbyte*)test));
                }
            }
            if (_RtlInitUnicodeString == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("RtlInitUnicodeString"))
                {
                    _RtlInitUnicodeString = Marshal.GetDelegateForFunctionPointer<RtlInitUnicodeString>(Win32Headers.GetProcAddress(_ntdll, (sbyte*)test));
                }
            }
            /*
            if (_RtlFreeUnicodeString == null)
            {
                fixed (byte* test = Encoding.ASCII.GetBytes("RtlFreeUnicodeString"))
                {
                    _RtlFreeUnicodeString = Marshal.GetDelegateForFunctionPointer<RtlFreeUnicodeString>(Win32Headers.GetProcAddress(_ntdll, (sbyte*)test));
                }
            }
            */
        }

        private static byte[]? ReadInfo(string valueName)
        {
            Initialize();
            fixed (char* key = @"\Registry\Machine\SOFTWARE\Microsoft\Windows NT\CurrentVersion")
            {
                UNICODE_STRING uKeyName;
                _RtlInitUnicodeString!(&uKeyName, (ushort*)key);
                OBJECT_ATTRIBUTES objectAttributes = new OBJECT_ATTRIBUTES
                {
                    Length = (uint)sizeof(OBJECT_ATTRIBUTES),
                    ObjectName = &uKeyName,
                    Attributes = 0x00000040,
                    SecurityDescriptor = null,
                    SecurityQualityOfService = null,
                    RootDirectory = Win32Headers.HANDLE.NULL
                };
                Win32Headers.HANDLE hKey;
                _NtOpenKey!(&hKey, 0x80000000, &objectAttributes);

                fixed (char* pValue = valueName)
                {
                    UNICODE_STRING uValueName;
                    _RtlInitUnicodeString(&uValueName, (ushort*)pValue);
                    void* buffer = null;
                    uint resultLength;
                    int error = _NtQueryValueKey!(hKey, &uValueName, 2, null, 0, &resultLength);
                    if (error == -1073741772)
                    {
                        _NtClose!(hKey);
                        return null;
                    }
                    else if (error == -1073741571)
                    {
                        do
                        {
                            buffer = HeapAlloc(GetProcessHeap(), 0, resultLength + 1024 + sizeof(ushort));
                            error = _NtQueryValueKey(hKey, &uValueName, 2, buffer, resultLength, &resultLength);
                        } while (error == -1073741571);
                    }
                    else
                    {
                        buffer = HeapAlloc(GetProcessHeap(), 0, resultLength + 1024);
                        _NtQueryValueKey(hKey, &uValueName, 2, buffer, resultLength, &resultLength);
                    }
                    _NtClose!(hKey);
                    _KEY_VALUE_PARTIAL_INFORMATION* partialInfo = (_KEY_VALUE_PARTIAL_INFORMATION*)buffer;
                    byte[] test = new byte[partialInfo->DataLength];
                    byte* infoData = partialInfo->Data;
                    Unsafe.CopyBlock(ref test[0], ref *infoData, partialInfo->DataLength);
                    return test;
                }
            }
        }

        public static uint? GetInfoDWord(string valueName)
        {
            byte[]? buf = ReadInfo(valueName);
            return buf != null ? BitConverter.ToUInt32(buf, 0) : null;
        }

        public static string? GetInfoString(string valueName)
        {
            byte[]? buf = ReadInfo(valueName);
            return buf != null ? Encoding.Unicode.GetString(buf).Replace("\0", "") : null;
        }
    }
}
