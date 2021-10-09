using System.Threading;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Voxell.NativeContainers
{
  [StructLayout(LayoutKind.Sequential), NativeContainer, NativeContainerIsAtomicWriteOnly]
  unsafe public struct NativeAdd : System.IDisposable
  {
    // the actual pointer to the allocated count needs to have restrictions relaxed
    // so jobs can be schedled with this container
    [NativeDisableUnsafePtrRestriction] private int* _valueHolder;

    #if ENABLE_UNITY_COLLECTIONS_CHECKS
    private AtomicSafetyHandle _safety;
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

    public NativeAdd(Allocator allocator)
    {
      _allocatorLabel = allocator;

      // allocate native memory for a single integer
      _valueHolder = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), 4, allocator);

      // create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Create(out _safety, out _disposeSentinel, 0, _allocatorLabel);
      #endif
      // initialize the value to 0 to avoid uninitialized data
      Value = 0;
    }

    public void AtomicAdd(int value)
    {
      // verify that the caller has write permission on this data
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      AtomicSafetyHandle.CheckWriteAndThrow(_safety);
      #endif
      Interlocked.Add(ref *_valueHolder, value);
    }

    public int Value
    {
      get
      {
        // verify that the caller has read permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckReadAndThrow(_safety);
        #endif
        return *_valueHolder;
      }
      set
      {
        // verify that the caller has write permission on this data
        #if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle.CheckWriteAndThrow(_safety);
        #endif
        *_valueHolder = value;
      }
    }

    public bool IsCreated => _valueHolder != null;

    public void Dispose()
    {
      // let the dispose sentinel know that the data has been freed so it does not report any memory leaks
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      DisposeSentinel.Dispose(ref _safety, ref _disposeSentinel);
      #endif

      UnsafeUtility.Free(_valueHolder, _allocatorLabel);
      _valueHolder = null;
    }
  }
}