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

                if (lengthAttr.LengthProperty is null)
                    ItemSize = lengthAttr.Length;
                else
                    ItemSize = GetIntFromProperty(target, lengthAttr.LengthProperty);
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
        /// <returns>Read status from <see cref="PiffReader"/>.</returns>
        public static PiffReadStatuses ReadObject(object target, BitReadStream input, PiffReadContext ctx)
        {
            foreach (var prop in GetProperties(target))
            {
                if (prop.Format == PiffDataFormats.Skip) continue;

                var status = prop.ReadValue(target, input, ctx);
                if (status != PiffReadStatuses.Continue)
                {
                    // EOF at the expected place means we're finished reading this object
                    return status == PiffReadStatuses.Eof ? PiffReadStatuses.Continue : status;
                }
            }

            return PiffReadStatuses.Continue;
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

            else if (valueType == typeof(sbyte))
                return PiffDataFormats.Int8;

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
        private static int? GetArraySize(PropertyInfo targetProperty, bool isArray, object target)
        {
            var sizeAttr = targetProperty.GetCustomAttribute<PiffArraySizeAttribute>();

            if (!isArray)
            {
                if (sizeAttr != null)
                    throw new ArgumentException($"Property '{targetProperty.DeclaringType.Name}.{targetProperty.Name}' is not an array and thus cannot have {nameof(PiffArraySizeAttribute)}.");

                return null;
            }

            if (sizeAttr is null)
                return null;

            if (sizeAttr.SizeProp == null)
                return sizeAttr.Size;

            return GetIntFromProperty(target, sizeAttr.SizeProp);
        }


        private static int GetIntFromProperty(object target, string propName)
        {
            const BindingFlags propFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var sizeProp = target.GetType().GetProperty(propName, propFlags);

            if (sizeProp is null)
                throw new ArgumentException($"Property {propName} not found on object {target.GetType().Name}.");

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
        /// Read <see langword="this"/> value into the given <paramref name="targetObject"/>.
        /// </summary>
        private PiffReadStatuses ReadValue(object targetObject, BitReadStream input, PiffReadContext ctx)
        {
            if (IsArray)
            {
                return ReadArray(targetObject, input, ctx);
            }
            else
            {
                var status = ReadSingleValue(targetObject, input, ctx, out var value);
                if (status != PiffReadStatuses.Continue) return status;

                if (Property.CanWrite)
                {
                    Property.SetValue(targetObject, value);
                }

                return PiffReadStatuses.Continue;
            }
        }


        private PiffReadStatuses ReadArray(object targetObject, BitReadStream input, PiffReadContext ctx)
        {
            PiffReadStatuses status;
            Array array;

            // Can add short, ushort, int, uint, ulong, string, Guid
            switch (Format)
            {
                case PiffDataFormats.UInt8:
                    status = ReadByteArray(input, ctx, out array);
                    break;
                default:
                    status = ReadVariableSizeArray(targetObject, input, ctx, out array);
                    break;
            }

            if (Property.CanWrite)
            {
                Property.SetValue(targetObject, array);
            }

            if (ArraySize.HasValue && ArraySize.Value < array.Length)
            {
                ctx.AddWarning($"Expected {ArraySize.Value} elements, got only {array.Length} for {targetObject.GetType().Name}.{Property.Name}.");
            }

            return status;
        }


        private PiffReadStatuses ReadByteArray(BitReadStream input, PiffReadContext ctx, out Array result)
        {
            var count = ArraySize ?? (int) input.BytesLeft;
            var buffer = new byte[count];
            var read = input.Read(buffer, 0, count);
            result = buffer;

            return read == count ? PiffReadStatuses.Continue : PiffReadStatuses.EofPremature;
        }


        /// <summary>
        /// Read array element by element, assuming their sizes can differ.
        /// </summary>
        private PiffReadStatuses ReadVariableSizeArray(object targetObject, BitReadStream input, PiffReadContext ctx, out Array result)
        {
            var list = new List<object>();
            var status = PiffReadStatuses.Continue;

            var count = ArraySize ?? int.MaxValue;
            while (!input.IsEmpty && count > 0)
            {
                status = ReadSingleValue(targetObject, input, ctx, out var item);
                if (status != PiffReadStatuses.Continue) break;

                list.Add(item);
                count--;
            }

            if (ArraySize.HasValue && count > 0)
                ctx.AddError($"Not enough items read in {targetObject.GetType().Name}: expected {ArraySize}, got {ArraySize - count}.");

            var array = Array.CreateInstance(ElementType, list.Count);

            for (var i = 0; i < list.Count; i++)
                array.SetValue(list[i], i);

            result = array;

            return status;
        }


        /// <summary>
        /// Read a plain object. If it has a constructor with a parent, use it.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static PiffReadStatuses ReadPoco(
            object parentObject, BitReadStream input, Type propertyType, PiffReadContext ctx, out object obj)
        {
            if (propertyType.GetConstructor(Type.EmptyTypes) != null)
                obj = Activator.CreateInstance(propertyType);
            else
                obj = Activator.CreateInstance(propertyType, parentObject);
        
            return ReadObject(obj, input, ctx);
        }


        /// <summary>
        /// Read an integer (of many formats), a GUID, a string (of many formats),
        /// a box, or a POCO.
        /// </summary>
        private PiffReadStatuses ReadSingleValue(object targetObject, BitReadStream input, PiffReadContext ctx, out object result)
        {
            PiffReadStatuses status;

            switch (Format)
            {
                case PiffDataFormats.InlineObject:
                    status = ReadPoco(targetObject, input, ElementType, ctx, out result);
                    return status;

                case PiffDataFormats.Box:
                    status = PiffReader.ReadBox(input, ctx, out var box);
                    result = box;
                    return status;

                case PiffDataFormats.UInt1:
                    status = input.ReadBits(1, false, out var u1);
                    result = (byte) u1;
                    return status;

                case PiffDataFormats.UInt2Minus1:
                    status = input.ReadBits(2, false, out var u2m);
                    result = (byte) (u2m + 1);
                    return status;

                case PiffDataFormats.UInt3:
                    status = input.ReadBits(3, false, out var u3);
                    result = (byte) u3;
                    return status;

                case PiffDataFormats.UInt4:
                    status = input.ReadBits(4, false, out var u4);
                    result = (byte) u4;
                    return status;

                case PiffDataFormats.UInt5:
                    status = input.ReadBits(5, false, out var u5);
                    result = (byte) u5;
                    return status;

                case PiffDataFormats.UInt6:
                    status = input.ReadBits(6, false, out var u6);
                    result = (byte) u6;
                    return status;

                case PiffDataFormats.UInt7:
                    status = input.ReadBits(7, false, out var u7);
                    result = (byte) u7;
                    return status;

                case PiffDataFormats.Int8:
                    status = input.ReadByte(out var s8);
                    result = (sbyte) s8;
                    return status;

                case PiffDataFormats.UInt8:
                    status = input.ReadByte(out var u8);
                    result = (byte) u8;
                    return status;

                case PiffDataFormats.Int12:
                    status = input.ReadBits(12, true, out var s12);
                    result = (short) s12;
                    return status;

                case PiffDataFormats.Int16:
                    status = input.ReadInt16(out var s16);
                    result = s16;
                    return status;

                case PiffDataFormats.UInt16:
                    status = input.ReadUInt16(out var u16);
                    result = u16;
                    return status;

                case PiffDataFormats.Int24:
                    status = input.ReadInt24(out var s24);
                    result = s24;
                    return status;

                case PiffDataFormats.Int32:
                    status = input.ReadInt32(out var s32);
                    result = s32;
                    return status;

                case PiffDataFormats.UInt32:
                    status = input.ReadUInt32(out var u32);
                    result = u32;
                    return status;

                case PiffDataFormats.Int64:
                    status = input.ReadInt64(out var s64);
                    result = s64;
                    return status;

                case PiffDataFormats.UInt64:
                    status = input.ReadUInt64(out var u64);
                    result = u64;
                    return status;

                case PiffDataFormats.DynamicInt:
                    status = input.ReadDynamicInt(out var sdyn);
                    result = sdyn;
                    return status;

                case PiffDataFormats.GuidBytes:
                    status = input.ReadGuid(out var guid);
                    result = guid;
                    return status;

                case PiffDataFormats.Ascii:
                    status = input.ReadAsciiString(ItemSize, out var ascii);
                    result = ascii;
                    return status;

                case PiffDataFormats.AsciiZero:
                    status = input.ReadAsciiZeroString(out var strz);
                    result = strz;
                    return status;

                case PiffDataFormats.AsciiPascal:
                    status = input.ReadAsciiPascalString(out var strp);
                    result = strp;
                    return status;

                case PiffDataFormats.Utf8Zero:
                    status = input.ReadUtf8ZeroString(out var utfz);
                    result = utfz;
                    return status;

                case PiffDataFormats.Utf8Or16Zero:
                    status = input.ReadUtf8Or16ZeroString(out var utfz8or16);
                    result = utfz8or16;
                    return status;

                default:
                    throw new ArgumentException($"Format '{Format}' is not yet supported for reading.");
            }
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
                case PiffDataFormats.UInt1:
                    return 1;

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

                case PiffDataFormats.UInt7:
                    return 7;

                case PiffDataFormats.Int8:
                case PiffDataFormats.UInt8:
                    return 8;

                case PiffDataFormats.Int12:
                    return 12;

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
                case PiffDataFormats.AsciiPascal:
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
                case PiffDataFormats.UInt1:
                    output.WriteBits((byte) value, 1);
                    break;

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

                case PiffDataFormats.UInt7:
                    output.WriteBits((byte) value, 7);
                    break;

                case PiffDataFormats.Int8:
                    output.WriteByte((byte)(sbyte)value);
                    break;

                case PiffDataFormats.UInt8:
                    output.WriteByte((byte)value);
                    break;

                case PiffDataFormats.Int12:
                    output.WriteBits((short)value, 12);
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

                case PiffDataFormats.AsciiPascal:
                    output.WriteByte((byte) ((string)value).Length);
                    output.WriteBytes(Encoding.ASCII.GetBytes((string)value));
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
