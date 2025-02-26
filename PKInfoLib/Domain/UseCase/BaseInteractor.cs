using PKInfo.Data.Interface;

namespace PKInfo.Domain.UseCase
{
    internal abstract class BaseInteractor
    {
        protected readonly IRepository __repository;

        #region .ctors
        protected BaseInteractor(IRepository repository)
        {
            __repository = repository;    
        }
        #endregion
    }
}
