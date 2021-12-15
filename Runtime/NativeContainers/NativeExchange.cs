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
    // the actual pointer to the allocated count needs to have restrictions relaxed
    // so jobs can be schedled with this container
    [NativeDisableUnsafePtrRestriction] private int* _valueHolder;

    #if ENABLE_UNITY_COLLECTIONS_CHECKS
    private AtomicSafetyHandle m_Safety;
    /*
    the dispose sentinel tracks memory leaks, it is a managed type so it is cleared to null when scheduling a job
    the job cannot dispose the container, and no one else can dispose it until the job has run,
    so it is ok to not pass it along
    this attribute is required, without it this NativeContainer cannot be passed to a job
    since that would give the job access to a managed object
    */
    [NativeSetClassTypeToNullOnSchedule]
    private DisposeSentinel _disposeSentinel;
    #endif

    // Keep track of where the memory for this was allocated
    private Allocator _allocatorLabel;

    public NativeExchange(Allocator allocator)
    {
      // create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      if (allocator <= Allocator.None)
        throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", nameof(allocator));
      DisposeSentinel.Create(out m_Safety, out _disposeSentinel, 0, allocator);
      #endif

      // allocate native memory for a single integer
      _valueHolder = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), allocator);
      _allocatorLabel = allocator;

      // initialize the value to 0 to avoid uninitialized data
      Value = 0;
    }

    public int AtomicExchange(int value)
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
      #endif
      return Interlocked.Exchange(ref *_valueHolder, value);
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
      set
      {
        // verify that the caller has write permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        #endif
        *_valueHolder = value;
      }
    }

    public bool IsCreated => _valueHolder != null;

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