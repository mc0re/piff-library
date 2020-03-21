namespace PiffLibrary
{
    internal class PiffReader
    {
        internal static int GetInt32(byte[] bytes, int offset)
        {
            var res = (bytes[offset] << 24) +
                      (bytes[offset + 1] << 16) +
                      (bytes[offset + 2] << 8) +
                      bytes[offset + 3];

            return res;
        }
    }
}