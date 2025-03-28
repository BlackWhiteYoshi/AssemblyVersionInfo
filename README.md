# AssemblyVersionInfo

AssemblyVersionInfo is a very simple source generator that generates constant strings of your assembly name and version.  
The intended usage is for the [GeneratedCodeAttribute](https://learn.microsoft.com/en-us//dotnet/api/system.codedom.compiler.generatedcodeattribute).

All it does is generating a the static class *Assembly* in the namespace *AssemblyVersionInfo*:

```csharp
// <auto-generated/>

namespace AssemblyVersionInfo;

internal static class Assembly {
    public const string NAME = "{{compilation.name}}";

    public const string VERSION_MAJOR = "{{compilation.version.Major}}";
    public const string VERSION_MINOR = "{{compilation.version.Minor}}";
    public const string VERSION_BUILD = "{{compilation.version.Build}}";
    public const string VERSION_REVISION = "{{compilation.version.Revision}}";

    public const string VERSION = "{{compilation.version}}";
    public const string VERSION_MAJOR_MINOR = "{{compilation.version.Major}}.{{compilation.version.Minor}}";
    public const string VERSION_MAJOR_MINOR_BUILD = "{{compilation.version.Major}}.{{compilation.version.Minor}}.{{compilation.version.Build}}";
}
```


<br></br>
## Get Started

1. Add PackageReference to your .csproj file.

```xml
<ItemGroup>
  <PackageReference Include="AssemblyVersionInfo" Version="{latest version}" PrivateAssets="all" />
</ItemGroup>
```

2. Access the strings in the *Assembly* class.

```csharp
using AssemblyVersionInfo;

const string example = $"AssemblyName={Assembly.NAME}, AssemblyVersion={Assembly.VERSION}";
Console.WriteLine(example);
```
