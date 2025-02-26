using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Domain.UseCase.KeyContainers.Get
{
    internal sealed class GetKeyMediaKeyContainersInteractor : BaseSnapshotInteractor
    {
        #region .ctors
        internal GetKeyMediaKeyContainersInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal IList<KeyContainerSnapshotInfo> Execute(string mediaPath)
        {
            return __repository.GetKeyContainersInfo(mediaPath, needDeleted: true)
                .Where(x => x.Error.Length < 1)
                ?.Select(x => KeyContainerSnapshotInfo.Create(x, __snapshotTime))
                .ToList()
                ?? [];
        }
    }
}
