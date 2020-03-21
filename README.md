# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM.

The library was created to be able to write a PIFF header for Smooth Streaming data protected with PlayReady. There are a lot of hard-coded values, mostly flags. The idea is to write the header and then append the data chunks.

## Usage

Main entry: `PiffWriter.WriteHeader()`.
A helper method `PiffWriter.GetDuration()` might also come handy.

## Known issues

The library does produce the correct header for a certain manifest, but not for any manifest. So far the problem stays unresolved.
