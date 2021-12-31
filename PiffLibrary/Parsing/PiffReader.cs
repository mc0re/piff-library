using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace PiffLibrary
{
    /// <summary>
    /// Reading data from the binary stream.
    /// </summary>
    public class PiffReader
    {
        #region Fields

        private const int HeaderLength = 8;

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


        #region Fragment API

        public static int GetFragmentSequenceNumber(byte[] data)
        {
            var ms = new MemoryStream(data);
            var moof = ReadBox<PiffMovieFragment>(ms);

            return moof.Header.Sequence;
        }


        public static int GetTrackId(byte[] data)
        {
            var ms = new MemoryStream(data);
            var moof = ReadBox<PiffMovieFragment>(ms);

            return moof.Track.Header.TrackId;
        }

        #endregion


        #region Box reading API

        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        internal static TBox ReadBox<TBox>(Stream input) where TBox : PiffBoxBase
        {
            ReadBox(input, typeof(TBox), out var box);
            return box as TBox;
        }


        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <param name="expectedType">Expected box type or <see langword="null"/> for any box</typeparam>
        /// <returns>Box object or <see langword="null"/> (if unexpected)</returns>
        internal static long ReadBox(Stream input, Type expectedType, out PiffBoxBase box)
        {
            long length = input.ReadUInt32();
            
            if (length == 0)
            {
                // Box extends to the end of the file
                throw new NotImplementedException("Auto-extended boxes not implemented");
            }
            else if (length == 1)
            {
                // 64-bit size
                throw new NotImplementedException("64-bit box length not implemented");
            }
            else if (length < HeaderLength)
            {
                throw new ArgumentException($"Improper box length {length}.");
            }

            var id = input.GetFixedString(4);

            if (!sBoxes.TryGetValue(id, out var type))
            {
                throw new ArgumentException($"Inrecognized box type '{id}'.");
            }

            if (expectedType != null && type != expectedType)
            {
                // Not expected type, back off
                input.Seek(-HeaderLength, SeekOrigin.Current);
                box = null;
                return 0;
            }

            box = (PiffBoxBase) Activator.CreateInstance(type);

            var bytesLeft = length - HeaderLength;
            foreach (var prop in type.GetProperties())
            {
                var read = new ValueToRead(prop, box);
                var readBytes = read.ReadValue(input, bytesLeft, box);
                bytesLeft -= readBytes;
            }

            return length - bytesLeft;
        }

        #endregion
    }
}