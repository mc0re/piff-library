﻿namespace PiffLibrary
{
    [BoxName("moof")]
    internal class PiffMovieFragment
    {
        public PiffMovieFragmentHeader Header { get; internal set; }

        public PiffTrackFragment Track { get; internal set; }
    }
}
