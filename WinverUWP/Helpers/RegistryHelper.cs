#nullable enable
using System.Runtime.InteropServices;
using System.Text;
using static WinverUWP.Interop.Windows;

namespace WinverUWP.Helpers;

/*
 * This code uses materials from RegistryRT, license included below:
 * 
 * MIT License
 * 
 * Copyright (c) 2019 Gustave Monce - @gus33000 - gus33000.me
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
public static unsafe class RegistryHelper
{
    [DllImport("api-ms-win-crt-heap-l1-1-0.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void* malloc(nuint size);

    [DllImport("api-ms-win-crt-heap-l1-1-0.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void* free(void* ptr);

#pragma warning disable CS0649
    private unsafe struct OBJECT_ATTRIBUTES
    {
        public uint Length;

        public HANDLE RootDirectory;

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

    private static HMODULE _ntdll;
    private static delegate* unmanaged[Stdcall]<HANDLE, int> _NtClose;
    private static delegate* unmanaged[Stdcall]<HANDLE*, uint, OBJECT_ATTRIBUTES*, int> _NtOpenKey;
    private static delegate* unmanaged[Stdcall]<HANDLE, UNICODE_STRING*, uint, void*, uint, uint*, int> _NtQueryValueKey;
    private static delegate* unmanaged[Stdcall]<UNICODE_STRING*, ushort*, void> _RtlInitUnicodeString;

    private static void Initialize()
    {
        if (_ntdll == HMODULE.NULL)
        {
            fixed (char* libName = "ntdll.dll")
            {
                _ntdll = GetModuleHandleW((ushort*)libName);
                var test = Marshal.GetLastWin32Error();
            }
        }
        if (_NtClose == null)
        {
            fixed (byte* test = "NtClose"u8)
            {
                _NtClose = (delegate* unmanaged[Stdcall]<HANDLE, int>)GetProcAddress(_ntdll, (sbyte*)test);
            }
        }
        if (_NtOpenKey == null)
        {
            fixed (byte* test = "NtOpenKey"u8)
            {
                _NtOpenKey = (delegate* unmanaged[Stdcall]<HANDLE*, uint, OBJECT_ATTRIBUTES*, int>)GetProcAddress(_ntdll, (sbyte*)test);
            }
        }
        if (_NtQueryValueKey == null)
        {
            fixed (byte* test = "NtQueryValueKey"u8)
            {
                _NtQueryValueKey = (delegate* unmanaged[Stdcall]<HANDLE, UNICODE_STRING*, uint, void*, uint, uint*, int>)GetProcAddress(_ntdll, (sbyte*)test);
            }
        }
        if (_RtlInitUnicodeString == null)
        {
            fixed (byte* test = "RtlInitUnicodeString"u8)
            {
                _RtlInitUnicodeString = (delegate* unmanaged[Stdcall]<UNICODE_STRING*, ushort*, void>)GetProcAddress(_ntdll, (sbyte*)test);
            }
        }
    }

    private static _KEY_VALUE_PARTIAL_INFORMATION* ReadInfo(string valueName)
    {
        Initialize();
        fixed (char* key = @"\Registry\Machine\SOFTWARE\Microsoft\Windows NT\CurrentVersion")
        {
            UNICODE_STRING* uKeyName = (UNICODE_STRING*)malloc((nuint)sizeof(UNICODE_STRING));
            _RtlInitUnicodeString(uKeyName, (ushort*)key);
            OBJECT_ATTRIBUTES objectAttributes = new OBJECT_ATTRIBUTES
            {
                Length = (uint)sizeof(OBJECT_ATTRIBUTES),
                ObjectName = uKeyName,
                Attributes = 0x00000040,
                SecurityDescriptor = null,
                SecurityQualityOfService = null,
                RootDirectory = HANDLE.NULL
            };
            HANDLE hKey;
            _NtOpenKey(&hKey, 0x80000000, &objectAttributes);

            fixed (char* pValue = valueName)
            {
                UNICODE_STRING* uValueName = (UNICODE_STRING*)malloc((nuint)sizeof(UNICODE_STRING));
                _RtlInitUnicodeString(uValueName, (ushort*)pValue);
                // Query once to get the size of KEY_VALUE_PARTIAL_INFORMATION we need
                uint resultLength;
                int error = _NtQueryValueKey(hKey, uValueName, 2, null, 0, &resultLength);
                if (error == -1073741772)
                {
                    // Key doesn't exist
                    _NtClose(hKey);
                    free(uValueName);
                    free(uKeyName);
                    return null;
                }
                // Second query for value.
                _KEY_VALUE_PARTIAL_INFORMATION* partialInfo = (_KEY_VALUE_PARTIAL_INFORMATION*)malloc(resultLength);
                _NtQueryValueKey(hKey, uValueName, 2, partialInfo, resultLength, &resultLength);
                _NtClose(hKey);
                free(uValueName);
                free(uKeyName);
                return partialInfo;
            }
        }
    }

    public static uint? GetInfoDWord(string valueName)
    {
        _KEY_VALUE_PARTIAL_INFORMATION* buf = ReadInfo(valueName);
        if (buf == null)
            return null;
        uint value = *(uint*)buf->Data;
        free(buf);
        return value;
    }

    public static string? GetInfoString(string valueName)
    {
        _KEY_VALUE_PARTIAL_INFORMATION* info = ReadInfo(valueName);
        if (info == null)
            return null;
        string test = new string((char*)info->Data);
        free(info);
        return test;
    }
}
