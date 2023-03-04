#nullable enable
#if DEBUG
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WinverUWP.Helpers
{
    public unsafe static class Interop
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void** GetVoidAddressOf<T>(this in ComPtr<T> ptr)
        where T : unmanaged
        {
            return (void**)Unsafe.AsPointer(ref Unsafe.AsRef(in ptr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UuidOfType __uuidof<T>(T value) // for type inference similar to C++'s __uuidof
        where T : unmanaged
        {
            return new UuidOfType(UUID<T>.RIID);
        }

        /// <summary>Retrieves the GUID of of a specified type.</summary>
        /// <param name="value">A pointer to a value of type <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type to retrieve the GUID for.</typeparam>
        /// <returns>A <see cref="UuidOfType"/> value wrapping a pointer to the GUID data for the input type. This value can be either converted to a <see cref="Guid"/> pointer, or implicitly assigned to a <see cref="Guid"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UuidOfType __uuidof<T>(T* value) // for type inference similar to C++'s __uuidof
            where T : unmanaged
        {
            return new UuidOfType(UUID<T>.RIID);
        }

        /// <summary>Retrieves the GUID of of a specified type.</summary>
        /// <typeparam name="T">The type to retrieve the GUID for.</typeparam>
        /// <returns>A <see cref="UuidOfType"/> value wrapping a pointer to the GUID data for the input type. This value can be either converted to a <see cref="Guid"/> pointer, or implicitly assigned to a <see cref="Guid"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UuidOfType __uuidof<T>()
            where T : unmanaged
        {
            return new UuidOfType(UUID<T>.RIID);
        }

        /// <summary>A proxy type that wraps a pointer to GUID data. Values of this type can be implicitly converted to and assigned to <see cref="Guid"/>* or <see cref="Guid"/> parameters.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly unsafe ref struct UuidOfType
        {
            private readonly Guid* riid;

            internal UuidOfType(Guid* riid)
            {
                this.riid = riid;
            }

            /// <summary>Reads a <see cref="Guid"/> value from the GUID buffer for a given <see cref="UuidOfType"/> instance.</summary>
            /// <param name="guid">The input <see cref="UuidOfType"/> instance to read data for.</param>
            public static implicit operator Guid(UuidOfType guid) => *guid.riid;

            /// <summary>Returns the <see cref="Guid"/>* pointer to the GUID buffer for a given <see cref="UuidOfType"/> instance.</summary>
            /// <param name="guid">The input <see cref="UuidOfType"/> instance to read data for.</param>
            public static implicit operator Guid*(UuidOfType guid) => guid.riid;
        }

        /// <summary>A helper type to provide static GUID buffers for specific types.</summary>
        /// <typeparam name="T">The type to allocate a GUID buffer for.</typeparam>
        private static unsafe class UUID<T>
            where T : unmanaged
        {
            /// <summary>The pointer to the <see cref="Guid"/> value for the current type.</summary>
            /// <remarks>The target memory area should never be written to.</remarks>
            public static readonly Guid* RIID = CreateRIID();

            /// <summary>Allocates memory for a <see cref="Guid"/> value and initializes it.</summary>
            /// <returns>A pointer to memory holding the <see cref="Guid"/> value for the current type.</returns>
            private static Guid* CreateRIID()
            {
                var p = (Guid*)Marshal.AllocHGlobal(sizeof(Guid));
                *p = typeof(T).GUID;
                return p;
            }
        }
        public unsafe struct ComPtr<T> : IDisposable where T : unmanaged
        {
            /// <summary>The raw pointer to a COM object, if existing.</summary>
            private T* ptr_;

            /// <summary>Creates a new <see cref="ComPtr{T}"/> instance from a raw pointer and increments the ref count.</summary>
            /// <param name="other">The raw pointer to wrap.</param>
            public ComPtr(T* other)
            {
                ptr_ = other;
                InternalAddRef();
            }

            /// <summary>Creates a new <see cref="ComPtr{T}"/> instance from a second one and increments the ref count.</summary>
            /// <param name="other">The other <see cref="ComPtr{T}"/> instance to copy.</param>
            public ComPtr(ComPtr<T> other)
            {
                ptr_ = other.ptr_;
                InternalAddRef();
            }

            /// <summary>Converts a raw pointer to a new <see cref="ComPtr{T}"/> instance and increments the ref count.</summary>
            /// <param name="other">The raw pointer to wrap.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator ComPtr<T>(T* other)
                => new ComPtr<T>(other);

            /// <summary>Unwraps a <see cref="ComPtr{T}"/> instance and returns the internal raw pointer.</summary>
            /// <param name="other">The <see cref="ComPtr{T}"/> instance to unwrap.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator T*(ComPtr<T> other)
                => other.Get();

            /// <summary>Converts the current object reference to type <typeparamref name="U"/> and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <typeparam name="U">The interface type to use to try casting the current COM object.</typeparam>
            /// <param name="p">A raw pointer to the target <see cref="ComPtr{T}"/> value to write to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target type <typeparamref name="U"/>.</returns>
            /// <remarks>This method will automatically release the target COM object pointed to by <paramref name="p"/>, if any.</remarks>
            public readonly HRESULT As<U>(ComPtr<U>* p)
                where U : unmanaged
            {
                return ((IUnknown*)ptr_)->QueryInterface(__uuidof<U>(), (void**)p->ReleaseAndGetAddressOf());
            }

            /// <summary>Converts the current object reference to type <typeparamref name="U"/> and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <typeparam name="U">The interface type to use to try casting the current COM object.</typeparam>
            /// <param name="other">A reference to the target <see cref="ComPtr{T}"/> value to write to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target type <typeparamref name="U"/>.</returns>
            /// <remarks>This method will automatically release the target COM object pointed to by <paramref name="other"/>, if any.</remarks>
            public readonly HRESULT As<U>(ref ComPtr<U> other)
                where U : unmanaged
            {
                U* ptr;
                HRESULT result = ((IUnknown*)ptr_)->QueryInterface(__uuidof<U>(), (void**)&ptr);

                other.Attach(ptr);
                return result;
            }

            /// <summary>Converts the current object reference to a type indicated by the given IID and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <param name="riid">The IID indicating the interface type to convert the COM object reference to.</param>
            /// <param name="other">A raw pointer to the target <see cref="ComPtr{T}"/> value to write to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target IID.</returns>
            /// <remarks>This method will automatically release the target COM object pointed to by <paramref name="other"/>, if any.</remarks>
            public readonly HRESULT AsIID(Guid* riid, ComPtr<IUnknown>* other)
            {
                return ((IUnknown*)ptr_)->QueryInterface(riid, (void**)other->ReleaseAndGetAddressOf());
            }

            /// <summary>Converts the current object reference to a type indicated by the given IID and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <param name="riid">The IID indicating the interface type to convert the COM object reference to.</param>
            /// <param name="other">A reference to the target <see cref="ComPtr{T}"/> value to write to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target IID.</returns>
            /// <remarks>This method will automatically release the target COM object pointed to by <paramref name="other"/>, if any.</remarks>
            public readonly HRESULT AsIID(Guid* riid, ref ComPtr<IUnknown> other)
            {
                IUnknown* ptr;
                HRESULT result = ((IUnknown*)ptr_)->QueryInterface(riid, (void**)&ptr);

                other.Attach(ptr);
                return result;
            }

            /// <summary>Releases the current COM object, if any, and replaces the internal pointer with an input raw pointer.</summary>
            /// <param name="other">The input raw pointer to wrap.</param>
            /// <remarks>This method will release the current raw pointer, if any, but it will not increment the references for <paramref name="other"/>.</remarks>
            public void Attach(T* other)
            {
                if (ptr_ != null)
                {
                    var @ref = ((IUnknown*)ptr_)->Release();
                    Debug.Assert((@ref != 0) || (ptr_ != other));
                }
                ptr_ = other;
            }

            /// <summary>Returns the raw pointer wrapped by the current instance, and resets the current <see cref="ComPtr{T}"/> value.</summary>
            /// <returns>The raw pointer wrapped by the current <see cref="ComPtr{T}"/> value.</returns>
            /// <remarks>This method will not change the reference count for the COM object in use.</remarks>
            public T* Detach()
            {
                T* ptr = ptr_;
                ptr_ = null;
                return ptr;
            }

            /// <summary>Increments the reference count for the current COM object, if any, and copies its address to a target raw pointer.</summary>
            /// <param name="ptr">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>This method always returns <see cref="S_OK"/>.</returns>
            public readonly HRESULT CopyTo(T** ptr)
            {
                InternalAddRef();
                *ptr = ptr_;
                return new HRESULT(0);
            }

            /// <summary>Increments the reference count for the current COM object, if any, and copies its address to a target <see cref="ComPtr{T}"/>.</summary>
            /// <param name="p">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>This method always returns <see cref="S_OK"/>.</returns>
            public readonly HRESULT CopyTo(ComPtr<T>* p)
            {
                InternalAddRef();
                *p->ReleaseAndGetAddressOf() = ptr_;
                return new HRESULT(0);
            }

            /// <summary>Increments the reference count for the current COM object, if any, and copies its address to a target <see cref="ComPtr{T}"/>.</summary>
            /// <param name="other">The target reference to copy the address of the current COM object to.</param>
            /// <returns>This method always returns <see cref="S_OK"/>.</returns>
            public readonly HRESULT CopyTo(ref ComPtr<T> other)
            {
                InternalAddRef();
                other.Attach(ptr_);
                return new HRESULT(0);
            }

            /// <summary>Converts the current COM object reference to a given interface type and assigns that to a target raw pointer.</summary>
            /// <param name="ptr">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target type <typeparamref name="U"/>.</returns>
            public readonly HRESULT CopyTo<U>(U** ptr)
                where U : unmanaged
            {
                return ((IUnknown*)ptr_)->QueryInterface(__uuidof<U>(), (void**)ptr);
            }

            /// <summary>Converts the current COM object reference to a given interface type and assigns that to a target <see cref="ComPtr{T}"/>.</summary>
            /// <param name="p">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target type <typeparamref name="U"/>.</returns>
            public readonly HRESULT CopyTo<U>(ComPtr<U>* p)
                where U : unmanaged
            {
                return ((IUnknown*)ptr_)->QueryInterface(__uuidof<U>(), (void**)p->ReleaseAndGetAddressOf());
            }

            /// <summary>Converts the current COM object reference to a given interface type and assigns that to a target <see cref="ComPtr{T}"/>.</summary>
            /// <param name="other">The target reference to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target type <typeparamref name="U"/>.</returns>
            public readonly HRESULT CopyTo<U>(ref ComPtr<U> other)
                where U : unmanaged
            {
                U* ptr;
                HRESULT result = ((IUnknown*)ptr_)->QueryInterface(__uuidof<U>(), (void**)&ptr);

                other.Attach(ptr);
                return result;
            }

            /// <summary>Converts the current object reference to a type indicated by the given IID and assigns that to a target address.</summary>
            /// <param name="riid">The IID indicating the interface type to convert the COM object reference to.</param>
            /// <param name="ptr">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target IID.</returns>
            public readonly HRESULT CopyTo(Guid* riid, void** ptr)
            {
                return ((IUnknown*)ptr_)->QueryInterface(riid, ptr);
            }

            /// <summary>Converts the current object reference to a type indicated by the given IID and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <param name="riid">The IID indicating the interface type to convert the COM object reference to.</param>
            /// <param name="p">The target raw pointer to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target IID.</returns>
            public readonly HRESULT CopyTo(Guid* riid, ComPtr<IUnknown>* p)
            {
                return ((IUnknown*)ptr_)->QueryInterface(riid, (void**)p->ReleaseAndGetAddressOf());
            }

            /// <summary>Converts the current object reference to a type indicated by the given IID and assigns that to a target <see cref="ComPtr{T}"/> value.</summary>
            /// <param name="riid">The IID indicating the interface type to convert the COM object reference to.</param>
            /// <param name="other">The target reference to copy the address of the current COM object to.</param>
            /// <returns>The result of <see cref="IUnknown.QueryInterface"/> for the target IID.</returns>
            public readonly HRESULT CopyTo(Guid* riid, ref ComPtr<IUnknown> other)
            {
                IUnknown* ptr;
                HRESULT result = ((IUnknown*)ptr_)->QueryInterface(riid, (void**)&ptr);

                other.Attach(ptr);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                T* pointer = ptr_;
                if (pointer != null)
                {
                    ptr_ = null;

                    _ = ((IUnknown*)pointer)->Release();
                }
            }

            /// <summary>Gets the currently wrapped raw pointer to a COM object.</summary>
            /// <returns>The raw pointer wrapped by the current <see cref="ComPtr{T}"/> instance.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly T* Get()
            {
                return ptr_;
            }

            /// <summary>Gets the address of the current <see cref="ComPtr{T}"/> instance as a raw <typeparamref name="T"/> double pointer. This method is only valid when the current <see cref="ComPtr{T}"/> instance is on the stack or pinned.
            /// </summary>
            /// <returns>The raw pointer to the current <see cref="ComPtr{T}"/> instance.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly T** GetAddressOf()
            {
                return (T**)Unsafe.AsPointer(ref Unsafe.AsRef(in this));
            }

            /// <summary>Gets the address of the current <see cref="ComPtr{T}"/> instance as a raw <typeparamref name="T"/> double pointer.</summary>
            /// <returns>The raw pointer to the current <see cref="ComPtr{T}"/> instance.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            [System.Diagnostics.CodeAnalysis.UnscopedRef]
            public readonly ref T* GetPinnableReference()
            {
                return ref Unsafe.AsRef(in this).ptr_;
            }

            /// <summary>Releases the current COM object in use and gets the address of the <see cref="ComPtr{T}"/> instance as a raw <typeparamref name="T"/> double pointer. This method is only valid when the current <see cref="ComPtr{T}"/> instance is on the stack or pinned.</summary>
            /// <returns>The raw pointer to the current <see cref="ComPtr{T}"/> instance.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T** ReleaseAndGetAddressOf()
            {
                _ = InternalRelease();
                return GetAddressOf();
            }

            /// <summary>Resets the current instance by decrementing the reference count for the target COM object and setting the internal raw pointer to <see langword="null"/>.</summary>
            /// <returns>The updated reference count for the COM object that was in use, if any.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint Reset()
            {
                return InternalRelease();
            }

            /// <summary>Swaps the current COM object reference with that of a given <see cref="ComPtr{T}"/> instance.</summary>
            /// <param name="r">The target <see cref="ComPtr{T}"/> instance to swap with the current one.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Swap(ComPtr<T>* r)
            {
                T* tmp = ptr_;
                ptr_ = r->ptr_;
                r->ptr_ = tmp;
            }

            /// <summary>Swaps the current COM object reference with that of a given <see cref="ComPtr{T}"/> instance.</summary>
            /// <param name="other">The target <see cref="ComPtr{T}"/> instance to swap with the current one.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Swap(ref ComPtr<T> other)
            {
                T* tmp = ptr_;
                ptr_ = other.ptr_;
                other.ptr_ = tmp;
            }

            // Increments the reference count for the current COM object, if any
            private readonly void InternalAddRef()
            {
                T* temp = ptr_;

                if (temp != null)
                {
                    _ = ((IUnknown*)temp)->AddRef();
                }
            }

            // Decrements the reference count for the current COM object, if any
            private uint InternalRelease()
            {
                uint @ref = 0;
                T* temp = ptr_;

                if (temp != null)
                {
                    ptr_ = null;
                    @ref = ((IUnknown*)temp)->Release();
                }

                return @ref;
            }
        }

        [Guid("00000000-0000-0000-C000-000000000046")]
        public unsafe partial struct IUnknown
        {
            public void** lpVtbl;

            public static ref readonly Guid IID_IUnknown
            {
                get
                {
                    ReadOnlySpan<byte> data = new byte[] {
                        0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00,
                        0x00, 0x00,
                        0xC0,
                        0x00,
                        0x00,
                        0x00,
                        0x00,
                        0x00,
                        0x00,
                        0x46
                    };

                    Debug.Assert(data.Length == Unsafe.SizeOf<Guid>());
                    return ref Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(data));
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public HRESULT QueryInterface(Guid* riid, void** ppvObject)
            {
                return ((delegate* unmanaged[Stdcall]<IUnknown*, Guid*, void**, int>)lpVtbl[0])((IUnknown*)Unsafe.AsPointer(ref this), riid, ppvObject);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint AddRef()
            {
                return ((delegate* unmanaged[Stdcall]<IUnknown*, uint>)lpVtbl[1])((IUnknown*)Unsafe.AsPointer(ref this));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint Release()
            {
                return ((delegate* unmanaged[Stdcall]<IUnknown*, uint>)lpVtbl[2])((IUnknown*)Unsafe.AsPointer(ref this));
            }
        }

        public readonly unsafe partial struct HSTRING : IComparable, IComparable<HSTRING>, IEquatable<HSTRING>, IFormattable
        {
            public readonly void* Value;


            public HSTRING(void* value)
            {
                Value = value;
            }


            public static HSTRING INVALID_VALUE => new HSTRING((void*)(-1));


            public static HSTRING NULL => new HSTRING(null);


            public static bool operator ==(HSTRING left, HSTRING right) => left.Value == right.Value;


            public static bool operator !=(HSTRING left, HSTRING right) => left.Value != right.Value;


            public static bool operator <(HSTRING left, HSTRING right) => left.Value < right.Value;


            public static bool operator <=(HSTRING left, HSTRING right) => left.Value <= right.Value;


            public static bool operator >(HSTRING left, HSTRING right) => left.Value > right.Value;


            public static bool operator >=(HSTRING left, HSTRING right) => left.Value >= right.Value;


            public static explicit operator HSTRING(void* value) => new HSTRING(value);


            public static implicit operator void*(HSTRING value) => value.Value;


            public static explicit operator HSTRING(Win32Headers.HANDLE value) => new HSTRING(value);
            public static implicit operator Win32Headers.HANDLE(HSTRING value) => new Win32Headers.HANDLE(value.Value);


            public static explicit operator HSTRING(byte value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator byte(HSTRING value) => (byte)(value.Value);


            public static explicit operator HSTRING(short value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator short(HSTRING value) => (short)(value.Value);


            public static explicit operator HSTRING(int value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator int(HSTRING value) => (int)(value.Value);


            public static explicit operator HSTRING(long value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator long(HSTRING value) => (long)(value.Value);


            public static explicit operator HSTRING(nint value) => new HSTRING(unchecked((void*)(value)));


            public static implicit operator nint(HSTRING value) => (nint)(value.Value);


            public static explicit operator HSTRING(sbyte value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator sbyte(HSTRING value) => (sbyte)(value.Value);


            public static explicit operator HSTRING(ushort value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator ushort(HSTRING value) => (ushort)(value.Value);


            public static explicit operator HSTRING(uint value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator uint(HSTRING value) => (uint)(value.Value);


            public static explicit operator HSTRING(ulong value) => new HSTRING(unchecked((void*)(value)));


            public static explicit operator ulong(HSTRING value) => (ulong)(value.Value);


            public static explicit operator HSTRING(nuint value) => new HSTRING(unchecked((void*)(value)));


            public static implicit operator nuint(HSTRING value) => (nuint)(value.Value);


            public int CompareTo(object? obj)
            {
                if (obj is HSTRING other)
                {
                    return CompareTo(other);
                }


                return (obj is null) ? 1 : throw new ArgumentException("obj is not an instance of HSTRING.");
            }


            public int CompareTo(HSTRING other) => ((nuint)(Value)).CompareTo((nuint)(other.Value));


            public override bool Equals(object? obj) => (obj is HSTRING other) && Equals(other);


            public bool Equals(HSTRING other) => ((nuint)(Value)).Equals((nuint)(other.Value));


            public override int GetHashCode() => ((nuint)(Value)).GetHashCode();


            public override string ToString() => ((nuint)(Value)).ToString((sizeof(nint) == 4) ? "X8" : "X16");


            public string ToString(string? format, IFormatProvider? formatProvider) => ((nuint)(Value)).ToString(format, formatProvider);
        }

        public readonly unsafe partial struct HRESULT : IComparable, IComparable<HRESULT>, IEquatable<HRESULT>, IFormattable
        {
            public readonly int Value;

            public HRESULT(int value)
            {
                Value = value;
            }

            public static bool operator ==(HRESULT left, HRESULT right) => left.Value == right.Value;

            public static bool operator !=(HRESULT left, HRESULT right) => left.Value != right.Value;

            public static bool operator <(HRESULT left, HRESULT right) => left.Value < right.Value;

            public static bool operator <=(HRESULT left, HRESULT right) => left.Value <= right.Value;

            public static bool operator >(HRESULT left, HRESULT right) => left.Value > right.Value;

            public static bool operator >=(HRESULT left, HRESULT right) => left.Value >= right.Value;

            public static implicit operator HRESULT(byte value) => new HRESULT(value);

            public static explicit operator byte(HRESULT value) => (byte)(value.Value);

            public static implicit operator HRESULT(short value) => new HRESULT(value);

            public static explicit operator short(HRESULT value) => (short)(value.Value);

            public static implicit operator HRESULT(int value) => new HRESULT(value);

            public static implicit operator int(HRESULT value) => value.Value;

            public static explicit operator HRESULT(long value) => new HRESULT(unchecked((int)(value)));

            public static implicit operator long(HRESULT value) => value.Value;

            public static explicit operator HRESULT(nint value) => new HRESULT(unchecked((int)(value)));

            public static implicit operator nint(HRESULT value) => value.Value;

            public static implicit operator HRESULT(sbyte value) => new HRESULT(value);

            public static explicit operator sbyte(HRESULT value) => (sbyte)(value.Value);

            public static implicit operator HRESULT(ushort value) => new HRESULT(value);

            public static explicit operator ushort(HRESULT value) => (ushort)(value.Value);

            public static explicit operator HRESULT(uint value) => new HRESULT(unchecked((int)(value)));

            public static explicit operator uint(HRESULT value) => (uint)(value.Value);

            public static explicit operator HRESULT(ulong value) => new HRESULT(unchecked((int)(value)));

            public static explicit operator ulong(HRESULT value) => (ulong)(value.Value);

            public static explicit operator HRESULT(nuint value) => new HRESULT(unchecked((int)(value)));

            public static explicit operator nuint(HRESULT value) => (nuint)(value.Value);

            public int CompareTo(object? obj)
            {
                if (obj is HRESULT other)
                {
                    return CompareTo(other);
                }

                return (obj is null) ? 1 : throw new ArgumentException("obj is not an instance of HRESULT.");
            }

            public int CompareTo(HRESULT other) => Value.CompareTo(other.Value);

            public override bool Equals(object? obj) => (obj is HRESULT other) && Equals(other);

            public bool Equals(HRESULT other) => Value.Equals(other.Value);

            public override int GetHashCode() => Value.GetHashCode();

            public override string ToString() => Value.ToString("X8");

            public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);
        }

        [DllImport("combase", ExactSpelling = true)]
        public static extern HRESULT WindowsCreateString(ushort* sourceString, uint length, HSTRING* @string);


        [DllImport("api-ms-win-core-winrt-l1-1-0.dll", EntryPoint = "RoGetActivationFactory", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        public static extern void RoGetActivationFactory(
            HSTRING activatableClassId,
            Guid* iid,
            void** factory);
    }

    // Index 37, GUID 0450CE77-AF0D-40AC-93FD-1E5D48C89419
    [Guid("0450CE77-AF0D-40AC-93FD-1E5D48C89419")]
    internal unsafe partial struct IPackageStatics_StateRepository
    {
#pragma warning disable CS0649 // Interop will assign this for us.
        public void** lpVtbl;
#pragma warning restore CS0649

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Interop.HRESULT ExistsByPackageFamilyName(Interop.HSTRING packageFamilyName, bool* result)
        {
            return ((delegate* unmanaged[Stdcall]<IPackageStatics_StateRepository*, Interop.HSTRING, bool*, int>)lpVtbl[37])((IPackageStatics_StateRepository*)Unsafe.AsPointer(ref this), packageFamilyName, result);
        }
    }
}
#endif