namespace PKInfo.Data.DTO
{
    internal class DriveMetaInfo
    {
        public string Name { get; }
        public string Label { get; }
        public long Size { get; }
        public string Error { get; set; }

        #region .ctors  
        public DriveMetaInfo(string name, string label, long size, string error)
        {
            Name = name;
            Label = label;
            Size = size;
            Error = error;
        }
        #endregion
    }
}
