using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Domain.UseCase.Readers.Get
{
    internal sealed class GetReadersInteractor: BaseSnapshotInteractor
    {
        #region .ctors
        internal GetReadersInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal IList<ReaderSnapshotInfo> Execute()
        {
            return __repository.GetReadersInfo()
                .Where(x => x.Error.Length < 1)
                ?.Select(x => ReaderSnapshotInfo.Create(x, __snapshotTime))
                .ToList()
                ?? [];
        }
    }
}
