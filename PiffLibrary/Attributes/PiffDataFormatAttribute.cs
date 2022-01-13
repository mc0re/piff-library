using System;


namespace PiffLibrary.Boxes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class PiffDataFormatAttribute : Attribute
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
        /// Only private functions are accepted (see <see cref="PiffPropertyInfo.GetPropertyFormat"/>).
        /// </summary>
        public PiffDataFormatAttribute(string formatFn)
        {
            FormatFn = formatFn;
        }
    }
}