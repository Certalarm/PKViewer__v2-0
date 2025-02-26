using PKInfo.Data.Implementation.DAO;
using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Data.Implementation.DAL.RepositoryHelper;

namespace PKInfo.Data.Implementation.DAL
{
    internal class Repository : IRepository
    {
        private readonly ReaderInfoDAO _DAO;
        private readonly List<ReaderInfo> _Cache = [];
        private bool _IsEmptyReadersInfo = false;

        #region .ctors
        public Repository(IDatabase db)
        {
            _DAO = new ReaderInfoDAO(db);
        }
        #endregion

        public string DeleteKeyContainer(string path) =>
            DeleteKeyContainers([path])[0];

        public IList<string> DeleteKeyContainers(IEnumerable<string> paths)
        {
            InitCache();
            IList<string> errors = DeleteEachKeyContainer(paths);
            UpdateAfterDelete(paths, errors);
            return errors;
        }

        public IList<KeyContainerInfo> GetAllKeyContainersInfo(bool needDeleted)
        {
            var allContainers = GetKeyContainersAll(GetReadersInfo());
            if (!needDeleted)
                allContainers = allContainers
                    .Where(x => x.IsDeleted != true);
            return allContainers.ToList();
        }

        public IList<KeyContainerInfo> GetKeyContainersInfo(string mediaPath, bool needDeleted)
        {
            var allContainers = GetKeyMediaInfo(mediaPath).KeyContainersInfo;
            return needDeleted
                ? allContainers
                : allContainers
                    .Where(x => x.IsDeleted != true)
                    .ToList();
        }

        public IList<KeyContainerInfo> GetKeyContainersInfo(ReaderType readerType, bool needDeleted) =>
            GetReaderInfo(readerType).KeyMediasInfo
                .SelectMany(x => GetKeyContainersInfo(x.RootPath, needDeleted))
                .ToList();

        public IList<KeyContainerInfo> GetKeyContainersInfo(string[] containerPaths) =>
            GetKeyContainersByPaths(containerPaths, GetReadersInfo())
                .ToList();

        public KeyContainerInfo GetKeyContainerInfo(string containerPath) =>
            GetKeyContainerByPath(containerPath, GetReadersInfo());

        public KeyMediaInfo GetKeyMediaInfo(string mediaPath) =>
            GetKeyMediaByPath(mediaPath, GetReadersInfo());

        public IList<KeyMediaInfo> GetKeyMediasInfo(KeyMediaType keyMediaType) =>
            GetKeyMediasByType(keyMediaType, GetReadersInfo())
                .ToList();

        public ReaderInfo GetReaderInfo(ReaderType readerType) => 
            GetReaderByType(readerType, GetReadersInfo());

        public IList<ReaderInfo> GetReadersInfo()
        {
            InitCache();
            return _Cache;
        }

        public void Update()
        {
            _Cache.Clear();
            _Cache.AddRange(_DAO.GetReadersInfo(needDeleted: true));
            _IsEmptyReadersInfo = !_Cache.Any();
        }

        private IList<string> DeleteEachKeyContainer(IEnumerable<string> paths)
        {
            IList<string> errors = [];
            foreach (var path in paths)
            {
                var containerInfo = GetKeyContainerByPath(path, GetReadersInfo());
                errors.Add(containerInfo.Error.Length > 0
                    ? containerInfo.Error
                    : _DAO.DeleteKeyContainer(path));
            }
            return errors;
        }

        private void UpdateAfterDelete(IEnumerable<string> paths, IEnumerable<string> errors)
        {
            var errorsNotEmptyCount = errors
                .Count(x => x.Length > 0);
            if (errorsNotEmptyCount < paths.Count())
                Update();
        }

        private void InitCache()
        {
            if (!_IsEmptyReadersInfo && !_Cache.Any())
                Update();
        }
    }
}
