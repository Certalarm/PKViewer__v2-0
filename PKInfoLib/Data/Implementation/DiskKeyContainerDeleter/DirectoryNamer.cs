using PKInfo.Utility.Enum;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerDeleter
{
    internal static class DirectoryNamer
    {
        internal static string GetDirName(string fullPath) =>
            string.IsNullOrEmpty(fullPath)
                ? string.Empty
                : fullPath
                    .TrimEnd(System.IO.Path.DirectorySeparatorChar)
                    .Split(System.IO.Path.DirectorySeparatorChar)
                    .Last();

        internal static bool IsPathContainsDeletedKeyStore(string path) =>
            string.IsNullOrWhiteSpace(path)
                ? false
                : path.IndexOf(DELETED_KEY_DIR_NAME) > 0;

        internal static string BuildDeletedKeyStoreFullPaths(string keyContainerFullPath, KeyContainerType type)
        {
            if (string.IsNullOrWhiteSpace(keyContainerFullPath))
                return string.Empty;
            var typeString = type.ToString();
            var basePath = GetBaseDeletedKeyStoreFullPath(keyContainerFullPath);
            return System.IO.Path.Combine(basePath, typeString);
        }

        private static string GetBaseDeletedKeyStoreFullPath(string keyContainerFullPath)
        {
            var pathRoot = System.IO.Path.GetPathRoot(keyContainerFullPath);
            return System.IO.Path.Combine(pathRoot, DELETED_KEY_DIR_NAME);
        }
    }
}
