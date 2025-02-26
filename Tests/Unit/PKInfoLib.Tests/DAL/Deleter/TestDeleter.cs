using PKInfo.Data.DTO;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static PKInfo.Utility.Exts;
//using static PKInfo.Utility.Txt;
using static PKInfoLib.Tests.DAL.Deleter.TestDeleterCryptoProHelper;


namespace PKInfoLib.Tests.DAL.Deleter
{
    internal class TestDeleter : IDeleter
    {
        private IList<ReaderInfoDB> _readers;

        #region .ctors
        public TestDeleter(IList<ReaderInfoDB> readers)
        {
            _readers = readers;    
        }
        #endregion

        public string Delete(string path)
        {
            KeyMediaInfoDB kmi = GetKeyMediaInfoDB(path);
            if (kmi is null)
                return Txt.KeyContainerNotExist;
            
            foreach (var type in kmi.KeyContainerTypesInfoDB)
            {
                if (type.Type == KeyContainerType.CryptoPro)
                    return DeleteCPKeyContainer(path, type);
            }
            return Txt.KeyMediaDeleterNotImplemented;
        }

        private KeyMediaInfoDB GetKeyMediaInfoDB(string path)
        {
            var rootPath = Path.GetPathRoot(path);
             
            foreach (var reader in _readers)
            {
                var kmi = reader.KeyMeadiasInfoDB
                    .FirstOrDefault(x => x.RootPath.EqualsIgnoreCase(rootPath));
                if (kmi is not null)
                    return kmi;
            }
            return null;
        }
    }
}
