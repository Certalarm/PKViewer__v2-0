using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Domain.UseCase.KeyMedias.Get
{
    internal sealed class GetKeyMediasInteractor : BaseSnapshotInteractor
    {
        #region .ctors
        internal GetKeyMediasInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal IList<KeyMediaSnapshotInfo> Execute(KeyMediaType keyMediaType)
        {
            return __repository.GetKeyMediasInfo(keyMediaType)
                .Where(x => x.Error.Length < 1)
                ?.Select(x => KeyMediaSnapshotInfo.Create(x, __snapshotTime))
                .ToList()
                ?? [];
        }
    }
}
