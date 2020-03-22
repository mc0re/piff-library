# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM.

The library was created to be able to write a PIFF header for Smooth Streaming data protected with PlayReady. There is a number of hard-coded values, mostly flags related to MP4 format.

## Usage

The main idea is to write the header, append the data chunks, the write the footer.

Start with `PiffWriter.WriteHeader()`.
A helper method `PiffWriter.GetDuration()` might also come handy.

Use `PiffReader.GetTrackId()` to retrieve the track IDs from the data and put it into `PiffManifest`.

Use `PiffReader.GetFragmentSequenceNumber()` to make sure the fragments come in increasing sequence order.

Finalize the file with `PiffWriter.WriteFooter()`.

## Known issues

The library is designed for streams having 1 video (H.264) and 1 audio (AAC-L) track.

The reading part is rudimentary, only to allow for required functionality.
