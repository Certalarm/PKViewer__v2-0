using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System.Collections.Generic;

namespace PKInfo.Data.Interface
{
    internal interface IRepository
    {
        public IList<ReaderInfo> GetReadersInfo();

        public ReaderInfo GetReaderInfo(ReaderType readerType);

        public IList<KeyMediaInfo> GetKeyMediasInfo(KeyMediaType keyMediaType);

        public KeyMediaInfo GetKeyMediaInfo(string mediaPath);

        public IList<KeyContainerInfo> GetAllKeyContainersInfo(bool needDeleted);

        public IList<KeyContainerInfo> GetKeyContainersInfo(string mediaPath, bool needDeleted);
        
        public IList<KeyContainerInfo> GetKeyContainersInfo(ReaderType readerType, bool needDeleted);

        public IList<KeyContainerInfo> GetKeyContainersInfo(string[] containerPaths);

        public KeyContainerInfo GetKeyContainerInfo(string containerPath);

        public IList<string> DeleteKeyContainers(IEnumerable<string> paths);

        public string DeleteKeyContainer(string path);

        public void Update();
    }
}
