# RichTap Vibration SDK

A .NET SDK for RichTap vibration.

## Setup

```xml
<PackageReference Include="RichTap" />
```

```csharp
using RichTap;
```

## How to use

Following are the methods to occur or abort vibration.

- `VibrationMotor.Play` Vibrates by a HE format file with specific options.
- `VibrationMotor.Stop` Abort current vibration.

You can also build or deserialize the HE format content by following model.

- `VibrationDescriptionModel`

## Requirement

Please ensure following C++ native libraries exist during runtime.

- `x86\RichTapWinSDK.dll`
- `x64\RichTapWinSDK.dll`
- `arm64\RichTapWinSDK.dll`

## CLI

The command line interface app is used to demo the vibration functionalities.
You need input a path of HE format file to test.
