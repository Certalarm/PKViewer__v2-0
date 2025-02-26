using PKInfo.Data.DTO;
using PKInfo.Utility;
using System;
using System.IO;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.CryptoProContainerHelper;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal static class FileMetaInfoReader
    {

        internal static string ReadMetaInfoCPNameKey(string containerPath, out FileMetaInfo metaInfo)
        {
            string fullPath = Path.Combine(containerPath, CPNameKey);
            return ReadMetaInfo(fullPath, out metaInfo);
        }

        internal static string ReadMetaInfoCPHeaderKey(string containerPath, out FileMetaInfo metaInfo)
        {
            string fullPath = Path.Combine(containerPath, CPHeaderKey);
            return ReadMetaInfo(fullPath, out metaInfo);
        }


        internal static string ReadMetaInfo(string fullPath, out FileMetaInfo metaInfo)
        {
            metaInfo = new FileMetaInfo(0, null);
            if (!File.Exists(fullPath))
                return Txt.KeyFileNotExist;
            return TryReadMetaInfo(fullPath, out metaInfo);
        }

        private static string TryReadMetaInfo(string fullPath, out FileMetaInfo metaInfo)
        {
            var error = string.Empty;
            metaInfo = new FileMetaInfo(0, null);
            try
            {
                FileInfo fi = new(fullPath);
                metaInfo = new FileMetaInfo(fi.Length, fi.LastWriteTimeUtc);
            }
            catch(Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }
    }
}
