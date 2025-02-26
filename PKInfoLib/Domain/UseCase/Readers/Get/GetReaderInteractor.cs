using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using PKInfo.Utility.Enum;

namespace PKInfo.Domain.UseCase.Readers.Get
{
    internal sealed class GetReaderInteractor: BaseSnapshotInteractor
    {
        #region .ctors
        internal GetReaderInteractor(IRepository repository, DateTime snapshotTime) : base(repository, snapshotTime)
        {
        }
        #endregion

        internal ReaderSnapshotInfo Execute(ReaderType readerType)
        {
            var readerInfo = __repository.GetReaderInfo(readerType);
            return readerInfo.Error.Length > 0
                ? null
                : ReaderSnapshotInfo.Create(readerInfo, __snapshotTime);
        }
    }
}
