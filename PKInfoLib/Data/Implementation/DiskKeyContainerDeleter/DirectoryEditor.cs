using PKInfo.Utility;
using System;
using System.IO;

namespace PKInfo.Data.Implementation.DiskKeyContainerDeleter
{
    internal static class DirectoryEditor
    {
        internal static string MoveDirectory(string oldPath, string newPath)
        {
            var error = IsBadMoveParam(oldPath, newPath);
            if (error.Length > 0)
                return error;
            error = CreateParentDirectory(newPath);
            return error.Length > 0
                ? error
                : TryMoveDirectory(oldPath, newPath);
        }

        internal static string DeleteDirectory(string dirFullPath)
        {
            var error = IsBadDeleteParam(dirFullPath);
            return error.Length > 0
                ? error
                : TryDeleteDirectory(dirFullPath);
        }

        internal static string CreateDirectory(string dirFullPath)
        {
            var error = IsBadCreateParam(dirFullPath);
            if(error.Length > 0)
                return error;
            return Directory.Exists(dirFullPath)
                ? string.Empty
                : TryCreateDirectory(dirFullPath);
        }

        private static string IsBadMoveParam(string oldPath, string newPath)
        {
            if(string.IsNullOrWhiteSpace(oldPath))
                return Txt.KeyContainerEmptyPath;
            if(string.IsNullOrWhiteSpace(newPath))
                return Txt.KeyContainerEmptyDeletedPath;
            if (!Directory.Exists(oldPath))
                return Txt.KeyContainerNotExist;
            if (Directory.Exists(newPath))
                return Txt.KeyContainerAlreadyExist;
            return string.Empty;
        }

        private static string IsBadDeleteParam(string dirFullPath)
        {
            if (string.IsNullOrWhiteSpace(dirFullPath))
                return Txt.KeyContainerEmptyPath;
            if (!Directory.Exists(dirFullPath))
                return Txt.KeyContainerNotExist;
            return string.Empty;
        }

        private static string IsBadCreateParam(string dirFullPath)
        {
            if (string.IsNullOrWhiteSpace(dirFullPath))
                return Txt.KeyContainerEmptyPath;
            return string.Empty;
        }

        private static string CreateParentDirectory(string dirFullPath)
        {
            var separ = Path.DirectorySeparatorChar;
            var parentPath = dirFullPath.Substring(0, dirFullPath.LastIndexOf(separ));
            return CreateDirectory(parentPath);
        }

        private static string TryMoveDirectory(string oldPath, string newPath)
        {
            var error = string.Empty;
            try
            {
                Directory.Move(oldPath, newPath);
            }
            catch (Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }

        private static string TryCreateDirectory(string dirFullPath)
        {
            var error = string.Empty;
            try
            {
                Directory.CreateDirectory(dirFullPath);
            }
            catch (Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }

        private static string TryDeleteDirectory(string dirFullPath)
        {
            var error = string.Empty;
            try
            {
                Directory.Delete(dirFullPath, true);
            }
            catch (Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }
    }
}
