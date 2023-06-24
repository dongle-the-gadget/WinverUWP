using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinverUWP.Interop;

public unsafe static partial class Windows
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int CompareTo(this nuint left, nuint right)
    {
        if (sizeof(nuint) == sizeof(uint))
        {
            return ((uint)left).CompareTo((uint)right);
        }

        return ((ulong)left).CompareTo(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(this T value, string? format)
    {
        throw new NotSupportedException("This API is not supported on this target.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(this T value, string? format, IFormatProvider? formatProvider)
    {
        throw new NotSupportedException("This API is not supported on this target.");
    }

    [DllImport("kernel32", ExactSpelling = true)]
    public static extern HMODULE LoadLibraryW(ushort* lpLibFileName);

    [DllImport("kernel32", ExactSpelling = true)]
    public static extern IntPtr GetProcAddress(HMODULE hModule, sbyte* lpProcName);

    [DllImport("kernel32", ExactSpelling = true)]
    public static extern BOOL FreeLibrary(HMODULE hLibModule);

    [DllImport("kernel32", ExactSpelling = true)]
    public static extern HMODULE GetModuleHandleW(ushort* lpModuleName);
}