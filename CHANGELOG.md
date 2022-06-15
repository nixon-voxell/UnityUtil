## [1.4.2]

### New Features

- Added inverse lerp functions to support `float`, `float2`, and `float4` operations.
- Added default fields to `VXEditorUtil`.
- `CreateReorderableList` (formerly known as `FoldableReorderableList`):
  - Supports serialized structs/classes.
  - Element height now depends on serialized property height.
  - Added "empty message" to display when the list is empty.
- Created `GroupableList` class for storing a group index for each reorderable list.
- Created `EditorListConfig` struct to store `ReorderableList` related data.
- Added `float3x.one` to `float3x.cs`.

### Changes

- Removed `AbstractVXScriptableEditor`.
- `serializedObject` now updates every frame in `OnInspectorGUI` (in `AbstractVXEditor`) to refresh `serializedObject` representation (especially in scriptable objects).
- Removed unused functions from `VXEditorUtil`.
- Renamed `FoldableReorderableList` to `CreateReorderableList`.
- Added new `namespace` -> `Voxell.Inspector.List` for list specific classes/structs.
- Moved `CreateReorderableList` to `EditorListUtil` file.
- Bump `com.unity.jobs` package version to `0.50.0-preview.9`.
- Bump `com.unity.mathematics` package version to `1.2.6`.
- Updated `README.md` docs for `Mathx`.

### Bug Fixes

- Fixed `InspectOnly` height problem. Previously, it can only handle properties with single line height, now it handles all properties with all kinds of height difference using `EditorGUI.GetPropertyHeight(property, true)`.

## [1.4.1]

### Changes

- `FoldableReorderableList`:
  - Optional parameters for `draggable`, `displayHeader`, `displayAddButton`, `displayRemoveButton`, `multiSelect`, and `prefix`.
  - `multiSelect` is a newly added parameter.
  - Increase header rect by 3.0f to allow characters like 'g' and 'y' to be fully rendered.
  - Uses serialized property from the list instead of the cached serialized property taken in as the function's parameter.
  - Added parameter to change header string.
- `spaceA` and `spaceB` changed to `SPACE_A` and `SPACE_B` respectively.
- `InspectorView` returns `Editor` when initializing.
- `OnChange` method implemented for abstract editors. This method will be called whenever there is any changes made to the editor.

## [1.4.0]

### New Features

- Custom Editor Views for UIBuilder
  - Inspector View
  - Split View
- Abstract Editor and Abstract Scriptable Editor.
- Graphics
  - `Blelloch Sum Scan` compute shader.
  - `Hillis Steele Sum Scan` compute shader.
  - `AABB Scan` compute shader.
  - `Radix Sort` compute shader.
  - unit tests for each of them.
- Added "writing" methods to streaming asset files in `FileUtilx`.

### Changes

- Added `ReadOnly` and `WriteOnly` tags to native arrays on `Jobx` where neccessary.
- Improved performance of `Min/Max/Sum Scan` by increasing the batch size.
- Prevent the allocation of new array everytime a new scan/sort is needed by caching it during initialization.
- Removed Logging class.
- added NativeGetTangents method (MeshUtil).
- Shuffle array method in MathUtil now uses UnityEngine.Random instead of Unity.Mathematics.Random for easier usage (no need to think about what seed to provide & since it is a serial method).
- Renamed `Init()` method to `InitKernels()`.
- `NativeContainers` completely removed. (this action has been made due to the additional complexitiy when using native containers on simple tasks like `Interlocked.Exchange` or `Interlocked.Increment`, these operations can be done quite easily using unsafe and pointers in a Job System)

## [1.3.0]

### New Features

- Jobx (parallel computation primitives)
  - ReverseJob: Reverse native array in parallel.
  - Hillis Steele inclusive sum scan implementation.
  - Hillis Steele inclusive min/max scan implementation.
  - Radix Sort (LSB) implementation.
- Native Containers
  - Native Increment (atomic incrementation of an int).
  - Native Add (atomic addition of an int).
  - Native Exchange (atomic exchange of an int).
- mathx:
  - `approximately_zero` function.
  - lower case for `long_axis` function.
- directional vectors added for `float2x` and `float3x` (e.g. up, down, forwad, etc.).
- Created unit tests for `Jobx` and `NativeContainers`.

### Changes

- Added package dependencies:
  - "com.unity.jobs": "0.11.0-preview.6"
  - "com.unity.burst": "1.5.5"
- Fixed `StreamingAssetFilePath` and `StreamingAssetFolderPath` stack error when opening file explorer.
- Renamed `float2_extension` and `float3_extension` to `float2x` and `float3x`.

### Bug Fixes

- Fixed Bug: Mesh Info tool throws error when mesh filter or skinned mesh renderer has no mesh/missing mesh.
- Hillis Steele inclusive sum scan does not include first element.

## [1.2.0]

### New Features

- New tool: Mesh Info (EditorWindow)
  - Under *Tools/Voxell/Mesh Info*
  - Shows mesh stats based on the GameObject/Prefab you select.
  - Recursively search for children objects inside the GameObject/Prefab for MeshFilter and SkinnedMeshRenderer.
  - Stats include:
    - Vertex Count
    - Sub Mesh Count
    - Triangle Count
  - Ping functionality allows user to ping GameObject or Mesh location.
  - Multi select support.

### Changes

- EditorStyles -> VXEditorStyles to prevent conflict with UnityEditor namespace
- EditorUtil -> VXEditorUtil (match the above naming convention)
- Moved NativeUtil from Voxell.Mathx to Voxell namespace

## [1.1.1]

### New Features

- float2_extension
  - float2 perpendicular vector
  - angle between 2 float2 vectors
- float3_extension
  - float3 LongAxis (select axis with the largest value)
- mathx
  - approximately using Unity.Mathematics instead of Mathf.
- Stride Size for all vector and matrix sizes

### Changes

- GraphicsUtil
  - Generalize dispose buffers and destroy object array functions
- NativeUtil
  - Inlining of array and list disposition functions
  - removed vector to float converter (since we can use NativeArray/NativeList.CopyFrom)

- Removed
  - VectorUtil.cs (seems unnecessary)

## [1.1.0]

### New Features

- Buffer Array disposing utilities in GraphicsUtil.
- common stride sizes in a static class called StrideSize.
- ComputeShaderUtil for handling simple parallel task.
- Sequence array generator.

## [1.0.0]

- Initial release.