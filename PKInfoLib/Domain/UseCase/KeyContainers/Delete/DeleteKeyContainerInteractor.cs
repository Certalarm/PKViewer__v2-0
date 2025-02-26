using PKInfo.Data.Interface;

namespace PKInfo.Domain.UseCase.Delete.KeyContainers
{
    internal sealed class DeleteKeyContainerInteractor : BaseInteractor
    {
        #region .ctors
        internal DeleteKeyContainerInteractor(IRepository repository) : base(repository)
        {
        }
        #endregion

        internal string Execute(string path)
        {
            return __repository.DeleteKeyContainer(path);
        }
    }
}
