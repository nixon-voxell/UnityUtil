using System.Threading;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Voxell.NativeContainers
{
  [StructLayout(LayoutKind.Sequential), NativeContainer, NativeContainerIsAtomicWriteOnly]
  unsafe public struct NativeIncrement : System.IDisposable
  {
    // the actual pointer to the allocated count needs to have restrictions relaxed
    // so jobs can be schedled with this container
    [NativeDisableUnsafePtrRestriction] private int* _counter;

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

    public void Increment()
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
      #endif
      Interlocked.Increment(ref *_counter);
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
      set
      {
        // verify that the caller has write permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        #endif
        *_counter = value;
      }
    }

    public bool IsCreated => _counter != null;

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