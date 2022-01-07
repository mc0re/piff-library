using System;


namespace PiffLibrary
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class PiffDataFormatAttribute : Attribute
    {
        public string FormatFn { get; }

        public PiffDataFormats Format { get; }


        /// <summary>
        /// Override the default format for a property.
        /// </summary>
        public PiffDataFormatAttribute(PiffDataFormats format)
        {
            Format = format;
        }


        /// <summary>
        /// Property format depends on the given (by name) function of the target box.
        /// </summary>
        public PiffDataFormatAttribute(string formatFn)
        {
            FormatFn = formatFn;
        }
    }
}