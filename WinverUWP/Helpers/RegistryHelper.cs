#nullable enable
using System;
using System.Runtime.InteropServices;

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
public static unsafe partial class RegistryHelper
{
#pragma warning disable CS0649
    private unsafe struct OBJECT_ATTRIBUTES
    {
        public uint Length;

        public IntPtr RootDirectory;

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

    [LibraryImport("ntdll.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial void RtlInitUnicodeString(UNICODE_STRING* DestinationString, string SourceString);

    [DllImport("ntdll.dll")]
    private static extern int NtClose(IntPtr hObject);

    [DllImport("ntdll.dll")]
    private static extern int NtOpenKey(IntPtr* KeyHandle, uint DesiredAccess, OBJECT_ATTRIBUTES* ObjectAttributes);

    [DllImport("ntdll.dll")]
    private static extern int NtQueryValueKey(IntPtr KeyHandle, UNICODE_STRING* ValueName, uint KeyValueInformationClass, void* KeyValueInformation, uint Length, uint* ResultLength);

    private static _KEY_VALUE_PARTIAL_INFORMATION* ReadInfo(string valueName)
    {
        UNICODE_STRING* uKeyName = (UNICODE_STRING*)NativeMemory.Alloc((nuint)sizeof(UNICODE_STRING));
        RtlInitUnicodeString(uKeyName, @"\Registry\Machine\SOFTWARE\Microsoft\Windows NT\CurrentVersion");
        OBJECT_ATTRIBUTES objectAttributes = new OBJECT_ATTRIBUTES
        {
            Length = (uint)sizeof(OBJECT_ATTRIBUTES),
            ObjectName = uKeyName,
            Attributes = 0x00000040,
            SecurityDescriptor = null,
            SecurityQualityOfService = null,
            RootDirectory = IntPtr.Zero
        };
        IntPtr hKey;
        NtOpenKey(&hKey, 0x80000000, &objectAttributes);

        UNICODE_STRING* uValueName = (UNICODE_STRING*)NativeMemory.Alloc((nuint)sizeof(UNICODE_STRING));
        RtlInitUnicodeString(uValueName, valueName);
        // Query once to get the size of KEY_VALUE_PARTIAL_INFORMATION we need
        uint resultLength;
        int error = NtQueryValueKey(hKey, uValueName, 2, null, 0, &resultLength);
        if (error == -1073741772)
        {
            // Key doesn't exist
            NtClose(hKey);
            NativeMemory.Free(uValueName);
            NativeMemory.Free(uKeyName);
            return null;
        }
        // Second query for value.
        _KEY_VALUE_PARTIAL_INFORMATION* partialInfo = (_KEY_VALUE_PARTIAL_INFORMATION*)NativeMemory.Alloc(resultLength);
        NtQueryValueKey(hKey, uValueName, 2, partialInfo, resultLength, &resultLength);
        NtClose(hKey);
        NativeMemory.Free(uValueName);
        NativeMemory.Free(uKeyName);
        return partialInfo;
    }

    public static uint? GetInfoDWord(string valueName)
    {
        _KEY_VALUE_PARTIAL_INFORMATION* buf = ReadInfo(valueName);
        if (buf == null)
            return null;
        uint value = *(uint*)buf->Data;
        NativeMemory.Free(buf);
        return value;
    }

    public static string? GetInfoString(string valueName)
    {
        _KEY_VALUE_PARTIAL_INFORMATION* info = ReadInfo(valueName);
        if (info == null)
            return null;
        string test = new string((char*)info->Data);
        NativeMemory.Free(info);
        return test;
    }
}
