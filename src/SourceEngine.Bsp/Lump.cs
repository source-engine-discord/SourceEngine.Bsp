using System;
using System.IO;

namespace SourceEngine.Bsp
{
    internal struct Lump : IComparable<Lump>
    {
        public int FileOffset; // Offset into the file (bytes)
        public int FileLength; // Length of lump (bytes)
        public int Version; // Lump format version
        public byte[] FourCC; // Lump identifier code

        public Lump(BinaryReader reader)
        {
            FileOffset = reader.ReadInt32();
            FileLength = reader.ReadInt32();
            Version = reader.ReadInt32();
            FourCC = new byte[4];

            for (int i = 0; i < FourCC.Length; ++i)
                FourCC[i] = reader.ReadByte();
        }

        public int CompareTo(Lump other)
        {
            return FileOffset - other.FileOffset;
        }
    }
}
