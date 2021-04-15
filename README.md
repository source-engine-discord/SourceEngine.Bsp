# SourceEngine.Bsp

A library to calculate a CRC32 checksum for a BSP file. BSP versions 19 to 21 are supported.

The checksum is a concatenation of all lumps in the BSP except for the entities lump; it is not merely a checksum of the entire content file. This is the same algorithm the Source Engine server uses to verify a client's map. For CS:GO, a map's CRC can be found in the `ServerInfo` network message as [`map_crc`][1].

[1]: https://github.com/SteamDatabase/Protobufs/blob/9f853ceb7345bbbd3bc3b3731285638d8bdbf7b7/csgo/netmessages.proto#L240
