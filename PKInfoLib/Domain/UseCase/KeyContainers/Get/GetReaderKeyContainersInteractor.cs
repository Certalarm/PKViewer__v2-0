using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using PKInfo.Utility.Enum;

namespace PKInfo.Domain.UseCase.KeyContainers.Get
{
    internal sealed class GetReaderKeyContainersInteractor: BaseSnapshotInteractor
    {
        #region .ctors
        internal GetReaderKeyContainersInteractor(IRepository repository, DateTime snapshotTime) 
            : base(repository, snapshotTime)
        {
        }
        #endregion

        internal IList<KeyContainerSnapshotInfo> Execute(ReaderType readerType)
        {
            return __repository.GetKeyContainersInfo(readerType, needDeleted: true)
                .Where(x => x.Error.Length < 1)
                ?.Select(x => KeyContainerSnapshotInfo.Create(x, __snapshotTime))
                .ToList()
                ?? [];
        }
    }
}
