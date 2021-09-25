## [1.1.1]

- Changes:
  - GraphicsUtil
    - Generalize dispose buffers and destroy object array functions
  - NativeUtil
    - Inlining of array and list disposition functions
    - removed vector to float converter (since we can use NativeArray/NativeList.CopyFrom)

- Removed:
  - VectorUtil.cs (seems unnecessary)

- New Feature
  - float2 extensions
    - float2 perpendicular vector
    - angle between 2 float2 vectors

## [1.1.0]

- New Features:
  - Buffer Array disposing utilities in GraphicsUtil.
  - common stride sizes in a static class called StrideSize.
  - ComputeShaderUtil for handling simple parallel task.
  - Sequence array generator.

## [1.0.0]

- Initial release.