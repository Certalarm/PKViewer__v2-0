using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Domain.UseCase.KeyContainers.Get
{
    internal sealed class GetAllInteractor : BaseSnapshotInteractor
    {
        #region .ctors
        internal GetAllInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal IList<KeyContainerSnapshotInfo> Execute()
        {
            return __repository.GetAllKeyContainersInfo(needDeleted: true)
                .Where(x => x.Error.Length < 1)
                ?.Select(x => KeyContainerSnapshotInfo.Create(x, __snapshotTime))
                .ToList()
                ?? [];
        }
    }
}
