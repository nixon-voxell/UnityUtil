using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Voxell.NativeContainers
{
  [StructLayout(LayoutKind.Sequential), NativeContainer, NativeContainerIsAtomicWriteOnly]
  unsafe public struct NativeExchangeArray : IDisposable
  {
    public int Length => _length;
    public bool IsCreated => _buffer != null;

    [NativeDisableUnsafePtrRestriction] internal void* _buffer;
    private int _length;

    #if ENABLE_UNITY_COLLECTIONS_CHECKS
    private AtomicSafetyHandle m_Safety;
    [NativeSetClassTypeToNullOnSchedule] private DisposeSentinel _disposeSentinel;
    #endif

    private Allocator _allocatorLabel;

    public NativeExchangeArray(int length, Allocator allocator)
    {
      long size = UnsafeUtility.SizeOf<int>() * length;

      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      if (allocator <= Allocator.None)
        throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", nameof(allocator));

      if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be > 0");
      if (size > int.MaxValue)
      {
        throw new ArgumentOutOfRangeException(
          nameof(length), $"Length * sizeof(int) cannot exceed {(object)int.MaxValue} bytes"
        );
      }
      // create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
      DisposeSentinel.Create(out m_Safety, out _disposeSentinel, 1, allocator);
      #endif

      // allocate native memory for an array of integers
      _buffer = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<int>(), allocator);
      _length = length;
      _allocatorLabel = allocator;
    }

    public int this[int index]
    {
      get
      {
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
        #endif
        return UnsafeUtility.ReadArrayElement<int>(_buffer, index);
      }

      [WriteAccessRequired]
      set
      {
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        #endif
        UnsafeUtility.WriteArrayElement<int>(_buffer, index, value);
      }
    }

    [WriteAccessRequired]
    public int AtomicExchange(int index, int value)
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
      #endif
      return Interlocked.Exchange(ref *((int*)_buffer + index), value);
    }

    public void Dispose()
    {
      // let the dispose sentinel know that the data has been freed so it does not report any memory leaks
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Dispose(ref m_Safety, ref _disposeSentinel);
      #endif

      UnsafeUtility.Free(_buffer, _allocatorLabel);
      _buffer = null;
      _length = 0;
    }
  }
}