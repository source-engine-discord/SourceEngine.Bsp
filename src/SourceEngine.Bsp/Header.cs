using System.Collections.Immutable;
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

        /// <summary>
        /// BSP file identifier.
        /// </summary>
        /// <remarks>
        /// Should always be "VBSP" in ASCII.
        /// </remarks>
        public readonly int Identifier;

        /// <summary>
        /// BSP file version.
        /// </summary>
        /// <remarks>
        /// Only versions 19 to 21, inclusive, are supported.
        /// </remarks>
        public readonly int Version;

        /// <summary>
        /// Lump directory which contains information to locate lump data within the file.
        /// </summary>
        /// <seealso cref="Lump"/>
        public readonly ImmutableArray<Lump> Lumps;

        /// <summary>
        /// Map's revision/version number.
        /// </summary>
        /// <remarks>
        /// Increased each time the map is saved in the Hammer editor.
        /// </remarks>
        public readonly int MapRevision;

        /// <summary>
        /// Initialises a new instance of the <see cref="Header"/> struct based
        /// on the data read from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read BSP data from.</param>
        /// <exception cref="InvalidDataException">
        /// The identifier is invalid or the BSP version is unsupported.
        /// </exception>
        public Header(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            if (Identifier != IDENTIFIER)
                throw new InvalidDataException("Invalid BSP header identifier.");

            Version = reader.ReadInt32();
            if (Version is < MIN_BSP_VERSION or > BSP_VERSION)
                throw new InvalidDataException($"Map has incorrect BSP version ({Version} should be {BSP_VERSION}).");

            ImmutableArray<Lump>.Builder lumps = ImmutableArray.CreateBuilder<Lump>(64);
            for (byte lump = 0; lump < 64; ++lump)
                lumps[lump] = new Lump(reader, lump);

            Lumps = lumps.ToImmutable();
            MapRevision = reader.ReadInt32();
        }
    }
}
