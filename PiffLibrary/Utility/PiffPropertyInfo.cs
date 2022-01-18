using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PiffLibrary.Boxes;


namespace PiffLibrary
{
    /// <summary>
    /// Read and write individual values.
    /// 
    /// This class knows about objects and arrays, but not the box format.
    /// This knowledge is delegated to <see cref="PiffReader"/> and <see cref="PiffWriter"/>.
    /// </summary>
    internal sealed class PiffPropertyInfo
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
        /// Process property information, extract the needed bits for reading / writing.
        /// </summary>
        private PiffPropertyInfo(PropertyInfo prop, object target)
        {
            Property = prop;
            IsArray = prop.PropertyType.IsArray;
            ElementType = IsArray ? prop.PropertyType.GetElementType() : prop.PropertyType;
            Format = GetPropertyFormat(prop, ElementType, target);

            var lengthAttr = Property.GetCustomAttribute<PiffStringLengthAttribute>();
            if (Format == PiffDataFormats.Ascii)
            {
                if (lengthAttr is null)
                    throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' must have length specified using {nameof(PiffStringLengthAttribute)}.");

                ItemSize = lengthAttr.Length;
            }
            else if (lengthAttr != null)
            {
                throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' is not a string and thus cannot have {nameof(PiffStringLengthAttribute)}.");
            }

            ArraySize = GetArraySize(prop, IsArray, target);
        }


        /// <summary>
        /// Process all properties of the given object.
        /// </summary>
        public static IEnumerable<PiffPropertyInfo> GetProperties(object target)
        {
            const BindingFlags basePropFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
            var targetType = target.GetType();
            var baseProps = targetType.BaseType.GetProperties(basePropFlags);
            var ownProps = targetType.GetProperties().Where(p => !baseProps.Any(bp => bp.Name == p.Name));
            var before = baseProps.Where(p => p.GetCustomAttribute<AfterDescendantsAttribute>() is null);
            var after = baseProps.Where(p => p.GetCustomAttribute<AfterDescendantsAttribute>() != null);

            var props = before.Concat(ownProps).Concat(after);

            return props.Select(prop => new PiffPropertyInfo(prop, target));
        }

        #endregion


        #region API

        /// <summary>
        /// Read out properties of <paramref name="target"/> object (can be a box)
        /// from <paramref name="input"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        public static long ReadObject(object target, BitReadStream input, long bytesLeft, PiffReadContext ctx)
        {
            var readBytes = 0L;

            foreach (var prop in GetProperties(target))
            {
                if (prop.Format == PiffDataFormats.Skip) continue;

                var readSize = prop.ReadValue(target, input, bytesLeft, ctx);
                if (readSize < 0)
                    return readSize;

                readBytes += readSize;

                if (bytesLeft < readSize)
                {
                    ctx.AddWarning($"At position {input.Position} read beyond box boundaries (bytes left {bytesLeft}, read {readSize}). Backtracking.");
                    input.Seek(-(int)(readSize - bytesLeft), SeekOrigin.Current);
                    break;
                }

                bytesLeft -= readSize;
                if (bytesLeft <= 0)
                    break;
            }

            return readBytes;
        }


        /// <summary>
        /// Get data length in bits, if the object was written to a stream.
        /// </summary>
        internal static ulong GetObjectLength(object source)
        {
            if (source is null)
                return 0;

            // There is no Sum() for ulong, therefore using Aggregate
            return GetProperties(source).Aggregate(0uL, (sz, p) => sz + p.GetValueLength(source));
        }


        /// <summary>
        /// Write all properties of the given object.
        /// </summary>
        public static void WriteObject(BitWriteStream output, object source, PiffWriteContext ctx)
        {
            foreach (var p in GetProperties(source))
                p.WriteValue(output, source, ctx);
        }


        /// <summary>
        /// Write a single value or array with the given format.
        /// Skip the writing alltogether if the valus is <see langword="null"/>.
        /// </summary>
        public void WriteValue(BitWriteStream output, object target, PiffWriteContext ctx)
        {
            var value = Property.GetValue(target);

            if (value is null || Format == PiffDataFormats.Skip)
                return;

            if (IsArray)
            {
                foreach (var item in value as Array)
                {
                    WriteSingleValue(output, item, Format, ctx);
                }
            }
            else
            {
                WriteSingleValue(output, value, Format, ctx);
            }
        }

        #endregion


        #region Format utility

