using System.Threading;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Voxell.NativeContainers
{
  [StructLayout(LayoutKind.Sequential), NativeContainer, NativeContainerIsAtomicWriteOnly]
  unsafe public struct NativeIncrement : System.IDisposable
  {
    public bool IsCreated => _counter != null;

    [NativeDisableUnsafePtrRestriction] private int* _counter;

    #if ENABLE_UNITY_COLLECTIONS_CHECKS
    private AtomicSafetyHandle m_Safety;
    [NativeSetClassTypeToNullOnSchedule] private DisposeSentinel _disposeSentinel;
    #endif

    private Allocator _allocatorLabel;

    public NativeIncrement(Allocator allocator)
    {
      // allocate native memory for a single integer
      _counter = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), allocator);
      _allocatorLabel = allocator;

      // create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Create(out m_Safety, out _disposeSentinel, 1, _allocatorLabel);
      #endif
      // initialize the count to 0 to avoid uninitialized data
      Count = 0;
    }

    public int Count
    {
      get
      {
        // verify that the caller has read permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
        #endif
        return *_counter;
      }

      [WriteAccessRequired]
      set
      {
        // verify that the caller has write permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        #endif
        *_counter = value;
      }
    }

    [WriteAccessRequired]
    public void Increment()
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
      #endif
      Interlocked.Increment(ref *_counter);
    }

    public void Dispose()
    {
      // let the dispose sentinel know that the data has been freed so it does not report any memory leaks
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Dispose(ref m_Safety, ref _disposeSentinel);
      #endif

      UnsafeUtility.Free(_counter, _allocatorLabel);
      _counter = null;
    }
  }
}