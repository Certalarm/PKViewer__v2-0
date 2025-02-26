using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;

namespace PKInfo.Domain.UseCase.KeyMedias.Get
{
    internal sealed class GetKeyMediaInteractor: BaseSnapshotInteractor
    {
        #region .ctors
        internal GetKeyMediaInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal KeyMediaSnapshotInfo Execute(string path)
        {
            return KeyMediaSnapshotInfo.Create(__repository.GetKeyMediaInfo(path), __snapshotTime);
        }
    }
}
