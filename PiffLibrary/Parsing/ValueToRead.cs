using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace PiffLibrary
{
    internal class ValueToRead
    {
        #region Properties

        public PropertyInfo Property { get; }

        public bool IsArray { get; }

        public PiffDataFormats Format { get; }

        public int ItemSize { get; }
        
        /// <summary>
        /// If not specified, read until the end of box.
        /// </summary>
        public int? ArraySize { get; }

        #endregion


        #region Init and clean-up

        public ValueToRead(PropertyInfo prop, PiffBoxBase target)
        {
            Property = prop;
            IsArray = prop.PropertyType.IsArray;

            var formatAttr = prop.GetCustomAttribute<PiffDataFormatAttribute>();

            if (formatAttr is null)
            {
                Format = IsArray
                         ? PiffDataUtility.GetDefaultFormat(prop.PropertyType.GetElementType())
                         : PiffDataUtility.GetDefaultFormat(prop.PropertyType);
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
            if (IsArray)
            {
                ArraySize = sizeAttr?.Size;
            }
            else if (sizeAttr != null)
            {
                throw new ArgumentException($"Property '{prop.DeclaringType.Name}.{prop.Name}' is not an array and thus cannot have {nameof(PiffArraySizeAttribute)}.");
            }
        }

        #endregion


        #region API

        /// <summary>
        /// Read this value into the given <paramref name="target"/>.
        /// </summary>
        /// <returns>THe number of bytes read</returns>
        public long ReadValue(Stream input, long maxBytesLeft, PiffBoxBase target)
        {
            var readBytes = 0L;

            if (IsArray)
            {
                var list = new List<object>();
                var count = ArraySize ?? int.MaxValue;

                while (count > 0 && readBytes < maxBytesLeft)
                {
                    readBytes += ReadSingleValue(input, out object item);
                    list.Add(item);
                    count--;
                }

                if (Property.CanWrite)
                {
                    var origArray = Property.GetValue(target);
                    var array = Activator.CreateInstance(origArray.GetType(), list.Count) as Array;
                    Property.SetValue(target, array);

                    for (var i = 0; i < list.Count; i++)
                        array.SetValue(list[i], i);
                }
            }
            else
            {
                readBytes = ReadSingleValue(input, out object val);

                if (Property.CanWrite)
                {
                    Property.SetValue(target, val);
                }
            }

            return readBytes;
        }

        #endregion


        #region Utility

        private long ReadSingleValue(Stream input, out object val)
        {
            long readBytes;
            if (Format == PiffDataFormats.Ascii)
            {
                val = input.GetFixedString(ItemSize);
                readBytes = ItemSize;
            }
            else if (Format == PiffDataFormats.Box)
            {
                readBytes = PiffReader.ReadBox(input, Property.PropertyType, out var box);
                val = box;
            }
            else
            {
                readBytes = ReadPrimitiveValue(input, Format, out val);
            }

            return readBytes;
        }


        private static int ReadPrimitiveValue(Stream bytes, PiffDataFormats format, out object value)
        {
            switch (format)
            {
                case PiffDataFormats.Int8:
                    value = (byte)bytes.ReadByte();
                    return 1;

                case PiffDataFormats.Int16:
                    value = (short)bytes.ReadInt16();
                    return 2;

                case PiffDataFormats.Int24:
                    value = bytes.ReadInt24();
                    return 3;

                case PiffDataFormats.Int32:
                    value = (int)bytes.ReadUInt32();
                    return 4;

                case PiffDataFormats.UInt32:
                    value = bytes.ReadUInt32();
                    return 4;

                case PiffDataFormats.Int64:
                    value = (long)bytes.ReadUInt64();
                    return 8;

                default:
                    throw new ArgumentException($"Format '{format}' is not yet supported for reading.");
            }
        }

        #endregion
    }
}