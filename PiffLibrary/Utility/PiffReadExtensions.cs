using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


[assembly:InternalsVisibleTo("PiffLibrary.Test")]


namespace PiffLibrary
{
    /// <summary>
    /// The methods here are not thread-safe due to use of static buffers.
    /// Switching to .NET Standard 2.1 or .NET 5+ would allow using Span<>
    /// and stackalloc, eliminating this problem.
    /// </summary>
    internal static class PiffReadExtensions
    {
        #region Static buffers

        private static readonly byte[] mInt16Buffer = new byte[sizeof(ushort)];

        private static readonly byte[] mInt24Buffer = new byte[3];

        private static readonly byte[] mInt32Buffer = new byte[sizeof(uint)];

        private static readonly byte[] mInt64Buffer = new byte[sizeof(ulong)];

        private static readonly byte[] mGuidBuffer = new byte[16];

        private static UnicodeEncoding mUtf16 = new UnicodeEncoding(true, true);

        #endregion


        #region Read data from a byte array

        /// <summary>
        /// Read a 16-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static short GetInt16(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 8) |
                       bytes[offset + 1];

            return (short) res;
        }


        /// <summary>
        /// Read a 16-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static ushort GetUInt16(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 8) |
                       bytes[offset + 1];

            return (ushort) res;
        }


        /// <summary>
        /// Read a 24-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static int GetInt24(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 16) |
                      (bytes[offset + 1] << 8) |
                       bytes[offset + 2];

            return res;
        }


        /// <summary>
        /// Read a 32-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static int GetInt32(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 24) |
                      (bytes[offset + 1] << 16) |
                      (bytes[offset + 2] << 8) |
                       bytes[offset + 3];

            return res;
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static uint GetUInt32(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 24) |
                      (bytes[offset + 1] << 16) |
                      (bytes[offset + 2] << 8) |
                       bytes[offset + 3];

            return (uint) res;
        }


        /// <summary>
        /// Read a 64-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static long GetInt64(this byte[] bytes, int offset = 0)
        {
            return ((long) bytes.GetInt32(offset) << 32) |
                           bytes.GetUInt32(offset + 4);
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static ulong GetUInt64(this byte[] bytes, int offset = 0)
        {
            return ((ulong) bytes.GetUInt32(offset) << 32) |
                            bytes.GetUInt32(offset + 4);
        }

        #endregion


        #region Read data from a stream

        /// <summary>
        /// Read a 16-bit integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadInt16(this BitReadStream bytes, out short result)
        {
            var len = bytes.Read(mInt16Buffer, 0, mInt16Buffer.Length);
            if (len < mInt16Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt16Buffer.GetInt16();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 16-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadUInt16(this BitReadStream bytes, out ushort result)
        {
            var len = bytes.Read(mInt16Buffer, 0, mInt16Buffer.Length);
            if (len < mInt16Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt16Buffer.GetUInt16();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 24-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadInt24(this BitReadStream bytes, out int result)
        {
            var len = bytes.Read(mInt24Buffer, 0, mInt24Buffer.Length);
            if (len < mInt24Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt24Buffer.GetInt24();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadInt32(this BitReadStream bytes, out int result)
        {
            var len = bytes.Read(mInt32Buffer, 0, mInt32Buffer.Length);
            if (len < mInt32Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt32Buffer.GetInt32();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadUInt32(this BitReadStream bytes, out uint result)
        {
            var len = bytes.Read(mInt32Buffer, 0, mInt32Buffer.Length);
            if (len < mInt32Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt32Buffer.GetUInt32();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 64-bit integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadInt64(this BitReadStream bytes, out long result)
        {
            var len = bytes.Read(mInt64Buffer, 0, mInt64Buffer.Length);
            if (len < mInt64Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt64Buffer.GetInt64();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadUInt64(this BitReadStream bytes, out ulong result)
        {
            var len = bytes.Read(mInt64Buffer, 0, mInt64Buffer.Length);
            if (len < mInt64Buffer.Length)
            {
                result = 0;
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            result = mInt64Buffer.GetUInt64();
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read 8..32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadDynamicInt(this BitReadStream bytes, out int result)
        {
            byte b;
            var read = 0;
            result = 0;

            do
            {
                var status = bytes.ReadByte(out b);
                if (status != PiffReadStatuses.Continue)
                    return result == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;

                result = (result << 7) | (b & 0x7F);
                read++;
            }
             while (read < 4 && b > 0x7F);

            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 16-byte GUID in big-endian format from a stream.
        /// </summary>
        internal static PiffReadStatuses ReadGuid(this BitReadStream bytes, out Guid result)
        {
            var len = bytes.Read(mGuidBuffer, 0, mGuidBuffer.Length);
            if (len < mGuidBuffer.Length)
                return len == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;

            var fromBe = (from i in PiffWriteExtensions.GuidByteOrder select mGuidBuffer[i]).ToArray();
            result = new Guid(fromBe);
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read the given number of ASCII characters from a string.
        /// </summary>
        public static PiffReadStatuses ReadAsciiString(this BitReadStream bytes, int length, out string ascii)
        {
            var buffer = new byte[length];
            var read = bytes.Read(buffer, 0, length);

            if (read < length)
            {
                ascii = string.Empty;
                return read == 0 ? PiffReadStatuses.Eof : PiffReadStatuses.EofPremature;
            }

            ascii = Encoding.ASCII.GetString(buffer);
            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a 0-terminated ASCII string.
        /// </summary>
        public static PiffReadStatuses ReadAsciiZeroString(this BitReadStream bytes, out string ascii)
        {
            var str = new List<byte>();
            PiffReadStatuses status;

            while (true)
            {
                status = bytes.ReadByte(out var b);
                if (status != PiffReadStatuses.Continue)
                {
                    if (str.Count != 0) status = PiffReadStatuses.EofPremature;
                    break;
                }
                if (b == 0) break;

                str.Add(b);
            }

            ascii = Encoding.ASCII.GetString(str.ToArray());
            return status;
        }


        /// <summary>
        /// Read a Pascal-style ASCII string.
        /// </summary>
        public static PiffReadStatuses ReadAsciiPascalString(this BitReadStream bytes, out string ascii)
        {
            var status = bytes.ReadByte(out var length);
            if (status != PiffReadStatuses.Continue)
            {
                ascii = string.Empty;
                return PiffReadStatuses.Eof;
            }

            var buf = new byte[length];
            var read = bytes.Read(buf, 0, length);
            if (read < length)
            {
                ascii = string.Empty;
                return PiffReadStatuses.EofPremature;
            }

            ascii = Encoding.ASCII.GetString(buf);
            return status;
        }


        /// <summary>
        /// Read a 0-terminated UTF8 string.
        /// </summary>
        public static PiffReadStatuses ReadUtf8ZeroString(this BitReadStream bytes, out string utf)
        {
            var str = new List<byte>();
            PiffReadStatuses status;

            while (true)
            {
                status = bytes.ReadByte(out var b);
                if (status != PiffReadStatuses.Continue)
                {
                    if (str.Count == 0) status = PiffReadStatuses.Eof;
                    break;
                }
                if (b == 0) break;

                str.Add(b);
            }

            utf = Encoding.UTF8.GetString(str.ToArray());
            return status;
        }


        /// <summary>
        /// Read a 0-terminated UTF-8 or UTF-16 string.
        /// UTF-16 must start with byte order mark 0xFEFF.
        /// </summary>
        public static PiffReadStatuses ReadUtf8Or16ZeroString(this BitReadStream bytes, out string utf)
        {
            var str = new List<byte>();
            PiffReadStatuses status;

            while (true)
            {
                status = bytes.ReadByte(out var b);
                if (status != PiffReadStatuses.Continue)
                {
                    if (str.Count == 0) status = PiffReadStatuses.Eof;
                    break;
                }
                if (b == 0) break;

                str.Add(b);
            }

            if (str.Count > 2 && str[0] == 0xFE && str[1] == 0xFF)
                utf = mUtf16.GetString(str.ToArray());
            else
                utf = Encoding.UTF8.GetString(str.ToArray());

            return status;
        }

        #endregion
    }
}
