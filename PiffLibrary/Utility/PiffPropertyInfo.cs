﻿using System.IO;
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
        /// Process property information, extract the needed bits for reading / writing.
        /// </summary>
        private PiffPropertyInfo(PropertyInfo prop, object target)
        {
            Property = prop;
            IsArray = prop.PropertyType.IsArray;
            ElementType = IsArray ? prop.PropertyType.GetElementType() : prop.PropertyType;

            var formatAttr = prop.GetCustomAttribute<PiffDataFormatAttribute>();

            if (formatAttr is null)
            {
                Format = GetDefaultFormat(ElementType);
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


        /// <summary>
        /// Process all properties of the given object.
        /// </summary>
        public static IEnumerable<PiffPropertyInfo> GetProperties(object target)
        {
            var targetType = target.GetType();
            var baseProps = targetType.BaseType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            var ownProps = targetType.GetProperties().Where(p => !baseProps.Any(bp => bp.Name == p.Name));
            var before = baseProps.Where(p => p.GetCustomAttribute<BeforeDescendantsAttribute>() != null);
            var after = baseProps.Where(p => p.GetCustomAttribute<AfterDescendantsAttribute>() != null);

            var props = before.Concat(ownProps).Concat(after);

            return props.Select(prop => new PiffPropertyInfo(prop, target));
        }

        #endregion


        #region API

        /// <summary>
        /// Read out properties of <paramref name="target"/> object from <paramref name="input"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        public static ulong ReadObject(object target, Stream input, ulong bytesLeft, PiffReadContext ctx)
        {
            var readBytes = 0uL;

            foreach (var prop in GetProperties(target))
            {
                var readSize = prop.ReadValue(target, input, bytesLeft, ctx);
                readBytes += readSize;
                bytesLeft -= readSize;
            }

            return readBytes;
        }


        /// <summary>
        /// Write all properties of the given object.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<byte> WriteObject(object source, PiffWriteContext ctx)
        {
            return GetProperties(source).SelectMany(p => p.WriteValue(source, ctx));
        }


        /// <summary>
        /// Get data length in bytes, if the box was written to a stream.
        /// </summary>
        public static ulong GetObjectLength(object source)
        {
            // There is no Sum() for ulong, therefore using Aggregate
            return GetProperties(source).Aggregate(0uL, (sz, p) => sz + p.GetValueLength(source));
        }


        /// <summary>
        /// Write a single value or array with the given format.
        /// Skip the writing alltogether if the valus is <see langword="null"/>.
        /// </summary>
        public IEnumerable<byte> WriteValue(object target, PiffWriteContext ctx)
        {
            var value = Property.GetValue(target);

            if (value is null || Format == PiffDataFormats.Skip)
                return Enumerable.Empty<byte>();

            var dataBytes = new List<byte>();

            if (IsArray)
            {
                foreach (var item in value as Array)
                {
                    dataBytes.AddRange(WriteSingleValue(item, Format, ctx));
                }
            }
            else
            {
                dataBytes.AddRange(WriteSingleValue(value, Format, ctx));
            }

            return dataBytes;
        }

        #endregion


        #region Format utility

        /// <summary>
        /// Unless the format is explicitly specified, use the default one.
        /// </summary>
        private static PiffDataFormats GetDefaultFormat(Type valueType)
        {
            if (valueType == typeof(byte) || valueType == typeof(char))
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

        #endregion


        #region Reading utility

        /// <summary>
        /// Read this value into the given <paramref name="targetObject"/>.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private ulong ReadValue(object targetObject, Stream input, ulong bytesLeft, PiffReadContext ctx)
        {
            var readBytes = 0uL;

            if (IsArray)
            {
                var list = new List<object>();
                var count = ArraySize ?? int.MaxValue;

                while (count > 0 && bytesLeft > 0)
                {
                    var readSize = ReadSingleValue(targetObject, input, bytesLeft, ctx, out object item);
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
            else if (Format != PiffDataFormats.Skip)
            {
                readBytes = ReadSingleValue(targetObject, input, bytesLeft, ctx, out object value);

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
        /// <returns>The number of bytes read</returns>
        private ulong ReadSingleValue(
            object targetObject, Stream input, ulong bytesLeft,
            PiffReadContext ctx, out object value)
        {
            ulong readBytes;

            switch (Format)
            {
                case PiffDataFormats.Ascii:
                    value = input.ReadAsciiString(ItemSize);
                    readBytes = (ulong)ItemSize;
                    break;

                case PiffDataFormats.InlineObject:
                    readBytes = ReadPoco(targetObject, input, bytesLeft, ElementType, ctx, out value);
                    break;

                case PiffDataFormats.Box:
                    ctx.Push((PiffBoxBase)targetObject);
                    readBytes = PiffReader.ReadBox(input, ctx, out PiffBoxBase box);
                    ctx.Pop();
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
        private static ulong ReadPoco(
            object targetObject, Stream input, ulong bytesLeft, Type propertyType,
            PiffReadContext ctx, out object obj)
        {
            if (propertyType.GetConstructor(Type.EmptyTypes) != null)
                obj = Activator.CreateInstance(propertyType);
            else
                obj = Activator.CreateInstance(propertyType, targetObject);
        
            return ReadObject(obj, input, bytesLeft, ctx);
        }


        /// <summary>
        /// Read an integer (of many formats), a GUID or a zero-terminated string.
        /// </summary>
        /// <returns>The number of bytes read</returns>
        private static ulong ReadPrimitiveValue(Stream bytes, PiffDataFormats format, out object value)
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

                default:
                    throw new ArgumentException($"Format '{format}' is not yet supported for reading.");
            }

            return (ulong)(bytes.Position - pos);
        }

        #endregion


        #region Writing utility

        /// <summary>
        /// Return the length (in bytes) of a single value or array with the given format.
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
        /// Get the length (in bytes) of a single value with the given format.
        /// </summary>
        private static ulong GetSingleValueLength(
            object value, PiffDataFormats format)
        {
            switch (format)
            {
                case PiffDataFormats.Int2Minus1:
                case PiffDataFormats.Int5:
                case PiffDataFormats.Int8:
                    return 1;

                case PiffDataFormats.Int16:
                case PiffDataFormats.UInt16:
                    return 2;

                case PiffDataFormats.Int24:
                    return 3;

                case PiffDataFormats.Int32:
                case PiffDataFormats.UInt32:
                    return 4;

                case PiffDataFormats.Int64:
                case PiffDataFormats.UInt64:
                    return 8;

                case PiffDataFormats.DynamicInt:
                    return (ulong)((int)value).ToDynamic().Count();

                case PiffDataFormats.Ascii:
                    return (ulong)((string)value).Length;

                case PiffDataFormats.AsciiZero:
                    return (ulong)((string)value).Length + 1;

                case PiffDataFormats.Utf8Zero:
                    return (ulong)Encoding.UTF8.GetBytes((string)value).Count() + 1;

                case PiffDataFormats.Ucs2:
                    return (ulong)Encoding.Unicode.GetBytes((string)value).Count();

                case PiffDataFormats.GuidBytes:
                    return 16;

                case PiffDataFormats.InlineObject:
                    return GetObjectLength(value);

                case PiffDataFormats.Box:
                    return PiffWriter.GetBoxLength((PiffBoxBase)value);

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }
        }


        /// <summary>
        /// Write a single value with the given format.
        /// </summary>
        private static IEnumerable<byte> WriteSingleValue(
            object value, PiffDataFormats format, PiffWriteContext ctx)
        {
            var dataBytes = new List<byte>();

            switch (format)
            {
                case PiffDataFormats.Int8:
                    dataBytes.Add((byte)value);
                    break;

                case PiffDataFormats.Int16:
                    dataBytes.AddRange(((short)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt16:
                    dataBytes.AddRange(((ushort)value).ToBigEndian());
                    break;

                case PiffDataFormats.Int24:
                    dataBytes.AddRange(((int)value).ToBigEndian().Skip(1));
                    break;

                case PiffDataFormats.Int32:
                    dataBytes.AddRange(((int)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt32:
                    dataBytes.AddRange(((uint)value).ToBigEndian());
                    break;

                case PiffDataFormats.Int64:
                    dataBytes.AddRange(((long)value).ToBigEndian());
                    break;

                case PiffDataFormats.UInt64:
                    dataBytes.AddRange(((ulong)value).ToBigEndian());
                    break;

                case PiffDataFormats.DynamicInt:
                    dataBytes.AddRange(((int)value).ToDynamic());
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

                case PiffDataFormats.AsciiZero:
                    dataBytes.AddRange(Encoding.ASCII.GetBytes((string)value));
                    dataBytes.Add(0);
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
                    dataBytes.AddRange(WriteObject(value, ctx));
                    break;

                case PiffDataFormats.Box:
                    dataBytes.AddRange(PiffWriter.WriteBox((PiffBoxBase)value, ctx));
                    break;

                default:
                    throw new ArgumentException($"Unsupported format '{format}'.");
            }

            return dataBytes;
        }

        #endregion
    }
}
