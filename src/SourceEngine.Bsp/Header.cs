using System;
using System.IO;

namespace SourceEngine.Bsp
{
    internal struct Header
    {
        private const int IDENTIFIER = ('P' << 24) + ('S' << 16) + ('B' << 8) + 'V';
        private const int MIN_BSP_VERSION = 19;
        private const int BSP_VERSION = 21;

        public int Identifier; // BSP file identifier
        public int Version; // BSP file version
        public Lump[] Lumps; // Lump directory array
        public int MapRevision; // Map's version/revision number

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
