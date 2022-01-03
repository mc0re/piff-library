# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM. It is an ISO Base Media File Format brand (ISO 14496-12), so the library can essentially read ISO media files (some amendments might be needed).

The library was created to read and write PIFF files for Smooth Streaming data protected with PlayReady (CENC). There is a number of hard-coded values, mostly flags related to MP4 format.

## Usage - writing

The process is:
- Write the header `PiffWriter.WriteHeader()`.
  Use `PiffReader.GetTrackId()` to retrieve the track IDs from the data and put it into `PiffManifest`.
  A helper method `PiffWriter.GetDuration()` might also come handy.
- Append the data fragments (which are essentially `moof` boxes) as they are downloaded.
  Use `PiffReader.GetFragmentSequenceNumber()` to make sure the fragments come in increasing sequence order.
  Keep the fragment offsets in the output file for the footer.
- Write the footer `PiffWriter.WriteFooter()`.

## Usage - reading

Simple reading:
```csharp
var inputFile = PiffFile.Parse(inputStream);
```

## Known issues

The library is designed for streams having 1 video (H.264) and 1 audio (AAC-L) track.
