using System.IO;

namespace SourceEngine.Bsp
{
    internal struct Header
    {
        public int Identifier; // BSP file identifier
        public int Version; // BSP file version
        public Lump[] Lumps; // Lump directory array
        public int MapRevision; // Map's version/revision number

        public Header(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            Version = reader.ReadInt32();
            Lumps = new Lump[64];

            for (int lump = 0; lump < Lumps.Length; ++lump)
                Lumps[lump] = new Lump(reader);

            MapRevision = reader.ReadInt32();
        }
    }
}
