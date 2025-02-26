using PKInfo.Utility.Enum;

namespace PKInfo.Data.Implementation.DiskKeyMediaReader
{
    internal static class BaseDiskKeyMediaReaderExts
    {
        internal static KeyMediaType AsKeyMediaType(this ReaderType driveType) =>
            driveType switch
            {
                ReaderType.AllRemovable => KeyMediaType.FlashDrive,
                ReaderType.AllFixed => KeyMediaType.LocalDrive,
                // Add here other known types 
                _ => KeyMediaType.NotImplemented
            };

    }
}
