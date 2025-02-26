using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKInfo.Domain.UseCase.KeyContainers.Get
{
    internal sealed class GetKeyContainerInteractor : BaseSnapshotInteractor
    {
        #region .ctors
        internal GetKeyContainerInteractor(IRepository repository, DateTime snapsotTime) : base(repository, snapsotTime)
        {
        }
        #endregion

        internal KeyContainerInfo Execute(string path)
        {
            return __repository.GetKeyContainerInfo(path);
        }

    }
}
