# PIFF library

PIFF - Protected Interoperable File Format - is used for transmitting video and audio via streams. It is also the file format used in PlayReady DRM.

The library was created to be able to write a PIFF header for Smooth Streaming data protected with PlayReady. There are a lot of hard-coded values, mostly flags. The idea is to write the header and then append the data chunks.

## Known issues

There are two boxes I couldn't decifer yet - "avcC" and "esds". The data hardcoded in those is the data I extracted from a sample file.

I actually don't know yet how well it works, the system isn't quite ready yet. I will update this file when I learn more.