        /// <summary>
        /// Check <see cref="PiffDataFormatAttribute"/> and retrieve info from it,
        /// or use the default format.
        /// </summary>
        private static PiffDataFormats GetPropertyFormat(PropertyInfo prop, Type propType, object target)
        {
            var formatAttr = prop.GetCustomAttribute<PiffDataFormatAttribute>();

            if (formatAttr is null)
            {
                return GetDefaultFormat(propType);
            }
            else if (formatAttr.FormatFn != null)
            {
                const BindingFlags formatFnFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
                var fn = target.GetType().GetMethod(formatAttr.FormatFn, formatFnFlags);

                if (fn is null)
                    throw new ArgumentException($"Method {formatAttr.FormatFn} not found on object {prop.DeclaringType.Name}.");

                return (PiffDataFormats) fn.Invoke(target, null);
            }
            else
            {
                return formatAttr.Format;
            }
        }


        /// <summary>
        /// Unless the format is explicitly specified, use the default one.
        /// </summary>
        private static PiffDataFormats GetDefaultFormat(Type valueType)
        {
            if (valueType == typeof(byte) || valueType == typeof(char))
                return PiffDataFormats.UInt8;

            else if (valueType == typeof(short))
                return PiffDataFormats.Int16;

            else if (valueType == typeof(ushort))
                return PiffDataFormats.UInt16;

            else if (valueType == typeof(int))
                return PiffDataFormats.Int32;

            else if (valueType == typeof(uint))
                return PiffDataFormats.UInt32;

            else if (valueType == typeof(long))
                return PiffDataFormats.Int64;

            else if (valueType == typeof(ulong))
                return PiffDataFormats.UInt64;

            else if (valueType == typeof(string))
                return PiffDataFormats.Ascii;

            else if (valueType == typeof(Guid))
                return PiffDataFormats.GuidBytes;

            else if (valueType.IsClass && typeof(PiffBoxBase).IsAssignableFrom(valueType))
                return PiffDataFormats.Box;

            else if (valueType.IsClass)
                return PiffDataFormats.InlineObject;

            else
                throw new ArgumentException($"Unsupported data type '{valueType.Name}'.");
        }


        /// <summary>
        /// Retrieve array size information from <see cref="PiffArraySizeAttribute"/>.
        /// </summary>
        private static int? GetArraySize(PropertyInfo prop, bool isArray, object target)
        {
            var sizeAttr = prop.GetCustomAttribute<PiffArraySizeAttribute>();

            if (!isArray)
            {
                if (sizeAttr != null)
                    throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' is not an array and thus cannot have {nameof(PiffArraySizeAttribute)}.");

                return null;
            }

            if (sizeAttr is null)
                return null;

            if (sizeAttr.SizeProp == null)
                return sizeAttr.Size;

            const BindingFlags sizePropFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var sizeProp = target.GetType().GetProperty(sizeAttr.SizeProp, sizePropFlags);

            if (sizeProp is null)
                throw new ArgumentException($"Property {sizeAttr.SizeProp} not found on object {prop.DeclaringType.Name}.");

            object size = sizeProp.GetValue(target, null);

            try
            {
                return (int) Convert.ChangeType(size, typeof(int));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Cannot convert property {sizeProp.DeclaringType.Name}.{sizeProp.Name} value {size} to an integer array size.", ex);
            }
        }

        #endregion


        #region Reading utility

        /// <summary>
        /// Read this value into the given <paramref name="targetObject"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private long ReadValue(object targetObject, BitReadStream input, long bytesLeft, PiffReadContext ctx)
        {
            var readBytes = 0L;

            if (IsArray)
            {
                var list = new List<object>();
                var count = ArraySize ?? int.MaxValue;

                while (count > 0 && bytesLeft > 0)
                {
                    var readSize = ReadSingleValue(targetObject, input, bytesLeft, ctx, out object item);
                    if (readSize < 0) return readSize;

                    if (item != null)
                        list.Add(item);
    
                    readBytes += readSize;
                    bytesLeft -= readSize;
                    count--;
                }

                if (Property.CanWrite)
                {
                    var array = Array.CreateInstance(ElementType, list.Count);
                    Property.SetValue(targetObject, array);

                    for (var i = 0; i < list.Count; i++)
                        array.SetValue(list[i], i);
                }
            }
            else
            {
                readBytes = ReadSingleValue(targetObject, input, bytesLeft, ctx, out object value);
                if (readBytes < 0) return readBytes;

                if (Property.CanWrite)
                {
                    Property.SetValue(targetObject, value);
                }
            }

            return readBytes;
        }


