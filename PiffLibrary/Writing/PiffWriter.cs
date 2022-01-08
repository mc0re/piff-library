﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PiffLibrary
{
    public static class PiffWriter
    {
        #region API

        public static void WriteHeader(Stream strm, PiffManifest manifest, PiffWriteContext ctx)
        {
            var ftypBytes = WriteBoxObject(new PiffFileType(), ctx).ToArray();
            strm.Write(ftypBytes, 0, ftypBytes.Length);

            var movie = new PiffMovieMetadata(manifest);
            var hdrBytes = WriteBoxObject(movie, ctx).ToArray();
            strm.Write(hdrBytes, 0, hdrBytes.Length);
        }


        /// <summary>
        /// Write the finishing boxes of a file.
        /// The <paramref name="manifest"/> is used to extract track iDs.
        /// </summary>
        public static void WriteFooter(
            Stream strm, PiffManifest manifest,
            IEnumerable<PiffSampleOffset> audioOffsets,
            IEnumerable<PiffSampleOffset> videoOffsets,
            PiffWriteContext ctx)
        {
            var access = new PiffMovieFragmentRandomAccess(
                manifest.AudioTrackId, audioOffsets,
                manifest.VideoTrackId, videoOffsets);
            var mfraBytes = WriteBoxObject(access, ctx).ToArray();
            strm.Write(mfraBytes, 0, mfraBytes.Length);
        }


        public static TimeSpan GetDuration(long duration, int timeScale)
            => TimeSpan.FromSeconds(duration / (double)timeScale);

        #endregion


        #region Writing utility

        internal static ulong GetSecondsFromEpoch(DateTime time)
        {
            return (ulong)(time - new DateTime(1904, 1, 1)).TotalSeconds;
        }


        internal static long GetTicks(TimeSpan duration, int timeScale)
            => (long)(duration.TotalSeconds * timeScale);


        /// <summary>
        /// Create a byte stream representation of the given object.
        /// </summary>
        internal static IEnumerable<byte> WriteBoxObject(PiffBoxBase obj, PiffWriteContext ctx)
        {
            if (obj is null)
                return Enumerable.Empty<byte>();

            ctx.Start(obj);
            var type = obj.GetType();

            var boxNameAttr = type.GetCustomAttribute<BoxNameAttribute>();
            if (boxNameAttr is null)
                throw new ArgumentException($"Box name is not defined for type '{type.Name}'.");

            var propValues = PiffPropertyInfo.GetProperties(obj).ToArray();

            var bytes = WriteBoxValues(boxNameAttr.Name, obj, propValues, ctx);
            ctx.End(obj);
            return bytes;
        }


        /// <summary>
        /// Write a box with values.
        /// </summary>
        private static IEnumerable<byte> WriteBoxValues(
            string boxName, object obj, PiffPropertyInfo[] values, PiffWriteContext ctx)
        {
            var dataBytes = new List<byte>();

            foreach (var value in values)
            {
                dataBytes.AddRange(value.WriteValue(obj, ctx));
            }

            var boxLength = sizeof(int) + boxName.Length + dataBytes.Count;
            var hdrBytes = boxLength.ToBigEndian().Concat(
                           Encoding.ASCII.GetBytes(boxName));

            return hdrBytes.Concat(dataBytes);
        }

        #endregion
    }
}
