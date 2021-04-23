using System;
using System.IO;

namespace SourceEngine.Bsp
{
    internal readonly struct Lump : IComparable<Lump>
    {
        public readonly int FileOffset; // Offset into the file (bytes)
        public readonly int FileLength; // Length of lump (bytes)
        public readonly int Version; // Lump format version
        public readonly byte[] FourCC; // Lump identifier code
        public readonly byte Type;

        public Lump(BinaryReader reader, byte type)
        {
            FileOffset = reader.ReadInt32();
            FileLength = reader.ReadInt32();
            Version = reader.ReadInt32();
            FourCC = new byte[4];
            Type = type;

            for (int i = 0; i < FourCC.Length; ++i)
                FourCC[i] = reader.ReadByte();
        }

        public int CompareTo(Lump other)
        {
            return FileOffset - other.FileOffset;
        }
    }
}
