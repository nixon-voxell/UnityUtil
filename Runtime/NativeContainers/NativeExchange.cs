using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Voxell.NativeContainers
{
  [StructLayout(LayoutKind.Sequential), NativeContainer, NativeContainerIsAtomicWriteOnly]
  unsafe public struct NativeExchange : IDisposable
  {
    public bool IsCreated => _valueHolder != null;

    [NativeDisableUnsafePtrRestriction] private int* _valueHolder;

    #if ENABLE_UNITY_COLLECTIONS_CHECKS
    private AtomicSafetyHandle m_Safety;
    [NativeSetClassTypeToNullOnSchedule] private DisposeSentinel _disposeSentinel;
    #endif

    private Allocator _allocatorLabel;

    public NativeExchange(Allocator allocator)
    {
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      if (allocator <= Allocator.None)
        throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", nameof(allocator));
      // create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
      DisposeSentinel.Create(out m_Safety, out _disposeSentinel, 0, allocator);
      #endif

      // allocate native memory for a single integer
      _valueHolder = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), allocator);
      _allocatorLabel = allocator;

      // initialize the value to 0 to avoid uninitialized data
      Value = 0;
    }

    public int Value
    {
      get
      {
        // verify that the caller has read permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
        #endif
        return *_valueHolder;
      }

      [WriteAccessRequired]
      set
      {
        // verify that the caller has write permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        #endif
        *_valueHolder = value;
      }
    }

    [WriteAccessRequired]
    public int AtomicExchange(int value)
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
      #endif
      return Interlocked.Exchange(ref *_valueHolder, value);
    }

    public void Dispose()
    {
      // let the dispose sentinel know that the data has been freed so it does not report any memory leaks
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Dispose(ref m_Safety, ref _disposeSentinel);
      #endif

      UnsafeUtility.Free(_valueHolder, _allocatorLabel);
      _valueHolder = null;
    }
  }
}