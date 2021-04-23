namespace SourceEngine.Bsp
{
    internal struct Lump
    {
        public int FileOffset; // Offset into the file (bytes)
        public int FileLength; // Length of lump (bytes)
        public int Version; // Lump format version
        public byte[] FourCC; // Lump identifier code
    }
}
