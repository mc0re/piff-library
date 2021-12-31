using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace PiffLibrary
{
    /// <summary>
    /// Read and write individual values.
    /// 
    /// This class knows about objects and arrays, but not the box format.
    /// This knowledge is delegated to <see cref="PiffReader"/> and <see cref="PiffWriter"/>.
    /// </summary>
    internal class PiffPropertyInfo
    {
        #region Properties

        /// <summary>
        /// The original property defining this field.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Whether the property is an array.
        /// In this case, <see cref="ArraySize"/> might define the size of the array.
        /// </summary>
        public bool IsArray { get; }

        /// <summary>
        /// Type of the field or item type if it is an array.
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        /// Data format for this property or for an item, if this is an array.
        /// </summary>
        public PiffDataFormats Format { get; }

        /// <summary>
        /// The number of bytes in the value or for an item, if it is an array.
        /// </summary>
        public int ItemSize { get; }
        
        /// <summary>
        /// If not specified, read until the end of box.
        /// </summary>
        public int? ArraySize { get; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Process property information, extract the needed bits for reading.
        /// </summary>
        public PiffPropertyInfo(PropertyInfo prop, object target)
        {
            Property = prop;
            IsArray = prop.PropertyType.IsArray;
            ElementType = IsArray ? prop.PropertyType.GetElementType() : prop.PropertyType;

            var formatAttr = prop.GetCustomAttribute<PiffDataFormatAttribute>();

            if (formatAttr is null)
            {
                Format = PiffDataUtility.GetDefaultFormat(ElementType);
            }
            else if (formatAttr.FormatFn != null)
            {
                var fn = target.GetType().GetMethod(formatAttr.FormatFn);
                Format = (PiffDataFormats) fn.Invoke(target, null);
            }
            else
            {
                Format = formatAttr.Format;
            }

            var lengthAttr = Property.GetCustomAttribute<PiffDataLengthAttribute>();
            if (Format == PiffDataFormats.Ascii)
            {
                if (lengthAttr is null)
                    throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' must have length specified using {nameof(PiffDataLengthAttribute)}.");

                ItemSize = lengthAttr.Length;
            }
            else if (lengthAttr != null)
            {
                throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' is not a string and thus cannot have {nameof(PiffDataLengthAttribute)}.");
            }

            var sizeAttr = Property.GetCustomAttribute<PiffArraySizeAttribute>();
            if (IsArray && sizeAttr != null)
            {
                if (sizeAttr.SizeProp != null)
                {
                    var sizeProp = target.GetType().GetProperty(sizeAttr.SizeProp);
                    ArraySize = (int)Convert.ChangeType(sizeProp.GetValue(target, null), typeof(int));
                }
                else
                {
                    ArraySize = sizeAttr.Size;
                }
            }
            else if (sizeAttr != null)
            {
                throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' is not an array and thus cannot have {nameof(PiffArraySizeAttribute)}.");
            }
        }

        #endregion


        #region API

        /// <summary>
        /// Read out properties of <paramref name="target"/> object from <paramref name="input"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        public static long ReadObject(object target, Stream input, long bytesLeft)
        {
            var readBytes = 0L;

            foreach (var prop in target.GetType().GetProperties())
            {
                var readInfo = new PiffPropertyInfo(prop, target);
                var readSize = readInfo.ReadValue(input, bytesLeft, target);
                readBytes += readSize;
                bytesLeft -= readSize;
            }

            return readBytes;
        }


        /// <summary>
        /// Write all properties of the given object.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<byte> WriteObject(object source)
        {
            return source.GetType().GetProperties()
                        .Select(prop => new PiffPropertyInfo(prop, source))
                        .SelectMany(p => p.WriteValue(source));
        }


        /// <summary>
        /// Read this value into the given <paramref name="target"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        public long ReadValue(Stream input, long bytesLeft, object target)
        {
            var readBytes = 0L;

            if (IsArray)
            {
                var list = new List<object>();
                var count = ArraySize ?? int.MaxValue;

                while (count > 0 && bytesLeft > 0)
                {
                    var readSize = ReadSingleValue(input, bytesLeft, target, out object item);
                    list.Add(item);
                    readBytes += readSize;
                    bytesLeft -= readSize;
                    count--;
                }

                if (Property.CanWrite)
                {
                    var array = Array.CreateInstance(ElementType, list.Count);
                    Property.SetValue(target, array);

                    for (var i = 0; i < list.Count; i++)
                        array.SetValue(list[i], i);
                }
            }
            else if (Format != PiffDataFormats.Skip)
            {
                readBytes = ReadSingleValue(input, bytesLeft, target, out object val);

                if (Property.CanWrite)
                {
                    Property.SetValue(target, val);
                }
            }

            return readBytes;
        }


        /// <summary>
        /// Write a single value or array with the given format.
        /// Skip the writing alltogether if the valus is <see langword="null"/>.
        /// </summary>
        public IEnumerable<byte> WriteValue(object target)
        {
            var value = Property.GetValue(target);

            if (value is null || Format == PiffDataFormats.Skip)
                return Enumerable.Empty<byte>();

            var dataBytes = new List<byte>();

            if (IsArray)
            {
                foreach (var item in value as Array)
                {
                    dataBytes.AddRange(WriteSingleValue(item, Format));
                }
            }
            else
            {
                dataBytes.AddRange(WriteSingleValue(value, Format));
            }

            return dataBytes;
        }

        #endregion


        #region Reading utility

        /// <summary>
        /// Read a box, a POCO, a fixed-length string, or a primitive value.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private long ReadSingleValue(Stream input, long bytesLeft, object parent, out object val)
        {
            long readBytes;

            switch (Format)
            {
                case PiffDataFormats.Ascii:
                    val = input.ReadAsciiString(ItemSize);
                    readBytes = ItemSize;
                    break;

                case PiffDataFormats.InlineObject:
                    readBytes = ReadPoco(input, bytesLeft, ElementType, parent, out val);
                    break;

                case PiffDataFormats.Box:
                    readBytes = PiffReader.ReadBox(input, ElementType, out PiffBoxBase box);
                    val = box;
                    break;

                default:
                    readBytes = ReadPrimitiveValue(input, Format, out val);
                    break;
            }

            return readBytes;
        }


        /// <summary>
        /// Read an integer (of many formats), a GUID or a zero-terminated string.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static long ReadPrimitiveValue(Stream bytes, PiffDataFormats format, out object value)
        {
            var pos = bytes.Position;

            switch (format)
            {
                case PiffDataFormats.Int2Minus1:
                    value = (byte)((bytes.ReadByte() & 0x3) + 1);
                    break;

                case PiffDataFormats.Int5:
                    value = (byte)(bytes.ReadByte() & 0x1F);
                    break;

                case PiffDataFormats.Int8:
                    value = (byte)bytes.ReadByte();
                    break;

                case PiffDataFormats.Int16:
                    value = (short)bytes.ReadInt16();
                    break;

                case PiffDataFormats.Int24:
                    value = bytes.ReadInt24();
                    break;

                case PiffDataFormats.Int32:
                    value = (int)bytes.ReadUInt32();
                    break;

                case PiffDataFormats.UInt32:
                    value = bytes.ReadUInt32();
                    break;

                case PiffDataFormats.Int64:
                    value = (long)bytes.ReadUInt64();
                    break;

                case PiffDataFormats.DynamicInt:
                    value = bytes.ReadDynamicInt();
                    break;

                case PiffDataFormats.GuidBytes:
                    value = bytes.ReadGuid();
                    break;

                case PiffDataFormats.Utf8Zero:
                    value = bytes.ReadUtf8String();
                    break;

                default:
                    throw new ArgumentException($"Format '{format}' is not yet supported for reading.");
            }

            return bytes.Position - pos;
        }


        /// <summary>
        /// Read a plain object. If it has a constructor with a parent, use it.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static long ReadPoco(Stream input, long bytesLeft, Type propertyType, object parent, out object obj)
        {
            if (propertyType.GetConstructor(Type.EmptyTypes) != null)
                obj = Activator.CreateInstance(propertyType);
            else
                obj = Activator.CreateInstance(propertyType, parent);
        
            return ReadObject(obj, input, bytesLeft);
        }

        #endregion


        #region Writing utility

        /// <summary>
        /// Write a single value with the given format.
        /// </summary>
        private static IEnumerable<byte> WriteSingleValue(object value, PiffDataFormats format)
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
                    dataBytes.AddRange(((int)value).ToBigEndian().Skip(1));
                    break;

                case PiffDataFormats.Int32:
                    dataBytes.AddRange(((int)value).ToBigEndian());
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

                case PiffDataFormats.Int2Minus1:
                    dataBytes.Add((byte)(((byte)value - 1) | 0xFC));
                    break;

                case PiffDataFormats.Int5:
                    dataBytes.Add((byte)((byte)value | 0xE0));
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
                    // Need reformatting
                    dataBytes.AddRange(((Guid)value).ToBigEndianArray());
                    break;

                case PiffDataFormats.InlineObject:
                    // Write the object
                    dataBytes.AddRange(WriteObject(value));
                    break;

                case PiffDataFormats.Box:
                    dataBytes.AddRange(PiffWriter.WriteBoxObject((PiffBoxBase)value));
                    break;

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }

            return dataBytes;
        }


        private static IEnumerable<byte> ConvertInt16ToBigEndian(short value)
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
