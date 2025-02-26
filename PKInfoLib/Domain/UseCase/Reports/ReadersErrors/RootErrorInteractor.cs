using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;

namespace PKInfo.Domain.UseCase.Reports.ReadersErrors
{
    internal sealed class RootErrorInteractor : BaseSnapshotInteractor
    {
        #region .ctors
        public RootErrorInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        public RootError Execute()
        {
            return RootError.Create(__snapshotTime, [.. __repository.GetReadersInfo()]);
        }
    }
}
