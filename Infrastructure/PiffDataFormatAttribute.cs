using System;


namespace PiffLibrary
{
    public class PiffDataFormatAttribute : Attribute
    {
        public PiffDataFormats Format { get; }


        public PiffDataFormatAttribute(PiffDataFormats format)
        {
            Format = format;
        }
    }
}