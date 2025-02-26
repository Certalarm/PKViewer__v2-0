using PKInfo.Data.Interface;
using System.Collections.Generic;

namespace PKInfo.Domain.UseCase.Delete.KeyContainers
{
    internal sealed class DeleteKeyContainersInteractor : BaseInteractor
    {
        #region .ctors
        internal DeleteKeyContainersInteractor(IRepository repository) : base(repository)
        {
        }
        #endregion

        internal IList<string> Execute(IEnumerable<string> paths)
        {
            return __repository.DeleteKeyContainers(paths);
        }
    }
}
