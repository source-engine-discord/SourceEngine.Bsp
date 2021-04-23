namespace SourceEngine.Bsp
{
    internal struct Header
    {
        public int Identifier; // BSP file identifier
        public int Version; // BSP file version
        public Lump[] Lumps; // Lump directory array
        public int MapRevision; // Map's version/revision number
    }
}
