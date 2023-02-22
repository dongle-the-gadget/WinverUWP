#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinverUWP.Helpers
{
    public unsafe static class Win32Headers
    {
        public readonly unsafe partial struct HANDLE : IComparable, IComparable<HANDLE>, IEquatable<HANDLE>, IFormattable
        {
            public readonly void* Value;

            public HANDLE(void* value)
            {
                Value = value;
            }

            public static HANDLE INVALID_VALUE => new HANDLE((void*)(-1));

            public static HANDLE NULL => new HANDLE(null);

            public static bool operator ==(HANDLE left, HANDLE right) => left.Value == right.Value;

            public static bool operator !=(HANDLE left, HANDLE right) => left.Value != right.Value;

            public static bool operator <(HANDLE left, HANDLE right) => left.Value < right.Value;

            public static bool operator <=(HANDLE left, HANDLE right) => left.Value <= right.Value;

            public static bool operator >(HANDLE left, HANDLE right) => left.Value > right.Value;

            public static bool operator >=(HANDLE left, HANDLE right) => left.Value >= right.Value;

            public static explicit operator HANDLE(void* value) => new HANDLE(value);

            public static implicit operator void*(HANDLE value) => value.Value;

            public static explicit operator HANDLE(byte value) => new HANDLE((void*)(value));

            public static explicit operator byte(HANDLE value) => (byte)(value.Value);

            public static explicit operator HANDLE(short value) => new HANDLE((void*)(value));

            public static explicit operator short(HANDLE value) => (short)(value.Value);

            public static explicit operator HANDLE(int value) => new HANDLE((void*)(value));

            public static explicit operator int(HANDLE value) => (int)(value.Value);

            public static explicit operator HANDLE(long value) => new HANDLE((void*)(value));

            public static explicit operator long(HANDLE value) => (long)(value.Value);

            public static explicit operator HANDLE(nint value) => new HANDLE((void*)(value));

            public static implicit operator nint(HANDLE value) => (nint)(value.Value);

            public static explicit operator HANDLE(sbyte value) => new HANDLE((void*)(value));

            public static explicit operator sbyte(HANDLE value) => (sbyte)(value.Value);

            public static explicit operator HANDLE(ushort value) => new HANDLE((void*)(value));

            public static explicit operator ushort(HANDLE value) => (ushort)(value.Value);

            public static explicit operator HANDLE(uint value) => new HANDLE((void*)(value));

            public static explicit operator uint(HANDLE value) => (uint)(value.Value);

            public static explicit operator HANDLE(ulong value) => new HANDLE((void*)(value));

            public static explicit operator ulong(HANDLE value) => (ulong)(value.Value);

            public static explicit operator HANDLE(nuint value) => new HANDLE((void*)(value));

            public static implicit operator nuint(HANDLE value) => (nuint)(value.Value);

            public int CompareTo(object? obj)
            {
                if (obj is HANDLE other)
                {
                    return CompareTo(other);
                }

                return (obj is null) ? 1 : throw new ArgumentException("obj is not an instance of HANDLE.");
            }

            public int CompareTo(HANDLE other) => ((nuint)(Value)).CompareTo((nuint)(other.Value));

            public override bool Equals(object? obj) => (obj is HANDLE other) && Equals(other);

            public bool Equals(HANDLE other) => ((nuint)(Value)).Equals((nuint)(other.Value));

            public override int GetHashCode() => ((nuint)(Value)).GetHashCode();

            public override string ToString() => ((nuint)(Value)).ToString((sizeof(nint) == 4) ? "X8" : "X16");

            public string ToString(string? format, IFormatProvider? formatProvider) => ((nuint)(Value)).ToString(format, formatProvider);
        }

        public readonly unsafe partial struct HMODULE : IComparable, IComparable<HMODULE>, IEquatable<HMODULE>, IFormattable
        {
            public readonly void* Value;

            public HMODULE(void* value)
            {
                Value = value;
            }

            public static HMODULE INVALID_VALUE => new HMODULE((void*)(-1));

            public static HMODULE NULL => new HMODULE(null);

            public static bool operator ==(HMODULE left, HMODULE right) => left.Value == right.Value;

            public static bool operator !=(HMODULE left, HMODULE right) => left.Value != right.Value;

            public static bool operator <(HMODULE left, HMODULE right) => left.Value < right.Value;

            public static bool operator <=(HMODULE left, HMODULE right) => left.Value <= right.Value;

            public static bool operator >(HMODULE left, HMODULE right) => left.Value > right.Value;

            public static bool operator >=(HMODULE left, HMODULE right) => left.Value >= right.Value;

            public static explicit operator HMODULE(void* value) => new HMODULE(value);

            public static implicit operator void*(HMODULE value) => value.Value;

            public static explicit operator HMODULE(Win32Headers.HANDLE value) => new HMODULE(value);

            public static implicit operator Win32Headers.HANDLE(HMODULE value) => new Win32Headers.HANDLE(value.Value);

            public static explicit operator HMODULE(byte value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator byte(HMODULE value) => (byte)(value.Value);

            public static explicit operator HMODULE(short value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator short(HMODULE value) => (short)(value.Value);

            public static explicit operator HMODULE(int value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator int(HMODULE value) => (int)(value.Value);

            public static explicit operator HMODULE(long value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator long(HMODULE value) => (long)(value.Value);

            public static explicit operator HMODULE(nint value) => new HMODULE(unchecked((void*)(value)));

            public static implicit operator nint(HMODULE value) => (nint)(value.Value);

            public static explicit operator HMODULE(sbyte value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator sbyte(HMODULE value) => (sbyte)(value.Value);

            public static explicit operator HMODULE(ushort value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator ushort(HMODULE value) => (ushort)(value.Value);

            public static explicit operator HMODULE(uint value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator uint(HMODULE value) => (uint)(value.Value);

            public static explicit operator HMODULE(ulong value) => new HMODULE(unchecked((void*)(value)));

            public static explicit operator ulong(HMODULE value) => (ulong)(value.Value);

            public static explicit operator HMODULE(nuint value) => new HMODULE(unchecked((void*)(value)));

            public static implicit operator nuint(HMODULE value) => (nuint)(value.Value);

            public int CompareTo(object? obj)
            {
                if (obj is HMODULE other)
                {
                    return CompareTo(other);
                }

                return (obj is null) ? 1 : throw new ArgumentException("obj is not an instance of HMODULE.");
            }

            public int CompareTo(HMODULE other) => ((nuint)(Value)).CompareTo((nuint)(other.Value));

            public override bool Equals(object? obj) => (obj is HMODULE other) && Equals(other);

            public bool Equals(HMODULE other) => ((nuint)(Value)).Equals((nuint)(other.Value));

            public override int GetHashCode() => ((nuint)(Value)).GetHashCode();

            public override string ToString() => ((nuint)(Value)).ToString((sizeof(nint) == 4) ? "X8" : "X16");

            public string ToString(string? format, IFormatProvider? formatProvider) => ((nuint)(Value)).ToString(format, formatProvider);
        }

        public readonly partial struct BOOL : IComparable, IComparable<BOOL>, IEquatable<BOOL>, IFormattable
        {
            public readonly int Value;

            public BOOL(int value)
            {
                Value = value;
            }

            public static BOOL FALSE => new BOOL(0);

            public static BOOL TRUE => new BOOL(1);

            public static bool operator ==(BOOL left, BOOL right) => left.Value == right.Value;

            public static bool operator !=(BOOL left, BOOL right) => left.Value != right.Value;

            public static bool operator <(BOOL left, BOOL right) => left.Value < right.Value;

            public static bool operator <=(BOOL left, BOOL right) => left.Value <= right.Value;

            public static bool operator >(BOOL left, BOOL right) => left.Value > right.Value;

            public static bool operator >=(BOOL left, BOOL right) => left.Value >= right.Value;

            public static implicit operator bool(BOOL value) => value.Value != 0;

            public static implicit operator BOOL(bool value) => new BOOL(value ? 1 : 0);

            public static bool operator false(BOOL value) => value.Value == 0;

            public static bool operator true(BOOL value) => value.Value != 0;

            public static implicit operator BOOL(byte value) => new BOOL(value);

            public static explicit operator byte(BOOL value) => (byte)(value.Value);

            public static implicit operator BOOL(short value) => new BOOL(value);

            public static explicit operator short(BOOL value) => (short)(value.Value);

            public static implicit operator BOOL(int value) => new BOOL(value);

            public static implicit operator int(BOOL value) => value.Value;

            public static explicit operator BOOL(long value) => new BOOL(unchecked((int)(value)));

            public static implicit operator long(BOOL value) => value.Value;

            public static explicit operator BOOL(nint value) => new BOOL(unchecked((int)(value)));

            public static implicit operator nint(BOOL value) => value.Value;

            public static implicit operator BOOL(sbyte value) => new BOOL(value);

            public static explicit operator sbyte(BOOL value) => (sbyte)(value.Value);

            public static implicit operator BOOL(ushort value) => new BOOL(value);

            public static explicit operator ushort(BOOL value) => (ushort)(value.Value);

            public static explicit operator BOOL(uint value) => new BOOL(unchecked((int)(value)));

            public static explicit operator uint(BOOL value) => (uint)(value.Value);

            public static explicit operator BOOL(ulong value) => new BOOL(unchecked((int)(value)));

            public static explicit operator ulong(BOOL value) => (ulong)(value.Value);

            public static explicit operator BOOL(nuint value) => new BOOL(unchecked((int)(value)));

            public static explicit operator nuint(BOOL value) => (nuint)(value.Value);

            public int CompareTo(object? obj)
            {
                if (obj is BOOL other)
                {
                    return CompareTo(other);
                }

                return (obj is null) ? 1 : throw new ArgumentException("obj is not an instance of BOOL.");
            }

            public int CompareTo(BOOL other) => Value.CompareTo(other.Value);

            public override bool Equals(object? obj) => (obj is BOOL other) && Equals(other);

            public bool Equals(BOOL other) => Value.Equals(other.Value);

            public override int GetHashCode() => Value.GetHashCode();

            public override string ToString() => Value.ToString();

            public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);
        }

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

        /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
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
}
