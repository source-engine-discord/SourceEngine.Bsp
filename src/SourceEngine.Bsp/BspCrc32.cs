using System;
using System.IO;

using Force.Crc32;

namespace SourceEngine.Bsp
{
    public static class BspCrc32
    {
        private const int LUMP_ENTITIES = 0;

        public static uint Compute(Stream input)
        {
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
                    crc = ~Crc32Algorithm.Append(~crc, chunk,0 , bytesRead);
            }

            return crc;
        }
    }
}
