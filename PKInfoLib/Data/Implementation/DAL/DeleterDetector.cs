using PKInfo.Data.Implementation.DiskKeyContainerDeleter;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;

namespace PKInfo.Data.Implementation.DAL
{
    internal static class DeleterDetector
    {
        internal static IDeleter GetDeleter((KeyMediaType, KeyContainerType) types) =>
            types.Item1 switch
            {
                KeyMediaType.LocalDrive or
                KeyMediaType.FloppyDrive or
                KeyMediaType.FlashDrive => GetDiskDeleter(types.Item2),
                _ => null
            };

        private static IDeleter GetDiskDeleter(KeyContainerType keyContainerType) =>
            keyContainerType switch
            {
                KeyContainerType.CryptoPro => new DiskCryptoProContainerDeleter(),
                _ => null
            };

    }
}
