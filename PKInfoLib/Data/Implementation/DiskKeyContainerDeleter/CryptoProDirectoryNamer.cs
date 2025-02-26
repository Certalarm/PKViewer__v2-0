using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Linq;
using static PKInfo.Data.Implementation.DiskKeyContainerDeleter.DirectoryNamer;

namespace PKInfo.Data.Implementation.DiskKeyContainerDeleter
{
    internal static class CryptoProDirectoryNamer
    {
        internal static string GetCPDeletedContainerFullPath(string keyContainerFullPath)
        {
            var deletedFullPath = BuildCPDeletedKeyContainerFullPath(keyContainerFullPath);
            if(!System.IO.Directory.Exists(deletedFullPath)) 
                return deletedFullPath;
            while(System.IO.Directory.Exists(deletedFullPath))
            {
                deletedFullPath = GetNewFullPath(deletedFullPath);
            }
            return deletedFullPath;
        }

        private static string GetNewFullPath(string existFullPath)
        {
            var extWoDot = System.IO.Path.GetExtension(existFullPath).TrimStart(Txt.Dot);
            var curNumValue = IsNum(extWoDot)
                ? int.Parse(extWoDot) 
                : 1000;
             return ReassignmentFullPath(curNumValue, existFullPath);
        }

        private static string ReassignmentFullPath(int existValue, string existFullPath)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (i == existValue)
                    continue;
                var newExtWithDot = $"{Txt.Dot}{i:D3}";
                var fullPathWithChangedExt = System.IO.Path.ChangeExtension(existFullPath, newExtWithDot);
                if (!System.IO.Directory.Exists(fullPathWithChangedExt))
                    return fullPathWithChangedExt;
            }
            return string.Empty;
        }

        private static string BuildCPDeletedKeyContainerFullPath(string keyContainerFullPath)
        {
            var keyDir = keyContainerFullPath.Split(System.IO.Path.DirectorySeparatorChar).Last();
            var keyStore = BuildDeletedKeyStoreFullPaths(keyContainerFullPath, KeyContainerType.CryptoPro);
            return System.IO.Path.Combine(keyStore, keyDir);
        }

        private static bool IsNum(string value) => int.TryParse(value, out _);
    }
}
