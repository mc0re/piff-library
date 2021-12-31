# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM.

The library was created to be able to write a PIFF header for Smooth Streaming data protected with PlayReady. There is a number of hard-coded values, mostly flags related to MP4 format.

The library can also parse a PIFF (`.cenc`) file.

## Usage - writing

The process is:
- Write the header `PiffWriter.WriteHeader()`.
  Use `PiffReader.GetTrackId()` to retrieve the track IDs from the data and put it into `PiffManifest`.
  A helper method `PiffWriter.GetDuration()` might also come handy.
- Append the data fragments as the are downloaded.
  Use `PiffReader.GetFragmentSequenceNumber()` to make sure the fragments come in increasing sequence order.
  Keep the fragment offsets in the output file for the footer.
- Write the footer `PiffWriter.WriteFooter()`.

## Usage - parsing


## Known issues

The library is designed for streams having 1 video (H.264) and 1 audio (AAC-L) track.
