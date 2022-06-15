# Unity Utilities

This package is where all the utility functions as well as all the custom inspector drawer code lives.

- [Unity Utilities](#unity-utilities)
  - [Installation](#installation)
  - [Custom Property Attribute](#custom-property-attribute)
  - [Utilities](#utilities)
    - [MathUtil](#mathutil)
    - [Under the hood](#under-the-hood)
  - [Support the project!](#support-the-project)
  - [Join the community!](#join-the-community)
  - [License](#license)
  - [References](#references)

## Installation

There are no external dependencies for this package.

1. Clone this repository into your `Packages` folder.
2. And you are ready to go!

## Custom Property Attribute

```cs
using UnityEngine;
using Voxell.Inspector;

public class CustomInspectorTest : MonoBehaviour
{
  [Scene]
  public string testScene;
  [Scene]
  public string[] testSceneArray;
  [InspectOnly]
  public int inspectOnlyInt;

  [StreamingAssetFilePath]
  public string streamingAssetFilePath;
  [StreamingAssetFolderPath]
  public string streamingAssetFolderPath;

  [Button]
  void TestButton() => Debug.Log("TestButton function invoked!");
  [Button("Super Button")]
  void AnotherTestButton() => Debug.Log("Button with Super Button name pressed!");
}
```

![CustomPropertyAttribute](./Pictures~/CustomPropertyAttribute.png)

## Utilities

### MathUtil

```cs
using UnityEngine;
using Voxell.Mathx;

// generate array [0, 1, 2, 3, 4]
int[] shuffledArray = MathUtil.GenerateSeqArray(5);
// shuffles array
MathUtil.ShuffleArray<int>(ref shuffledArray, 3);
Debug.Log(shuffledArray);

```
### Under the hood

```cs
using Unity.Mathematics;

// GenerateSeqArray method
int length = 5;
int[] shuffledArray = new int[length];
for (int i=0; i < length; i++) shuffledArray[i] = i;

// ShuffleArray method
for (int i = 0; i < shuffledArray.Length; i++)
{
  int randomIdx = UnityEngine.Random.Range(0, shuffledArray.Length);
  int tempItem = shuffledArray[randomIdx];
  shuffledArray[randomIdx] = shuffledArray[i];
  shuffledArray[i] = tempItem;
}

Debug.Log(shuffledArray);
```

## Support the project!

<a href="https://www.patreon.com/voxelltech" target="_blank">
  <img src="https://teaprincesschronicles.files.wordpress.com/2020/03/support-me-on-patreon.png" alt="patreon" width="200px" height="56px"/>
</a>

<a href ="https://ko-fi.com/voxelltech" target="_blank">
  <img src="https://uploads-ssl.webflow.com/5c14e387dab576fe667689cf/5cbed8a4cf61eceb26012821_SupportMe_red.png" alt="kofi" width="200px" height="40px"/>
</a>

## Join the community!

<a href ="https://discord.gg/Mhnyp6VYEQ" target="_blank">
  <img src="https://gist.githubusercontent.com/nixon-voxell/e7ba303906080ffdf65b106f684801b5/raw/97c6dfce3459c0a2c2ea8e1b9593612346f3abfc/JoinVXDiscord.svg" alt="discord" width="200px" height="200px"/>
</a>

<a href ="https://discord.gg/X3ZucbxXFc" target="_blank">
  <img src="https://gist.githubusercontent.com/nixon-voxell/e7ba303906080ffdf65b106f684801b5/raw/97c6dfce3459c0a2c2ea8e1b9593612346f3abfc/JoinVXGithubDiscord.svg" alt="discord" width="200px" height="200px"/>
</a>


## License

This repository as a whole is licensed under the Apache License 2.0. Individual files may have a different, but compatible license.

See [license file](./LICENSE) for details.

## References

- https://github.com/dbrizov/NaughtyAttributes