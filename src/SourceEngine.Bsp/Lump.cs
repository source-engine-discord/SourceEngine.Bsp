using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace SourceEngine.Bsp
{
    /// <summary>
    /// Contains information about BSP lump data of a specific type.
    /// It is mainly useful for locating the lump data within the BSP file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each lump in a BSP stores different parts of the map data in the BSP.
    /// This structure is part of the BSP <see cref="Header"/> in what is known
    /// as a "lump directory" (<see cref="Header.Lumps"/>).
    /// </para>
    ///
    /// <para>
    /// There are 64 different lumps (though not all are always used) and each
    /// one is a different lump type. A lump's type is determined by its
    /// position in the lump directory.
    /// </para>
    /// </remarks>
    /// <seealso href="https://developer.valvesoftware.com/wiki/Source_BSP_File_Format#Lump_structure"/>
    internal readonly struct Lump : IComparable<Lump>
    {
        /// <summary>
        /// Offset, in bytes, at which the lump's data is located in the file.
        /// </summary>
        public readonly int FileOffset;

        /// <summary>
        /// Length, in bytes, of the lump's data.
        /// </summary>
        public readonly int FileLength;

        /// <summary>
        /// Format version of the lump.
        /// </summary>
        public readonly int Version;

        /// <summary>
        /// 4-byte identifier code for the lump.
        /// </summary>
        /// <remarks>
        /// In practice, it's always all zeros.
        /// </remarks>
        public readonly ImmutableArray<byte> FourCC;

        /// <summary>
        /// Type of the lump.
        /// </summary>
        public readonly byte Type;

        /// <summary>
        /// <see cref="BinaryReader"/> from which to read lump data.
        /// </summary>
        private readonly BinaryReader reader;

        /// <summary>
        /// Initialises a new instance of the <see cref="Lump"/> struct based
        /// on the data read from the specified <paramref name="reader"/> and
        /// <paramref name="type"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read BSP data from.</param>
        /// <param name="type">The type of the lump.</param>
        public Lump(BinaryReader reader, byte type)
        {
            FileOffset = reader.ReadInt32();
            FileLength = reader.ReadInt32();
            Version = reader.ReadInt32();
            Type = type;
            this.reader = reader;

            ImmutableArray<byte>.Builder fourCC = ImmutableArray.CreateBuilder<byte>(4);
            for (int i = 0; i < 4; ++i)
                fourCC[i] = reader.ReadByte();

            FourCC = fourCC.ToImmutable();
        }

        /// <summary>
        /// Compares the current instance against another <see cref="Lump"/>
        /// based on the <see cref="Lump.FileOffset"/>.
        /// </summary>
        /// <param name="other">The <see cref="Lump"/> to compare against.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <seealso cref="IComparable{T}.CompareTo"/>
        public int CompareTo(Lump other)
        {
            return FileOffset - other.FileOffset;
        }

        /// <summary>
        /// An iterator method that reads the lump data pointed to by this instance in chunks.
        /// Each iteration, the <paramref name="buffer"/> is updated with the read data
        /// and the number of bytes read is yielded.
        /// </summary>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <returns>Yields the number of bytes read for the current chunk.</returns>
        public IEnumerable<int> Read(byte[] buffer)
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
