# SourceEngine.Bsp

A library to calculate a CRC32 checksum for a BSP file. BSP versions 19 to 21 are supported.

The checksum is a concatenation of all lumps in the BSP except for the entities lump; it is not merely a checksum of the entire content file. This is the same algorithm the Source Engine server uses to verify a client's map. For CS:GO, a map's CRC can be found in the `ServerInfo` network message as [`map_crc`][1].

## Example

```c#
using System;
using System.IO;

using SourceEngine.Bsp;

class Program
{
    static void Main(string[] args)
    {
        using FileStream stream = File.OpenRead("map.bsp");
        uint crc = BspCrc32.Compute(stream);
        Console.WriteLine($"Crc is {crc}");
    }
}
```

## Releases

For every release, the package is automatically published to the NuGet gallery at nuget.org.

### Creating a Release

Update the version number in [`Directory.Build.props`][2] and commit the change. Then, create a tag with git. It is recommended to use annotated tags:

```
git tag -a v3.5.0 -m 'A brief description of the release'
```

Note that CI enforces [SemVer 2.0.0][3] format compliance as well as the versions in the tag and the project being equal.

Finally, push the tag along with the commit for the version bump:

```
git push --follow-tags
```

[1]: https://github.com/SteamDatabase/Protobufs/blob/9f853ceb7345bbbd3bc3b3731285638d8bdbf7b7/csgo/netmessages.proto#L240
[2]: Directory.Build.props
[3]: https://semver.org/spec/v2.0.0.html
