using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PiffLibrary
{
    public static class PiffWriter
    {
        #region Helper class

        private class ValueToFormat
        {
            public object Value { get; set; }

            public bool IsArray { get; internal set; }

            public PiffDataFormats Format { get; set; }
        }

        #endregion


        #region API

        public static void WriteHeader(Stream strm, PiffManifest manifest)
        {
            var movie = new PiffMovieMetadata(manifest);

            var ftypBytes = WriteBoxObject(new PiffFileType()).ToArray();
            strm.Write(ftypBytes, 0, ftypBytes.Length);

            var hdrBytes = WriteBoxObject(movie).ToArray();
            strm.Write(hdrBytes, 0, hdrBytes.Length);
        }


        public static TimeSpan GetDuration(long duration, int timeScale)
            => TimeSpan.FromSeconds(duration / (double)timeScale);


        internal static long GetSecondsFromEpoch(DateTime time)
        {
            return (long)(time - new DateTime(1904, 1, 1)).TotalSeconds;
        }


        internal static long GetTicks(TimeSpan duration, int timeScale)
            => (long)(duration.TotalSeconds * timeScale);

        #endregion


        #region Writing utility

        /// <summary>
        /// Create a byte stream representation of the given object.
        /// </summary>
        private static IEnumerable<byte> WriteBoxObject(object obj)
        {
            if (obj is null)
                return Enumerable.Empty<byte>();

            var type = obj.GetType();

            var boxNameAttr = type.GetCustomAttribute<BoxNameAttribute>();
            if (boxNameAttr is null)
                throw new ArgumentException($"Box name is not defined for type '{type.Name}'.");

            var propValues =
                from prop in type.GetProperties()
                let val = prop.GetValue(obj, Array.Empty<object>())
                let isArray = prop.PropertyType.IsArray
                select new ValueToFormat
                {
                    Value = val,
                    IsArray = isArray,
                    Format = val == null
                            ? PiffDataFormats.Skip
                            : prop.GetCustomAttribute<PiffDataFormatAttribute>()?.Format
                            ?? (isArray ? GetDefaultFormat(prop.PropertyType.GetElementType()) : GetDefaultFormat(prop.PropertyType))
                };

            return WriteBoxValues(boxNameAttr.Name, propValues.ToArray());
        }


        /// <summary>
        /// Write a box with values.
        /// </summary>
        private static IEnumerable<byte> WriteBoxValues(string boxName, params ValueToFormat[] values)
        {
            var dataBytes = new List<byte>();

            foreach (var value in values)
            {
                dataBytes.AddRange(WriteValue(value));
            }

            var boxLength = sizeof(int) + boxName.Length + dataBytes.Count;
            var hdrBytes = ConvertInt32ToBigEndian(boxLength).Concat(
                           Encoding.ASCII.GetBytes(boxName));

            return hdrBytes.Concat(dataBytes);
        }


        /// <summary>
        /// Unless the format is explicitly specified, use the default one.
        /// </summary>
        private static PiffDataFormats GetDefaultFormat(Type valueType)
        {
            if (valueType == typeof(byte) || valueType == typeof(char))
                return PiffDataFormats.Int8;

            else if (valueType == typeof(short))
                return PiffDataFormats.Int16;

            else if (valueType == typeof(int))
                return PiffDataFormats.Int32;

            else if (valueType == typeof(uint))
                return PiffDataFormats.UInt32;

            else if (valueType == typeof(long))
                return PiffDataFormats.Int64;

            else if (valueType == typeof(string))
                return PiffDataFormats.Ascii;

            else if (valueType == typeof(Guid))
                return PiffDataFormats.GuidBytes;

            else if (valueType.IsClass)
                return PiffDataFormats.Object;

            else
                throw new ArgumentException($"Unsupported data type '{valueType.Name}'.");
        }


        /// <summary>
        /// Write a single value or array with the given format.
        /// </summary>
        private static IEnumerable<byte> WriteValue(ValueToFormat value)
        {
            var dataBytes = new List<byte>();

            if (value.Format == PiffDataFormats.Skip)
                return dataBytes;

            if (value.IsArray)
            {
                foreach (var item in value.Value as Array)
                {
                    dataBytes.AddRange(WritePrimitiveValue(item, value.Format));
                }
            }
            else
            {
                dataBytes.AddRange(WritePrimitiveValue(value.Value, value.Format));
            }

            return dataBytes;
        }


        /// <summary>
        /// Write a single value with the given format.
        /// </summary>
        private static IEnumerable<byte> WritePrimitiveValue(object value, PiffDataFormats format)
        {
            var dataBytes = new List<byte>();

            switch (format)
            {
                case PiffDataFormats.Int8:
                    dataBytes.Add((byte)value);
                    break;

                case PiffDataFormats.Int16:
                    dataBytes.AddRange(ConvertInt16ToBigEndian((short)value));
                    break;

                case PiffDataFormats.Int24:
                    dataBytes.AddRange(ConvertInt32ToBigEndian((int)value).Skip(1));
                    break;

                case PiffDataFormats.Int32:
                    dataBytes.AddRange(ConvertInt32ToBigEndian((int)value));
                    break;

                case PiffDataFormats.UInt32:
                    dataBytes.AddRange(ConvertUnsignedInt32ToBigEndian((uint)value));
                    break;

                case PiffDataFormats.Int64:
                    dataBytes.AddRange(ConvertInt64ToBigEndian((long)value));
                    break;

                case PiffDataFormats.DynamicInt:
                    dataBytes.AddRange(ConvertInt32ToDynamic((int)value));
                    break;

                case PiffDataFormats.Ascii:
                    dataBytes.AddRange(Encoding.ASCII.GetBytes((string)value));
                    break;

                case PiffDataFormats.Utf8Zero:
                    dataBytes.AddRange(Encoding.UTF8.GetBytes((string)value));
                    dataBytes.Add(0);
                    break;

                case PiffDataFormats.Ucs2:
                    dataBytes.AddRange(Encoding.Unicode.GetBytes((string)value));
                    break;

                case PiffDataFormats.GuidBytes:
                    var guidBytes = ((Guid)value).ToByteArray();
                    // Need reformatting
                    dataBytes.AddRange(from i in new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 } select guidBytes[i]);
                    break;

                case PiffDataFormats.Object:
                    dataBytes.AddRange(WriteBoxObject(value));
                    break;

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }

            return dataBytes;
        }

        #endregion


        #region Primitives utility

        private static IEnumerable<byte> ConvertInt16ToBigEndian(short value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        private static IEnumerable<byte> ConvertInt32ToBigEndian(int value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        private static IEnumerable<byte> ConvertUnsignedInt32ToBigEndian(uint value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        private static IEnumerable<byte> ConvertInt64ToBigEndian(long value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Write multiple bytes, with highest bit signifying that another byte follows.
        /// </summary>
        /// <remarks>
        /// To simplify length calculation, always write 4 bytes.
        /// </remarks>
        private static IEnumerable<byte> ConvertInt32ToDynamic(int value)
        {
            var bytes = new byte[4];
            var idx = 3;

            while (value > 0)
            {
                var part = value & 0x7F;
                value >>= 7;
                bytes[idx] = (byte)part;
                idx--;
            }

            for (var setBitIdx = 0; setBitIdx < 3; setBitIdx++)
                bytes[setBitIdx] |= 0x80;

            return bytes;
        }

        #endregion
    }
}