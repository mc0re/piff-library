using System;
using System.Collections.Generic;
using System.Linq;

namespace PiffLibrary
{
    /// <summary>
    /// Outcome of reading the edit lists of a file, see <see cref="PiffEditListAnalyzer"/>.
    /// </summary>
    public sealed class PiffEditListReport
    {
        #region Properties

        /// <summary>
        /// First 8 bytes of the file, non-printable characters shown as dots, e.g. "RIFF$...".
        /// </summary>
        public string Header { get; }


        /// <summary>
        /// Edit list of every track in file order; <see langword="null"/> when the file does not start like an MP4.
        /// </summary>
        public IReadOnlyList<PiffEditListInfo> Tracks { get; }


        /// <summary>
        /// Parser messages, e.g. skipped or unrecognised boxes.
        /// </summary>
        public IReadOnlyList<string> Messages { get; }


        /// <summary>
        /// Media time below which an edit list is trivial, per track type.
        /// </summary>
        public PiffEditListTolerance Tolerance { get; }


        /// <summary>
        /// Whether the file starts like an MP4 and its structure was read.
        /// </summary>
        public bool IsMp4 => Tracks != null;


        /// <summary>
        /// First track with a non-trivial edit list at its <see cref="Tolerance"/>; <see langword="null"/> if none.
        /// </summary>
        public PiffEditListInfo FirstNonTrivial => Tracks?.FirstOrDefault(t => t.IsNonTrivial(Tolerance));

        #endregion


        #region Init and clean-up

        public PiffEditListReport(
            string header, IReadOnlyList<PiffEditListInfo> tracks, IReadOnlyList<string> messages, PiffEditListTolerance tolerance)
        {
            if (tolerance == null) throw new ArgumentNullException(nameof(tolerance));

            Header = header;
            Tracks = tracks;
            Messages = messages ?? Array.Empty<string>();
            Tolerance = tolerance;
        }

        #endregion
    }
}
