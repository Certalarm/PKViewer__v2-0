using PKInfo.Data.Interface;
using System;

namespace PKInfo.Domain.UseCase
{
    internal abstract class BaseSnapshotInteractor : BaseInteractor
    {
        protected DateTime __snapshotTime;

        #region .ctors
        protected BaseSnapshotInteractor(IRepository repository, DateTime snapsotTime) : base(repository)
        {
            __snapshotTime = snapsotTime;
        }
        #endregion
    }
}
