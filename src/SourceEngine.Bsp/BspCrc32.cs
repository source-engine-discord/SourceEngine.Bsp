using System;
using System.IO;

using Force.Crc32;

namespace SourceEngine.Bsp
{
    /// <summary>
    /// Provides a static method for computing the CRC32 checksum of a BSP file.
    /// </summary>
    public static class BspCrc32
    {
        private const int LUMP_ENTITIES = 0;

        /// <summary>
        /// Computes a CRC-32 for a BSP file read from a <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// The checksum is a concatenation of all lumps in the BSP except for
        /// the entities lump; it is not merely a checksum of the entire content file.
        /// </remarks>
        /// <param name="input">The stream of data for the BSP file.</param>
        /// <returns>The CRC32 checksum for the given BSP file.</returns>
        /// <exception cref="ArgumentException">
        /// The input stream does not support reading or seeking, is <see langword="null"/>, or is already closed.
        /// </exception>
        /// <exception cref="Exception">The identifier is invalid or the BSP version is unsupported.</exception>
        public static uint Compute(Stream input)
        {
            if (!input.CanSeek)
                throw new ArgumentException("Input stream must be seekable.", nameof(input));

            using var reader = new BinaryReader(input);

            var header = new Header(reader);
            Array.Sort(header.Lumps);

            uint crc = uint.MaxValue;
            byte[] chunk = new byte[65536];

            foreach (Lump lump in header.Lumps)
            {
                if (lump.Type == LUMP_ENTITIES)
                    continue; // Entities lump should never be in the checksum.

                // Every append XORs the CRC with uint.MaxValue at the start and
                // at the end. Source Engine does not do this. Therefore, take
                // the bitwise complement to cancel out the XORs.
                foreach (var bytesRead in lump.Read(reader, chunk))
                    crc = ~Crc32Algorithm.Append(~crc, chunk, 0, bytesRead);
            }

            return crc;
        }
    }
}
