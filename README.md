# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM.

The library was created to be able to write a PIFF header for Smooth Streaming data protected with PlayReady. There are a lot of hard-coded values, mostly flags. The idea is to write the header and then append the data chunks.

## Usage

Main entry: `PiffWriter.WriteHeader()`.
A helper method `PiffWriter.GetDuration()` might also come handy.

## Known issues

There is one box I didn't decifer yet - `avcC`. The data hardcoded in it is the data I extracted from a sample file.

The audio and video duration are not separated, a single duration is used for both "audio", "video", and "total" (which is the longest of the other two).

The library does produce the correct header for a certain manifest, but not for my own. Probably due to the AVC configuration box. Have to keep looking.
