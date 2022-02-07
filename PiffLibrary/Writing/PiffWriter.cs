using PiffLibrary.Boxes;
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
        #region API

        public static void WriteHeader(Stream output, PiffManifest manifest, PiffWriteContext ctx)
        {
            var bits = new BitWriteStream(output, false);
            var ftyp = new PiffFileTypeBox
            {
                MajorBrand = "isml",
                MinorVersion = 1,
                CompatibleBrands = new[] { "piff", "iso2" }
            };
            WriteBox(bits, ftyp, ctx);
            var movie = new PiffMovieBox(manifest);
            WriteBox(bits, movie, ctx);
        }


        /// <summary>
        /// Write the finishing boxes of a file.
        /// The <paramref name="manifest"/> is used to extract track iDs.
        /// </summary>
        public static void WriteFooter(
            Stream output, PiffManifest manifest,
            IEnumerable<PiffSampleOffsetDto> audioOffsets,
            IEnumerable<PiffSampleOffsetDto> videoOffsets,
            PiffWriteContext ctx)
        {
            var bits = new BitWriteStream(output, false);
            var access = new PiffMovieFragmentRandomAccessBox(
                manifest.AudioTrackId, audioOffsets,
                manifest.VideoTrackId, videoOffsets);
            WriteBox(bits, access, ctx);
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
        /// Calculate the box length (in bytes), if it was written to output.
        /// </summary>
        internal static ulong GetBoxLength(PiffBoxBase box)
        {
            var body = PiffPropertyInfo.GetObjectLength(box) / 8;

            if (body + PiffBoxBase.HeaderLength <= uint.MaxValue)
            {
                return body + PiffBoxBase.HeaderLength;
            }
            else
            {
                // 64-bit length
                return body + PiffBoxBase.HeaderLength + sizeof(ulong);
            }
        }


        /// <summary>
        /// Create a byte stream representation of the given object.
        /// </summary>
        internal static void WriteBox(BitWriteStream output, PiffBoxBase box, PiffWriteContext ctx)
        {
            if (box is null)
                return;

            ctx.Start(box, output.Position);

            var type = box.GetType();
            var boxNameAttr = type.GetCustomAttribute<BoxNameAttribute>();
            if (boxNameAttr is null)
                throw new ArgumentException($"Box name is not defined for type '{type.Name}'.");

            var boxLength = GetBoxLength(box);

            if (boxLength <= uint.MaxValue)
            {
                output.WriteBytes(((uint)boxLength).ToBigEndian());
                output.WriteBytes(Encoding.ASCII.GetBytes(boxNameAttr.Name));
            }
            else
            {
                output.WriteBytes(PiffBoxBase.Length64.ToBigEndian());
                output.WriteBytes(Encoding.ASCII.GetBytes(boxNameAttr.Name));
                output.WriteBytes(boxLength.ToBigEndian());
            }

            PiffPropertyInfo.WriteObject(output, box, ctx);

            ctx.End(box);
        }

        #endregion
    }
}
