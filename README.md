# Unity Utilities

This package is where all the utility functions as well as all the custom inspector drawer code lives.

- [Unity Utilities](#unity-utilities)
  - [Installation](#installation)
  - [Custom Property Attribute](#custom-property-attribute)
  - [Utilities](#utilities)
    - [Mathx](#mathx)
    - [Under the hood](#under-the-hood)
  - [Logging](#logging)
    - [A simple example:](#a-simple-example)
  - [Support the project!](#support-the-project)
  - [Join the community!](#join-the-community)
  - [License](#license)
  - [References](#references)

## Installation

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

### Mathx

```cs
using UnityEngine;
using Voxell.Mathx;

// generate array [0, 1, 2, 3, 4]
int[] shuffledArray = GenerateSeqArray(5);
// shuffles array
ShuffleArray<int>(ref shuffledArray, 3);
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
int seed = 3;
Random rand = new Random(math.max(seed, 1));
for (int i = 0; i < shuffledArray.Length; i++)
{
  int randomIdx = rand.NextInt(0, shuffledArray.Length);
  int tempItem = shuffledArray[randomIdx];
  shuffledArray[randomIdx] = shuffledArray[i];
  shuffledArray[i] = tempItem;
}

Debug.Log(shuffledArray);
```

## Logging

### A simple example:
```cs
using UnityEngine;
using Voxell;

public class LoggingTest : MonoBehaviour
{
  public Logger logger;

  public void NormalLog() => logger.ConditionalLog("NormalLog", LogImportance.Info, LogType.Log);
  public void ImportantLog() => logger.ConditionalLog("ImportantLog", LogImportance.Important, LogType.Log);
  public void CrucialWarning() => logger.ConditionalLog("CrucialWarning", LogImportance.Crucial, LogType.Warning);
  public void CriticalError() => logger.ConditionalLog("CriticalError", LogImportance.Critical, LogType.Error);
}
```

## Support the project!

<a href="https://www.patreon.com/voxelltech" target="_blank">
  <img src="https://teaprincesschronicles.files.wordpress.com/2020/03/support-me-on-patreon.png" alt="patreon" width="200px" height="56px"/>
</a>

<a href ="https://ko-fi.com/voxelltech" target="_blank">
  <img src="https://uploads-ssl.webflow.com/5c14e387dab576fe667689cf/5cbed8a4cf61eceb26012821_SupportMe_red.png" alt="kofi" width="200px" height="40px"/>
</a>

## Join the community!

<a href ="https://discord.gg/WDBnuNH" target="_blank">
  <img src="https://gist.githubusercontent.com/nixon-voxell/e7ba303906080ffdf65b106f684801b5/raw/65b0338d5f4e82f700d3c9f14ec9fc62f3fd278e/JoinVXDiscord.svg" alt="discord" width="200px" height="200px"/>
</a>


## License

This repository as a whole is licensed under the Apache License 2.0. Individual files may have a different, but compatible license.

See [license file](./LICENSE) for details.

## References

- https://github.com/dbrizov/NaughtyAttributes