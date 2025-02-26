using PKInfo.Data.Interface;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Domain.UseCase.Certificates.CertHelper;

namespace PKInfo.Domain.UseCase.Certificates.Save
{
    internal sealed class SaveCertsInteractor: BaseInteractor
    {
        private readonly ICertWriter _certWriter;

        #region .ctors
        public SaveCertsInteractor(IRepository repository, ICertWriter certWriter) : base(repository)
        {
            _certWriter = certWriter;
        }
        #endregion

        internal IList<string> Execute(IEnumerable<string> containerPaths) => 
            containerPaths
                .Select(x => SaveOneCert(x, __repository, _certWriter))
                .ToList();
    }
}
