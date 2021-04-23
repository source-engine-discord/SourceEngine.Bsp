using System;
using System.Collections.Generic;
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

        public IEnumerable<int> Read(BinaryReader reader, byte[] buffer)
        {
            int size = FileLength;
            if (size <= 0)
                yield break;

            reader.BaseStream.Seek(FileOffset, SeekOrigin.Begin);

            while (size > 0)
            {
                int countToRead = size > buffer.Length ? buffer.Length : size;
                int bytesRead = reader.Read(buffer, 0, countToRead);

                if (bytesRead > 0)
                {
                    size -= bytesRead;
                    yield return bytesRead;
                }
            }
        }
    }
}
