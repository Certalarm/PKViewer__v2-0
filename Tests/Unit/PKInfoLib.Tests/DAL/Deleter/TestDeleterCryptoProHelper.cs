using PKInfo.Data.DTO;
using PKInfo.Data.Implementation.DiskKeyContainerDeleter;
using PKInfo.Utility;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfoLib.Tests.DAL.Deleter
{
    public class TestDeleterCryptoProHelper
    {
        internal static string DeleteCPKeyContainer(string path, KeyContainerTypeInfoDB type)
        {
            var keyContainers = DirectoryNamer.IsPathContainsDeletedKeyStore(path)
                ? type.KeyContainersInfoDeletedDB
                : type.KeyContainersInfoDB;
            return Delete(path, keyContainers);
        }

        private static string Delete(string path, IList<KeyContainerInfoDB> keyContainers)
        {
            var key = keyContainers
                ?.FirstOrDefault(x => path.EqualsIgnoreCase(x.Path));
            if(key is not null)
            {
                keyContainers.Remove(key);
                return string.Empty;
            }
            return KeyContainerNotExist;
        }
    }
}
