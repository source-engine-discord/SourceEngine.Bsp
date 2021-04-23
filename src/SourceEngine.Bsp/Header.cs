using System;
using System.IO;

namespace SourceEngine.Bsp
{
    /// <summary>
    /// The header of a BSP file.
    /// </summary>
    /// <remarks>
    /// Supports BSP versions 19 to 21, inclusive.
    /// </remarks>
    /// <seealso href="https://developer.valvesoftware.com/wiki/Source_BSP_File_Format#BSP_file_header"/>
    internal readonly struct Header
    {
        private const int IDENTIFIER = ('P' << 24) + ('S' << 16) + ('B' << 8) + 'V';
        private const int MIN_BSP_VERSION = 19;
        private const int BSP_VERSION = 21;

        public readonly int Identifier; // BSP file identifier
        public readonly int Version; // BSP file version
        public readonly Lump[] Lumps; // Lump directory array
        public readonly int MapRevision; // Map's version/revision number

        /// <summary>
        /// Initialises a new instance of the <see cref="Header"/> struct based
        /// on the data read from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read BSP data from.</param>
        /// <exception cref="Exception">The identifier is invalid or the BSP version is unsupported.</exception>
        public Header(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            if (Identifier != IDENTIFIER)
                throw new Exception("Invalid BSP header identifier.");

            Version = reader.ReadInt32();
            if (Version is < MIN_BSP_VERSION or > BSP_VERSION)
                throw new Exception($"Map has incorrect BSP version ({Version} should be {BSP_VERSION}).");

            Lumps = new Lump[64];
            for (byte lump = 0; lump < Lumps.Length; ++lump)
                Lumps[lump] = new Lump(reader, lump);

            MapRevision = reader.ReadInt32();
        }
    }
}
