using PKInfo.Data.Interface;
using static PKInfo.Domain.UseCase.Certificates.CertHelper;

namespace PKInfo.Domain.UseCase.Certificates.Save
{
    internal sealed class SaveCertInteractor : BaseInteractor
    {
        private readonly ICertWriter __certWriter;

        #region .ctors
        public SaveCertInteractor(IRepository repository, ICertWriter certWriter) : base(repository)
        {
            __certWriter = certWriter;
        }
        #endregion

        internal string Execute(string containerPath) =>
            SaveOneCert(containerPath, __repository, __certWriter);
    }
}