        /// <summary>
        /// Read a box, a POCO, a fixed-length string, or a primitive value.
        /// </summary>
        /// <returns>
        /// The number of bytes read. Can be 0 if we only read a few bits.
        /// -1 of there was an error. Skip to the end of the box.
        /// </returns>
        private long ReadSingleValue(
            object targetObject, BitReadStream input, long bytesLeft,
            PiffReadContext ctx, out object value)
        {
            long readBytes;

            switch (Format)
            {
                case PiffDataFormats.Ascii:
                    value = input.ReadAsciiString(ItemSize);
                    readBytes = ItemSize;
                    break;

                case PiffDataFormats.InlineObject:
                    readBytes = ReadPoco(targetObject, input, bytesLeft, ElementType, ctx, out value);
                    break;

                case PiffDataFormats.Box:
                    readBytes = PiffReader.ReadBox(input, ctx, out PiffBoxBase box);
                    value = box;
                    break;

                default:
                    readBytes = ReadPrimitiveValue(input, Format, out value);
                    break;
            }

            return readBytes;
        }


        /// <summary>
        /// Read a plain object. If it has a constructor with a parent, use it.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static long ReadPoco(
            object parentObject, BitReadStream input, long bytesLeft, Type propertyType,
            PiffReadContext ctx, out object obj)
        {
            if (propertyType.GetConstructor(Type.EmptyTypes) != null)
                obj = Activator.CreateInstance(propertyType);
            else
                obj = Activator.CreateInstance(propertyType, parentObject);
        
            return ReadObject(obj, input, bytesLeft, ctx);
        }


        /// <summary>
        /// Read an integer (of many formats), a GUID or a zero-terminated string.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static long ReadPrimitiveValue(BitReadStream bytes, PiffDataFormats format, out object value)
        {
            var pos = bytes.Position;

            switch (format)
            {
                case PiffDataFormats.UInt2Minus1:
                    value = (byte)(bytes.ReadBits(2) + 1);
                    break;

                case PiffDataFormats.UInt3:
                    value = (byte)bytes.ReadBits(3);
                    break;

                case PiffDataFormats.UInt4:
                    value = (byte)bytes.ReadBits(4);
                    break;

                case PiffDataFormats.UInt5:
                    value = (byte)bytes.ReadBits(5);
                    break;

                case PiffDataFormats.UInt6:
                    value = (byte)bytes.ReadBits(6);
                    break;

                case PiffDataFormats.UInt8:
                    value = (byte)bytes.ReadByte();
                    break;

                case PiffDataFormats.Int16:
                    value = bytes.ReadInt16();
                    break;

                case PiffDataFormats.UInt16:
                    value = bytes.ReadUInt16();
                    break;

                case PiffDataFormats.Int24:
                    value = bytes.ReadInt24();
                    break;

                case PiffDataFormats.Int32:
                    value = bytes.ReadInt32();
                    break;

                case PiffDataFormats.UInt32:
                    value = bytes.ReadUInt32();
                    break;

                case PiffDataFormats.Int64:
                    value = bytes.ReadInt64();
                    break;

                case PiffDataFormats.UInt64:
                    value = bytes.ReadUInt64();
                    break;

                case PiffDataFormats.DynamicInt:
                    value = bytes.ReadDynamicInt();
                    break;

                case PiffDataFormats.GuidBytes:
                    value = bytes.ReadGuid();
                    break;

                case PiffDataFormats.AsciiZero:
                    value = bytes.ReadAsciiString();
                    break;

                case PiffDataFormats.Utf8Zero:
                    value = bytes.ReadUtf8String();
                    break;

                case PiffDataFormats.Utf8Or16Zero:
                    value = bytes.ReadUtf8Or16String();
                    break;

                default:
                    throw new ArgumentException($"Format '{format}' is not yet supported for reading.");
            }

            return bytes.Position - pos;
        }

        #endregion


        #region Length utility

        /// <summary>
        /// Return the length (in bits) of a single value or array with the given format.
        /// </summary>
        private ulong GetValueLength(object source)
        {
            var value = Property.GetValue(source);

            if (value is null || Format == PiffDataFormats.Skip)
                return 0;

            var sz = 0uL;

            if (IsArray)
            {
                foreach (var item in value as Array)
                {
                   sz += GetSingleValueLength(item, Format);
                }
            }
            else
            {
                sz += GetSingleValueLength(value, Format);
            }

            return sz;
        }


        /// <summary>
        /// Get the length (in bits) of a single value with the given format.
        /// </summary>
        private static ulong GetSingleValueLength(
            object value, PiffDataFormats format)
        {
            switch (format)
            {
                case PiffDataFormats.UInt2Minus1:
                    return 2;

                case PiffDataFormats.UInt3:
                    return 3;

                case PiffDataFormats.UInt4:
                    return 4;

                case PiffDataFormats.UInt5:
                    return 5;

                case PiffDataFormats.UInt6:
                    return 6;

                case PiffDataFormats.UInt8:
                    return 8;

                case PiffDataFormats.Int16:
                case PiffDataFormats.UInt16:
                    return 16;

                case PiffDataFormats.Int24:
                    return 24;

                case PiffDataFormats.Int32:
                case PiffDataFormats.UInt32:
                    return 32;

                case PiffDataFormats.Int64:
                case PiffDataFormats.UInt64:
                    return 64;

                case PiffDataFormats.DynamicInt:
                    return (ulong)((int)value).ToDynamic().Count() * 8;

                case PiffDataFormats.Ascii:
                    return (ulong)((string)value).Length * 8;

                case PiffDataFormats.AsciiZero:
                    return (ulong)((string)value).Length * 8 + 8;

                case PiffDataFormats.Utf8Zero:
                    return (ulong)Encoding.UTF8.GetBytes((string)value).Count() * 8 + 8;

                case PiffDataFormats.Utf8Or16Zero:
                    return (ulong) new UnicodeEncoding(true, true).GetByteCount((string) value) * 8 + 8;

                case PiffDataFormats.Ucs2:
                    return (ulong)Encoding.Unicode.GetBytes((string)value).Count() * 8;

                case PiffDataFormats.GuidBytes:
                    return 16 * 8;

                case PiffDataFormats.InlineObject:
                    return GetObjectLength(value);

                case PiffDataFormats.Box:
                    return PiffWriter.GetBoxLength((PiffBoxBase)value) * 8;

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }
        }

        #endregion


        #region Writing utility

        /// <summary>
        /// Write a single value with the given format.
        /// </summary>
        private static void WriteSingleValue(
            BitWriteStream output, object value, PiffDataFormats format, PiffWriteContext ctx)
        {
            switch (format)
            {
                case PiffDataFormats.UInt2Minus1:
                    output.WriteBits((byte) ((byte) value - 1), 2);
                    break;

                case PiffDataFormats.UInt3:
                    output.WriteBits((byte) value, 3);
                    break;

                case PiffDataFormats.UInt4:
                    output.WriteBits((byte) value, 4);
                    break;

                case PiffDataFormats.UInt5:
                    output.WriteBits((byte) value, 5);
                    break;

                case PiffDataFormats.UInt6:
                    output.WriteBits((byte) value, 6);
                    break;

                case PiffDataFormats.UInt8:
                    output.WriteByte((byte)value);
                    break;

                case PiffDataFormats.Int16:
                    output.WriteBytes(((short)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt16:
                    output.WriteBytes(((ushort)value).ToBigEndian());
                    break;

                case PiffDataFormats.Int24:
                    output.WriteBytes(((int)value).ToBigEndian().Skip(1));
                    break;

                case PiffDataFormats.Int32:
                    output.WriteBytes(((int)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt32:
                    output.WriteBytes(((uint)value).ToBigEndian());
                    break;

                case PiffDataFormats.Int64:
                    output.WriteBytes(((long)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt64:
                    output.WriteBytes(((ulong)value).ToBigEndian());
                    break;

                case PiffDataFormats.DynamicInt:
                    output.WriteBytes(((int)value).ToDynamic());
                    break;

                case PiffDataFormats.Ascii:
                    output.WriteBytes(Encoding.ASCII.GetBytes((string)value));
                    break;

                case PiffDataFormats.AsciiZero:
                    output.WriteBytes(Encoding.ASCII.GetBytes((string)value).Append((byte)0));
                    break;

                case PiffDataFormats.Utf8Zero:
                    output.WriteBytes(Encoding.UTF8.GetBytes((string)value).Append((byte)0));
                    break;

                case PiffDataFormats.Utf8Or16Zero:
                    output.WriteBytes(new UnicodeEncoding(true, true).GetBytes((string)value).Append((byte)0));
                    break;

                case PiffDataFormats.Ucs2:
                    output.WriteBytes(Encoding.Unicode.GetBytes((string)value));
                    break;

                case PiffDataFormats.GuidBytes:
                    // Need reformatting
                    output.WriteBytes(((Guid)value).ToBigEndianArray());
                    break;

                case PiffDataFormats.InlineObject:
                    // Write the object
                    WriteObject(output, value, ctx);
                    break;

                case PiffDataFormats.Box:
                    PiffWriter.WriteBox(output, (PiffBoxBase)value, ctx);
                    break;

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }
        }

        #endregion


        #region ToString

        public override string ToString()
        {
            string fmt;
            switch (Format)
            {
                case PiffDataFormats.InlineObject:
                case PiffDataFormats.Box:
                    fmt = IsArray ? Property.PropertyType.GetElementType().Name : Property.PropertyType.Name;
                    break;

                default:
                    fmt = Format.ToString();
                    break;
            }

            var arr = IsArray ? $"[{(ArraySize.HasValue ? ArraySize.ToString() : "")}]" : "";
            return $"{fmt}{arr} {Property.Name}";
        }

        #endregion
    }
}
