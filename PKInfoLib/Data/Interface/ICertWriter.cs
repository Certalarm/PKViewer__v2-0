namespace PKInfo.Data.Interface
{
    internal interface ICertWriter
    {
        public string Write(byte[] certRawData, string filename);
    }
}
