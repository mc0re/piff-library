using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PiffLibrary
{
    /// <summary>
    /// Reading data from the binary stream.
    /// </summary>
    public class PiffReader
    {
        #region Helper class

        private class ValueToRead
        {
            public PropertyInfo Property { get; set; }

            public bool IsArray { get; set; }

            public PiffDataFormats Format { get; set; }
        }

        #endregion


        #region Fields

        private static readonly Dictionary<string, Type> sBoxes;

        #endregion


        #region Init and clean-up

        static PiffReader()
        {
            var boxTypes =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                let boxNameAttr = t.GetCustomAttribute<BoxNameAttribute>()
                where boxNameAttr != null
                group t by boxNameAttr.Name into g
                select new { Id = g.Key, Type = g.First() };

            sBoxes = boxTypes.ToDictionary(t => t.Id, t => t.Type);
        }

        #endregion


        #region API

        public static int GetFragmentSequenceNumber(byte[] data)
        {
            var ms = new MemoryStream(data);
            var moof = ReadBox<PiffMovieFragment>(ms);

            return moof.Header.Sequence;
        }


        public static int GetInt32(byte[] bytes, int offset)
        {
            var res = (bytes[offset] << 24) +
                      (bytes[offset + 1] << 16) +
                      (bytes[offset + 2] << 8) +
                       bytes[offset + 3];

            return res;
        }

        #endregion


        #region Box reading utility

        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        internal static TBox ReadBox<TBox>(Stream bytes) where TBox : class
        {
            return ReadBox(bytes, typeof(TBox)) as TBox;
        }


        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="expectedType">Expected box type</typeparam>
        /// <param name="bytes"></param>
        /// <returns>Box object or null</returns>
        internal static object ReadBox(Stream bytes, Type expectedType)
        {
            var length = GetInt32(bytes);
            if (length < 8)
                throw new ArgumentException($"Improper box length {length}.");

            var id = GetFixedString(bytes, 4);

            if (!sBoxes.TryGetValue(id, out var type))
            {
                throw new ArgumentException($"Inrecognized box type '{id}'.");
            }

            if (type != expectedType)
            {
                // Not expected type, back off
                bytes.Seek(-8, SeekOrigin.Current);
                return null;
            }

            var obj = Activator.CreateInstance(type);

            var props =
                from prop in type.GetProperties()
                let isArray = prop.PropertyType.IsArray
                select new ValueToRead
                {
                    Property = prop,
                    IsArray = isArray,
                    Format = prop.GetCustomAttribute<PiffDataFormatAttribute>()?.Format
                            ?? (isArray
                                ? PiffDataUtility.GetDefaultFormat(prop.PropertyType.GetElementType())
                                : PiffDataUtility.GetDefaultFormat(prop.PropertyType))
                };

            ReadBoxValues(bytes, obj, props.ToArray());

            return obj;
        }


        private static void ReadBoxValues(Stream bytes, object obj, ValueToRead[] valuesToRead)
        {
            foreach (var value in valuesToRead)
            {
                ReadValue(bytes, obj, value);
            }
        }


        private static void ReadValue(Stream bytes, object obj, ValueToRead value)
        {
            if (value.IsArray)
            {
                throw new ArgumentException("Arrays are not yet supported.");
            }
            else
            {
                var val = ReadPrimitiveValue(bytes, value.Format, value.Property.PropertyType);
                value.Property.SetValue(obj, val);
            }
        }

        #endregion


        #region Value reading utility

        private static object ReadPrimitiveValue(Stream bytes, PiffDataFormats format, Type targetType)
        {
            switch (format)
            {
                case PiffDataFormats.Int8:
                    return (byte)bytes.ReadByte();

                case PiffDataFormats.Int24:
                    return GetInt24(bytes);

                case PiffDataFormats.Int32:
                    return GetInt32(bytes);

                case PiffDataFormats.Box:
                    return ReadBox(bytes, targetType);

                default:
                    throw new ArgumentException($"Format '{format}' is not yet supported for reading.");
            }
        }


        internal static int GetInt24(Stream bytes)
        {
            var res = (bytes.ReadByte() << 16) +
                      (bytes.ReadByte() << 8) +
                       bytes.ReadByte();

            return res;
        }


        internal static int GetInt32(Stream bytes)
        {
            var res = (bytes.ReadByte() << 24) +
                      (bytes.ReadByte() << 16) +
                      (bytes.ReadByte() << 8) +
                       bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read the given number of ASCII characters.
        /// </summary>
        private static string GetFixedString(Stream bytes, int length)
        {
            var chars = Enumerable.Range(0, length).Select(_ => (byte)bytes.ReadByte()).ToArray();

            return Encoding.ASCII.GetString(chars);
        }

        #endregion
    }
}