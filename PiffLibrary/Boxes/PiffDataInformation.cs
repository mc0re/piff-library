namespace PiffLibrary
{
    [BoxName("dinf")]
    internal class PiffDataInformation : PiffBoxBase
    {
        public PiffDataReference Reference { get; } = new PiffDataReference();
    }
}